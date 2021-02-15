using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Size
{
    [SerializeField]
    public int Width;
    [SerializeField]
    public int Height;

    public Size(int width, int height)
    {
        Width = width;
        Height = height;
    }
}

public class Rect
{
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public int Width
    {
        get { return Right - Left; }
    }

    public int Height
    {
        get { return Bottom - Top; }
    }

    public Rect(int left, int top, int right, int bottom)
    {
        Left = left;
        Right = right;
        Top = top;
        Bottom = bottom;
    }

    public override string ToString()
    {
        return "Left: " + Left + " Right: " + Right + " Top: " + Top + " Bottom:" + Bottom;
    }
}

class Section
{
    public Rect Bound;
    public Rect Room;
    public Rect Path;

    public Section(Rect rect)
    {
        Bound = rect;
    }
}

public enum TileType
{
    Wall,
    Floor,
    Player,
    Enemy,
    Exit
}

public struct Pos
{
    public int X;
    public int Y;

    public Pos(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Room
{
    public Rect Rect;

    public Room(Rect rect)
    {
        Rect = rect;
    }

    public Pos RandomPosInRect()
    {
        int x = Random.Range(Rect.Left, Rect.Right);
        int y = Random.Range(Rect.Top, Rect.Bottom);
        return new Pos(x, y);
    }
}

public class DangeonGenerator
{

    private TileType[,] map;
    private List<Section> sectionList = new List<Section>();
    private List<Room> rooms = new List<Room>();


    const int MIN_ROOM = 5;
    const int MAX_ROOM = 8;
    const int OUTER_MERGIN = 1;
    const int POS_MERGIN = 2;


    public (TileType[,], List<Room>) Generate(Size mapSize)
    {
        map = new TileType[mapSize.Width, mapSize.Height];

        CreateSection(new Rect(0, 0, mapSize.Width - 1, mapSize.Height - 1));

        bool bVertical = Random.Range(0, 2) == 0;
        SplitDivison(bVertical);

        CreateRoom();
        ConnectRooms();


        return (map, rooms);
    }

    void CreateSection(Rect rect)
    {
        sectionList.Add(new Section(rect));
    }

    bool CheckDivisionSize(int size)
    {
        int min = (MIN_ROOM + OUTER_MERGIN) * 2 + 1;

        return size >= min;
    }


    void SplitDivison(bool bVertical)
    {
        Section parent = sectionList[sectionList.Count - 1];
        sectionList.Remove(parent);

        Section child;

        if (bVertical)
        {
            if (CheckDivisionSize(parent.Bound.Height) == false)
            {
                sectionList.Add(parent);
                return;
            }

            int a = parent.Bound.Top + (MIN_ROOM + OUTER_MERGIN);
            int b = parent.Bound.Bottom - (MIN_ROOM + OUTER_MERGIN);
            int ab = b - a;
            ab = Mathf.Min(ab, MAX_ROOM);

            int p = a + Random.Range(0, ab + 1);

            child = new Section(new Rect(parent.Bound.Left, p, parent.Bound.Right, parent.Bound.Bottom));

            parent.Bound.Bottom = child.Bound.Top;
        }
        else
        {
            if (CheckDivisionSize(parent.Bound.Width) == false)
            {
                sectionList.Add(parent);
                return;
            }

            int a = parent.Bound.Left + (MIN_ROOM + OUTER_MERGIN);
            int b = parent.Bound.Right - (MIN_ROOM + OUTER_MERGIN);
            int ab = b - a;
            ab = Mathf.Min(ab, MAX_ROOM);

            int p = a + Random.Range(0, ab + 1);

            child = new Section(new Rect(p, parent.Bound.Top, parent.Bound.Right, parent.Bound.Bottom));

            parent.Bound.Right = child.Bound.Left;
        }

        if (Random.Range(0, 2) == 0)
        {
            sectionList.Add(parent);
            sectionList.Add(child);
        }
        else
        {
            sectionList.Add(child);
            sectionList.Add(parent);
        }

        SplitDivison(!bVertical);
    }

    void CreateRoom()
    {
        foreach (Section section in sectionList)
        {
            int dw = section.Bound.Width - OUTER_MERGIN;
            int dh = section.Bound.Height - OUTER_MERGIN;

            int sw = Random.Range(MIN_ROOM, dw);
            int sh = Random.Range(MIN_ROOM, dh);

            sw = Mathf.Min(sw, MAX_ROOM);
            sh = Mathf.Min(sh, MAX_ROOM);

            int rw = dw - sw;
            int rh = dh - sh;

            int rx = Random.Range(0, rw) + POS_MERGIN;
            int ry = Random.Range(0, rh) + POS_MERGIN;

            int left = section.Bound.Left + rx;
            int right = left + sw;
            int top = section.Bound.Top + ry;
            int bottom = top + sh;

            section.Room = new Rect(left, top, right, bottom);
            rooms.Add(new Room(section.Room));
            FillDgRect(section.Room);
        }
    }

    void ConnectRooms()
    {
        for (int i = 0; i < sectionList.Count - 1; i++)
        {
            Section a = sectionList[i];
            Section b = sectionList[i + 1];

            CreatePath(a, b);

            for (int j = i + 2; j < sectionList.Count; j++)
            {
                Section c = sectionList[j];
                if (CreatePath(a, c, true))
                {
                    break;
                }
            }
        }
    }


    bool CreatePath(Section divA, Section divB, bool bGrandChild = false)
    {
        if (divA.Bound.Bottom == divB.Bound.Top ||
            divA.Bound.Top == divB.Bound.Bottom)
        {
            int x1 = Random.Range(divA.Room.Left, divA.Room.Right);
            int x2 = Random.Range(divB.Room.Left, divB.Room.Right);
            int y;

            if (bGrandChild)
            {
                if (divA.Path != null) { x1 = divA.Path.Left; }
                if (divB.Path != null) { x2 = divB.Path.Left; }
            }

            if (divA.Bound.Top > divB.Bound.Top)
            {
                y = divA.Bound.Top;
                divA.Path = new Rect(x1, y + 1, x1 + 1, divA.Room.Top);
                divB.Path = new Rect(x2, divB.Room.Bottom, x2 + 1, y);
            }
            else
            {
                y = divB.Bound.Top;
                divA.Path = new Rect(x1, divA.Room.Bottom, x1 + 1, y);
                divB.Path = new Rect(x2, y, x2 + 1, divB.Room.Top);
            }
            FillDgRect(divA.Path);
            FillDgRect(divB.Path);

            FillHLine(x1, x2, y);

            return true;
        }

        if (divA.Bound.Left == divB.Bound.Right ||
            divA.Bound.Right == divB.Bound.Left)
        {
            int y1 = Random.Range(divA.Room.Top, divA.Room.Bottom);
            int y2 = Random.Range(divB.Room.Top, divB.Room.Bottom);
            int x;

            if (bGrandChild)
            {
                if (divA.Path != null) { y1 = divA.Path.Top; }
                if (divB.Path != null) { y2 = divB.Path.Top; }
            }

            if (divA.Bound.Left > divB.Bound.Left)
            {
                x = divA.Bound.Left;
                divB.Path = new Rect(divB.Room.Right, y2, x, y2 + 1);
                divA.Path = new Rect(x + 1, y1, divA.Room.Left, y1 + 1);
            }
            else
            {
                x = divB.Bound.Left;
                divA.Path = new Rect(divA.Room.Right, y1, x, y1 + 1);
                divB.Path = new Rect(x, y2, divB.Room.Left, y2 + 1);
            }
            FillDgRect(divA.Path);
            FillDgRect(divB.Path);

            FillVLine(y1, y2, x);

            return true;
        }

        return false;
    }

    void FillHLine(int left, int right, int y)
    {
        if (left > right)
        {
            int tmp = left;
            left = right;
            right = tmp;
        }
        FillDgRect(new Rect(left, y, right + 1, y + 1));
    }

    void FillVLine(int top, int bottom, int x)
    {
        if (top > bottom)
        {
            int tmp = top;
            top = bottom;
            bottom = tmp;
        }


        FillDgRect(new Rect(x, top, x + 1, bottom + 1));
    }

    void FillDgRect(Rect r)
    {
        for (int j = r.Top; j < r.Bottom; j++)
        {
            for (int i = r.Left; i < r.Right; i++)
            {
                map[i, j] = TileType.Floor;
            }
        }
    }
}

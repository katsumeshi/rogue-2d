using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Size
{
    [SerializeField]
    public int width;
    [SerializeField]
    public int height;
}

class Rect
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

    //public Rect(Rect rect)
    //{
    //    lx = rect.lx;
    //    ly = rect.ly;
    //    rx = rect.rx;
    //    ry = rect.ry;
    //}

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
    //public bool doneSplitVertical;
    //public bool doneSplitHorizontal;

    public Section(Rect rect)
    {
        Bound = rect;
        //room = new Rect(rect);
        //doneSplitVertical = false;
        //doneSplitHorizontal = false;
    }

    //public Section(Section section)
    //{
    //    rect = new Rect(section.rect);
    //    room = new Rect(section.room);
    //    doneSplitVertical = section.doneSplitVertical;
    //    doneSplitHorizontal = section.doneSplitHorizontal;
    //}
}

//class Path
//{
//    public PathType type;
//    public Section section0;
//    public Section section1;

//    public Path(PathType type, Section section0, Section section1)
//    {
//        this.type = type;
//        this.section0 = new Section(section0);
//        this.section1 = new Section(section1);
//    }
//}

enum TileType
{
    Floor,
    Wall
}

public class DangeonGenerator : MonoBehaviour
{

    public GameObject tile;
    //    public GameObject room;
    //    public GameObject path;
    public Size mapSize;
    //    public int minRoomSize = 2;
    //    public int marginBetweenSectionRoom = 4;
    //    int minRectSize;
    private bool[,] mapSection;
    //    private bool[,] mapRoom;
    //    private bool[,] mapPath;
    private List<Section> sectionList = new List<Section>();
    //    private List<Path> pathList = new List<Path>();
    //    private List<Rect> roomList = new List<Rect>();


    const int MIN_ROOM = 13;
    const int MAX_ROOM = 15;
    const int OUTER_MERGIN = 3;
    int loopcount = 20;
    const int POS_MERGIN = 2;


    //    // Start is called before the first frame update
    void Start()
    {
        //        minRectSize = minRoomSize + (marginBetweenSectionRoom * 2);
        //        Random.InitState(System.DateTime.Now.Millisecond);
        mapSection = new bool[mapSize.width, mapSize.height];
        //        mapRoom = new bool[mapSize.width, mapSize.height];
        //        mapPath = new bool[mapSize.width, mapSize.height];

        //        SectionSplit(SectionAdd(new Section(new Rect(0, 0, mapSize.width - 1, mapSize.height - 1))));

        //        RoomMake();


        //        sectionList.ForEach(section =>
        //        {
        //            //print(section.rect.lx + "," + section.rect.ly + "," + section.rect.rx + "," + section.rect.ry);
        //            Rect rect = section.rect;
        //            for (int i = rect.lx, j = rect.ly; i <= rect.rx; i++) mapSection[i, j] = true;
        //            for (int i = rect.lx, j = rect.ry; i <= rect.rx; i++) mapSection[i, j] = true;
        //            for (int i = rect.lx, j = rect.ly; j <= rect.ry; j++) mapSection[i, j] = true;
        //            for (int i = rect.rx, j = rect.ly; j <= rect.ry; j++) mapSection[i, j] = true;
        //        });

        //        roomList.ForEach(room =>
        //        {
        //            for (int i = room.lx; i <= room.rx; i++)
        //            {
        //                for (int j = room.ly; j <= room.ry; j++)
        //                {
        //                    mapRoom[i, j] = true;
        //                }
        //            }
        //        });

        //        pathList.ForEach(path =>
        //        {

        //            //print(path.section0.rect.ToString());
        //            //print(path.section0.room.ToString());
        //            //print(path.section1.rect.ToString());
        //            //print(path.section1.room.ToString());
        //            //print("!!!!!");
        //            //print("!!!!!");
        //            int c0x = 0;
        //            int c0y = 0;
        //            int c1x = 0;
        //            int c1y = 0;
        //            switch (path.type)
        //            {
        //                case PathType.Horizontal:
        //                    c0x = path.section0.rect.rx;
        //                    c0y = Random.Range(path.section0.room.ly + 1, path.section0.room.ry);
        //                    c1x = path.section1.rect.lx;
        //                    c1y = Random.Range(path.section1.room.ly + 1, path.section1.room.ry);
        //                    //print(path.section0.room.ToString());
        //                    //print(path.section1.room.ToString());
        //                    //print(c0x);
        //                    //print(c0y);
        //                    //print(c1x);
        //                    //print(c1y);
        //                    //print("!!!!!");
        //                    line(c0x, c0y, c1x, c1y);
        //                    line(path.section0.room.rx, c0y, c0x, c0y);
        //                    line(path.section1.room.lx, c1y, c1x, c1y);
        //                    break;
        //                case PathType.Vertical:
        //                    c0x = Random.Range(path.section0.room.lx + 1, path.section0.room.rx);
        //                    c0y = path.section0.rect.ry;
        //                    c1x = Random.Range(path.section1.room.lx + 1, path.section1.room.rx);
        //                    c1y = path.section1.rect.ly;
        //                    line(c0x, c0y, c1x, c1y);
        //                    line(c0x, path.section0.room.ry, c0x, c0y);
        //                    line(c1x, path.section1.room.ly, c1x, c1y);
        //                    break;
        //            }
        //        });

        CreateSection(new Rect(0, 0, mapSize.width - 1, mapSize.height - 1));

        bool bVertical = (Random.Range(0, 2) == 0);
        SplitDivison(bVertical);

        CreateRoom();

        ConnectRooms();


        for (int j = 0; j < mapSize.height; j++)
        {
            for (int i = 0; i < mapSize.width; i++)
            {
                //if (mapSection[i, j] == true) Instantiate(tile, new Vector3(1.0f * i - 100f, 1.0f * j, 0.0f), Quaternion.identity);
                //if (mapRoom[i, j] == true) Instantiate(room, new Vector3(1.0f * i, 1.0f * j, 0.0f), Quaternion.identity);
                //if (mapPath[i, j] == true) Instantiate(path, new Vector3(1.0f * i + 100f, 1.0f * j, 0.0f), Quaternion.identity);
                
                if (mapSection[i, j] == false)
                {
                    GameObject obj = Instantiate(tile, new Vector3(1.0f * i, 1.0f * j, 0.0f), Quaternion.identity);
                    obj.name = "tile" + i + j;
                }
            }
        }

        //List<Color> colors = new List<Color>()
        //{
        //    Color.black,
        //    Color.gray,
        //    Color.red,
        //    Color.magenta,
        //    Color.black,
        //    Color.cyan,
        //    Color.red,
        //};

        //int i2 = 0;
        //sectionList.ForEach(section =>
        //{
        //    Color color = colors[i2];
        //    for (int j = section.Bound.Top; j <= section.Bound.Bottom; j++)
        //    {
        //        for (int i = section.Bound.Left; i <= section.Bound.Right; i++)
        //        {
        //            GameObject tile = GameObject.Find("tile" + i + j);
        //            if (tile != null)
        //            {
        //                SpriteRenderer renderer = tile.GetComponent<SpriteRenderer>();
        //                renderer.color = color;
        //            }

        //        }
        //    }
        //    i2 += 1;
        //});
        //    }

        //    void line(int x0, int y0, int x1, int y1)
        //    {
        //        int min_x, max_x, min_y, max_y, i, j;
        //        min_x = Mathf.Min(x0, x1);
        //        max_x = Mathf.Max(x0, x1);
        //        min_y = Mathf.Min(y0, y1);
        //        max_y = Mathf.Max(y0, y1);
        //        //g_assert((min_x >= 0) && (max_x < MAP_W) && (min_y >= 0) && (max_y < MAP_H));
        //        if ((x0 <= x1) && (y0 >= y1))
        //        {
        //            for (i = min_x; i <= max_x; i++) mapPath[i, max_y] = true;
        //            for (j = min_y; j <= max_y; j++) mapPath[max_x, j] = true;
        //            return;
        //        };
        //        if ((x0 > x1) && (y0 > y1))
        //        {
        //            for (i = min_x; i <= max_x; i++) mapPath[i, min_y] = true;
        //            for (j = min_y; j <= max_y; j++) mapPath[max_x, j] = true;
        //            return;
        //        };
        //        if ((x0 > x1) && (y0 <= y1))
        //        {
        //            for (i = min_x; i <= max_x; i++) mapPath[i, min_y] = true;
        //            for (j = min_y; j <= max_y; j++) mapPath[min_x, j] = true;
        //            return;
        //        };
        //        if ((x0 <= x1) && (y0 < y1))
        //        {
        //            for (i = min_x; i <= max_x; i++) mapPath[i, max_y] = true;
        //            for (j = min_y; j <= max_y; j++) mapPath[min_x, j] = true;
        //            return;
        //        };
        //    }


        //    void SectionSplit(Section parent)
        //    {
        //        if (parent.rect.ry - parent.rect.ly <= minRectSize * 2)
        //        {
        //            parent.doneSplitVertical = true;
        //        }
        //        if (parent.rect.rx - parent.rect.lx <= minRectSize * 2)
        //        {
        //            parent.doneSplitHorizontal = true;
        //        }

        //        if (parent.doneSplitVertical && parent.doneSplitHorizontal)
        //        {
        //            return;
        //        }

        //        Section child = SectionAdd(parent);
        //        if (parent.doneSplitVertical == false)
        //        {
        //            int splitY = Random.Range(parent.rect.ly + minRectSize, parent.rect.ry - minRectSize);
        //            parent.rect.ry = splitY;
        //            child.rect.ly = splitY;
        //            parent.doneSplitVertical = true;
        //            child.doneSplitVertical = true;
        //            PathAdd(new Path(PathType.Vertical, new Section(parent), new Section(child)));
        //            SectionSplit(parent);
        //            SectionSplit(child);
        //            return;
        //        }
        //        if (parent.doneSplitHorizontal == false)
        //        {
        //            int splitX = Random.Range(parent.rect.lx + minRectSize, parent.rect.rx - minRectSize);
        //            parent.rect.rx = splitX;
        //            child.rect.lx = splitX;
        //            parent.doneSplitHorizontal = true;
        //            child.doneSplitHorizontal = true;
        //            PathAdd(new Path(PathType.Horizontal, new Section(parent), new Section(child)));
        //            SectionSplit(parent);
        //            SectionSplit(child);
        //            return;
        //        }
        //    }

        //    void RoomMake()
        //    {
        //        int i = 0;
        //        sectionList.ForEach(section =>
        //        {
        //            Rect rect = section.rect;
        //            int width = Random.Range(minRoomSize, rect.rx - rect.lx - (marginBetweenSectionRoom * 2) + 1);
        //            int height = Random.Range(minRoomSize, rect.ry - rect.ly - (marginBetweenSectionRoom * 2) + 1);
        //            int x = Random.Range(rect.lx + marginBetweenSectionRoom, rect.rx - marginBetweenSectionRoom - width + 1);
        //            int y = Random.Range(rect.ly + marginBetweenSectionRoom, rect.ry - marginBetweenSectionRoom - height + 1);
        //            //print(rect.lx + "," + rect.ly + "," + (rect.rx - rect.lx) + "," + (rect.ry - rect.ly));
        //            //print(x + "," + y + "," +  width + "," + height);
        //            sectionList[i].room = RoomAdd(new Rect(x, y, x + width, y + height));
        //            i++;
        //        });
        //    }

        //    Section SectionAdd(Section section)
        //    {
        //        Section s = new Section(section);
        //        s.doneSplitHorizontal = false;
        //        s.doneSplitVertical = false;
        //        sectionList.Add(s);
        //        return s;
        //    }

        //    Rect RoomAdd(Rect room)
        //    {
        //        Rect r = new Rect(room);
        //        roomList.Add(r);
        //        //print(r.ToString());
        //        //print("@#$@#@$@#@#@#@");
        //        return r;
        //    }

        //    Path PathAdd(Path path)
        //    {
        //        pathList.Add(path);

        //        pathList.ForEach(p =>
        //        {
        //            print(p.section0.rect.ToString());
        //            print(p.section1.rect.ToString());
        //            print(p.section0.room.ToString());
        //            print(p.section1.room.ToString());
        //        });
        //        print("#################");
        //        return path;
        //    }

        //    // Update is called once per frame
        //    void Update()
        //    {

    }

    void CreateSection(Rect rect)
    {
        //section.Section = ;
        //DgDivision div = new DgDivision();
        //div.Outer.Set(left, top, right, bottom);
        sectionList.Add(new Section(rect));
    }

    bool CheckDivisionSize(int size)
    {
        // (最小の部屋サイズ + 余白)
        // 2分割なので x2 する
        // +1 して連絡通路用のサイズも残す
        int min = (MIN_ROOM + OUTER_MERGIN) * 2 + 1;

        return size >= min;
    }


    void SplitDivison(bool bVertical)
    {
        Section parent = sectionList[sectionList.Count - 1];
        sectionList.Remove(parent);

        // 子となる区画を生成
        Section child;

        if (bVertical)
        {
            // ▼縦方向に分割する
            if (CheckDivisionSize(parent.Bound.Height) == false)
            {
                // 縦の高さが足りない
                // 親区画を戻しておしまい
                sectionList.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Bound.Top + (MIN_ROOM + OUTER_MERGIN);
            int b = parent.Bound.Bottom - (MIN_ROOM + OUTER_MERGIN);
            // AB間の距離を求める
            int ab = b - a;
            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, MAX_ROOM);

            // 分割点を決める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            //child.Set(parent.Left, p, parent.Right, parent.Bottom);?
            child = new Section(new Rect(parent.Bound.Left, p, parent.Bound.Right, parent.Bound.Bottom));

            // 親の下側をp地点まで縮める
            parent.Bound.Bottom = child.Bound.Top;
        }
        else
        {
            // ▼横方向に分割する
            if (CheckDivisionSize(parent.Bound.Width) == false)
            {
                // 横幅が足りない
                // 親区画を戻しておしまい
                sectionList.Add(parent);
                return;
            }

            // 分割ポイントを求める
            int a = parent.Bound.Left + (MIN_ROOM + OUTER_MERGIN);
            int b = parent.Bound.Right - (MIN_ROOM + OUTER_MERGIN);
            // AB間の距離を求める
            int ab = b - a;
            // 最大の部屋サイズを超えないようにする
            ab = Mathf.Min(ab, MAX_ROOM);

            // 分割点を求める
            int p = a + Random.Range(0, ab + 1);

            // 子区画に情報を設定
            //child.Set(
            //    p, parent.Top, parent.Right, parent.Bottom);

            child = new Section(new Rect(p, parent.Bound.Top, parent.Bound.Right, parent.Bound.Bottom));

            // 親の右側をp地点まで縮める
            parent.Bound.Right = child.Bound.Left;
        }

        // 次に分割する区画をランダムで決める
        if (Random.Range(0, 2) == 0)
        {
            // 子を分割する
            sectionList.Add(parent);
            sectionList.Add(child);
        }
        else
        {
            // 親を分割する
            sectionList.Add(child);
            sectionList.Add(parent);
        }

        // 分割処理を再帰呼び出し (分割方向は縦横交互にする)
        SplitDivison(!bVertical);
    }

    void CreateRoom()
    {
        foreach (Section div in sectionList)
        {
            // 基準サイズを決める
            int dw = div.Bound.Width - OUTER_MERGIN;
            int dh = div.Bound.Height - OUTER_MERGIN;

            // 大きさをランダムに決める
            int sw = Random.Range(MIN_ROOM, dw);
            int sh = Random.Range(MIN_ROOM, dh);

            // 最大サイズを超えないようにする
            sw = Mathf.Min(sw, MAX_ROOM);
            sh = Mathf.Min(sh, MAX_ROOM);

            // 空きサイズを計算 (区画 - 部屋)
            int rw = (dw - sw);
            int rh = (dh - sh);

            // 部屋の左上位置を決める
            int rx = Random.Range(0, rw) + POS_MERGIN;
            int ry = Random.Range(0, rh) + POS_MERGIN;

            int left = div.Bound.Left + rx;
            int right = left + sw;
            int top = div.Bound.Top + ry;
            int bottom = top + sh;

            //// 部屋のサイズを設定
            //div.Room.Set(left, top, right, bottom);
            div.Room = new Rect(left, top, right, bottom);
            //// 部屋を通路にする
            FillDgRect(div.Room);
        }
    }

    void ConnectRooms()
    {
        for (int i = 0; i < sectionList.Count - 1; i++)
        {
            // リストの前後の区画は必ず接続できる
            Section a = sectionList[i];
            Section b = sectionList[i + 1];

            // 2つの部屋をつなぐ通路を作成
            CreateRoad(a, b);

            // 孫にも接続する
            for (int j = i + 2; j < sectionList.Count; j++)
            {
                Section c = sectionList[j];
                if (CreateRoad(a, c, true))
                {
                    // 孫に接続できたらおしまい
                    break;
                }
            }
        }
    }


    bool CreateRoad(Section divA, Section divB, bool bGrandChild = false)
    {
        if (divA.Bound.Bottom == divB.Bound.Top ||
            divA.Bound.Top == divB.Bound.Bottom)
        {
            // 上下でつながっている
            // 部屋から伸ばす通路の開始位置を決める
            int x1 = Random.Range(divA.Room.Left, divA.Room.Right);
            int x2 = Random.Range(divB.Room.Left, divB.Room.Right);
            int y = 0;

            if (bGrandChild)
            {
                // すでに通路が存在していたらその情報を使用する
                if (divA.Path != null) { x1 = divA.Path.Left; }
                if (divB.Path != null) { x2 = divB.Path.Left; }
            }

            if (divA.Bound.Top > divB.Bound.Top)
            {
                // B - A (Bが上側)
                y = divA.Bound.Top;
                // 通路を作成
                divA.Path = new Rect(x1, y + 1, x1 + 1, divA.Room.Top);
                divB.Path = new Rect(x2, divB.Room.Bottom, x2 + 1, y);
            }
            else
            {
                // A - B (Aが上側)
                y = divB.Bound.Top;
                // 通路を作成
                divA.Path = new Rect(x1, divA.Room.Bottom, x1 + 1, y);
                divB.Path = new Rect(x2, y, x2 + 1, divB.Room.Top);
            }
            FillDgRect(divA.Path);
            FillDgRect(divB.Path);

            // 通路同士を接続する
            FillHLine(x1, x2, y);

            // 通路を作れた
            return true;
        }

        if (divA.Bound.Left == divB.Bound.Right ||
            divA.Bound.Right == divB.Bound.Left)
        {
            // 左右でつながっている
            // 部屋から伸ばす通路の開始位置を決める
            int y1 = Random.Range(divA.Room.Top, divA.Room.Bottom);
            int y2 = Random.Range(divB.Room.Top, divB.Room.Bottom);
            int x = 0;

            if (bGrandChild)
            {
                // すでに通路が存在していたらその情報を使う
                if (divA.Path != null) { y1 = divA.Path.Top; }
                if (divB.Path != null) { y2 = divB.Path.Top; }
            }

            if (divA.Bound.Left > divB.Bound.Left)
            {
                // B - A (Bが左側)
                x = divA.Bound.Left;
                // 通路を作成
                divB.Path = new Rect(divB.Room.Right, y2, x, y2 + 1);
                divA.Path = new Rect(x + 1, y1, divA.Room.Left, y1 + 1);
            }
            else
            {
                // A - B (Aが左側)
                x = divB.Bound.Left;
                divA.Path = new Rect(divA.Room.Right, y1, x, y1 + 1);
                divB.Path = new Rect(x, y2, divB.Room.Left, y2 + 1);
            }
            FillDgRect(divA.Path);
            FillDgRect(divB.Path);

            // 通路同士を接続する
            FillVLine(y1, y2, x);

            // 通路を作れた
            return true;
        }


        // つなげなかった
        return false;
    }

    void FillHLine(int left, int right, int y)
    {
        if (left > right)
        {
            // 左右の位置関係が逆なので値をスワップする
            int tmp = left;
            left = right;
            right = tmp;
        }
        FillDgRect(new Rect(left, y, right + 1, y + 1));
        //_layer.FillRectLTRB(left, y, right + 1, y + 1, CHIP_NONE);
    }

    void FillVLine(int top, int bottom, int x)
    {
        if (top > bottom)
        {
            // 上下の位置関係が逆なので値をスワップする
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
                mapSection[i, j] = true;
            }
        }
    }
}

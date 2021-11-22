using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileSwap
{
    public int tileNum; //������ ������ ������
    public GameObject swapPrefab; //������ ����� ��� ��������
    public GameObject guaranteedltemDrop; //��� ������, �� ������� ������ ���� �������
    public int overrideTileNum = -1; //������ ������, ������� �������� ��������
}


public class TileCamera : MonoBehaviour
{
    static private int W, H;
    static private int[,] MAP;
    static public Sprite[] SPRITES;
    static public Transform TILE_ANCHOR;
    static public Tile[,] TILES;
    static public string COLLISIONS;

    [Header("Set in Inspector")]
    public TextAsset mapData;
    public Texture2D mapTiles;
    public TextAsset mapcollisions; // ������ �����-���� ��� �������� �� �����
    public Tile tilePrefab;

    //��� ������ ������ �� ������ � ��������
    [Header("������ ������ ��� ������ �� ���������")]
    public int defaultTileNum;
    [Header("������ ������ ������")]
    public List<TileSwap> tileSwaps;
    private Dictionary<int, TileSwap> tileSwapDict;
    private Transform enemyAnchor, itemAnchor;




    void Awake()
    {
        //��������� �������� ������� ������
        COLLISIONS = Utils.RemoveLineEndings(mapcollisions.text);

        PrepareTileSwapDict(); 
        enemyAnchor = (new GameObject("Enemy Anchor")).transform;
        itemAnchor = (new GameObject("Item Anchor")).transform;

        LoadMap();
    }

    public void LoadMap()
    {
        // ������� TILE_ANCHOR. �� ����� ������ ���� �������� ��� ���� ������ Tile.
        GameObject go = new GameObject("TILE_ANCHOR");
        TILE_ANCHOR = go.transform;
        // ��������� ��� ������� �� mapTiles
        SPRITES = Resources.LoadAll<Sprite>(mapTiles.name); // a
                                                            // ��������� ���������� ��� �����
        string[] lines = mapData.text.Split('\n'); // b
        H = lines.Length;
        string[] tileNums = lines[0].Split(' ');
        W = tileNums.Length;
        System.Globalization.NumberStyles hexNum; // c
        hexNum = System.Globalization.NumberStyles.HexNumber;
        // ��������� ���������� ��� ����� � ��������� ������ ��� ��������� �������
        MAP = new int[W, H];

        for (int j = 0; j < H; j++)
        {
            tileNums = lines[j].Split(' ');
            for (int i = 0; i < W; i++)
            {
                if (tileNums[i] == "..")
                {
                    MAP[i,j] = 0;
                }
                else
                {
                    MAP[i, j] = int.Parse(tileNums[i], hexNum); // d
                }

                CheckTileSwaps(i, j);
            }
        }

        print("Parsed " + SPRITES.Length + " sprites."); 
        print("Map size: " + W + " wide by " + H + " high");

        ShowMap();
    }

    //����������� ������ � �������
    void PrepareTileSwapDict()
    {
        tileSwapDict = new Dictionary<int, TileSwap>();
        foreach (TileSwap ts in tileSwaps)
        {
            tileSwapDict.Add(ts.tileNum, ts);
        }
    }

    void CheckTileSwaps(int i, int j)
    {
        int tNum = GET_MAP(i, j);
        if (!tileSwapDict.ContainsKey(tNum))  //���� ��� ������ � �������
            return;
        // �� ������ �������� ������
        TileSwap ts = tileSwapDict[tNum];
        if (ts.swapPrefab != null)
        {
            GameObject go = Instantiate(ts.swapPrefab);
            Enemy enemy = go.GetComponent<Enemy>();
            if (enemy != null)
            {
                go.transform.SetParent(enemyAnchor);
            }
            else
            {
                go.transform.SetParent(itemAnchor);
            }
            go.transform.position = new Vector3(i, j, 0);

            if (ts.guaranteedltemDrop != null)
            {
                if (enemy != null)
                {
                    enemy.guaranteedltemDrop = ts.guaranteedltemDrop;
                }
            }
        }

        // �������� ������ �������
        if (ts.overrideTileNum == -1)
        {
            SET_MAP(i, j, defaultTileNum);
        }
        else
        {
            SET_MAP(i, j, ts.overrideTileNum);
        }
    }


    ///<summary>
    /// ���������� ������ ����� ��� ���� �����.
    ///</summary>
    void ShowMap() {
        TILES = new Tile[W, H];
        // ����������� ��� ����� � ������� ������, ��� ����������
        for (int j = 0; j < H; j++)
        {
            for (int i = 0; i < W; i++)
            {
                if (MAP[i, j] != 0)
                {
                    Tile ti = Instantiate<Tile>(tilePrefab); // b
                    ti.transform.SetParent(TILE_ANCHOR);
                    ti.SetTile(i, j); // c
                    TILES[i, j] = ti;
                }
            }
        }
    }

    static public int GET_MAP(int x, int y)
    { 
        if (x < 0 || x >= W || y < 0 || y >= H)
        {
            return -1; // ������������� ���������� IndexOutOfRangeException
        }
        return MAP[x, y];
    }

    static public int GET_MAP(float x, float y)
    {
        int tX = Mathf.RoundToInt(x);
        int tY = Mathf.RoundToInt(y - 0.25f); // g
        return GET_MAP(tX, tY);
    }
    static public void SET_MAP(int x, int y, int tNum)
    {
        // ���� ����� ��������� �������������� ������ ��� ����� ��������,
        if (x < 0 || x >= W || y < 0 || y >= H)
        {
            return; // ������������� ���������� IndexOutOfRangeException
        }
        MAP[x,y] = tNum;

    }
}

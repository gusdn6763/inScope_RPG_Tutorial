using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform map = null;
    [SerializeField] private Sprite defultTile = null;
    [SerializeField] private Texture2D[] mapData = null;
    [SerializeField] private MapElement[] mapElements = null;
    [SerializeField] private MapAtlas[] mapAlements = null;
    private Dictionary<Point, GameObject> tiles = new Dictionary<Point, GameObject>();
    private List<Color> colors = new List<Color>();

    private Vector3 WorldStartPos
    {
        get { return Camera.main.ScreenToWorldPoint(new Vector3(0, 0)); }
    }

    // Use this for initialization
    void Start()
    {
        GenerateMap();
    }


    private void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;
        int count = 0;
        int atlascount = 0;

        MapElement newElement = null;

        for (int i = 0; i < mapData.Length; i++)
        {
            count = 0;
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y);
                    newElement = Array.Find(mapElements, find => find.Color == c);
                    if (newElement != null)
                    {
                        float xPos = WorldStartPos.x + (defultTile.bounds.size.x * x);
                        float yPos = WorldStartPos.y + (defultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.ElementPrefab);

                        if (newElement.SpriteAtlas)
                        {
                            tiles.Add(new Point(x, y), go);
                            colors.Add(c);
                            count = 1;
                        }

                        if (newElement.ObstacleObject)
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height * 2 - y * 2;
                        }
                        go.transform.position = new Vector2(xPos, yPos);
                        go.transform.parent = map;
                    }
                }
            }
            atlascount += count;
        }
        if (atlascount != mapAlements.Length)
        {
            Debug.LogError("LevelManager Error! AtlasCount is not right");
            Application.Quit();
        }
        CheckWater(newElement);
    }

    private string TileCheck(Point currentPoint)
    {
        string composition = string.Empty;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x != 0 || y != 0)
                {
                    if (tiles.ContainsKey(new Point(currentPoint.X + x, currentPoint.Y + y)))
                    {
                        composition += "W";
                    }

                    else
                    {
                        composition += "E";
                    }
                }
            }
        }
        return composition;
    }


    private void CheckWater(MapElement newElement)
    {
        for (int i = 0; i < mapAlements.Length; i++)
        {
            MapAtlas Mapatlas = Array.Find(mapAlements, find => find.Color == colors.Find(x => x == mapAlements[i].Color));

            foreach (KeyValuePair<Point, GameObject> tile in tiles)
            {
                string composition = TileCheck(tile.Key);
                SpriteRenderer tileSprite = tile.Value.GetComponent<SpriteRenderer>();

                if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("0");
                }
                if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("1");
                }
                if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("2");
                }
                if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("3");
                }
                if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("4");
                }
                if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("5");
                }
                if (composition[1] == 'W' && composition[4] == 'W' && composition[3] == 'E' && composition[6] == 'W')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("6");
                }
                if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("7");
                }
                if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'E')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("8");
                }
                if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("9");
                }
                if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'E' && composition[6] == 'W')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("10");
                }
                if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("11");
                }
                if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("12");
                }
                if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E')
                {
                    tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("13");
                }
                if (composition[3] == 'W' && composition[5] == 'E' && composition[6] == 'W')
                {
                    GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                    go.GetComponent<SpriteRenderer>().sprite = Mapatlas.SpriteAtlas.GetSprite("14");
                    go.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                if (composition[1] == 'W' && composition[2] == 'E' && composition[4] == 'W')
                {
                    GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                    go.GetComponent<SpriteRenderer>().sprite = Mapatlas.SpriteAtlas.GetSprite("15");
                    go.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                if (composition[4] == 'W' && composition[6] == 'W' && composition[7] == 'E')
                {
                    GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                    go.GetComponent<SpriteRenderer>().sprite = Mapatlas.SpriteAtlas.GetSprite("16");
                    go.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                if (composition[0] == 'E' && composition[1] == 'W' && composition[3] == 'W')
                {
                    GameObject go = Instantiate(tile.Value, tile.Value.transform.position, Quaternion.identity, map);
                    go.GetComponent<SpriteRenderer>().sprite = Mapatlas.SpriteAtlas.GetSprite("17");
                    go.GetComponent<SpriteRenderer>().sortingOrder = 1;
                }
                if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W')
                {
                    int randomChance = UnityEngine.Random.Range(0, 100);

                    if (randomChance < 15)
                    {
                        tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("19");
                    }
                }
                if (composition[1] == 'W' && composition[2] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[5] == 'W' & composition[6] == 'W')
                {
                    int randomChance = UnityEngine.Random.Range(0, 100);

                    if (randomChance < 10)
                    {
                        tileSprite.sprite = Mapatlas.SpriteAtlas.GetSprite("20");
                    }
                }
            }
        }
    }
}


[Serializable]
public class MapAtlas
{
    [SerializeField] private SpriteAtlas spriteAtlas;
    [SerializeField] private Color color;
    public Color Color
    {
        get
        {
            return color;
        }
    }
    public SpriteAtlas SpriteAtlas
    {
        get
        {
            return spriteAtlas;
        }
    }
}

[Serializable]
public class MapElement
{
    [SerializeField] private GameObject elementPrefab = null;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private bool obstacleObject = false;
    [SerializeField] private bool spriteAtlas = false;

    public GameObject ElementPrefab
    {
        get
        {
            return elementPrefab;
        }
    }

    public Color Color
    {
        get
        {
            return color;
        }
    }

    public bool ObstacleObject
    {
        get
        {
            return obstacleObject;
        }
    }

    public bool SpriteAtlas
    {
        get
        {
            return spriteAtlas;
        }
    }
}

public struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Transform map = null;
    [SerializeField] private Texture2D[] mapData = null;
    [SerializeField] private MapElement[] mapElements = null;
    [SerializeField] private Sprite defultTile = null;

    private Vector3 WorldStartPos
    {
        get
        {
            return Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
        }
    }

    // Use this for initialization
    void Start()
    {
        GenerateMap();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateMap()
    {
        int height = mapData[0].height;
        int width = mapData[0].width;

        for (int i = 0; i < mapData.Length; i++)
        {
            for (int x = 0; x < mapData[i].width; x++)
            {
                for (int y = 0; y < mapData[i].height; y++)
                {
                    Color c = mapData[i].GetPixel(x, y);

                    MapElement newElement = Array.Find(mapElements, e => e.Color == c);

                    if (newElement == null) Debug.Log(c.gamma);

                    if (newElement != null)
                    {
                        float xPos = WorldStartPos.x + (defultTile.bounds.size.x * x);
                        float yPos = WorldStartPos.y + (defultTile.bounds.size.y * y);

                        GameObject go = Instantiate(newElement.ElementPrefab);


                        if (newElement.ObstacleObject)
                        {
                            go.GetComponent<SpriteRenderer>().sortingOrder = height * 2 - y * 2;
                        }


                        go.transform.position = new Vector2(xPos, yPos);
                        go.transform.parent = map;

                    }
                }
            }
        }
    }
}

[Serializable]
public class MapElement
{
    [SerializeField] private GameObject elementPrefab = null;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private string tileTag = null;
    [SerializeField] private bool obstacleObject = false;

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

    public string TileTag
    {
        get
        {
            return tileTag;
        }
    }

    public bool ObstacleObject
    {
        get
        {
            return obstacleObject;
        }
    }
}
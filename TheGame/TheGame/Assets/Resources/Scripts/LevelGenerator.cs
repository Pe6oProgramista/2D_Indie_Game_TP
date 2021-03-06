﻿using System.IO;
using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    public Color color;
    public GameObject prefab;
}
public class LevelGenerator : MonoBehaviour {

    public SpriteRenderer background;
    public ColorToPrefab[] colorMappings;

    private Texture2D map;
    private Vector2 offset;

    void Start () {
        map = (Texture2D)Resources.Load("Sprites\\Levels\\Level" + ApplicationModel.level);

        offset = new Vector2(background.bounds.center.x - map.width / 2, background.bounds.center.y - map.height / 2);
        for (int x = 0; x < map.width; x++)
        {
            for(int y = 0; y < map.height; y++)
            {
                GeneratePixel(x, y);
            }
        }
	}

    public static Texture2D LoadPNG(string filePath)
    {

        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            Application.Quit();
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData);
        }
        return tex;
    }

    void GeneratePixel(int x, int y)
    {
        Color pixelColor = map.GetPixel(x, y);

        if(pixelColor.a == 0)
        {
            return;
        }

        foreach(ColorToPrefab colorMapping in colorMappings)
        {
            Vector3 pos = new Vector3(x + offset.x, y + offset.y, 0);

            if (pixelColor.Equals(new Color(0, 1, 0, 1)))
            {
                colorMapping.prefab.transform.position = pos;
            }
            else if (colorMapping.color.Equals(pixelColor))
            {
                Instantiate(colorMapping.prefab, pos, Quaternion.identity, transform);
            }
        }
    }
}


//Vector3 pos = new Vector3(offset.x + (x * colorMapping.prefab.GetComponent<SpriteRenderer>().bounds.size.x),
//offset.y + (y * colorMapping.prefab.GetComponent<SpriteRenderer>().bounds.size.y),
//(map.GetPixel(x, y + 1).Equals(pixelColor)) ? 0.1f : 0);
//pos.x /= colorMapping.prefab.transform.localScale.x;
//pos.y /= colorMapping.prefab.transform.localScale.y;

//public SpriteRenderer background;
//offset = new Vector2(background.bounds.center.x - background.bounds.size.x / 2, background.bounds.center.y - background.bounds.size.y / 2);
//if (pixelColor.Equals(new Color(0, 1, 0, 1)))
//{
//    colorMapping.prefab.transform.position = new Vector3(x - map.width / 2, y - map.height / 2, 0);
//}

//if (lastObj == null)
//{
//    pos = new Vector3(offset.x + x, offset.y + y, (map.GetPixel(x, y + 1).Equals(pixelColor)) ? 0.1f : 0);
//}
//else
//{
//    float x1 = 1 - (lastObj.GetComponent<SpriteRenderer>().bounds.size.x / 2 + colorMapping.prefab.GetComponent<SpriteRenderer>().bounds.size.x / 2);
//    float y1 = 1 - (lastObj.GetComponent<SpriteRenderer>().bounds.size.y / 2 + colorMapping.prefab.GetComponent<SpriteRenderer>().bounds.size.y / 2);
//    pos = new Vector3(offset.x + x - x1, offset.y + y - y1, (map.GetPixel(x, y + 1).Equals(pixelColor)) ? 0.1f : 0);
//}

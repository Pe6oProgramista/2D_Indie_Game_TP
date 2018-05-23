using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorToPrefab
{
    public Color color;
    public GameObject prefab;
}
public class LevelGenerator : MonoBehaviour {

    public Texture2D map;
    public ColorToPrefab[] colorMappings;

	void Start () {
		for(int x = 0; x < map.width; x++)
        {
            for(int y = 0; y < map.height; y++)
            {
                GeneratePixel(x, y);
            }
        }
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
            if(colorMapping.color.Equals(pixelColor))
            {
                Vector3 pos = new Vector3((x - map.width / 2) / 2.29f, (y - map.height / 2) / 2.29f, (map.GetPixel(x, y + 1).Equals(pixelColor)) ? 0.1f : 0);
                Instantiate(colorMapping.prefab, pos, Quaternion.identity, transform);
            }
        }
    }
}

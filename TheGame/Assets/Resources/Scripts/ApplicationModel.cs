using System.Collections.Generic;
using UnityEngine;

public class ApplicationModel {
    static public string authenticationToken = "";

    static public bool fpsOn = false;
    static public int targetFrameRate = 60;

    static public Texture2D map = (Texture2D) Resources.Load("Sprites\\Levels\\Level" + 1);
    static public List<int> sceneIndexes = new List<int>();
}

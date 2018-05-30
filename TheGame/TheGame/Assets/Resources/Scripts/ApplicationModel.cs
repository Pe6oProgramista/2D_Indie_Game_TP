using System.Collections.Generic;
using UnityEngine;

public class ApplicationModel {
	static public string URL = "https://grapplinghook-game-server.herokuapp.com/";
    static public string authenticationToken = "";

    static public bool fpsOn = false;
    static public int targetFrameRate = 60;

    static public int level = 1;
    static public List<int> sceneIndexes = new List<int>();

    static public int leaderboardLevl;
}

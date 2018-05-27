using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LevelsButtonsManager : MonoBehaviour {

    public Color normalTextColor;
    public Color disabledTextColor;

    public Sprite normalSprite;
    public Sprite highlightedSprite;

    public ButtonOptions canvas;

    private List<FileInfo> files;

    void Start () {
        this.files = new List<FileInfo>();
        FileInfo[] files = new DirectoryInfo("Assets\\Resources\\Sprites\\Levels").GetFiles();
        foreach(FileInfo file in files)
        {
            if(file.Extension.Contains("png") || file.Extension.Contains("jpg"))
            {
                this.files.Add(file);
            }
        }
        GenerateButtons(this.files.Count);
	}

    private void GenerateButtons(int levelsCount)
    {
        for(int i = 0; i < levelsCount; i++)
        {
            Vector3 position = new Vector3(70 * i, 0, 0);
            GameObject button = Instantiate((GameObject) Resources.Load("preFab\\Level Button"), transform);


            RectTransform rt = button.GetComponent<RectTransform>();
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 45 + i * 70, rt.rect.width);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 35, rt.rect.height);

            button.name = "Level " + (i + 1) + " Button";

            button.GetComponentInChildren<Text>().text = (i + 1).ToString("00");

            string levelNumber = (i + 1).ToString();

            button.GetComponent<Button>().onClick.AddListener(delegate { GenerateButtonLevel(levelNumber); });
            button.GetComponent<Button>().onClick.AddListener(delegate { canvas.OnSelect(6); });
        }
    }

    public void GenerateButtonLevel(string levelNumber)
    {
        ApplicationModel.map = (Texture2D)Resources.Load("Sprites\\Levels\\Level" + levelNumber);
    }
}

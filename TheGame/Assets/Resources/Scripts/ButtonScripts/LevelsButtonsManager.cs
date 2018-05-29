﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class LevelsButtonsManager : MonoBehaviour {

    public Color normalTextColor;
    public Color disabledTextColor;

    public Sprite normalSprite;
    public Sprite highlightedSprite;

    public ButtonOptions canvas;
    public GameObject levelButton;
    public GameObject arrowButton;

    private int page = 0;
    private int levelsCount = 0;
    private Vector2 panelSize;
    private float pages;
    private GameObject backArrow;
    private GameObject nextArrow;

    void Start () {

        if (SceneManager.GetActiveScene().name.Equals("Leaderboard"))
        {
            canvas.transform.GetChild(3).gameObject.SetActive(false);
        }
        panelSize = GetComponent<RectTransform>().rect.size;

        FileInfo[] files = new DirectoryInfo("Assets\\Resources\\Sprites\\Levels").GetFiles();
        foreach(FileInfo file in files)
        {
            if(file.Extension.Contains("png") || file.Extension.Contains("jpg"))
            {
                levelsCount++;
            }
        }
        pages = (levelsCount % 16 == 0) ? levelsCount / 16 : levelsCount / 16 + 1;
        GenerateArrows();
        GenerateButtons();
    }

    private void GenerateButtons()
    {
        Vector2 buttonSize = levelButton.GetComponent<RectTransform>().rect.size;

        float xSpacing = ((panelSize.x - 90) - (buttonSize.x * 4)) / 3 + buttonSize.x;
        float ySpacing = ((panelSize.y - 70) - (buttonSize.y * 4)) / 3 + buttonSize.y;

        int statement = (page == pages - 1) ? (page * 16) + (levelsCount - (page * 16)) : (page + 1) * 16;

        for (int i = page * 16; i < statement; i++)
        {
            GameObject button = Instantiate(levelButton, transform);

            RectTransform rt = button.GetComponent<RectTransform>();

            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 45 + ((i + 4) % 4) * xSpacing, rt.rect.width);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 35 + (((i / 4) + 4) % 4) * ySpacing, rt.rect.height);

            button.name = "Level " + (i + 1) + " Button";

            button.GetComponentInChildren<Text>().text = (i + 1).ToString("00");

            int levelNumber = i + 1;

            if (SceneManager.GetActiveScene().name.Equals("Level Selection"))
            {
                button.GetComponent<Button>().onClick.AddListener(delegate { GenerateLevel(levelNumber); });
                button.GetComponent<Button>().onClick.AddListener(delegate { canvas.OnSelect(6); });
            }
            else
            {
                button.GetComponent<Button>().onClick.AddListener(delegate { GenerateLevelLeaderboard(levelNumber); });
                button.GetComponent<Button>().onClick.AddListener(delegate { canvas.OnSelect(8); });
            }
        }
    }

    private void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            if (!child.name.Contains("Arrow"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    private void GenerateArrows()
    {
        backArrow = Instantiate(arrowButton, transform);
        nextArrow = Instantiate(arrowButton, transform);
        Vector3 scale = nextArrow.GetComponent<RectTransform>().localScale;
        scale.x *= -1;
        nextArrow.GetComponent<RectTransform>().localScale = scale;

        RectTransform rt = backArrow.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(-20, -panelSize.y / 2 + 15, 0);
        rt = nextArrow.GetComponent<RectTransform>();
        rt.localPosition = new Vector3(20, -panelSize.y / 2 + 15, 0);

        backArrow.GetComponentInChildren<Button>().onClick.AddListener(delegate { ChangePage(page - 1); });
        nextArrow.GetComponentInChildren<Button>().onClick.AddListener(delegate { ChangePage(page + 1); });

        backArrow.GetComponentInChildren<Button>().interactable = false;
        nextArrow.GetComponentInChildren<Button>().interactable = (pages < 2) ? false : true;
    }

    public void GenerateLevel(int levelNumber)
    {
        // TODO: Sending request for relevant level file
        ApplicationModel.map = (Texture2D)Resources.Load("Sprites\\Levels\\Level" + levelNumber);
    }

    public void GenerateLevelLeaderboard(int levelNumber)
    {
        ApplicationModel.leaderboardLevl = levelNumber;
    }

    public void ChangePage(int pageNumber)
    {
        page = pageNumber;
        if (page == 0)
        {
            backArrow.GetComponentInChildren<Button>().interactable = false;
            nextArrow.GetComponentInChildren<Button>().interactable = true;
        }
        else if (page > 0)
        {
            backArrow.GetComponentInChildren<Button>().interactable = true;
            nextArrow.GetComponentInChildren<Button>().interactable = (page == pages - 1) ? false : true;
        }
        DestroyAllChildren();
        GenerateButtons();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LeaderboardGenerator : MonoBehaviour {

    public GameObject field;
    public GameObject arrowButton;

    private int page = 0;
    private int scoresCount = 0;
    private Vector2 panelSize;
    private float pages;
    private GameObject backArrow;
    private GameObject nextArrow;

    private string[] usernames;
    private string[] highScores;

    void Start () {
        panelSize = GetComponent<RectTransform>().rect.size;

        transform.GetChild(0).GetComponent<Text>().text = "Level" + ApplicationModel.leaderboardLevl;

        // Set scoreCount from request
        scoresCount = 10;
        usernames = new string[10];
        highScores = new string[10];
        for(int i = 0; i < scoresCount; i++)
        {
            usernames[i] = i.ToString();
            highScores[i] = (i + 1).ToString();
        }

        // 5 on page
        // 40 distance  190 -90
        pages = (scoresCount % 5 == 0) ? scoresCount / 5 : scoresCount / 5 + 1;
        GenerateArrows();
        GenerateLeaderboard();
    }

    private void GenerateLeaderboard()
    {
        int statement = (page == pages - 1) ? (page * 5) + (scoresCount - (page * 5)) : (page + 1) * 5;

        for (int i = page * 5; i < statement; i++)
        {
            GameObject field = Instantiate(this.field, transform);

            field.name = "Field " + (i + 1);

            field.transform.GetChild(0).GetComponent<Text>().text = usernames[i];
            field.transform.GetChild(1).GetComponent<Text>().text = highScores[i];

            RectTransform rt = field.GetComponent<RectTransform>();

            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, (panelSize.x - rt.rect.width) / 2, rt.rect.width);
            rt.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 90 + ((i + 5) % 5) * 40, rt.rect.height);
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

    private void DestroyAllChildren()
    {
        foreach (Transform child in transform)
        {
            if (!child.name.Contains("Header") && !child.name.Contains("Level Text") && !child.name.Contains("Arrow"))
            {
                Destroy(child.gameObject);
            }
        }
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
        GenerateLeaderboard();
    }
}

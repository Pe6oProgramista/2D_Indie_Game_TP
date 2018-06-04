using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Linq;

public class LeaderboardGenerator : MonoBehaviour {

    public GameObject field;
    public GameObject arrowButton;

    private int page = 0;
    private int scoresCount = 0;
    private Vector2 panelSize;
    private float pages;
    private GameObject backArrow;
    private GameObject nextArrow;

    private Dictionary<string, string> userScore;

    private string URL;

    void Start () {
        URL = ApplicationModel.URL + ApplicationModel.authenticationToken + "/leaderboard/" + ApplicationModel.leaderboardLevl;

        panelSize = GetComponent<RectTransform>().rect.size;

        transform.GetChild(0).GetComponent<Text>().text = "Level" + ApplicationModel.leaderboardLevl;

        userScore = new Dictionary<string, string>();

        Action();
    }

    private void GenerateLeaderboard()
    {
        int statement = (page >= pages - 1) ? ((page * 5) + (scoresCount - (page * 5))) : ((page + 1) * 5);

        for (int i = page * 5; i < statement; i++)
        {
            GameObject field = Instantiate(this.field, transform);

            field.name = "Field " + (i + 1);

            field.transform.GetChild(0).GetComponent<Text>().text = userScore.Keys.ElementAt(i);
            field.transform.GetChild(1).GetComponent<Text>().text = userScore.Values.ElementAt(i);

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

    void Action()
    {
        WWW www;
        www = new WWW(URL);
        StartCoroutine(WaitForRequest(www));
    }

    private IEnumerator WaitForRequest(WWW data)
    {
        Debug.Log("adadasd" + ApplicationModel.leaderboardLevl);
        yield return data;
        if (data.error != null)
        {
            Debug.Log("There was an error sending request: " + data.error);
        }
        else
        {
            string[] result = data.text.Split(' ');
            if (result[0].Contains("Success"))
            {
                scoresCount = 0;
                if (result[1].Contains(".."))
                {
                    string[] resultData = result[1].Split(new string[] { "..." }, StringSplitOptions.None);
                    foreach (string field in resultData)
                    {
                        string[] f = field.Split(new string[] { ".." }, StringSplitOptions.None);
                        userScore.Add(f[0], f[1]);
                    }
                    scoresCount = userScore.Count;
                    Debug.Log("ScoresCount: " + scoresCount + "  AND  " + userScore.Count);
                }

                pages = (scoresCount % 5 == 0) ? scoresCount / 5 : scoresCount / 5 + 1;
                GenerateArrows();
                GenerateLeaderboard();
            }
            else
            {
                Debug.Log(result[1]);
            }
        }
    }
}

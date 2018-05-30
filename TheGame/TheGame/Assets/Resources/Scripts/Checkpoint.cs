using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour {

    private static readonly string URL = "https://grapplinghook-game-server.herokuapp.com/" + ApplicationModel.authenticationToken + "/leaderboards/" + ApplicationModel.level;

    private Text time;

    void Start () {
        time = GameObject.Find("Timer/Text").GetComponent<Text>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        Action();
    }

    void Action()
    {
        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");
        WWWForm form = new WWWForm();
        form.AddField("data", time.text);
        www = new WWW(URL, form);
        StartCoroutine(WaitForRequest(www));
    }

    private IEnumerator WaitForRequest(WWW data)
    {
        yield return data;
        if (data.error != null)
        {
            Debug.Log("There was an error sending request: " + data.error);
        }
        else
        {
            string[] result = data.text.Split(',');
            if (result[0].Contains("Success"))
            {
                Debug.Log("Successful!");
            }
            else
            {
                Debug.Log(result[1]);
            }
        }
    }
}

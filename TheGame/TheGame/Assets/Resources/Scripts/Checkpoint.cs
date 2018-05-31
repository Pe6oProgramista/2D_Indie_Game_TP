using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour {

    private string URL;

    private Text time;
    private bool IsTriggered = false;

    void Start () {
        URL = ApplicationModel.URL + ApplicationModel.authenticationToken + "/leaderboard/" + ApplicationModel.level;
        time = GameObject.Find("Timer/Text").GetComponent<Text>();
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsTriggered)
        {
            IsTriggered = true;
            Debug.Log(ApplicationModel.authenticationToken);
            Debug.Log("Triggered");
            Action();
        }
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
        Camera.main.gameObject.GetComponent<ButtonOptions>().Back();
    }
}

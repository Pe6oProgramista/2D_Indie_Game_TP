using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    public InputField IUsername;
    public InputField IPassword;

    private string Username = "";
    private string Password = "";

    private string URL;

    void Start()
    {
        URL = ApplicationModel.URL + "login";
        GetComponent<Button>().onClick.AddListener(delegate { Action(); });
    }

    private void Update()
    {
        Username = IUsername.text;
        Password = IPassword.text;
    }

    void Action()
    {
        if (Username == "" || Password == "")
        {
            Debug.Log("Fill all");
            return;
        }

        WWW www;
        WWWForm form = new WWWForm();
        form.AddField("username", Username);
        form.AddField("password", Password);
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
                ApplicationModel.authenticationToken = result[1].Trim().Substring(1, result[1].Length - 3);
                GameObject.Find("Login Fields").GetComponent<ButtonOptions>().OnSelect(4);
                Debug.Log(ApplicationModel.authenticationToken);
            }
            else
            {
                Debug.Log(result[1]);
            }
        }
    }
}

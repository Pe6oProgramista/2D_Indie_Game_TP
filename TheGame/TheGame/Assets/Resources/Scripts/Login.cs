using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        string hashedPassword = Hash(Password);

        string jsonString = "{\"username\":\"" + Username + "\"," +
                            "\"password\":\"" + hashedPassword + "\"}";

        WWW www;
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "application/json");
        WWWForm form = new WWWForm();
        form.AddField("data", jsonString);
        www = new WWW(URL, form);
        StartCoroutine(WaitForRequest(www));
    }

    static string Hash(string input)
    {
        var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
        return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
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

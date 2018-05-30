using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Registration : MonoBehaviour {

    public InputField IUsername;
    public InputField IEmail;
    public InputField IPassword;
    public InputField IConfPassword;

    private string Username = "";
    private string Email = "";
    private string Password = "";
    private string ConfPassword = "";

    private static readonly string URL = "https://grapplinghook-game-server.herokuapp.com/register";

    void Start () {
        GetComponent<Button>().onClick.AddListener(delegate { Action(); });
    }

    private void Update()
    {
        Username = IUsername.text;
        Email = IEmail.text;
        Password = IPassword.text;
        ConfPassword = IConfPassword.text;
    }

    void Action()
    {
        //Fill in all fields
        if(Username == "" || Email == "" || Password == "" || ConfPassword == "")
        {
            Debug.Log("Fill all");
            return;
        }
        else if(!Password.Equals(ConfPassword))
        {
            Debug.Log("Passwords not match");
            return;
        }
        else if(!IsValidEmail(Email))
        {
            Debug.Log("Not valid email");
            return;
        }

        string hashedPassword = Hash(Password);

        string jsonString = "{\"username\":\"" + Username + "\"," +
                            "\"email\":\"" + Email + "\"," +
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

    public bool IsValidEmail(string email)
    {
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        if (match.Success)
        {
            return true;
        }
        return false;
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
            Debug.Log(result[0]);
            if(result[0].Contains("Success"))
            {
                GameObject.Find("Registration Fields").GetComponent<ButtonOptions>().OnSelect(1);
            }
            else
            {
                Debug.Log(result[1]);
            }
        }
    }
}

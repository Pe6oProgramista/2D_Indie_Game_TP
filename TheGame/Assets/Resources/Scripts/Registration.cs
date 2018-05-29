using System.Collections;
using System.Collections.Generic;
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

        Debug.Log(Username + " " + Email + " " + Password + " " + ConfPassword);

    }
}

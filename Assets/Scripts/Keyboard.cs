using System;
using UnityEngine;
using System.Collections;
using System.Text;
//using System.Diagnostics;
using MessageLibrary;
using UnityEngine.UI;
using System.Collections.Generic;

public class Keyboard : MonoBehaviour
{
    // Start is called before the first frame update
    string str;
    public InputField mainInputField;
    public SimpleWebBrowser.WebBrowser browser;

    void Start()
    {
       // browser = GetComponent<SimpleWebBrowser.WebBrowser>();
        str = "";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void key(Text s)
    {
        str += s.text;
        Debug.Log(str);
        setText();
    }

    public void space()
    {
        str += " ";
        setText();
    }

    public void backspace()
    {
        if (str.Length > 1)
        {
            str = str.Substring(0, str.Length - 1);
        }
        else if (str.Length == 1)
        {
            str = "";
        }
        setText();
        Debug.Log(str);
    }

    public void enter()
    {
        browser.OnNavigate();
        mainInputField.text = str.ToLower();
        Debug.Log("Enter");
    }

    void setText()
    {
        Debug.Log("Pozvan setText");
        mainInputField.text = str.ToLower();
    }
}

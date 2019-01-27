using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldSet : MonoBehaviour
{
    static string str = "";
    // Start is called before the first frame update
    public InputField mainInputField;

    public void setText(string s)
    {
        mainInputField.text = s;
    }


}

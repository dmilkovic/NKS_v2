using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class Logger : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string user;
    //true kontroler vive, false body tracking
    [SerializeField] private bool mode = false;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnApplicationQuit()
    {
        WriteString();
    }

    [MenuItem("Tools/Write file")]
    private void WriteString()
    {
        string path;
        if (mode)
        {
            path = "Assets/test_vive.txt";
        }
        else
        {
            path = "Assets/test_body_tracking.txt";
        }

        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(user + "\t" + Time.time);
        writer.Close();

        //Re-import the file to update the reference in the editor
        AssetDatabase.ImportAsset(path);
        if (mode)
        {
            TextAsset asset = (TextAsset)Resources.Load("test_vive.txt");
        }
        else
        {
            TextAsset asset = (TextAsset)Resources.Load("test_body_tracking.txt");
        }
    }

}
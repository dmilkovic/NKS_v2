using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleWebBrowser;

public class HandTrackerJSQueryHandler : MonoBehaviour
{
    public WebBrowserHandTracker MainBrowser;

    void Start()
    {
        MainBrowser.OnJSQuery += MainBrowser_OnJSQuery;
    }

    private void MainBrowser_OnJSQuery(string query)
    {
        Debug.Log("Javascript query:" + query);
        MainBrowser.RespondToJSQuery("My response: OK");
    }
}

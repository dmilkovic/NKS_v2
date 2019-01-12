using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;


public class SteamActionsTest : MonoBehaviour
{
    public SteamVR_Input_Sources handType; // 1
    public SteamVR_Action_Boolean teleportAction; // 2
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetTrigger())
        {
            Debug.Log(handType);
        }
    }

    public bool GetTrigger()
    {
        return teleportAction.GetStateDown(handType);
    }
}

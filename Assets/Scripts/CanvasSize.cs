using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasSize : MonoBehaviour {
    private static RectTransform canvasSize;
	// Use this for initialization
	void Start () {
        canvasSize = gameObject.GetComponent<RectTransform>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public  static RectTransform getCanvasSize()
    {
        return canvasSize;
    }
}

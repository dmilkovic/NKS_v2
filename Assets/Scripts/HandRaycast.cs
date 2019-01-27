using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandRaycast : MonoBehaviour
{
    RaycastHit hit;
    bool init = false; 

    // Start is called before the first frame update


    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

        //if (teleportAction.GetState(handType))

        if (Physics.Raycast(transform.position, transform.forward, out hit))
        {
            // Vector3 forward = teleportReticleTransform.TransformDirection(Vector3.forward) * 10;
            if (/*hit.collider.tag == "BackButton" ||*/ hit.collider.tag == "keyboard")
            {
                Button b = hit.collider.gameObject.GetComponent<Button>();
                if(b != null)
                {
                    b.Select();
                }
            }

            init = true;
        }
      
        Debug.DrawRay(transform.position, Vector3.forward * 1000, Color.green);
   
    }

    private void sendMessage(RaycastHit hit, bool flag)
    {
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x = (1 - pixelUV.x) * 1920;
        pixelUV.y *= 1080;

        //string s = pixelUV.x + "|" + pixelUV.y;
        ArrayList arr = new ArrayList();
        arr.Add(pixelUV);
        arr.Add(flag);
        hit.transform.SendMessage("RaycastUV", arr);
        //Debug.Log(handType);

    }

    public RaycastHit getHit()
    {
        return hit;
    }

    public bool checkRay()
    {
        if (init) return true;
        else return false;
    }
  
}

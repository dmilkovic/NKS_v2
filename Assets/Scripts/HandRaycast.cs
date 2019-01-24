using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HandRaycast : MonoBehaviour
{
    public GameObject laserPrefab; // 1
    private GameObject laser; // 2
    private Transform laserTransform; // 3
    private Vector3 hitPoint; // 4
    public Camera cam;
    public RectTransform baseRect;
    // 1
    public Transform cameraRigTransform;
    // 2
    public GameObject teleportReticlePrefab;
    // 3
    private GameObject reticle;
    // 4
    private Transform teleportReticleTransform;
    // 5
    public Transform headTransform;
    // 6
    public Vector3 teleportReticleOffset;
   
    public bool click = false;
    public string name = "sefs";
    RaycastHit hit;

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

        }

        Debug.Log("click " + click);

        if (click)
        {
            Debug.Log("pozvan");
            sendMessage(hit, true);
            sendMessage(hit, false);
        }
        Debug.DrawRay(transform.position, Vector3.forward * 1000, Color.green);
        //Debug.DrawRay(transform.position + transform.forward * 3 + transform.up * -1, Vector3.forward * 1000, Color.green);
        /* if (Physics.Raycast(baseRect.transform.position, transform.forward, out hit))
         {
             hitPoint = hit.point;
             ShowLaser(hit);
             // 1
             reticle.SetActive(true);
             // 2
             teleportReticleTransform.position = hitPoint + teleportReticleOffset;
             Vector3 forward = teleportReticleTransform.TransformDirection(Vector3.forward) * 10;
             Debug.DrawRay(transform.position, forward, Color.green);

         }





         /*}
          else // 3
          {
              laser.SetActive(false);
          }*/

    }


    private void ShowLaser(RaycastHit hit)
    {
        // 1
        laser.SetActive(true);
        // 2
        laserTransform.position = Vector3.Lerp(baseRect.transform.position, hitPoint, .5f);
        // 3
        laserTransform.LookAt(hitPoint);
        // 4
        laserTransform.localScale = new Vector3(laserTransform.localScale.x,
                                                laserTransform.localScale.y,
                                                hit.distance);

        /* Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(laserTransform.position);
         int X = (int)System.Math.Round(pointOnScreenPosition.x);
         int Y = (int)System.Math.Round(pointOnScreenPosition.y);
         MouseOperations.SetCursorPosition(X, Y);
         MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);*/

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

  
}

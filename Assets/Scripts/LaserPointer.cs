﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Valve.VR;

public class LaserPointer : MonoBehaviour
{
    public SteamVR_Input_Sources handType;
    public SteamVR_Behaviour_Pose controllerPose;
    public SteamVR_Action_Boolean teleportAction;
    public GameObject laserPrefab; // 1
    private GameObject laser; // 2
    private Transform laserTransform; // 3
    private Vector3 hitPoint; // 4
    public Camera cam;

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

    bool scrollActive = false;


    // Start is called before the first frame update


    void Start()
    {
        laser = Instantiate(laserPrefab);
        // 2
        laserTransform = laser.transform;
        // 1
        reticle = Instantiate(teleportReticlePrefab);
        // 2
        teleportReticleTransform = reticle.transform;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //if (teleportAction.GetState(handType))
        // {
        RaycastHit hit;
        Button previous = null;

        if (Physics.Raycast(controllerPose.transform.position, transform.forward, out hit))
        {
            hitPoint = hit.point;
            ShowLaser(hit);
            // 1
            reticle.SetActive(true);
            // 2
            teleportReticleTransform.position = hitPoint + teleportReticleOffset;
            Vector3 forward = teleportReticleTransform.TransformDirection(Vector3.forward) * 10;
            Debug.DrawRay(transform.position, forward, Color.green);

            if (hit.collider.tag == "BackButton" || hit.collider.tag == "keyboard" || hit.collider.tag == "Scroll")
            {
                Button b = hit.collider.gameObject.GetComponent<Button>();
                b.Select();
            }
            else
            {
                EventSystem.current.GetComponent<EventSystem>().SetSelectedGameObject(null);
            }
        }

        if (GetTriggerDown())
        {

            if (hit.collider.tag == "BackButton" || hit.collider.tag == "keyboard" || hit.collider.tag == "Scroll")
            {

                IPointerClickHandler clickHandler = hit.collider.gameObject.GetComponent<IPointerClickHandler>();
               // IPointerEnterHandler
                //do what ever
                if (clickHandler != null)
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    clickHandler.OnPointerClick(pointerEventData);
                    Debug.Log("backButton");
                }

                if (hit.collider.tag == "Scroll")
                {
                    scrollActive = true;
                }
             
               // sendMessage(hit, true, "back");
            }
            else
            { 
                sendMessage(hit, true, "click");
            }
        }

        if (GetTriggerUp())
        {
            if (hit.collider.tag == "BackButton" || hit.collider.tag == "keyboard" || hit.collider.tag == "Scroll")
            {
                if (hit.collider.tag == "Scroll")
                {
                    scrollActive = false;
                }
            }
            else {
                sendMessage(hit, false, "click");
            }
        }

        if (scrollActive)
        {
            if (hit.collider.tag == "Scroll")
            {
                IPointerClickHandler clickHandler = hit.collider.gameObject.GetComponent<IPointerClickHandler>();
                if (clickHandler != null)
                {
                    PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                    clickHandler.OnPointerClick(pointerEventData);
                }
            }
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
        laserTransform.position = Vector3.Lerp(controllerPose.transform.position, hitPoint, .5f);
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

    private void sendMessage(RaycastHit hit, bool flag, string tag)
    {

        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x = (1 - pixelUV.x) * 1920;
        pixelUV.y *= 1080;

        //string s = pixelUV.x + "|" + pixelUV.y;
        ArrayList arr = new ArrayList();
        arr.Add(pixelUV);
        arr.Add(flag);
        hit.transform.SendMessage("RaycastUV", arr);
        Debug.Log(handType);


    }

    public bool GetTriggerUp()
    {
        return teleportAction.GetStateUp(handType);
        //return teleportAction.GetStateDown(handType);
    }

    public bool GetTriggerDown()
    {
        return teleportAction.GetStateDown(handType);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerWeb_2 : MonoBehaviour
{
    public enum Hands { left = 0, right = 1 };
    [SerializeField]
    Hands currentHand;

    [Header("Visualization")]
    [SerializeField]
    RectTransform baseRect;

    [SerializeField]
    Image background;

    [SerializeField]
    Sprite defaultSprite;

    [SerializeField]
    Sprite pressSprite;

    bool active = false;
    bool press = false;
    bool pressActive = false;

    [SerializeField] GameObject[] buttons;

    [Header("Raycasting")]

    [SerializeField]
    public Camera cam;

    ImageItem selectedButton;

    PointerEventData eventData = new PointerEventData(null);
    List<RaycastResult> raycastResults = new List<RaycastResult>();

    [SerializeField]
    float dragSensitivity = 5f;
    private int handX, handY;

    public GameObject cube;
    HandRaycast raycast;
    
    private void Start()
    {
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;
        dragSensitivity *= dragSensitivity;

        raycast = cube.GetComponent<HandRaycast>();
        Debug.Log(cube.name);
       // raycast = GetComponentInChildren<HandRaycast>();
        // Debug.Log("Screen: " + Screen.width + "  " + Screen.height + "Canvas:" + CanvasSize.getCanvasSize().rect.width + "  " + CanvasSize.getCanvasSize().rect.height);
    }

    private void OnDestroy()
    {
        NuitrackManager.onHandsTrackerUpdate -= NuitrackManager_onHandsTrackerUpdate;
    }

    private void Update()
    {

        if (raycast.checkRay())
        {

            if (press && !pressActive)
            {
                pressActive = true;
                Debug.Log("+");
                //MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);*/
                //      raycast.click = true;
                Debug.Log(raycast.getHit().collider.tag);

                if (raycast.getHit().collider.tag == "Browser")
                {
                    sendMessage(raycast.getHit(), true);

                }
                else if (raycast.getHit().collider.tag == "BackButton" || raycast.getHit().collider.tag == "keyboard" || raycast.getHit().collider.tag == "Scroll")
                {
                    // clickHandler = raycast.getHit().collider.gameObject.GetComponent<IPointerClickHandler>();
                    Debug.Log("BackButton");
                    IPointerClickHandler clickHandler = raycast.getHit().collider.gameObject.GetComponent<IPointerClickHandler>();
                    if (clickHandler != null)
                    {
                        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                        clickHandler.OnPointerClick(pointerEventData);
                    }
                    Debug.Log("backButton");
                }
                else if (raycast.getHit().collider.tag == "Finish")
                {
                    IPointerClickHandler clickHandler = raycast.getHit().collider.gameObject.GetComponent<IPointerClickHandler>();
                    if (clickHandler != null)
                    {
                        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                        clickHandler.OnPointerClick(pointerEventData);
                    }
                    Debug.Log("Finish");
                }
               
                //  sendMessage(raycast.getHit(), true);
                // StartCoroutine(Example());
            }
            else if (!press && pressActive)
            {

                pressActive = false;
                Debug.Log("-");


                if (raycast.getHit().collider.tag == "Browser")
                {
                    sendMessage(raycast.getHit(), false);
                }
                else
                //if (raycast.getHit().collider.tag == "BackButton")
                {
                    Debug.Log("f");
                }

                //raycast.click = false;
                /*MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);*/
                //sendMessage(hit, false);
            }
            else if (press && pressActive)
            {
                if (raycast.getHit().collider.tag == "Scroll")
                {
                    IPointerClickHandler clickHandler = raycast.getHit().collider.gameObject.GetComponent<IPointerClickHandler>();
                    if (clickHandler != null)
                    {
                        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                        clickHandler.OnPointerClick(pointerEventData);
                    }
                }
            }
        }

    }

    private void NuitrackManager_onHandsTrackerUpdate(nuitrack.HandTrackerData handTrackerData)
    {
        raycast = cube.GetComponent<HandRaycast>();
        active = false;
        press = false;

        if (handTrackerData != null)
        {
            nuitrack.UserHands userHands = handTrackerData.GetUserHandsByID(CurrentUserTracker.CurrentUser);

            if (userHands != null)
            {
                //float rightHandX = userHands.RightHand.Value.X * 0.5f;
                if (currentHand == Hands.right && userHands.RightHand != null)
                {
                    //   baseRect.anchoredPosition = new Vector2(userHands.RightHand.Value.X * Screen.width, -userHands.RightHand.Value.Y * Screen.height);
                    baseRect.anchoredPosition = new Vector2(userHands.RightHand.Value.X * CanvasSize.getCanvasSize().rect.width, -userHands.RightHand.Value.Y * CanvasSize.getCanvasSize().rect.height);
                  //  Debug.Log("X: " + userHands.RightHand.Value.X + "Width" + CanvasSize.getCanvasSize().rect.width + "rez: " + userHands.RightHand.Value.X * CanvasSize.getCanvasSize().rect.width);
                    Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(baseRect.position);
                    handX = (int)System.Math.Round(pointOnScreenPosition.x);
                    handY = (int)System.Math.Round(pointOnScreenPosition.y);

                    active = true;
                    press = userHands.RightHand.Value.Click;
                }
                else if (currentHand == Hands.left && userHands.LeftHand != null)
                {

                    //baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * CanvasSize.getCanvasSize().rect.width, -userHands.LeftHand.Value.Y * CanvasSize.getCanvasSize().rect.height);
                    Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(baseRect.position);
                    handX = (int)System.Math.Round(pointOnScreenPosition.x);
                    //handY = (int)System.Math.Round(userHands.LeftHand.Value.Y * CanvasSize.getCanvasSize().rect.height);
                    handY = (int)System.Math.Round(pointOnScreenPosition.y);

                    active = true;
                    press = userHands.LeftHand.Value.Click;
                }
            }

        }
      
        background.enabled = active;
        background.sprite = active && press ? pressSprite : defaultSprite;
    }

    IEnumerator Example()
    {
        Debug.Log(Time.time);
        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
        yield return new WaitForSecondsRealtime(1);
        pressActive = false;
        Debug.Log(Time.time);
    }


    private void sendMessage(RaycastHit hit, bool flag)
    {
        Vector2 pixelUV = hit.textureCoord;
        pixelUV.x = (1 - pixelUV.x) * 1920;
        pixelUV.y *= 1080;

        Debug.Log(pixelUV.x + "  " + pixelUV.y);
        //string s = pixelUV.x + "|" + pixelUV.y;
        ArrayList arr = new ArrayList();
        arr.Add(pixelUV);
        arr.Add(flag);
        hit.transform.SendMessage("RaycastUV", arr);
      //  Debug.Log(handType);*/
    }


}


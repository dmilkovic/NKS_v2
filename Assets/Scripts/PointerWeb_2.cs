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

    public GameObject laserPrefab; // 1
    private GameObject laser; // 2
    private Transform laserTransform; // 3
    private Vector3 hitPoint; // 4

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

    private void Start()
    {
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;
        dragSensitivity *= dragSensitivity;

        laser = Instantiate(laserPrefab);
        // 2
        laserTransform = laser.transform;
        // 1
        reticle = Instantiate(teleportReticlePrefab);
        // 2
        teleportReticleTransform = reticle.transform;
        // Debug.Log("Screen: " + Screen.width + "  " + Screen.height + "Canvas:" + CanvasSize.getCanvasSize().rect.width + "  " + CanvasSize.getCanvasSize().rect.height);
    }

    private void OnDestroy()
    {
        NuitrackManager.onHandsTrackerUpdate -= NuitrackManager_onHandsTrackerUpdate;
    }

    private void NuitrackManager_onHandsTrackerUpdate(nuitrack.HandTrackerData handTrackerData)
    {
        active = false;
        press = false;
        RaycastHit hit;


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
                    //MouseOperations.SetCursorPosition(handX + 140, (int)Screen.height + 100 - handY);
                   // MouseOperations.SetCursorPosition(handX, (int)Screen.height - handY);
                    //baseRect.anchoredPosition = new Vector2(userHands.RightHand.Value.X * CanvasSize.getCanvasSize().rect.width, -userHands.RightHand.Value.Y * CanvasSize.getCanvasSize().rect.height);
                    // Debug.Log(userHands.RightHand.Value.X * Screen.width + "   " + -userHands.RightHand.Value.Y * Screen.height + "INT: " + (int)System.Math.Round(userHands.RightHand.Value.X * Screen.width) + "   " + (int)System.Math.Round(- userHands.RightHand.Value.Y * Screen.height));
                    //handX = (int)System.Math.Round(userHands.RightHand.Value.X);
                    //handY = (int)System.Math.Round(userHands.RightHand.Value.Y);
                    //baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    active = true;
                    press = userHands.RightHand.Value.Click;
                }
                else if (currentHand == Hands.left && userHands.LeftHand != null)
                {

                    //baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * CanvasSize.getCanvasSize().rect.width, -userHands.LeftHand.Value.Y * CanvasSize.getCanvasSize().rect.height);
                    Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(baseRect.position);
                    //  handX = (int)System.Math.Round(userHands.LeftHand.Value.X * Screen.width);
                    //  handY = (int)System.Math.Round(userHands.LeftHand.Value.Y * Screen.height);
                    //handX = (int)System.Math.Round(userHands.LeftHand.Value.X * CanvasSize.getCanvasSize().rect.width);
                    handX = (int)System.Math.Round(pointOnScreenPosition.x);
                    //handY = (int)System.Math.Round(userHands.LeftHand.Value.Y * CanvasSize.getCanvasSize().rect.height);
                    handY = (int)System.Math.Round(pointOnScreenPosition.y);
                    //MouseOperations.SetCursorPosition(handX + 140, (int)Screen.height + 100 - handY);
                    //MouseOperations.SetCursorPosition(handX, (int)Screen.height - handY);
                    //Debug.Log("Base Y: " + baseRect.anchoredPosition.y + "Mouse Y: " + handY);
                    active = true;
                    press = userHands.LeftHand.Value.Click;
                }
            }

        }
        Ray landingRay = new Ray(transform.position, Vector3.forward);
        Debug.DrawRay(transform.position, Vector3.forward*1000, Color.green);

        if (Physics.Raycast(baseRect.transform.position, transform.forward, out hit))
        {
            hitPoint = hit.point;
        }

        background.enabled = active;
        background.sprite = active && press ? pressSprite : defaultSprite;

        if (press && !pressActive)
        {
            /*pressActive = true;
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);*/
            sendMessage(hit, true);
            // StartCoroutine(Example());
        }
        else if (!press)
        {
            pressActive = false;
            /*MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);*/
            //sendMessage(hit, false);
        }

    }

    IEnumerator Example()
    {
        Debug.Log(Time.time);
        MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
        yield return new WaitForSecondsRealtime(1);
        pressActive = false;
        Debug.Log(Time.time);
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
       // hit.transform.SendMessage("RaycastUV", arr);
      //  Debug.Log(handType);*/
    }
}


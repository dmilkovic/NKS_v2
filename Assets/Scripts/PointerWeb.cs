using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PointerWeb : MonoBehaviour
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
    Camera cam;

    ImageItem selectedButton;

    PointerEventData eventData = new PointerEventData(null);
    List<RaycastResult> raycastResults = new List<RaycastResult>();

    [SerializeField]
    float dragSensitivity = 5f;
    private int handX, handY;
    private void Start()
    {
        NuitrackManager.onHandsTrackerUpdate += NuitrackManager_onHandsTrackerUpdate;
        dragSensitivity *= dragSensitivity;

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

        if (handTrackerData != null)
        {
            nuitrack.UserHands userHands = handTrackerData.GetUserHandsByID(CurrentUserTracker.CurrentUser);

            if (userHands != null)
            {
                if (currentHand == Hands.right && userHands.RightHand != null)
                {
                    baseRect.anchoredPosition = new Vector2(userHands.RightHand.Value.X * Screen.width, -userHands.RightHand.Value.Y * Screen.height);
                    Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(baseRect.position);
                    handX = (int)System.Math.Round(pointOnScreenPosition.x);
                    handY = (int)System.Math.Round(pointOnScreenPosition.y);
                    //MouseOperations.SetCursorPosition(handX + 140, (int)Screen.height + 100 - handY);
                    MouseOperations.SetCursorPosition(handX, (int)Screen.height - handY);
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

                    baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * Screen.width, -userHands.LeftHand.Value.Y * Screen.height);
                    //  baseRect.anchoredPosition = new Vector2(userHands.LeftHand.Value.X * CanvasSize.getCanvasSize().rect.width, -userHands.LeftHand.Value.Y * CanvasSize.getCanvasSize().rect.height);
                    Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(baseRect.position);
                    //  handX = (int)System.Math.Round(userHands.LeftHand.Value.X * Screen.width);
                    //  handY = (int)System.Math.Round(userHands.LeftHand.Value.Y * Screen.height);
                    //handX = (int)System.Math.Round(userHands.LeftHand.Value.X * CanvasSize.getCanvasSize().rect.width);
                    handX = (int)System.Math.Round(pointOnScreenPosition.x);
                    //handY = (int)System.Math.Round(userHands.LeftHand.Value.Y * CanvasSize.getCanvasSize().rect.height);
                    handY = (int)System.Math.Round(pointOnScreenPosition.y);
                    //MouseOperations.SetCursorPosition(handX + 140, (int)Screen.height + 100 - handY);
                    MouseOperations.SetCursorPosition(handX, (int)Screen.height - handY);
                    //Debug.Log("Base Y: " + baseRect.anchoredPosition.y + "Mouse Y: " + handY);
                    active = true;
                    press = userHands.LeftHand.Value.Click;
                }
            }
        }

        background.enabled = active;
        background.sprite = active && press ? pressSprite : defaultSprite;

        if (press && !pressActive)
        {
            pressActive = true;
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            // StartCoroutine(Example());
        }
        else if (!press)
        {
            pressActive = false;
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
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



}

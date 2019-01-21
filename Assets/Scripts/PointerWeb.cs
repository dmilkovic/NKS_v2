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
                    MouseOperations.SetCursorPosition(handX + 140, (int)Screen.height + 100 - handY);
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
                    MouseOperations.SetCursorPosition(handX + 140, (int)Screen.height + 100 - handY);
                    //Debug.Log("Base Y: " + baseRect.anchoredPosition.y + "Mouse Y: " + handY);
                    active = true;
                    press = userHands.LeftHand.Value.Click;
                }
            }
        }

        background.enabled = active;
        background.sprite = active && press ? pressSprite : defaultSprite;

      /*  if (Input.inputString != "")
        {
            int buttonIndex;
            int.TryParse(Input.inputString, out buttonIndex);
            buttonIndex -= 1;
            if(buttonIndex >= 0 && buttonIndex < buttons.Length)
            {
                ExecuteEvents.Execute<IPointerClickHandler>(buttons[buttonIndex], new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            }
            Debug.Log("Press");
        }*/
        if (press && !pressActive)
        {
            /*   if (Input.GetKeyDown(KeyCode.Space))*/
            //MouseOperations.SetCursorPosition(handX, handY);

            // Debug.Log("Press" + handX + "     "+ handY + "mouse: " + MouseOperations.GetCursorPosition().X + "  " + MouseOperations.GetCursorPosition().Y);
            //Debug.Log("Size of rect: " + (int)System.Math.Round(baseRect.sizeDelta.x) + "   " + (int)System.Math.Round(baseRect.sizeDelta.y));
            //MouseOperations.SetCursorPosition(handX-55, handY+100);

            //MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp | MouseOperations.MouseEventFlags.LeftDown);
            pressActive = true;
            StartCoroutine(Example());
            
            //  var pointer = new PointerEventData(EventSystem.current); // pointer event for Execute
            //   ExecuteEvents.Execute<IPointerClickHandler>(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
            //  Debug.Log("Press");
        }
        /*  if (!active)
              return;

          Vector2 pointOnScreenPosition = cam.WorldToScreenPoint(transform.position);
          eventData.delta = pointOnScreenPosition - eventData.position;
          eventData.position = pointOnScreenPosition;

          raycastResults.Clear();
          EventSystem.current.RaycastAll(eventData, raycastResults);

          ImageItem newButton = null;

          for (int i = 0; i < raycastResults.Count && newButton == null; i++)
              newButton = raycastResults[i].gameObject.GetComponent<ImageItem>();

          if (newButton != selectedButton)
          {
              if (selectedButton != null)
                  selectedButton.OnPointerExit(eventData);

              selectedButton = newButton;

              if (selectedButton != null)
                  selectedButton.OnPointerEnter(eventData);
          }
          else if (selectedButton != null)
          {
              if (press)
              {
                  if (eventData.delta.sqrMagnitude < dragSensitivity && !eventData.dragging)
                  {
                      eventData.dragging = true;
                      selectedButton.OnPointerDown(eventData);
                  }
              }
              else if (eventData.dragging)
              {
                  eventData.dragging = false;
                  selectedButton.OnPointerUp(eventData);
              }

              if (press)
                  selectedButton.OnDrag(eventData);
          }*/
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

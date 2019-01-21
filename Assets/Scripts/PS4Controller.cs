using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS4Controller : MonoBehaviour
{
    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    public static bool keyboardUp = false;
    MouseOperations.MousePoint lastPoint;
    // Start is called before the first frame update

    VirtualKeyboard vk = new VirtualKeyboard();

    void Start()
    {
        MouseOperations.MousePoint lastPoint = MouseOperations.GetCursorPosition();
    }

    // Update is called once per frame
    void Update()
    {
        MouseOperations.MousePoint m = MouseOperations.GetCursorPosition();
        if (m.X != lastPoint.X || m.Y != lastPoint.Y)
        {
            float translation = m.X + Input.GetAxis("Mouse X") * speed;
            float rotation = m.Y + Input.GetAxis("Mouse Y") * rotationSpeed;
            MouseOperations.SetCursorPosition((int)translation, (int)rotation);
        }

        if (Input.GetMouseButtonDown(0))
            Debug.Log("Pressed primary button.");

        if (Input.GetMouseButtonDown(1))
            Debug.Log("Pressed secondary button.");

        if (Input.GetMouseButtonDown(2))
            Debug.Log("Pressed middle click.");

        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("Fire1");
            //vk.ShowTouchKeyboard();
        }

        if (Input.GetButtonDown("Submit"))
        {
            Debug.Log("Submit");
            //vk.ShowTouchKeyboard();
        }


        if (Input.GetButtonDown("Fire2"))
        {
            if (!keyboardUp)
            {
                vk.ShowTouchKeyboard();
                keyboardUp = true;
            }
            else
            {
                vk.HideTouchKeyboard();
                keyboardUp = false;
            }

            Debug.Log("Fire2");

        }



    }
}

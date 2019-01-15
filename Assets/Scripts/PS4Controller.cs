using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PS4Controller : MonoBehaviour
{
    public float speed = 10.0F;
    public float rotationSpeed = 100.0F;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseOperations.MousePoint m = MouseOperations.GetCursorPosition();
        Debug.Log(m.X + m.Y);
        float translation = m.X + Input.GetAxis("Mouse X") * speed;
        float rotation = m.Y + Input.GetAxis("Mouse Y") * rotationSpeed;

        MouseOperations.SetCursorPosition((int)translation, (int)rotation);
    }
}

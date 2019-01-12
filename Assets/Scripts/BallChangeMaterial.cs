using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallChangeMaterial : MonoBehaviour
{
    public Color controllerRayColor;
    public Renderer objectRenderer;
    protected Color originalColor;
    protected Material objectMaterial;

  /*  void Awake()
    {
        //Get object's material from the renderer, and the original color
        objectMaterial = objectRenderer.material;
        originalColor = objectMaterial.color;
    }

    public override void RayEnter(RaycastHit hit)
    {
        Color c;
        //if (controller == null)
      //  {
            //Use color designated for head
            c = headRayColor;
     /*   }
        else
        {
            //Triggered by Hands
        }*/
    //}

   /* public override void RayExit()
    {
        objectMaterial.color = originalColor;
    }*/
}

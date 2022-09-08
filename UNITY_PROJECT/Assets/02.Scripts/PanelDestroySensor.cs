using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDestroySensor : MonoBehaviour
{
    public void OnHoverEntered_LEFT()
    {
        Debug.Log($"{gameObject.name} - OnHoverEntered_LEFT");
        GameManager.instance.isSensorLeft = true;
    }

    public void OnHoverExited_LEFT()
    {
        Debug.Log($"{gameObject.name} - OnHoverExited_LEFT");
        GameManager.instance.isSensorLeft = false;
    }


    public void OnHoverEntered_RIGHT()
    {
        Debug.Log($"{gameObject.name} - OnHoverEntered_RIGHT");
        GameManager.instance.isSensorRight = true;
    }

    public void OnHoverExited_RIGHT()
    {
        Debug.Log($"{gameObject.name} - OnHoverExited_RIGHT");
        GameManager.instance.isSensorRight = false;
    }
}

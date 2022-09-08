using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PanelInstanceSensor : MonoBehaviour
{
    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "TRIGGER PANEL")
        {
            GameManager.instance.isSensor = true;
        }
    }

    private void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "TRIGGER PANEL")
        {
            GameManager.instance.isSensor = false;
        }
    }
}

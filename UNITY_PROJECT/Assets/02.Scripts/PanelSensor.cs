using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSensor : MonoBehaviour
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "TRIGGER PANEL")
        {
            Destroy(c.gameObject);
        }
    }
}

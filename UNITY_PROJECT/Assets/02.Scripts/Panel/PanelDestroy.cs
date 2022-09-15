using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelDestroy : MonoBehaviour
{
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "QUIZ")
        {
            Destroy(c.gameObject);
            PanelManager.instance.lastIndex--;
        }

        if (c.gameObject.tag == "BLOCK")
        {
            Destroy(c.gameObject);
            PanelManager.instance.lastIndex--;
        }

        if (c.gameObject.tag == "MOTION")
        {
            Destroy(c.gameObject);
            PanelManager.instance.lastIndex--;
        }
    }
}
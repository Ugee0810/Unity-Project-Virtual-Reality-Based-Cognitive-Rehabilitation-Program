using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSensorHandRight : MonoBehaviour
{
    public static PanelSensorHandRight panelRight;

    public int rightscore = 0;

    public void OnTriggerEnter(Collider right)
    {
        if (right.gameObject.tag == "PANEL RIGHT")
        {
            rightscore++;
        }
    }
}

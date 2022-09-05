using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelSensorHands : MonoBehaviour
{
    public int leftscore = 0;
    public int rightscore = 0;

    public void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "PANEL LEFT")
        {
            leftscore++;
        }

        if (c.gameObject.tag == "PANEL RIGHT")
        {
            rightscore++;
        }

        if (leftscore == 1 && rightscore == 1)
        {
            Destroy(c.gameObject);
        }
    }

    private void OnCollisionExit(Collision c)
    {
        if (c.gameObject.tag == "PANEL LEFT")
        {
            leftscore = 0;
        }

        if (c.gameObject.tag == "PANEL RIGHT")
        {
            rightscore = 0;
        }
    }
}

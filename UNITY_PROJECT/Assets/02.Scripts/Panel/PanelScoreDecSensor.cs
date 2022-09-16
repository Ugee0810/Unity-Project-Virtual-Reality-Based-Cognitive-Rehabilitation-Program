using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelScoreDecSensor : MonoBehaviour
{
    int dec = -100;
    public void OnHoverEnterd_ScoreDec()
    {
        GameManager.instance.score -= dec;
    }
}

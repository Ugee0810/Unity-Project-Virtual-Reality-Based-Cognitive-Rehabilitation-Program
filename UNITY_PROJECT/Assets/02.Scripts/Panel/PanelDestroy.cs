using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelDestroy : MonoBehaviour
{
    public UnityEvent _SFX;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "QUIZ")
        {
            Destroy(c.gameObject);
            GameManager.instance.panelLastIndex--;
            _SFX?.Invoke();
        }

        if (c.gameObject.tag == "BLOCK")
        {
            Destroy(c.gameObject);
            GameManager.instance.panelLastIndex--;
        }

        if (c.gameObject.tag == "MOTION")
        {
            Destroy(c.gameObject);
            GameManager.instance.panelLastIndex--;
            _SFX?.Invoke();
        }
    }
}
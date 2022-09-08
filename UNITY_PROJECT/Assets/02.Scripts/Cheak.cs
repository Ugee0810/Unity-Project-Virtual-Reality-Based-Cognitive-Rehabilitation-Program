using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheak : MonoBehaviour
{
    public void CheakDes()
    {
        if (GameManager.instance.isSensorLeft && GameManager.instance.isSensorRight)
        {
            Destroy(gameObject.transform.parent.transform);
            Debug.Log($"{gameObject.transform.parent.transform.name} - ªË¡¶");
        }
    }
}

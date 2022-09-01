/// <summary>
/// PanelCtrl.cs
/// 
/// ＃역할
/// 패널 프리팹들에게 적용받고 각 이벤트 별 동작을 제어합니다.
/// 
/// ＃스크립팅 기술
/// 코루틴
/// 오브젝트 풀링
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PanelCtrl : MonoBehaviour
{
    float timer = 0;

    private void Update()
    {
        Move();

        timer += Time.deltaTime;

        if (timer == 5)
        {
            //Quiz();

            timer -= 5;
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.collider.tag == "HAND LEFT" && c.collider.tag == "HAND RIGHT")
        {
            Destroy(gameObject);
        }
    }

    void Move()
    {
        transform.position += Time.deltaTime * transform.forward * -2;
    }
}

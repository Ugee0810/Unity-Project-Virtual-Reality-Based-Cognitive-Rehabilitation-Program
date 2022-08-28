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

public class PanelCtrl : MonoBehaviour
{
    private void Update()
    {
        Move();
    }


    void Move()
    {
        transform.position += Time.deltaTime * transform.forward * -2;
    }


    void Stop()
    {

    }

    void Quiz()
    {

    }
}

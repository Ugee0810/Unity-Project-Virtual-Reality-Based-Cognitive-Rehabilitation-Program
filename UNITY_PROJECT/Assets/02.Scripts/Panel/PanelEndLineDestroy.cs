/// <summary>
/// PanelEndLineDestroy.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// - 플레이어 뒤로 지나간 노트들을 삭제해줍니다.
/// - panelLastIndex를 감산하여 퀴즈 패널의 생성 로직을 유지합니다.
/// - _SFX 유니티 이벤트로 트리거 됐을 때 효과음 오디오 소스를 플레이하는 함수를 지정합니다.
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Events;

public class PanelEndLineDestroy : MonoBehaviour
{
    public UnityEvent _SFX;

    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "QUIZ")
        {
            Destroy(c.gameObject);
            GameManager.instance.panelLastIndex--;
            _SFX?.Invoke();
            ComboManager.instance.Clear();
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
            ComboManager.instance.Clear();
        }
    }
}
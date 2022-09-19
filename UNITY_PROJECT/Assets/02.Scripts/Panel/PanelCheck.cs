/// <summary>
/// PanelCheck.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// - panelLastIndex를 감산하여 퀴즈 패널의 생성 로직을 유지합니다.
/// - _SFX 유니티 이벤트로 트리거 됐을 때 효과음 오디오 소스를 플레이하는 함수를 지정합니다.
/// </summary>

using UnityEngine;
using UnityEngine.Events;

public class PanelCheck : MonoBehaviour
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

        if (c.gameObject.tag == "MOTION")
        {
            Destroy(c.gameObject);
            GameManager.instance.panelLastIndex--;
            _SFX?.Invoke();
        }
    }
}
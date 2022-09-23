/// <summary>
/// MotionPanelCheck.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// - PanelCheck 게임 오브젝트가 PanelCheck()에 의해 Enable 됐을 때 스코어와 칼로리를 올리고, PanelCheck()의 PanelCheck 게임오브젝트를 Enable 해주는 조건에서 탈출합니다.
/// - _SFX 유니티 이벤트로 트리거 됐을 때 효과음 오디오 소스를 플레이하는 함수를 지정합니다.
/// </summary>

using UnityEngine;
using UnityEngine.Events;

public class MotionPanelCheck : MonoBehaviour
{
    public UnityEvent _SFX;

    private void OnEnable()
    {
        _SFX?.Invoke();
        StartCoroutine(ScoreManager.instance.Increase());
        Destroy(PanelManager.instance.panelSpawnPoint.transform.GetChild(0).gameObject);
    }
}
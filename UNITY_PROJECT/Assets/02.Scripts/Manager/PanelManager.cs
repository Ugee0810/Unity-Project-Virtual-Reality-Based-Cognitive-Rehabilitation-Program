/// <summary>
/// PanelManager.cs
/// 
/// ＃역할
/// PanelCtrl을 상속합니다.
/// 
/// ＃스크립팅 기술
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    [Header("[Manager Scripts]")]
    GameManager  _GameManager;
    UIManager    _UIManager;
    MusicManager _MusicManager;
    PanelManager _PanelManager;


    [Header("[Prefab Scripts]")]
    UIElements _UIElements;
    PanelCtrl  _PanelCtrl;


    [Header("[패널 프리팹]")]
    public PanelCtrl[] Panels;
    public Transform panelSpawnPoint; // 패널 생성 위치


    private void Update()
    {
        if (_GameManager.isStart) // 게임이 시작되면
        {
            if (_MusicManager.timer > _MusicManager.beat)
            {
                Instantiate(Panels[UnityEngine.Random.Range(0, 16)], panelSpawnPoint);
                _MusicManager.timer -= _MusicManager.beat;
            }

            _MusicManager.timer += Time.deltaTime;
        }
        else if (_GameManager.isStart && _GameManager.isStop)
        {
            // 일시정지 UI 출력

            // 패널/시간 정지
        }
        else
        {

            // 타이머, 패널 초기화
        }
        // 경과 시간이 종료되면 결과 출력
    }
}

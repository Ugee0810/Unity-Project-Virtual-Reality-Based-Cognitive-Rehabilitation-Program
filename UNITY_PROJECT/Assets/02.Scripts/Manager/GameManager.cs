/// <summary>
/// GameManager.cs
/// 
/// ＃역할
/// 게임에서 발생하는 이벤트(버튼, 플래그)를 처리 합니다.
/// PanelManager에게 해당된 레벨에 따른 패턴을 지시합니다.
/// 
/// ＃스크립팅 기술
/// 
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("[Manager Scripts]")]
    GameManager  _GameManager;
    UIManager    _UIManager;
    MusicManager _MusicManager;
    PanelManager _PanelManager;


    [Header("[Prefab Scripts]")]
    UIElements _UIElements;
    PanelCtrl  _PanelCtrl;


    [Header("[Environment GameObj]")]
    public GameObject inGameEnvironment;


    [Header("[플래그 변수]")]
    [HideInInspector] public bool isStart;       // Music Start
    [HideInInspector] public bool isStop;        // Music Pause

    [HideInInspector] public bool isOriginal;    // [Button] Original MusicList Selected
    [HideInInspector] public bool isCustom;      // [Button] Custom MusicList Selected

    [HideInInspector] public bool isLevelEasy;   // [Button] Level Easy Selected
    [HideInInspector] public bool isLevelNormal; // [Button] Level Normal Selected
    [HideInInspector] public bool isLevelHard;   // [Button] Level Hard Selected


    private void Awake()
    {
        isOriginal = true;
    }


    // [Button] Original MusicList Selected
    public void Original_Selected()
    {
        if (!isOriginal && isCustom)
        {
            _UIManager.originalScrollView.SetActive(true);
            _UIManager.customScrollView.SetActive(false);
            StartCoroutine(_UIManager.Original_List_Renewal());
        }
    }

    // [Button] Custom MusicList Selected
    public void Custom_Selected()
    {
        if (isOriginal && !isCustom)
        {
            _UIManager.originalScrollView.SetActive(false);
            _UIManager.customScrollView.SetActive(true);
            StartCoroutine(_UIManager.Custom_List_Renewal());
        }
    }



    // [Button] Easy(쉬움)
    public void Easy_Selected()
    {

    }

    // [Button] Normal(보통)
    public void Normal_Selected()
    {

    }

    // [Button] Hard(어려움)
    public void Hard_Selected()
    {

    }



    // [Button] Play(노래 재생)
    public void InGame_Start()
    {
        isStart = true;

        inGameEnvironment.SetActive(true);

        _UIManager.ui_Music_Select.SetActive(false);
        _UIManager.ui_Music_Start.SetActive(true);

        _MusicManager.vizualizationObjects.SetActive(true);

        _MusicManager.Play_Music(); // 음악 재생

        // ＃Music Start UI(Kcal, Score 등)
        // ＃오리진 콘트롤러 변경
    }

    // [Button] Pause(일시 정지)
    public void InGame_Pause()
    {
        isStop = true;

        _UIManager.ui_Music_Paused.SetActive(true); // Music Paused UI

        _MusicManager.Pause_Music(); // 노래 정지

        // ＃패널 정지
    }

    // End(게임 종료)
    public void InGame_End()
    {
        inGameEnvironment.SetActive(false);

        // ＃결과 UI
    }
}
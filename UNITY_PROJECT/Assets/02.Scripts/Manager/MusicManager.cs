/// <summary>
/// MusicManager.cs
/// 
/// ＃역할
/// 
/// ＃스크립팅 기술
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [Header("[Manager Scripts]")]
    GameManager  _GameManager;
    UIManager    _UIManager;
    MusicManager _MusicManager;
    PanelManager _PanelManager;


    [Header("[Prefab Scripts]")]
    UIElements _UIElements;
    PanelCtrl  _PanelCtrl;


    [Header("[SFX]")]
    public GameObject vizualizationObjects;

    [Header("[Music Start]")]
    [SerializeField] AudioSource selectedMusic;  // 선택된 노래
    public float timer;                          // BPM 계산 타이머
    public float beat;                           // BPM


    public void Play_Music()
    {

    }

    public void Pause_Music()
    {
        selectedMusic.Pause();
    }

    public void Stop_Music()
    {

    }
}
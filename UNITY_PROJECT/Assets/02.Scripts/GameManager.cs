// 프리팹 하위 인스턴스 조회
//customMusicElements.transform.GetChild(0).gameObject.GetComponent<Image>();
//customMusicElements.transform.GetChild(1).gameObject.GetComponent<Text>();
//customMusicElements.transform.GetChild(2).gameObject.GetComponent<Text>();
//customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>();

/// <summary>
/// GameManager.cs
/// 
/// ＃역할
/// 게임에서 발생하는 이벤트(버튼, 플래그)를 처리 합니다.
/// PanelManager에게 해당된 레벨에 따른 패턴을 지시합니다.
/// 오리지널 또는 커스텀 노래 조회 버튼을 눌렀을 때 라이브러리 내 음악을 조회 후 각 정보들을 Element들에게 전달합니다.
/// 
/// ＃스크립팅 기술
/// 
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("[UI]")]
    public GameObject uiMusicSelect;      // Lobby UI
    public GameObject uiMusicStart;       // Ingame UI
    public GameObject uiMusicPaused;      // Pause UI

    public GameObject originalScrollView; // Original View UI
    public GameObject customScrollView;   // Custom View UI

    public GameObject contentOriginal;    // 오리지널 리소스 프리팹 생성 위치(부모)
    public GameObject contentCustom;      // 커스텀   리소스 프리팹 생성 위치(부모)

    [Header("[환경 오브젝트]")]
    public GameObject inGameEnvironment;

    [Header("[SFX]")]
    public GameObject vizualizationObjects;

    [Header("[패널 프리팹]")]
    public Transform panelSpawnPoint; // 패널 생성 좌표
    public GameObject[] Panels;       // 패널 프리팹 배열

    [Header("[플래그 변수]")]
    /*[HideInInspector]*/ public bool isStart;       // Music Start
    /*[HideInInspector]*/ public bool isStop;        // Music Pause

    /*[HideInInspector]*/ public bool isOriginal;    // [Button] Original MusicList Selected
    /*[HideInInspector]*/ public bool isCustom;      // [Button] Custom MusicList Selected

    /*[HideInInspector]*/ public bool isLevelEasy;   // [Button] Level Easy Selected
    /*[HideInInspector]*/ public bool isLevelNormal; // [Button] Level Normal Selected
    /*[HideInInspector]*/ public bool isLevelHard;   // [Button] Level Hard Selected

    [Header("[Prefabs]")]
    public GameObject musicElement; // Instantiate될 프리팹

    [Header("[Music Info]")]
    public AudioSource musicBackGround; // BGM
    public AudioSource musicSelected;   // 선택된 노래
    public AudioSource musicPlayed;     // 플레이 할 노래
    public float timer;                 // BPM 계산 타이머
    public float beat;                  // BPM

    private void Awake()
    {
        isOriginal = true;
    }

    private void FixedUpdate()
    {
        if (isStart)
        {
            PanelInstance();
        }
    }

    public void PanelInstance()
    {
        if (timer > beat)
        {
            Instantiate(Panels[Random.Range(0, 16)], panelSpawnPoint);

            timer -= beat; // Timer = Timer - Beat
        }

        timer += Time.deltaTime; // Timer
    }


    // [Button] Original MusicList Selected
    public void BtnOriginalSelected()
    {
        if (!isOriginal && isCustom)
        {
            originalScrollView.SetActive(true);
            customScrollView.SetActive(false);
            StartCoroutine(OriginalListRenewal());
        }
    }

    // [Button] Custom MusicList Selected
    public void BtnCustomSelected()
    {
        if (isOriginal && !isCustom)
        {
            originalScrollView.SetActive(false);
            customScrollView.SetActive(true);
            StartCoroutine(CustomListRenewal());
        }
    }


    // [Button] Easy
    public void BtnLvEasy()
    {

    }

    // [Button] Normal
    public void BtnLvNormal()
    {

    }

    // [Button] Hard
    public void BtnLvHard()
    {

    }


    // [Button] Play
    public void BtnInGameStart()
    {
        uiMusicSelect.SetActive(false);       // Lobby UI Off
        uiMusicStart.SetActive(true);         // Ingame UI On
        vizualizationObjects.SetActive(true); // VizualizationObj On
        inGameEnvironment.SetActive(true);    // 인게임 환경 요소 On
        isStart = true;
        MusicPlay(); // 플레이 음악 재생

        // ＃Music Start UI(Kcal, Score 등)
        // ＃오리진 콘트롤러 변경
    }

    // [Button] Pause
    public void BtnInGamePause()
    {
        uiMusicPaused.SetActive(true); // Music Paused UI
        isStop = true;
        MusicPause(); // 플레이 중 노래 일시 정지

        // ＃패널 정지
    }

    // End
    public void InGameEnd()
    {
        inGameEnvironment.SetActive(false); // Ingame Env Obj Off
        MusicStop(); // Played Song Reset

        // ＃결과 UI
    }


    public void MusicPlay()
    {
        musicPlayed.Play();
    }

    public void MusicPause()
    {
        musicPlayed.Pause();
    }

    public void MusicStop()
    {
        musicPlayed.Stop();
    }


    object[] originalMusics;
    object[] customMusics;

    public IEnumerator OriginalListRenewal()
    {
        isOriginal = true;
        isCustom = false;

        // Custom Music 폴더의 AudioClip 속성 파일 조회
        originalMusics = Resources.LoadAll<AudioClip>("Original Music");

        for (int i = 0; i < originalMusics.Length; i++)
        {
            // AudioClip to GameObject
            GameObject originalMusicElements = originalMusics[i] as GameObject;

            originalMusicElements = Instantiate(musicElement);
            originalMusicElements.name = "Original Music Element_" + i;
            originalMusicElements.transform.parent = contentOriginal.transform;
            originalMusicElements.transform.localScale = Vector3.one;
            originalMusicElements.transform.position = contentOriginal.transform.position;

            originalMusicElements.transform.GetChild(3).GetComponent<AudioSource>().playOnAwake = false; // Off 'Play On Awake'
            // AudioSource.clip ← Resources-Custom Musics.AudioClip
            originalMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)originalMusics[i];
            // textTitle.text ← customMusicElements.AudioSource.text
            originalMusicElements.transform.GetChild(1).gameObject.GetComponent<Text>().text = originalMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }
        yield return new WaitForSeconds(1);
        StopCoroutine(OriginalListRenewal());
    }

    public IEnumerator CustomListRenewal()
    {
        isOriginal = false;
        isCustom = true;

        // Custom Music 폴더의 AudioClip 속성 파일 조회
        customMusics = Resources.LoadAll<AudioClip>("Custom Music");

        for (int i = 0; i < customMusics.Length; i++)
        {
            // AudioClip to GameObject
            GameObject customMusicElements = customMusics[i] as GameObject;

            customMusicElements = Instantiate(musicElement);
            customMusicElements.name = "Custom Music Element_" + i;
            customMusicElements.transform.parent = contentCustom.transform;
            customMusicElements.transform.localScale = Vector3.one;
            customMusicElements.transform.position = contentCustom.transform.position;
            customMusicElements.transform.GetChild(3).GetComponent<AudioSource>().playOnAwake = false; // Off 'Play On Awake'
            // AudioSource.clip ← Resources-Custom Musics.AudioClip
            customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)customMusics[i];
            // textTitle.text ← customMusicElements.AudioSource.text
            customMusicElements.transform.GetChild(1).gameObject.GetComponent<Text>().text = customMusicElements.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }
        yield return new WaitForSeconds(1);
        StopCoroutine(CustomListRenewal());
    }
}
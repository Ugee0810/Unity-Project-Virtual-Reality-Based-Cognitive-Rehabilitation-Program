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
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;   //싱글턴을 할당할 전역 변수

    [Header("[UI]")]
    public GameObject uiMusicSelect;      // Lobby UI
    public GameObject uiMusicStart;       // Ingame UI
    public GameObject uiMusicPaused;      // Pause UI
    public GameObject uiResult;           // Result UI

    public GameObject originalScrollView; // Original View UI
    public GameObject customScrollView;   // Custom View UI

    public GameObject contentOriginal;    // 오리지널 리소스 프리팹 생성 위치(부모)
    public GameObject contentCustom;      // 커스텀   리소스 프리팹 생성 위치(부모)
    public GameObject contentResult;      // 결과     리소스 프리팹 생성 위치(부모)

    public GameObject infoTitle;          // Panel Music Info에서 Music Title

    public GameObject btnPlay;            // 노래 재생 버튼

    [Header("[환경 오브젝트]")]
    public GameObject inGameEnvironment;
    public GameObject baseGround;

    [Header("[SFX]")]
    public GameObject vizualizationObjects;

    [Header("[Music Info]")]
    public AudioSource musicBackGround; // BGM
    public AudioSource musicSelected;   // 선택된 노래
    public AudioSource musicPlayed;     // 플레이 할 노래

    [Header("[Prefabs]")]
    public GameObject musicElement;
    public GameObject resultElement;

    [Header("[Origin Controller]")]
    public GameObject layControllerActionLeft;
    public GameObject layControllerActionRight;
    public GameObject handControllerActionLeft;
    public GameObject handControllerActionRight;

    public GameObject layControllerDeviceLeft;
    public GameObject layControllerDeviceRight;
    public GameObject handControllerDeviceLeft;
    public GameObject handControllerDeviceRight;

    [Header("[플래그 변수]")]
    /*[HideInInspector]*/ public bool isOriginal;    // [Button] Original MusicList Selected
    /*[HideInInspector]*/ public bool isCustom;      // [Button] Custom MusicList Selected

    /*[HideInInspector]*/ public bool isLevelEasy;   // [Button] Level Easy Selected
    /*[HideInInspector]*/ public bool isLevelNormal; // [Button] Level Normal Selected
    /*[HideInInspector]*/ public bool isLevelHard;   // [Button] Level Hard Selected

    /*[HideInInspector]*/ public bool isHandChange;  // True : Hand Controller / False : Lay Controller

    /*[HideInInspector]*/ public bool isStart;       // Music Start
    /*[HideInInspector]*/ public bool isStop;        // Music Pause

    /*[HideInInspector]*/ public bool isSensor;      // 패널 생성 센서
    /*[HideInInspector]*/ public bool isSensorLeft;  // 패널 접촉 유/무 왼쪽
    /*[HideInInspector]*/ public bool isSensorRight; // 패널 접촉 유/무 오른쪽

    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
        else
        {
            Debug.LogWarning("씬에 두 개 이상의 게임 매니저가 존재합니다.");
            Destroy(gameObject); // GameManager 삭제
        }

        OriginalListRenewal();
    }

    // [Button] Original MusicList Selected
    public void BtnOriginalSelected()
    {
        if (!isOriginal && isCustom)
        {
            originalScrollView.SetActive(true);
            customScrollView.SetActive(false);
            OriginalListRenewal();

            musicSelected.Stop();
            musicBackGround.UnPause();
        }
    }

    // [Button] Custom MusicList Selected
    public void BtnCustomSelected()
    {
        if (isOriginal && !isCustom)
        {
            originalScrollView.SetActive(false);
            customScrollView.SetActive(true);
            CustomListRenewal();

            musicSelected.Stop();
            musicBackGround.UnPause();
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
        isStart = true;
        isHandChange = true;

        uiMusicSelect.SetActive(false);       // Lobby UI OFF
        baseGround.SetActive(false);
        uiMusicStart.SetActive(true);         // Ingame UI ON
        vizualizationObjects.SetActive(true); // VizualizationObj ON
        inGameEnvironment.SetActive(true);    // 인게임 환경 요소 ON

        musicBackGround.Pause();              // BGM Pause
        musicSelected.Stop();                 // Selected Music OFF
        musicPlayed.PlayDelayed(10);          // Played Music ON

        ControllerDeviceModeChange();         // Change Controller
    }

    // [Button] Pause
    public void BtnInGamePause()
    {
        if (isStart)
        {
            isStop = true;
            isHandChange = false;

            uiMusicPaused.SetActive(true); // Music Paused UI On

            Time.timeScale = 0;
            musicPlayed.Pause(); // 플레이 중 노래 일시 정지

            ControllerDeviceModeChange(); // Change Controller
        }
    }

    // [Button] UnPause
    public void BtnInGameUnPause()
    {
        if (isStart && isStop)
        {
            isStop = false;
            isHandChange = true;

            uiMusicPaused.SetActive(false); // Music Paused UI Off

            Time.timeScale = 1;
            musicPlayed.UnPause(); // 플레이 중 노래 일시 정지 해제

            ControllerDeviceModeChange(); // Change Controller
        }
    }

    // End
    public void InGameEnd()
    {
        isStart = false;
        isStop = false;
        isHandChange = false;

        uiResult.SetActive(true); // Result UI On

        musicBackGround.UnPause();
        musicPlayed.Stop(); // Played Song Reset

        PanelManager.instance.timer = 0;
        PanelManager.instance.lastIndex = 0;
        PanelManager.instance.safeQuiz = false;

        ControllerDeviceModeChange(); // Change Controller
    }

    // [Button] Back to the Lobby
    public void BtnBackLobby()
    {
        if (isStart && isStop)
        {
            isStart = false;
            isStop = false;
            isHandChange = false;

            uiMusicSelect.SetActive(true);         // Lobby UI On
            uiMusicStart.SetActive(false);         // Ingame UI Off
            uiMusicPaused.SetActive(false);        // MusicPaused UI Off
            uiResult.SetActive(false);             // Result UI Off
            vizualizationObjects.SetActive(false); // VizualizationObj Off
            inGameEnvironment.SetActive(false);    // Ingame Env Obj Off
            baseGround.SetActive(true);

            Time.timeScale = 1;
            musicBackGround.UnPause();
            musicPlayed.Stop(); // 플레이 중 노래 정지

            ControllerDeviceModeChange(); // Change Controller
        }
    }


    object[] originalMusics;
    object[] customMusics;

    GameObject originalMusicElementPrefab = null;
    GameObject customMusicElementPrefab   = null;

    public void OriginalListRenewal()
    {
        isOriginal = true;
        isCustom = false;

        // Custom Music 폴더의 AudioClip 속성 파일 조회
        originalMusics = Resources.LoadAll<AudioClip>("Original Music");

        for (int i = 0; i < originalMusics.Length; i++)
        {
            originalMusicElementPrefab = originalMusics[i] as GameObject; // AudioClip to GameObject
            originalMusicElementPrefab = Instantiate(musicElement);
            originalMusicElementPrefab.name = $"Original Music Element_{i}";
            originalMusicElementPrefab.transform.parent = contentOriginal.transform;
            originalMusicElementPrefab.transform.localScale = Vector3.one; 
            originalMusicElementPrefab.transform.position = contentOriginal.transform.position;

            //originalMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().playOnAwake = false; // Off 'Play On Awake'

            // AudioSource.clip ← Resources-Custom Musics.AudioClip
            originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)originalMusics[i];

            // (float)MusicLength to (string)PlayTime
            originalMusicElementPrefab.transform.GetChild(2).gameObject.GetComponent<Text>().text =
                TimeFormatter(originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length, false);

            // textTitle.text ← customMusicElements.AudioSource.text
            originalMusicElementPrefab.transform.GetChild(1).gameObject.GetComponent<Text>().text =
                originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }
    }

    public void CustomListRenewal()
    {
        isOriginal = false;
        isCustom = true;

        // Custom Music 폴더의 AudioClip 속성 파일 조회
        customMusics = Resources.LoadAll<AudioClip>("Custom Music");

        for (int i = 0; i < customMusics.Length; i++)
        {
            customMusicElementPrefab = customMusics[i] as GameObject; // AudioClip to GameObject
            customMusicElementPrefab = Instantiate(musicElement);
            customMusicElementPrefab.name = $"Custom Music Element_{i}";
            customMusicElementPrefab.transform.parent = contentCustom.transform;
            customMusicElementPrefab.transform.localScale = Vector3.one;
            customMusicElementPrefab.transform.position = contentCustom.transform.position;

            //customMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().playOnAwake = false; // Off 'Play On Awake'

            // AudioSource.clip ← Resources-Custom Musics.AudioClip
            customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)customMusics[i];

            // (float)MusicLength to (string)PlayTime
            customMusicElementPrefab.transform.GetChild(2).gameObject.GetComponent<Text>().text = 
                TimeFormatter(customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length, false);

            // textTitle.text ← customMusicElements.AudioSource.text
            customMusicElementPrefab.transform.GetChild(1).gameObject.GetComponent<Text>().text = 
                customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }
    }

    // 시간 변환 함수
    public static string TimeFormatter(float seconds, bool forceHHMMSS = false)
    {
        float secondsRemainder = Mathf.Floor((seconds % 60) * 100) / 100.0f;
        int minutes = ((int)(seconds / 60)) % 60;
        int hours = (int)(seconds / 3600);

        if (!forceHHMMSS)
        {
            if (hours == 0)
            {
                return System.String.Format("{0:00}:{1:00.00}", minutes, secondsRemainder);
            }
        }
        return System.String.Format("{0}:{1:00}:{2:00}", hours, minutes, secondsRemainder);
    }

    public void ControllerDeviceModeChange()
    {
        if /*Hand Controller*/ (isHandChange) 
        {
            layControllerActionLeft.SetActive(false);
            layControllerActionRight.SetActive(false);

            handControllerActionLeft.SetActive(true);
            handControllerActionRight.SetActive(true);


            //layControllerDeviceLeft.SetActive(false);
            //layControllerDeviceRight.SetActive(false);

            //handControllerDeviceLeft.SetActive(true);
            //handControllerDeviceRight.SetActive(true);

            isHandChange = false;
            return;
        }
        else /*Lay Controller*/
        {
            handControllerActionLeft.SetActive(false);
            handControllerActionRight.SetActive(false);

            layControllerActionLeft.SetActive(true);
            layControllerActionRight.SetActive(true);


            //handControllerDeviceLeft.SetActive(false);
            //handControllerDeviceRight.SetActive(false);

            //layControllerDeviceLeft.SetActive(true);
            //layControllerDeviceRight.SetActive(true);

            isHandChange = true;
            return;
        }
    }
}
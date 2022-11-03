/// <summary>
/// GameManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// 
/// </summary>

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : Singleton<GameManager>
{
    [Header("[Input Action Reference]")]
    public InputActionReference gamePause;

    [Header("[UI - 전체]")]
    public GameObject uiLobby;    // UI Lobby
    public GameObject uiLobbyOption;
    public GameObject uiLobbyResult;
    public GameObject uiTutorial; // UI Tutorial
    public GameObject uiIngame;   // UI Ingame
    public GameObject uiPause;    // UI Pause
    public GameObject uiResult;   // UI Result

    [Header("[UI - 옵션(밝기)]")]
    public Slider sliderBright;
    public Button btnBrightLeft;
    public Button btnBrightRight;

    [Header("[UI - 옵션(키 조절)]")]
    public Slider sliderHeight;
    public Button btnHeightLeft;
    public Button btnHeightRight;

    [Header("[UI - 모드]")]
    public Button[] btnModes;

    [Header("[UI - 노래 테마]")]
    public Button[] btnMusicTheme;
    public GameObject[] scrollView;

    [Header("[UI - 리소스 프리팹 생성 위치]")]
    public GameObject contentOriginal; // 오리지널 리소스 프리팹 생성 위치(부모)
    public GameObject contentCustom;   // 커스텀 리소스 프리팹 생성 위치(부모)
    public GameObject contentResult;   // 결과 리소스 프리팹 생성 위치(부모)

    [Header("[UI - 선택한 노래 정보]")]
    public TMP_Text infoTitle;
    public TMP_Text[] infoTmp_Text;
    public Image[] infoImages;

    [Header("[UI - 난이도 & 플레이]")]
    public Button[] btnLevels;
    public Button btnPlay;

    [Header("[UI - 튜토리얼 버튼]")]
    public GameObject bgTutorial;

    [Header("[UI - 결과]")]
    public Button btnReset;
    public Button btnEndBackLobby;

    [Header("[UI - 인게임]")]
    public Slider playedMusicSlide;
    public Button btnBackLobby;
    public Button btnUnPause;

    [Header("[Environment]")]
    public GameObject[] worlds;
    public GameObject[] lobbyBaseGround;
    public GameObject inGameEnv;

    [Header("[Prefabs]")]
    public GameObject musicElement;
    public GameObject resultElement;

    [Header("[Origin Controller]")]
    public GameObject rayInteractorLeft;
    public GameObject rayInteractorRight;

    [Header("[Audio Source]")]
    public AudioSource[] music;
    public AudioSource[] sFX;

    [Header("[InGame Data]")]
    public TMP_Text textIngameScore;
    public TMP_Text textIngameKcal;

    [Header("[Key]")]
    public TMP_Text[] textKeys;

    [Header("[Music Info]")]
    public float playTime;
    public float playTimeOffset;
    public float halfPlayTime;
    public float halfHalfPlayTimeOffset;
    public float offsetTimer;
    public float moveSpeed = 3.0f;
    public float modePanelSpeed = 1.0f;
    public int   bpm;
    public float secPerBeat;
    public float panelTimer; // BPM 계산 타이머

    [Header("[Score & Kcal]")]
    public int   score = 0;
    public float kcal  = 0;

    [Header("[Option]")]
    public float bright;
    public float height;

    [Header("[플래그 변수]")]
    public bool isStart;       // Game Start
    public bool isPause;       // Game Pause
    public bool isSensorLeft;  // 패널 접촉 유/무 왼쪽
    public bool isSensorRight; // 패널 접촉 유/무 오른쪽
    public bool isEmail;
    public bool isPassword;

    private void Awake()
    {
        // PlayerPrefs Key Value Reset
        if (PlayerPrefs.HasKey("Title")) PlayerPrefs.SetString("Title", "-");
        if (PlayerPrefs.HasKey("Level")) PlayerPrefs.SetString("Level", "-");
        if (PlayerPrefs.HasKey("Score")) PlayerPrefs.SetInt("Score", 0);
        if (PlayerPrefs.HasKey("Kcal"))  PlayerPrefs.SetInt("Kcal", 0);
        // Btn Option - Bright Dec / Inc | Height Dec / Inc
        btnBrightLeft.onClick.AddListener(()  => OnClick_Options(btnBrightLeft,  bright, height, sliderBright, sliderHeight, sFX[0]));
        btnBrightRight.onClick.AddListener(() => OnClick_Options(btnBrightRight, bright, height, sliderBright, sliderHeight, sFX[0]));
        btnHeightLeft.onClick.AddListener(()  => OnClick_Options(btnHeightLeft,  bright, height, sliderBright, sliderHeight, sFX[0]));
        btnHeightRight.onClick.AddListener(() => OnClick_Options(btnHeightRight, bright, height, sliderBright, sliderHeight, sFX[0]));
        // Btn Mode - Panel Speed, Music Length, Obstacle
        void BtnModes(int i) { btnModes[i].onClick.AddListener(() => OnClick_Mode(btnModes[i], btnModes, infoImages, infoTmp_Text, sFX[0])); }
        for (int i = 0; i < btnModes.Length; i++)
            BtnModes(i);
        // Btn Music Theme
        void BtnMusicTheme(int i) { btnMusicTheme[i].onClick.AddListener(() => OnClick_MusicTheme(btnMusicTheme[i], btnMusicTheme, sFX[0])); }
        for (int i = 0; i < btnMusicTheme.Length; i++)
            BtnMusicTheme(i);
        // Btn Level
        void BtnLevels(int i) { btnLevels[i].onClick.AddListener(() => OnClick_Level(btnLevels[i], btnLevels, sFX[0])); }
        for (int i = 0; i < btnLevels.Length; i++)
            BtnLevels(i);
        // Btn Play
        btnPlay.onClick.AddListener(OnClick_BtnPlay);
        // Btn Back Lobby
        btnBackLobby.onClick.AddListener(OnClick_BtnPauseBackLobby);
        // Btn UnPause
        btnUnPause.onClick.AddListener(OnClick_BtnInGameUnPause);
        // Btn End Back Lobby
        btnEndBackLobby.onClick.AddListener(OnClick_BtnEndBackLobby);
        // Btn Reset
        btnReset.onClick.AddListener(OnClick_BtnReset);
    }

    private void FixedUpdate()
    {
        // Option Sync
        bright = sliderBright.value;
        height = sliderHeight.value;
    }

    /// <summary>
    /// [Onclick Listener] BTN Options
    /// ＃밝기, 키 조절 슬라이더 증감
    /// ＃효과음
    /// </summary>
    /// <param name="button">할당 버튼</param>
    /// <param name="bright">Directional Light Intensity</param>
    /// <param name="height">Camera Offset Y</param>
    /// <param name="sBright">밝기 슬라이더</param>
    /// <param name="sHeight">키 조절 슬라이더</param>
    /// <param name="sfx">AudioSource - SFX Click</param>
    void OnClick_Options(Button button, float bright, float height, Slider sBright, Slider sHeight, AudioSource sfx)
    {
        // 밝기 - 왼쪽(감소)
        if (button == btnBrightLeft && (0 <= bright) && (bright <= 4.2))
        {
            bright -= 0.2f;
            sBright.value = bright;
            sfx.Play();
        }

        // 밝기 - 오른쪽(증가)
        if (button == btnBrightRight && (0 <= bright) && (bright <= 4.2))
        {
            bright += 0.2f;
            sBright.value = bright;
            sfx.Play();
        }

        // 키 조절 - 왼쪽(감소)
        if (button == btnHeightLeft && (1.9f <= height) && (height <= 2.1f))
        {
            height -= 0.01f;
            sHeight.value = height;
            sfx.Play();
        }

        // 키 조절 - 오른쪽(증가)
        if (button == btnHeightRight && (1.9f <= height) && (height <= 2.1f))
        {
            height += 0.01f;
            sHeight.value = height;
            sfx.Play();
        }
    }

    /// <summary>
    /// [Onclick Listener] BTN Modes
    /// ＃버튼 활성화/비활성화 전환
    /// ＃이미지 변경
    /// ＃안내 문구 변경
    /// ＃효과음
    /// </summary>
    /// <param name="button">할당 버튼</param>
    /// <param name="buttons">모드 버튼 배열</param>
    /// <param name="images">버튼 이미지</param>
    /// <param name="tmps">모드 텍스트</param>
    /// <param name="sfx">AudioSource - SFX Click</param>
    void OnClick_Mode(Button button, Button[] buttons, Image[] images, TMP_Text[] tmps, AudioSource sfx)
    {
        // Panel Speeds
        if (button == btnModes[0])
        {
            buttons[0].interactable = false;
            buttons[1].interactable = true;
            buttons[2].interactable = true;
            images[0].enabled = true;
            images[1].enabled = false;
            images[2].enabled = false;
            tmps[0].text = "x0.7";
            sfx.Play();
            modePanelSpeed = 0.7f;
        }
        if (button == btnModes[1])
        {
            buttons[0].interactable = true;
            buttons[1].interactable = false;
            buttons[2].interactable = true;
            images[0].enabled = false;
            images[1].enabled = true;
            images[2].enabled = false;
            tmps[0].text = "x1.0";
            sfx.Play();
            modePanelSpeed = 1.0f;
        }
        if (button == btnModes[2])
        {
            buttons[0].interactable = true;
            buttons[1].interactable = true;
            buttons[2].interactable = false;
            images[0].enabled = false;
            images[1].enabled = false;
            images[2].enabled = true;
            tmps[0].text = "x1.3";
            sfx.Play();
            modePanelSpeed = 1.3f;
        }
        // Music Lengths
        if (button == btnModes[3])
        {
            buttons[3].interactable = false;
            buttons[4].interactable = true;
            images[3].enabled = true;
            images[4].enabled = false;
            tmps[1].text = "절반";
            sfx.Play();
        }
        if (button == btnModes[4])
        {
            buttons[3].interactable = true;
            buttons[4].interactable = false;
            images[3].enabled = false;
            images[4].enabled = true;
            tmps[1].text = "전부";
            sfx.Play();
        }
        // Obstacle Panels
        if (button == btnModes[5])
        {
            buttons[5].interactable = false;
            buttons[6].interactable = true;
            images[5].enabled = true;
            images[6].enabled = false;
            tmps[2].text = "ON";
            sfx.Play();
        }
        if (button == btnModes[6])
        {
            buttons[5].interactable = true;
            buttons[6].interactable = false;
            images[5].enabled = false;
            images[6].enabled = true;
            tmps[2].text = "OFF";
            sfx.Play();
        }
    }

    /// <summary>
    /// [Onclick Listener] BTN Themes
    /// ＃버튼 활성화/비활성화 전환
    /// ＃효과음
    /// </summary>
    /// <param name="button">할당 버튼</param>
    /// <param name="sfx">AudioSource - SFX Click</param>
    void OnClick_MusicTheme(Button button, Button[] buttons, AudioSource sfx)
    {
        // Original Selected
        if (button == btnMusicTheme[0])
        {
            buttons[0].interactable = false;
            buttons[1].interactable = true;
            sfx.Play();

            // Resources -> Original Music 폴더의 AudioClip 속성 파일 조회 --> Original Music Element 생성
            if (!Singleton<TutorialManager>.Instance.isTutorial)
            {
                // List Reset
                foreach (Transform item in contentOriginal.transform) Destroy(item.gameObject);
                // Original Music 폴더의 AudioClip 속성 파일 조회
                object[] originalMusics = Resources.LoadAll<AudioClip>("Original Music");
                for (int i = 0; i < originalMusics.Length; i++)
                {
                    // AudioClip to GameObject
                    GameObject originalMusicElementPrefab = originalMusics[i] as GameObject;
                    originalMusicElementPrefab = Instantiate(musicElement, contentOriginal.transform.position, contentOriginal.transform.rotation);
                    originalMusicElementPrefab.transform.parent = contentOriginal.transform;
                    originalMusicElementPrefab.transform.localScale = Vector3.one;
                    // AudioSource.clip ← Resources-Custom Musics.AudioClip
                    originalMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip = (AudioClip)originalMusics[i];
                    // 분석한 BPM을 텍스트에 저장
                    originalMusicElementPrefab.transform.GetChild(2).GetComponent<TMP_Text>().text = $"BPM : {UniBpmAnalyzer.AnalyzeBpm(originalMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip)}";
                    // (float)MusicLength to (string)PlayTime
                    originalMusicElementPrefab.transform.GetChild(1).GetComponent<TMP_Text>().text = TimeFormatter(originalMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip.length, false);
                    // textTitle.text ← customMusicElements.AudioSource.text
                    originalMusicElementPrefab.transform.GetChild(0).GetComponent<TMP_Text>().text = originalMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip.name;
                }
            }
            // Resources -> Original Music 폴더의 Cat Life 파일 조회 --> Tutorial Music Element 생성
            else
            {
                // List Reset
                foreach (Transform item in contentOriginal.transform) Destroy(item.gameObject);
                foreach (Transform item in contentCustom.transform) Destroy(item.gameObject);
                object tutorialMusic = Resources.Load<AudioClip>("Original Music/Cat Life");
                GameObject tutorialMusicElementPrefab;
                tutorialMusicElementPrefab = tutorialMusic as GameObject;
                tutorialMusicElementPrefab = Instantiate(musicElement, contentOriginal.transform.position, contentOriginal.transform.rotation);
                tutorialMusicElementPrefab.transform.parent = contentOriginal.transform;
                tutorialMusicElementPrefab.transform.localScale = Vector3.one;
                // AudioSource.clip ← Resources-Custom Musics.AudioClip
                tutorialMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip = (AudioClip)tutorialMusic;
                // 분석한 BPM을 텍스트에 저장
                tutorialMusicElementPrefab.transform.GetChild(2).GetComponent<TMP_Text>().text = $"BPM : {UniBpmAnalyzer.AnalyzeBpm(tutorialMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip)}";
                // (float)MusicLength to (string)PlayTime
                tutorialMusicElementPrefab.transform.GetChild(1).GetComponent<TMP_Text>().text = TimeFormatter(tutorialMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip.length, false);
                // textTitle.text ← customMusicElements.AudioSource.text
                tutorialMusicElementPrefab.transform.GetChild(0).GetComponent<TMP_Text>().text = "<bounce a=0.3 f=0.2>Cat Life</bounce>";
                // Next Step
                Singleton<TutorialManager>.Instance.TutorialStep();
            }

            // 스크롤 뷰 전환
            scrollView[0].SetActive(true);
            scrollView[1].SetActive(false);
            // BGM 정지 해제
            music[0].UnPause();
            // 선택한 노래 정지
            music[1].Stop();
        }
        // Custom Selected
        if (button == btnMusicTheme[1])
        {
            buttons[0].interactable = true;
            buttons[1].interactable = false;
            sfx.Play();

            // Resources -> Custom Music 폴더의 AudioClip 속성 파일 조회 --> Custom Music Element 생성
            // List Reset
            foreach (Transform item in contentCustom.transform) Destroy(item.gameObject);
            // Custom Music 폴더의 AudioClip 속성 파일 조회
            object[] customMusics = Resources.LoadAll<AudioClip>("Custom Music");
            for (int i = 0; i < customMusics.Length; i++)
            {
                // AudioClip to GameObject
                GameObject customMusicElementPrefab = customMusics[i] as GameObject;
                customMusicElementPrefab = Instantiate(musicElement, contentCustom.transform.position, contentCustom.transform.rotation);
                customMusicElementPrefab.transform.parent = contentCustom.transform;
                customMusicElementPrefab.transform.localScale = Vector3.one;
                // AudioSource.clip ← Resources-Custom Musics.AudioClip
                customMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip = (AudioClip)customMusics[i];
                // 분석한 BPM을 텍스트에 저장
                customMusicElementPrefab.transform.GetChild(2).GetComponent<TMP_Text>().text = $"BPM : {UniBpmAnalyzer.AnalyzeBpm(customMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip)}";
                // (float)MusicLength to (string)PlayTime
                customMusicElementPrefab.transform.GetChild(1).GetComponent<TMP_Text>().text = TimeFormatter(customMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip.length, false);
                // textTitle.text ← customMusicElements.AudioSource.text
                customMusicElementPrefab.transform.GetChild(0).GetComponent<TMP_Text>().text = customMusicElementPrefab.transform.GetChild(3).GetComponent<AudioSource>().clip.name;
            }

            // 스크롤 뷰 전환
            scrollView[0].SetActive(false);
            scrollView[1].SetActive(true);
            // BGM 정지 해제
            music[0].UnPause();
            // 선택한 노래 정지
            music[1].Stop();
        }
    }

    /// <summary>
    /// [Onclick Listener] BTN Levels
    /// ＃버튼 활성화/비활성화 전환
    /// ＃효과음
    /// ＃레벨 선택에 따른 이벤트(레벨에 따른 패널 생성 속도, 퀴즈 쿨타임 변경)
    /// </summary>
    /// <param name="button">할당 버튼</param>
    /// <param name="buttons">난이도 버튼 배열</param>
    /// <param name="sfx">AudioSource - SFX Click</param>
    void OnClick_Level(Button button, Button[] buttons, AudioSource sfx)
    {
        // Easy Selected
        if (button == btnLevels[0])
        {
            buttons[0].interactable = false;
            buttons[1].interactable = true;
            buttons[2].interactable = true;
            sfx.Play();
            secPerBeat = 300f / bpm;
            Singleton<PanelManager>.Instance.quizCool = 14;
            btnPlay.interactable = true;

            Singleton<TutorialManager>.Instance.TutorialStep();
        }
        // Normal Selected
        if (button == btnLevels[1])
        {
            buttons[0].interactable = true;
            buttons[1].interactable = false;
            buttons[2].interactable = true;
            sfx.Play();
            secPerBeat = 240f / bpm;
            Singleton<PanelManager>.Instance.quizCool = 11;
            btnPlay.interactable = true;
        }
        // Hard Selected
        if (button == btnLevels[2])
        {
            buttons[0].interactable = true;
            buttons[1].interactable = true;
            buttons[2].interactable = false;
            sfx.Play();
            secPerBeat = 180f / bpm;
            Singleton<PanelManager>.Instance.quizCool = 8;
            btnPlay.interactable = true;
        }
    }

    /// <summary>
    /// [Onclick Listener] 로비 ---> 인게임
    /// </summary>
    void OnClick_BtnPlay()
    {
        if (!Singleton<TutorialManager>.Instance.isTutorial)
        {
            isStart = true;
            // Ray Controller OFF
            RayControllerMode(false);
            // 일시정지 인풋액션 활성화
            gamePause.action.started += XRI_InGamePause;
        }
        uiLobby.SetActive(false);
        lobbyBaseGround[0].SetActive(false);
        uiIngame.SetActive(true);
        inGameEnv.SetActive(true);
        // BGM Pause
        music[0].Pause();
        // Selected Music Stop
        music[1].Stop();
        // Played Music Play
        music[2].Play();
        // SFX Play
        sFX[1].Play();

        Singleton<TutorialManager>.Instance.TutorialStep();
    }

    /// <summary>
    /// [XRI Input Action Binding(Any Grip Button)]
    /// ＃인게임 ---> 일시정지
    /// </summary>
    /// <param name="context">Any Grip Button</param>
    internal void XRI_InGamePause(InputAction.CallbackContext context)
    {
        if (isStart && !isPause && !Singleton<TutorialManager>.Instance.isTutorial)
        {
            isPause = true;
            // Music Paused UI On
            uiPause.SetActive(true);
            // Ray Controller ON
            RayControllerMode(true);
            // 플레이 중 노래 일시 정지
            Time.timeScale = 0;
            music[2].Pause();
            // 일시정지 인풋액션 비활성화
            gamePause.action.started -= XRI_InGamePause;
            //Debug.Log("XRI_InGamePause CallBack Context : " + context);
        }
    }

    /// <summary>
    /// [Onclick Listener] 일시정지 ---> 메인
    /// </summary>
    public void OnClick_BtnPauseBackLobby()
    {
        if (isStart && isPause)
        {
            // [Event] 인게임 종료 이벤트
            EndResetEvent();
            uiLobby.SetActive(true);
            lobbyBaseGround[0].SetActive(true);
            uiIngame.SetActive(false);
            inGameEnv.SetActive(false);
            uiPause.SetActive(false);
            music[2].Stop();
            sFX[0].Play();
            // 남은 패널 삭제
            foreach (Transform childNum in Singleton<PanelManager>.Instance.panelSpawnPoint.transform)
                Destroy(childNum.gameObject);
            // 시간 정지 해제
            Time.timeScale = 1;
        }
    }

    /// <summary>
    /// [Onclick Listener] 일시정지 ---> 인게임
    /// </summary>
    public void OnClick_BtnInGameUnPause()
    {
        isPause = false;
        // Music Paused UI OFF
        uiPause.SetActive(false);
        // Ray Controller OFF
        RayControllerMode(false);
        // 플레이 중 노래 일시 정지 해제
        Time.timeScale = 1;
        music[2].UnPause();
        sFX[0].Play();
        // 일시정지 인풋액션 활성화
        gamePause.action.started += XRI_InGamePause;
    }

    /// <summary>
    /// [Event] 인게임 완곡 이후 이벤트
    /// </summary>
    public void InGameEnd()
    {
        // Ray Controller ON
        RayControllerMode(true);
        // Ingame Result Data Copy
        textKeys[0].text = PlayerPrefs.GetString("Title", $"{music[2].clip.name}");
        if (!btnLevels[0].interactable) textKeys[1].text = PlayerPrefs.GetString("Level", "Easy");
        if (!btnLevels[1].interactable) textKeys[1].text = PlayerPrefs.GetString("Level", "Normal");
        if (!btnLevels[2].interactable) textKeys[1].text = PlayerPrefs.GetString("Level", "어려움");
        textKeys[2].text = PlayerPrefs.GetString("Score", $"{textIngameScore.text}");
        textKeys[3].text = PlayerPrefs.GetString("Kcal", $"{textIngameKcal.text}");
        // [Event] 인게임 종료 이벤트
        EndResetEvent();
        // UI Result ON
        uiResult.SetActive(true);
    }

    /// <summary>
    /// [Event] 인게임 종료 이벤트
    /// </summary>
    public void EndResetEvent()
    {
        isStart = false;
        isPause = false;
        infoTitle.text = "※ Not Search";
        btnLevels[0].interactable = false;
        btnLevels[1].interactable = false;
        btnLevels[2].interactable = false;
        btnPlay.interactable = false;
        // 노래, 패널 관련 초기화
        playTime = 0;
        playTimeOffset = 0;
        halfPlayTime = 0;
        halfHalfPlayTimeOffset = 0;
        offsetTimer = 0;
        bpm = 0;
        secPerBeat = 0;
        panelTimer = 0;
        Singleton<PanelManager>.Instance.panelSpawnCount = -1;
        Singleton<PanelManager>.Instance.quizCool = 0;
        Singleton<PanelManager>.Instance.curColor = "";
        Singleton<PanelManager>.Instance.curLetter = "";
        Singleton<PanelManager>.Instance.isQuiz = false;
        Singleton<PanelManager>.Instance.isCurLeft = false;
        Singleton<PanelManager>.Instance.isCurRight = false;
        // 스코어 초기화
        ScoreManaged.SetScore(score = 0);
        // 칼로리 초기화
        ScoreManaged.SetKcal(kcal = 0);
        // 콤보 초기화
        Singleton<ComboManager>.Instance.Clear();
        // 인게임 플레이 타임 슬라이더 초기화
        playedMusicSlide.minValue = 0;
        playedMusicSlide.value = 0;
        // 일시정지 인풋액션 비활성화
        gamePause.action.started -= XRI_InGamePause;
        // BGM ON
        music[0].UnPause();
    }

    /// <summary>
    /// [Onclick Listener] 종료 ---> 결과 버튼
    /// </summary>
    public void OnClick_BtnEndBackLobby()
    {
        // Add Reuslt List
        if (!Singleton<TutorialManager>.Instance.isTutorial)
        {
            GameObject resultElementPrefab = Instantiate(resultElement, contentResult.transform.position, contentResult.transform.rotation);
            resultElementPrefab.transform.parent = contentResult.transform;
            resultElementPrefab.transform.localScale = Vector3.one;
            btnReset.interactable = true;
        }
        uiLobby.SetActive(true);
        lobbyBaseGround[0].SetActive(true);
        uiIngame.SetActive(false);
        inGameEnv.SetActive(false);
        uiResult.SetActive(false);
        btnLevels[0].interactable = false;
        btnLevels[1].interactable = false;
        btnLevels[2].interactable = false;
        btnPlay.interactable = false;
        music[0].UnPause();
        music[2].Stop();
        sFX[0].Play();
        Singleton<TutorialManager>.Instance.TutorialStep();
    }

    /// <summary>
    /// [Onclick Listener] 결과 리스트 초기화
    /// </summary>
    public void OnClick_BtnReset()
    {
        foreach (Transform item in contentResult.transform) Destroy(item.gameObject);
        btnReset.interactable = false;
        sFX[0].Play();
    }

    /// <summary>
    /// [Event] Ray Controller 표시
    /// </summary>
    /// <param name="_Bool"></param>
    public void RayControllerMode(bool _Bool)
    {
        if (_Bool)
        {
            rayInteractorLeft.SetActive(true);
            rayInteractorRight.SetActive(true);
        }
        else
        {
            rayInteractorLeft.SetActive(false);
            rayInteractorRight.SetActive(false);
        }
    }

    /// <summary>
    /// TimeFormatter Method
    /// </summary>
    /// <param name="seconds"></param>
    /// <param name="forceHHMMSS"></param>
    /// <returns></returns>
    public string TimeFormatter(float seconds, bool forceHHMMSS = false)
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

    /// <summary>
    /// [Onclick] Quit Game Method
    /// </summary>
    public void BtnQuit()
    {
#if UNITY_WEBPLAYER
     public static string webplayerQuitURL = "http://google.com/";
#endif
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
         Application.OpenURL(webplayerQuitURL);
#else
         Application.Quit();
#endif
        }
    }

    /// <summary>
    /// [Event] 이메일 인풋필드 선택
    /// </summary>
    public void ChangeEmail()
    {
        isPassword = false;
        isEmail = true;
    }

    /// <summary>
    /// [Event] 패스워드 인풋필드 선택
    /// </summary>
    public void ChangePassword()
    {
        isEmail = false;
        isPassword = true;
    }
}

[Serializable]
public static class ScoreManaged
{
    /// <summary>
    /// [Coroutine] 스코어 칼로리 증가
    /// </summary>
    /// <returns></returns>
    public static IEnumerator Increase()
    {
        // SFX(Currect)
        Singleton<GameManager>.Instance.sFX[1].Play();
        // Score
        if      /*x1*/ (0  <= Singleton<ComboManager>.Instance.combo && Singleton<ComboManager>.Instance.combo < 5)  SetScore(Singleton<GameManager>.Instance.score += 1000);
        else if /*x2*/ (5  <= Singleton<ComboManager>.Instance.combo && Singleton<ComboManager>.Instance.combo < 10) SetScore(Singleton<GameManager>.Instance.score += 2000);
        else if /*x4*/ (10 <= Singleton<ComboManager>.Instance.combo && Singleton<ComboManager>.Instance.combo < 15) SetScore(Singleton<GameManager>.Instance.score += 4000);
        else if /*x8*/ (15 <= Singleton<ComboManager>.Instance.combo)                                     SetScore(Singleton<GameManager>.Instance.score += 8000);
        // Kcal
        SetKcal(Singleton<GameManager>.Instance.kcal += Random.Range(0.05f, 0.15f));
        // Combo
        Singleton<ComboManager>.Instance.IncreaseCombo();

        yield break;
    }

    /// <summary>
    /// [Event] 스코어 증감 인게임 반영
    /// </summary>
    /// <param name="score"></param>
    public static void SetScore(int score)
    {
        Singleton<GameManager>.Instance.textIngameScore.text = score.ToString();
    }

    /// <summary>
    /// [Event] 칼로리 증감 인게임 반영
    /// </summary>
    /// <param name="kcal"></param>
    public static void SetKcal(float kcal)
    {
        Singleton<GameManager>.Instance.textIngameKcal.text = kcal.ToString("F2");
    }
}
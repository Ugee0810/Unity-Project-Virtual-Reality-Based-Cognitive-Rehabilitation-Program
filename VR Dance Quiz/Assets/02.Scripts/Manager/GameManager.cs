/// <summary>
/// GameManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// 게임에서 발생하는 이벤트(버튼, 플래그)를 처리 합니다.
/// PanelManager에게 해당된 레벨에 따른 패턴을 지시합니다.
/// 오리지널 또는 커스텀 노래 조회 버튼을 눌렀을 때 라이브러리 내 음악을 조회 후 각 정보들을 Element들에게 전달합니다.
/// </summary>

using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("[Input Action Reference]")]
    public InputActionReference gamePause;

    [Header("[SkyBox Rotate]")]
    [SerializeField] float rotateSpeed = 1f;

    [Header("[UI - 전체]")]
    public GameObject uiTutorial; // UI Tutorial
    public GameObject uiLobby;    // UI Lobby
    public GameObject uiIngame;   // UI Ingame
    public GameObject uiPause;    // UI Pause
    public GameObject uiResult;   // UI Result

    public GameObject uiLobbyOption;
    public GameObject uiLobbyResult;

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
    public GameObject lobbyBaseGround;
    public GameObject inGameEnv;

    [Header("[Prefabs]")]
    public GameObject musicElement;
    public GameObject resultElement;

    [Header("[Origin Controller]")]
    public GameObject rayInteractorLeft;
    public GameObject rayInteractorRight;

    [Header("[Audio Source]")]
    public AudioSource musicBackGround; // BGM
    public AudioSource musicSelected;   // Lobby Music
    public AudioSource musicPlayed;     // Ingame Music
    public UnityEvent[] sFX;

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
    public float moveSpeed = 2.0f;
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

    public static GameManager instance;
    private void Awake()
    {
        // Singleton
        if (instance == null) instance = this;
        else Destroy(gameObject);
        // 초기화
        if (PlayerPrefs.HasKey("Title")) PlayerPrefs.SetString("Title", "-");
        if (PlayerPrefs.HasKey("Level")) PlayerPrefs.SetString("Level", "-");
        if (PlayerPrefs.HasKey("Score")) PlayerPrefs.SetInt("Score", 0);
        if (PlayerPrefs.HasKey("Kcal")) PlayerPrefs.SetInt("Kcal", 0);
        // Btn Option - Bright
        btnBrightLeft.onClick.AddListener(OnClick_BrightDec);
        btnBrightRight.onClick.AddListener(OnClick_BrightInc);
        // Btn Option - Height
        btnHeightLeft.onClick.AddListener(OnClick_HeightDec);
        btnHeightRight.onClick.AddListener(OnClick_HeightInc);
        // Btn Mode - Panel Speed
        btnModes[0].onClick.AddListener(() => OnClick_Mode(btnModes[0], infoTmp_Text, infoImages));
        btnModes[1].onClick.AddListener(() => OnClick_Mode(btnModes[1], infoTmp_Text, infoImages));
        btnModes[2].onClick.AddListener(() => OnClick_Mode(btnModes[2], infoTmp_Text, infoImages));
        // Btn Mode - Music Length
        btnModes[3].onClick.AddListener(() => OnClick_Mode(btnModes[3], infoTmp_Text, infoImages));
        btnModes[4].onClick.AddListener(() => OnClick_Mode(btnModes[4], infoTmp_Text, infoImages));
        // Btn Mode - Obstacle
        btnModes[5].onClick.AddListener(() => OnClick_Mode(btnModes[5], infoTmp_Text, infoImages));
        btnModes[6].onClick.AddListener(() => OnClick_Mode(btnModes[6], infoTmp_Text, infoImages));
        // Btn Music Theme
        btnMusicTheme[0].onClick.AddListener(() => OnClick_MusicTheme(btnMusicTheme[0]));
        btnMusicTheme[1].onClick.AddListener(() => OnClick_MusicTheme(btnMusicTheme[1]));
        // Btn Level
        btnLevels[0].onClick.AddListener(() => OnClick_Level(btnLevels[0]));
        btnLevels[1].onClick.AddListener(() => OnClick_Level(btnLevels[1]));
        btnLevels[2].onClick.AddListener(() => OnClick_Level(btnLevels[2]));
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
        // Skybox Rotate
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
        // Option
        bright = sliderBright.value;
        height = sliderHeight.value;
    }
        
    // [Onclick Listener] 밝기 증가
    public void OnClick_BrightInc()
    {
        if (0 <= bright && bright <= 2.1)
        {
            bright += 0.1f;
            sliderBright.value = bright;
            sFX[0]?.Invoke();
        }
    }

    // [Onclick Listener] 밝기 감소
    public void OnClick_BrightDec()
    {
        if (0 <= bright && bright <= 2.1)
        {
            bright -= 0.1f;
            sliderBright.value = bright;
            sFX[0]?.Invoke();
        }
    }

    // [Onclick Listener] 키 조절 증가
    public void OnClick_HeightInc()
    {
        if (1.9f <= height && height <= 2.1f)
        {
            height += 0.01f;
            sliderHeight.value = height;
            sFX[0]?.Invoke();
        }
    }

    // [Onclick Listener] 키 조절 감소
    public void OnClick_HeightDec()
    {
        if (1.9f <= height && height <= 2.1f)
        {
            height -= 0.01f;
            sliderHeight.value = height;
            sFX[0]?.Invoke();
        }
    }

    // [Onclick Listener] 모드 선택에 따른 안내 문구, 이미지 변경
    public void OnClick_Mode(Button _Button, TMP_Text[] _TMP_Texts, Image[] _Images)
    {
        // Panel Speed
        if (_Button == btnModes[0])
        {
            btnModes[0].interactable = false;
            btnModes[1].interactable = true;
            btnModes[2].interactable = true;
            _Images[0].enabled = true;
            _Images[1].enabled = false;
            _Images[2].enabled = false;
            _TMP_Texts[0].text = "x0.7";
            modePanelSpeed = 0.7f;
            sFX[0]?.Invoke();
        }
        if (_Button == btnModes[1])
        {
            btnModes[0].interactable = true;
            btnModes[1].interactable = false;
            btnModes[2].interactable = true;
            _Images[0].enabled = false;
            _Images[1].enabled = true;
            _Images[2].enabled = false;
            _TMP_Texts[0].text = "x1.0";
            modePanelSpeed = 1.0f;
            sFX[0]?.Invoke();
        }
        if (_Button == btnModes[2])
        {
            btnModes[0].interactable = true;
            btnModes[1].interactable = true;
            btnModes[2].interactable = false;
            _Images[0].enabled = false;
            _Images[1].enabled = false;
            _Images[2].enabled = true;
            _TMP_Texts[0].text = "x1.3";
            modePanelSpeed = 1.3f;
            sFX[0]?.Invoke();
        }
        // Music Length
        if (_Button == btnModes[3])
        {
            btnModes[3].interactable = false;
            btnModes[4].interactable = true;
            _Images[3].enabled = true;
            _Images[4].enabled = false;
            _TMP_Texts[1].text = "절반";
            sFX[0]?.Invoke();
        }
        if (_Button == btnModes[4])
        {
            btnModes[3].interactable = true;
            btnModes[4].interactable = false;
            _Images[3].enabled = false;
            _Images[4].enabled = true;
            _TMP_Texts[1].text = "전부";
            sFX[0]?.Invoke();
        }
        // Obstacle Panel
        if (_Button == btnModes[5])
        {
            btnModes[5].interactable = false;
            btnModes[6].interactable = true;
            _Images[5].enabled = true;
            _Images[6].enabled = false;
            _TMP_Texts[2].text = "ON";
            sFX[0]?.Invoke();
        }
        if (_Button == btnModes[6])
        {
            btnModes[5].interactable = true;
            btnModes[6].interactable = false;
            _Images[5].enabled = false;
            _Images[6].enabled = true;
            _TMP_Texts[2].text = "OFF";
            sFX[0]?.Invoke();
        }
    }

    // [Onclick Listener] 테마 선택에 따른 이벤트
    public void OnClick_MusicTheme(Button _Button)
    {
        // Original
        if (_Button == btnMusicTheme[0])
        {
            btnMusicTheme[0].interactable = false;
            btnMusicTheme[1].interactable = true;
            OriginalListRenewal();
            scrollView[0].SetActive(true);
            scrollView[1].SetActive(false);
            musicBackGround.UnPause();
            musicSelected.Stop();
            sFX[0]?.Invoke();

            TutorialManager.instance.TutorialListRenewal();
            TutorialManager.instance.TutorialStep();
        }
        // Custom
        if (_Button == btnMusicTheme[1])
        {
            btnMusicTheme[0].interactable = true;
            btnMusicTheme[1].interactable = false;
            CustomListRenewal();
            scrollView[0].SetActive(false);
            scrollView[1].SetActive(true);
            musicBackGround.UnPause();
            musicSelected.Stop();
            sFX[0]?.Invoke();
        }
    }

    // [Onclick Listener] 레벨 선택에 따른 이벤트(레벨에 따른 속도, 퀴즈 쿨타임 변경)
    public void OnClick_Level(Button _Button)
    {
        // Easy Selected
        if (_Button == btnLevels[0])
        {
            btnLevels[0].interactable = false;
            btnLevels[1].interactable = true;
            btnLevels[2].interactable = true;
            secPerBeat = 420f / bpm;
            PanelManager.instance.quizCool = 15;
            btnPlay.interactable = true;
            sFX[0]?.Invoke();

            TutorialManager.instance.TutorialStep();
        }
        // Normal Selected
        if (_Button == btnLevels[1])
        {
            btnLevels[0].interactable = true;
            btnLevels[1].interactable = false;
            btnLevels[2].interactable = true;
            secPerBeat = 360f / bpm;
            PanelManager.instance.quizCool = 10;
            btnPlay.interactable = true;
            sFX[0]?.Invoke();
        }
        // Hard Selected
        if (_Button == btnLevels[2])
        {
            btnLevels[0].interactable = true;
            btnLevels[1].interactable = true;
            btnLevels[2].interactable = false;
            secPerBeat = 300f / bpm;
            PanelManager.instance.quizCool = 5;
            btnPlay.interactable = true;
            sFX[0]?.Invoke();
        }
    }

    // [Onclick Listener] 로비 ---> 인게임
    public void OnClick_BtnPlay()
    {
        if (!TutorialManager.instance.isTutorial)
        {
            isStart = true;
            // Ray Controller OFF
            RayControllerMode(false);
            // 일시정지 인풋액션 활성화
            gamePause.action.started += XRI_InGamePause;
        }
        uiLobby.SetActive(false);
        lobbyBaseGround.SetActive(false);
        uiIngame.SetActive(true);
        inGameEnv.SetActive(true);
        musicBackGround.Pause();
        musicSelected.Stop();
        musicPlayed.Play();
        sFX[1]?.Invoke();

        TutorialManager.instance.TutorialStep();
    }

    // [XRI Input Action Binding(Primary Button)] 인게임 ---> 일시정지
    public void XRI_InGamePause(InputAction.CallbackContext context)
    {
        if (isStart && !isPause && !TutorialManager.instance.isTutorial)
        {
            isPause = true;
            // Music Paused UI On
            uiPause.SetActive(true);
            // Ray Controller ON
            RayControllerMode(true);
            // 플레이 중 노래 일시 정지
            Time.timeScale = 0;
            musicPlayed.Pause();
        }
    }

    // [Onclick Listener] 일시정지 ---> 메인
    public void OnClick_BtnPauseBackLobby()
    {
        if (isStart && isPause)
        {
            // [Event] 인게임 종료 이벤트
            EndResetEvent();
            uiLobby.SetActive(true);
            lobbyBaseGround.SetActive(true);
            uiIngame.SetActive(false);
            inGameEnv.SetActive(false);
            uiPause.SetActive(false);
            musicPlayed.Stop();
            sFX[0]?.Invoke();
            // 남은 패널 삭제
            foreach (Transform childNum in PanelManager.instance.panelSpawnPoint.transform)Destroy(childNum.gameObject);
            // 시간 정지 해제
            Time.timeScale = 1;
        }
    }

    // [Onclick Listener] 일시정지 ---> 인게임
    public void OnClick_BtnInGameUnPause()
    {
        isPause = false;
        // Music Paused UI OFF
        uiPause.SetActive(false);
        // Ray Controller OFF
        RayControllerMode(false);
        // 플레이 중 노래 일시 정지 해제
        Time.timeScale = 1;
        musicPlayed.UnPause();
        sFX[0]?.Invoke();
    }

    // [Event] 인게임 완곡 이후 이벤트
    public void InGameEnd()
    {
        // Ray Controller ON
        RayControllerMode(true);
        // Ingame Result Data Copy
        textKeys[0].text = PlayerPrefs.GetString("Title", $"{musicPlayed.clip.name}");
        if (!btnLevels[0].interactable) textKeys[1].text = PlayerPrefs.GetString("Level", "쉬움");
        if (!btnLevels[1].interactable) textKeys[1].text = PlayerPrefs.GetString("Level", "보통");
        if (!btnLevels[2].interactable) textKeys[1].text = PlayerPrefs.GetString("Level", "어려움");
        textKeys[2].text = PlayerPrefs.GetString("Score", $"{textIngameScore.text}");
        textKeys[3].text = PlayerPrefs.GetString("Kcal", $"{textIngameKcal.text}");
        // [Event] 인게임 종료 이벤트
        EndResetEvent();
        // UI Result ON
        uiResult.SetActive(true);
    }

    // [Event] 인게임 종료 이벤트
    public void EndResetEvent()
    {
        isStart = false;
        isPause = false;

        // 로비 관련 초기화
        infoTitle.text = "- Not Search";
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
        PanelManager.instance.panelSpawnCount = -1;
        PanelManager.instance.quizCool = 0;
        PanelManager.instance.curColor = "";
        PanelManager.instance.curLetter = "";
        PanelManager.instance.isQuiz = false;
        PanelManager.instance.isCurLeft = false;
        PanelManager.instance.isCurRight = false;
        // 스코어 초기화
        ScoreManaged.SetScore(score = 0);
        // 칼로리 초기화
        ScoreManaged.SetKcal(kcal = 0);
        // 콤보 초기화
        ComboManager.instance.Clear();
        // 인게임 플레이 타임 슬라이더 초기화
        playedMusicSlide.minValue = 0;
        playedMusicSlide.value = 0;
        // 일시정지 인풋액션 비활성화
        gamePause.action.started -= XRI_InGamePause;
        // BGM ON
        musicBackGround.UnPause();
    }

    // [Onclick Listener] 종료 ---> 결과 버튼
    public void OnClick_BtnEndBackLobby()
    {
        // Add Reuslt List
        if (!TutorialManager.instance.isTutorial)
        {
            GameObject resultElementPrefab = Instantiate(resultElement, contentResult.transform.position, contentResult.transform.rotation);
            resultElementPrefab.transform.parent = contentResult.transform;
            resultElementPrefab.transform.localScale = Vector3.one;
            btnReset.interactable = true;
        }
        uiLobby.SetActive(true);
        lobbyBaseGround.SetActive(true);
        uiIngame.SetActive(false);
        inGameEnv.SetActive(false);
        uiResult.SetActive(false);
        btnLevels[0].interactable = false;
        btnLevels[1].interactable = false;
        btnLevels[2].interactable = false;
        btnPlay.interactable = false;
        musicBackGround.UnPause();
        musicPlayed.Stop();
        sFX[0]!.Invoke();
        TutorialManager.instance.TutorialStep();
    }

    // [Onclick Listener] 결과 리스트 초기화
    public void OnClick_BtnReset()
    {
        foreach (Transform item in contentResult.transform) Destroy(item.gameObject);
        btnReset.interactable = false;
        sFX[0]!.Invoke();
    }

    // [Event] Ray Controller 표시
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

    // [Event] Original Music 폴더의 AudioClip 속성 파일 조회 ---> Original Music Element 생성
    public void OriginalListRenewal()
    {
        if (!TutorialManager.instance.isTutorial)
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
    }

    // [Event] Custom Music 폴더의 AudioClip 속성 파일 조회 ---> Custom Music Element 생성
    public void CustomListRenewal()
    {
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
    }

    // TimeFormatter Method
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

    // [Onclick] Quit Game Method
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

    // [Event] 이메일 인풋필드 선택
    public void ChangeEmail()
    {
        isPassword = false;
        isEmail = true;
    }

    // [Event] 패스워드 인풋필드 선택
    public void ChangePassword()
    {
        isEmail = false;
        isPassword = true;
    }
}

[Serializable]
public static class ScoreManaged
{
    // [Coroutine] 스코어 칼로리 증가
    public static IEnumerator Increase()
    {
        // SFX(Currect)
        GameManager.instance.sFX[1]?.Invoke();
        // Score
        if      /*x1*/ (0  <= ComboManager.instance.combo && ComboManager.instance.combo < 5)  SetScore(GameManager.instance.score += 1000);
        else if /*x2*/ (5  <= ComboManager.instance.combo && ComboManager.instance.combo < 10) SetScore(GameManager.instance.score += 2000);
        else if /*x4*/ (10 <= ComboManager.instance.combo && ComboManager.instance.combo < 15) SetScore(GameManager.instance.score += 4000);
        else if /*x8*/ (15 <= ComboManager.instance.combo)                                     SetScore(GameManager.instance.score += 8000);
        // Kcal
        SetKcal(GameManager.instance.kcal += Random.Range(0.05f, 0.15f));
        // Combo
        ComboManager.instance.IncreaseCombo();

        yield break;
    }

    // [Event] 스코어 증감 인게임 반영
    public static void SetScore(int score)
    {
        GameManager.instance.textIngameScore.text = score.ToString();
    }

    // [Event] 칼로리 증감 인게임 반영
    public static void SetKcal(float kcal)
    {
        GameManager.instance.textIngameKcal.text = kcal.ToString("F2");
    }
}
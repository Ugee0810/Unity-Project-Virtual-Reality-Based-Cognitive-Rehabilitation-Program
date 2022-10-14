/// <summary>
/// GameManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// 게임에서 발생하는 이벤트(버튼, 플래그)를 처리 합니다.
/// PanelManager에게 해당된 레벨에 따른 패턴을 지시합니다.
/// 오리지널 또는 커스텀 노래 조회 버튼을 눌렀을 때 라이브러리 내 음악을 조회 후 각 정보들을 Element들에게 전달합니다.
/// </summary>

using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("[SkyBox Rotate]")]
    [SerializeField] float rotateSpeed = 1f;

    [Header("[UI - 전체]")]
    public GameObject uiTutorial;      // UI Tutorial
    public GameObject uiLobby;         // UI Lobby
    public GameObject uiIngame;        // UI Ingame
    public GameObject uiPause;         // UI Pause
    public GameObject uiResult;        // UI Result

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

    [Header("[UI - 모드(패널 속도)]")]
    public Button btn70;
    public Button btn100;
    public Button btn130;

    [Header("[UI - 모드(노래 길이)]")]
    public Button btnHalf;
    public Button btnAll;

    [Header("[UI - 모드(장애물)]")]
    public Button btnObOn;
    public Button btnObOff;

    [Header("[UI - 노래 리스트]")]
    public Button btnOriginal;
    public Button btnCustom;

    public GameObject contentOriginal; // 오리지널 리소스 프리팹 생성 위치(부모)
    public GameObject contentCustom;   // 커스텀   리소스 프리팹 생성 위치(부모)
    public GameObject contentResult;   // 결과     리소스 프리팹 생성 위치(부모)

    public TMP_Text infoTitle;         // Panel Music Info - Music Title
    public TMP_Text textPsInfo;
    public TMP_Text textMlInfo;
    public TMP_Text textObsInfo;

    public Image imagePs70;
    public Image imagePs100;
    public Image imagePs130;
    public Image imageMl50;
    public Image imageMl100;
    public Image imageObsOn;
    public Image imageObsOff;

    public Button btnEasy;
    public Button btnNormal;
    public Button btnHard;
    public Button btnPlay;

    public GameObject bgTutorial;

    [Header("[UI - 결과]")]
    public Button btnReset;

    [Header("[UI - 인게임]")]
    public Slider playedMusicSlide;

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

    [Header("[InGame Data]")]
    [SerializeField] TMP_Text textIngameScore;
    [SerializeField] TMP_Text textIngameKcal;

    [Header("[Key]")]
    public TMP_Text textTitle;
    public TMP_Text textLevel;
    public TMP_Text textScore;
    public TMP_Text textKcal;

    [Header("[Music Info]")]
    public float playTime;
    public float playTimeOffset;
    public float halfPlayTime;
    public float halfHalfPlayTimeOffset;
    public float offsetTimer;
    public float moveSpeed = 2.0f;
    public float modePanelSpeed;
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
    public bool isRayState;    // True : Hand Controller | False : Lay Controller
    public bool isSensorLeft;  // 패널 접촉 유/무 왼쪽
    public bool isSensorRight; // 패널 접촉 유/무 오른쪽
    public bool isEmail;
    public bool isPassword;

    [Header("[InputActionReference]")]
    public InputActionReference gamePause = null;

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        // 초기화
        if (PlayerPrefs.HasKey("Title")) PlayerPrefs.SetString("Title", "-");
        if (PlayerPrefs.HasKey("Level")) PlayerPrefs.SetString("Level", "-");
        if (PlayerPrefs.HasKey("Score")) PlayerPrefs.SetInt("Score", 0);
        if (PlayerPrefs.HasKey("Kcal")) PlayerPrefs.SetInt("Kcal", 0);

        SetScore();
        SetKcal();
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);

        // Panel Speed
        if      /*70%*/  (!btn70.interactable)  modePanelSpeed = 0.7f;
        else if /*100%*/ (!btn100.interactable) modePanelSpeed = 1.0f;
        else if /*130%*/ (!btn130.interactable) modePanelSpeed = 1.3f;

        // Option
        bright = sliderBright.value;
        height = sliderHeight.value;

        // Level
        if      /*EasySelected*/   (!btnEasy.interactable)
        {
            secPerBeat = 420f / bpm;
            PanelManager.instance.quizCool = 15;
        }
        else if /*NormalSelected*/ (!btnNormal.interactable)
        {
            secPerBeat = 360f / bpm;
            PanelManager.instance.quizCool = 10;
        }
        else if /*HardSelected*/   (!btnHard.interactable)
        {
            secPerBeat = 300f / bpm;
            PanelManager.instance.quizCool = 5;
        }

        // Lobby UI
        if (!btn70.interactable)
            textPsInfo.text = "x0.7";
        else if (!btn100.interactable)
            textPsInfo.text = "x1.0";
        else if (!btn130.interactable)
            textPsInfo.text = "x1.3";

        if (!btnHalf.interactable)
            textMlInfo.text = "절반";
        else if (!btnAll.interactable)
            textMlInfo.text = "전부";

        if (!btnObOn.interactable)
            textObsInfo.text = "ON";
        else if (!btnObOff.interactable)
            textObsInfo.text = "OFF";
    }

    // [Onclick] 밝기 증가
    public void BrightInc()
    {
        if (0 <= bright && bright <= 2.1)
        {
            bright += 0.1f;
            sliderBright.value = bright;
        }
    }

    // [Onclick] 밝기 감소
    public void BrightDec()
    {
        if (0 <= bright && bright <= 2.1)
        {
            bright -= 0.1f;
            sliderBright.value = bright;
        }
    }

    // [Onclick] 키 조절 증가
    public void HeightInc()
    {
        if (1.9f <= height && height <= 2.1f)
        {
            height += 0.01f;
            sliderHeight.value = height;
        }
    }

    // [Onclick] 키 조절 감소
    public void HeightDec()
    {
        if (1.9f <= height && height <= 2.1f)
        {
            height -= 0.01f;
            sliderHeight.value = height;
        }
    }

    // [Onclick] 로비 ---> 인게임
    public void BtnPlay()
    {
        if (!TutorialManager.instance.isTutorial)
        {
            isStart = true;
            isRayState = false;
            gamePause.action.started += XRI_InGamePause;
        }
    }

    // [XRI Input Action Binding(Primary Buuton)] 인게임 ---> 일시정지
    public void XRI_InGamePause(InputAction.CallbackContext context)
    {
        if (isStart && !isPause && !TutorialManager.instance.isTutorial)
        {
            isPause = true;
            isRayState = true;

            // Music Paused UI On
            uiPause.SetActive(true);
            rayInteractorLeft.SetActive(true);
            rayInteractorRight.SetActive(true);

            // 플레이 중 노래 일시 정지
            Time.timeScale = 0;
            musicPlayed.Pause();
        }
    }

    // [Onclick] 일시정지 ---> 인게임
    public void BtnInGameUnPause()
    {
        isPause = false;
        isRayState = false;

        // Music Paused UI Off
        uiPause.SetActive(false);
        rayInteractorLeft.SetActive(false);
        rayInteractorRight.SetActive(false);

        // 플레이 중 노래 일시 정지 해제
        Time.timeScale = 1;
        musicPlayed.UnPause();
    }

    // [Onclick] 일시정지 ---> 메인
    public void BtnPauseBackLobby()
    {
        if (isStart && isPause)
        {
            isStart = false;
            isPause = false;

            Time.timeScale = 1;

            // 패널 생성 된 거 삭제
            int numOfChild = PanelManager.instance.panelSpawnPoint.transform.childCount;
            if (numOfChild != 0)
                for (int i = 0; i < PanelManager.instance.panelSpawnPoint.transform.childCount; i++)
                    Destroy(PanelManager.instance.panelSpawnPoint.transform.GetChild(i).gameObject);

            infoTitle.text = "- Not Search";
            // 노래, 패널 관련 초기화
            playTime = 0;
            playTimeOffset = 0;
            halfPlayTime = 0;
            halfHalfPlayTimeOffset = 0;
            offsetTimer = 0;
            modePanelSpeed = 0;
            bpm = 0;
            secPerBeat = 0;
            panelTimer = 0;
            PanelManager.instance.panelSpawnCount = -1;
            PanelManager.instance.quizCool   = 0;
            PanelManager.instance.curColor   = "";
            PanelManager.instance.curLetter  = "";
            PanelManager.instance.isQuiz     = false;
            PanelManager.instance.isCurLeft  = false;
            PanelManager.instance.isCurRight = false;

            // 스코어, 칼로리 초기화
            score = 0;
            kcal  = 0;
            SetScore();
            SetKcal();
            ComboManager.instance.Clear();

            // 인게임 플레이 타임 슬라이더 초기화
            playedMusicSlide.minValue = 0;
            playedMusicSlide.value = 0;

            gamePause.action.started -= XRI_InGamePause;
        }
    }

    // [Event] 인게임 종료
    public void InGameEnd()
    {
        isStart = false;
        isPause = false;
        isRayState = true;
        ControllerModeChange();

        // 로비 관련 초기화
        ResultData();
        //AddReusltList();
        uiResult.SetActive(true);
        musicBackGround.UnPause();
        btnEasy.interactable   = false;
        btnNormal.interactable = false;
        btnHard.interactable   = false;
        infoTitle.text = "- Not Search";

        // 노래, 패널 관련 초기화
        playTime = 0;
        playTimeOffset = 0;
        halfPlayTime = 0;
        halfHalfPlayTimeOffset = 0;
        offsetTimer = 0;
        modePanelSpeed = 0;
        bpm = 0;
        secPerBeat = 0;
        panelTimer = 0;
        PanelManager.instance.panelSpawnCount = -1;
        PanelManager.instance.quizCool   = 0;
        PanelManager.instance.curColor   = "";
        PanelManager.instance.curLetter  = "";
        PanelManager.instance.isQuiz     = false;
        PanelManager.instance.isCurLeft  = false;
        PanelManager.instance.isCurRight = false;

        // 스코어, 칼로리 초기화
        score = 0;
        kcal  = 0;
        SetScore();
        SetKcal();
        ComboManager.instance.Clear();

        // 인게임 플레이 타임 슬라이더 초기화
        playedMusicSlide.minValue = 0;
        playedMusicSlide.value = 0;

        gamePause.action.started -= XRI_InGamePause;
    }

    // [Event] 컨트롤러 변경
    public void ControllerModeChange()
    {
        if (isRayState)
        {
            rayInteractorLeft.SetActive(true);
            rayInteractorRight.SetActive(true);

            isRayState = false;
        }
        else
        {
            rayInteractorLeft.SetActive(false);
            rayInteractorRight.SetActive(false);

            isRayState = true;
        }
    }

    // [Event] 인게임 결과 복사
    public void ResultData()
    {
        textTitle.text = PlayerPrefs.GetString("Title", $"{musicPlayed.clip.name}");
        if (!btnEasy.interactable)
            textLevel.text = PlayerPrefs.GetString("Level", "Easy");
        else if (!btnNormal.interactable)
            textLevel.text = PlayerPrefs.GetString("Level", "Normal");
        else if (!btnHard.interactable)
            textLevel.text = PlayerPrefs.GetString("Level", "Hard");
        textScore.text = PlayerPrefs.GetString("Score", $"{textIngameScore.text}");
        textKcal.text = PlayerPrefs.GetString("Kcal", $"{textIngameKcal.text}");
    }

    // [Event] Original Music 폴더의 AudioClip 속성 파일 조회 ---> Original Music Element 생성
    public void OriginalListRenewal()
    {
        if (!TutorialManager.instance.isTutorial)
        {
            foreach (Transform item in contentOriginal.transform) Destroy(item.gameObject);

            // Original Music 폴더의 AudioClip 속성 파일 조회
            object[] originalMusics = Resources.LoadAll<AudioClip>("Original Music");

            for (int i = 0; i < originalMusics.Length; i++)
            {
                // AudioClip to GameObject
                GameObject originalMusicElementPrefab = originalMusics[i] as GameObject;
                originalMusicElementPrefab = Instantiate(musicElement, contentOriginal.transform.position, contentOriginal.transform.rotation);
                originalMusicElementPrefab.name = $"Original Music Element_{i}";
                originalMusicElementPrefab.transform.parent = contentOriginal.transform;
                originalMusicElementPrefab.transform.localScale = Vector3.one;

                // AudioSource.clip ← Resources-Custom Musics.AudioClip
                originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)originalMusics[i];
                // 분석한 BPM을 텍스트에 저장
                originalMusicElementPrefab.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = $"BPM : {UniBpmAnalyzer.AnalyzeBpm(originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip)}";
                // (float)MusicLength to (string)PlayTime
                originalMusicElementPrefab.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = TimeFormatter(originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length, false);
                // textTitle.text ← customMusicElements.AudioSource.text
                originalMusicElementPrefab.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
            }
        }
    }

    // [Event] Custom Music 폴더의 AudioClip 속성 파일 조회 ---> Custom Music Element 생성
    public void CustomListRenewal()
    {
        foreach (Transform item in contentCustom.transform) Destroy(item.gameObject);

        // Custom Music 폴더의 AudioClip 속성 파일 조회
        object[] customMusics = Resources.LoadAll<AudioClip>("Custom Music");

        for (int i = 0; i < customMusics.Length; i++)
        {
            // AudioClip to GameObject
            GameObject customMusicElementPrefab = customMusics[i] as GameObject;
            customMusicElementPrefab = Instantiate(musicElement, contentCustom.transform.position, contentCustom.transform.rotation);
            customMusicElementPrefab.name = $"Custom Music Element_{i}";
            customMusicElementPrefab.transform.parent = contentCustom.transform;
            customMusicElementPrefab.transform.localScale = Vector3.one;

            // AudioSource.clip ← Resources-Custom Musics.AudioClip
            customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)customMusics[i];
            // 분석한 BPM을 텍스트에 저장
            customMusicElementPrefab.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = $"BPM : {UniBpmAnalyzer.AnalyzeBpm(customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip)}";
            // (float)MusicLength to (string)PlayTime
            customMusicElementPrefab.transform.GetChild(1).gameObject.GetComponent<TMP_Text>().text = TimeFormatter(customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length, false);
            // textTitle.text ← customMusicElements.AudioSource.text
            customMusicElementPrefab.transform.GetChild(0).gameObject.GetComponent<TMP_Text>().text = customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }
    }

    // 결과 리스트 초기화 버튼 활성화
    public void AddReusltList()
    {
        if (!TutorialManager.instance.isTutorial)
        {
            GameObject resultElementPrefab = Instantiate(resultElement, contentResult.transform.position, contentResult.transform.rotation);
            resultElementPrefab.transform.parent = contentResult.transform;
            resultElementPrefab.transform.localScale = Vector3.one;

            btnReset.interactable = true;
        }
    }

    // 결과 리스트 초기화 버튼 비활성화
    public void ResultListReset()
    {
        foreach (Transform item in contentResult.transform)
            Destroy(item.gameObject);

        btnReset.interactable = false;
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

    // [Coroutine] 스코어 칼로리 증가
    public IEnumerator Increase()
    {
        yield return null;

        ComboManager.instance.IncreaseCombo();

        if (0 <= ComboManager.instance.combo && ComboManager.instance.combo < 5)
        {
            score += 1000;
            SetScore();
        }
        else if /*x2*/ (5 <= ComboManager.instance.combo && ComboManager.instance.combo < 10)
        {
            score += 2000;
            SetScore();
        }
        else if /*x4*/ (10 <= ComboManager.instance.combo && ComboManager.instance.combo < 15)
        {
            score += 4000;
            SetScore();
        }
        else if /*x8*/ (15 <= ComboManager.instance.combo)
        {
            score += 8000;
            SetScore();
        }

        kcal += Random.Range(0.1f, 0.2f);
        SetKcal();

        yield break;
    }

    // [Event] 스코어 증감 인게임 반영
    public void SetScore()
    {
        textIngameScore.text = score.ToString();
    }

    // [Event] 칼로리 증감 인게임 반영
    public void SetKcal()
    {
        textIngameKcal.text = kcal.ToString("F2");
    }

    public void ChangeEmail()
    {
        isPassword = false;
        isEmail = true;
    }

    public void ChangePassword()
    {
        isEmail = false;
        isPassword = true;
    }
}
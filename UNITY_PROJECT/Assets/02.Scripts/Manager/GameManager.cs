/// <summary>
/// GameManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// 게임에서 발생하는 이벤트(버튼, 플래그)를 처리 합니다.
/// PanelManager에게 해당된 레벨에 따른 패턴을 지시합니다.
/// 오리지널 또는 커스텀 노래 조회 버튼을 눌렀을 때 라이브러리 내 음악을 조회 후 각 정보들을 Element들에게 전달합니다.
/// </summary>

using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("[SkyBox Rotate]")]
    public float RotateSpeed = 0.1f;

    [Header("[UI]")]
    public GameObject uiLobby;            // UI Lobby
    public GameObject uiIngame;           // UI Ingame
    public GameObject uiPause;            // UI Pause
    public GameObject uiResult;           // UI Result

    public GameObject contentOriginal;    // 오리지널 리소스 프리팹 생성 위치(부모)
    public GameObject contentCustom;      // 커스텀   리소스 프리팹 생성 위치(부모)
    public GameObject contentResult;      // 결과     리소스 프리팹 생성 위치(부모)

    public Text infoTitle;                // Panel Music Info - Music Title

    public Button btnEasy;
    public Button btnNormal;
    public Button btnHard;

    public Button btnPlay;

    public Button btnReset;

    public Button btn70;
    public Button btn100;
    public Button btn130;

    public Button btnHalf;
    public Button btnAll;

    public Button btnObOn;
    public Button btnObOff;

    public Slider sliderBright;
    public Button btnBrightLeft;
    public Button btnBrightRight;

    public Slider sliderHeight;
    public Button btnHeightLeft;
    public Button btnHeightRight;

    public Slider inGameSlider;

    [Header("[Prefabs]")]
    public GameObject musicElement;
    public GameObject resultElement;

    [Header("[Origin Controller]")]
    public GameObject layLeftController;
    public GameObject layRightController;
    public GameObject handLeftController;
    public GameObject handRightController;

    [Header("[Audio Source]")]
    public AudioSource musicBackGround; // BGM
    public AudioSource musicSelected;   // Lobby Music
    public AudioSource musicPlayed;     // Ingame Music

    [Header("[InGame Data]")]
    public Text _IngameTextScore;
    public Text _IngameTextKcal;

    [Header("[Key]")]
    public Text _TextTitle;
    public Text _TextLevel;
    public Text _TextScore;
    public Text _TextKcal;

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
    public bool isHandChange;  // True : Hand Controller | False : Lay Controller
    public bool isSensorLeft;  // 패널 접촉 유/무 왼쪽
    public bool isSensorRight; // 패널 접촉 유/무 오른쪽

    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        if (PlayerPrefs.HasKey("Title")) PlayerPrefs.SetString("Title", "-");
        if (PlayerPrefs.HasKey("Level")) PlayerPrefs.SetString("Level", "-");
        if (PlayerPrefs.HasKey("Score")) PlayerPrefs.SetInt("Score", 0);
        if (PlayerPrefs.HasKey("Kcal"))  PlayerPrefs.SetInt("Kcal", 0);
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * RotateSpeed);

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
    }

    // [Button] 밝기 증가
    public void BrightInc()
    {
        if (0 <= bright && bright <= 2.1)
        {
            bright += 0.1f;
            sliderBright.value = bright;
        }
    }

    // [Button] 밝기 감소
    public void BrightDec()
    {
        if (0 <= bright && bright <= 2.1)
        {
            bright -= 0.1f;
            sliderBright.value = bright;
        }
    }

    // [Button] 키 조절 증가
    public void HeightInc()
    {
        if (1.0f <= height && height <= 1.2f)
        {
            height += 0.01f;
            sliderHeight.value = height;
        }
    }

    // [Button] 키 조절 감소
    public void HeightDec()
    {
        if (1.0f <= height && height <= 1.2f)
        {
            height -= 0.01f;
            sliderHeight.value = height;
        }
    }

    // [Button] 로비 ---> 인게임
    public void BtnPlay()
    {
        isStart = true;
        isHandChange = true;
    }

    // [Button] 인게임 ---> 일시정지
    public void BtnInGamePause()
    {
        if (isStart && !isPause)
        {
            isPause = true;
            isHandChange = false;

            // Music Paused UI On
            uiPause.SetActive(true);

            // 플레이 중 노래 일시 정지
            Time.timeScale = 0;
            musicPlayed.Pause();
        }
    }

    // [Button] 일시정지 ---> 인게임
    public void BtnInGameUnPause()
    {
        isPause = false;
        isHandChange = true;

        // Music Paused UI Off
        uiPause.SetActive(false);

        // 플레이 중 노래 일시 정지 해제
        Time.timeScale = 1;
        musicPlayed.UnPause();
    }

    // [Button] 일시정지 ---> 메인
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
            ScoreManager.instance.SetScore();
            ScoreManager.instance.SetKcal();
            ComboManager.instance.Clear();

            // 인게임 플레이 타임 슬라이더 초기화
            inGameSlider.minValue = 0;
            inGameSlider.value = 0;
        }
    }

    // [Event] 인게임 종료
    public void InGameEnd()
    {
        isStart = false;
        isPause = false;
        isHandChange = false;
        ControllerModeChange();

        // 로비 관련 초기화
        ResultData();
        uiResult.SetActive(true);
        musicBackGround.UnPause();
        btnEasy.interactable = false;
        btnNormal.interactable = false;
        btnHard.interactable = false;

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
        ScoreManager.instance.SetScore();
        ScoreManager.instance.SetKcal();
        ComboManager.instance.Clear();

        // 인게임 플레이 타임 슬라이더 초기화
        inGameSlider.minValue = 0;
        inGameSlider.value = 0;
    }

    // [Event] 컨트롤러 변경
    public void ControllerModeChange()
    {
        if /*Hand Controller*/ (isHandChange)
        {
            layLeftController.SetActive(false);
            layRightController.SetActive(false);

            handLeftController.SetActive(true);
            handRightController.SetActive(true);

            isHandChange = false;
        }
        else /*Lay Controller*/
        {
            handLeftController.SetActive(false);
            handRightController.SetActive(false);

            layLeftController.SetActive(true);
            layRightController.SetActive(true);

            isHandChange = true;
        }
    }

    // [Event] 인게임 결과 복사
    public void ResultData()
    {
        _TextTitle.text = PlayerPrefs.GetString("Title", $"{musicPlayed.clip.name}");
        if      (!btnEasy.interactable)
            _TextLevel.text = PlayerPrefs.GetString("Level", "Easy");
        else if (!btnNormal.interactable)
            _TextLevel.text = PlayerPrefs.GetString("Level", "Normal");
        else if (!btnHard.interactable)
            _TextLevel.text = PlayerPrefs.GetString("Level", "Hard");
        _TextScore.text = PlayerPrefs.GetString("Score", $"{_IngameTextScore.text}");
        _TextKcal.text = PlayerPrefs.GetString("Kcal", $"{_IngameTextKcal.text}");
    }

    // [Event] Original Music 폴더의 AudioClip 속성 파일 조회 ---> Original Music Element 생성
    public void OriginalListRenewal()
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
            // (float)MusicLength to (string)PlayTime
            originalMusicElementPrefab.transform.GetChild(2).gameObject.GetComponent<Text>().text = TimeFormatter(originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length, false);
            // textTitle.text ← customMusicElements.AudioSource.text
            originalMusicElementPrefab.transform.GetChild(1).gameObject.GetComponent<Text>().text = originalMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
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
            // (float)MusicLength to (string)PlayTime
            customMusicElementPrefab.transform.GetChild(2).gameObject.GetComponent<Text>().text = TimeFormatter(customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length, false);
            // textTitle.text ← customMusicElements.AudioSource.text
            customMusicElementPrefab.transform.GetChild(1).gameObject.GetComponent<Text>().text = customMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }
    }

    // 결과 리스트 초기화 버튼 활성화
    public void AddReusltList()
    {
        GameObject resultElementPrefab = Instantiate(resultElement, contentResult.transform.position, contentResult.transform.rotation);
        resultElementPrefab.transform.parent = contentResult.transform;
        resultElementPrefab.transform.localScale = Vector3.one;

        btnReset.interactable = true;
    }

    // 결과 리스트 초기화 버튼 비활성화
    public void ResultListReset()
    {
        foreach (Transform item in contentResult.transform)
            Destroy(item.gameObject);

        btnReset.interactable = false;
    }

    // TimeFormatter Method
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

    // Quit Game Method
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
}
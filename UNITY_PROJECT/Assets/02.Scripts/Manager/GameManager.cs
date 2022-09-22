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
using UnityEngine.Events;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("[UI]")]
    public GameObject _UILobby;           // UI Lobby
    public GameObject _UIIngame;          // UI Ingame
    public GameObject _UIPause;           // UI Pause
    public GameObject _UIResult;          // UI Result
    public GameObject originalScrollView; // UI Original View
    public GameObject customScrollView;   // UI Custom View
    public GameObject contentOriginal;    // 오리지널 리소스 프리팹 생성 위치(부모)
    public GameObject contentCustom;      // 커스텀   리소스 프리팹 생성 위치(부모)
    public GameObject contentResult;      // 결과     리소스 프리팹 생성 위치(부모)
    public Text infoTitle;                // Panel Music Info - Music Title
    public Button btnOriginal;            // Original List
    public Button btnCustom;              // Custom List
    public Button btnEasy;               
    public Button btnNormal;             
    public Button btnHard;      
    public Button btnPlay;                // Game Start
    public Button btnReset;
    public Button btn70;
    public Button btn100;
    public Button btn130;
    public Button btnHalf;
    public Button btnAll;
    public Button btnObOn;
    public Button btnObOff;

    [Header("[Environment Objects]")]
    public GameObject baseGround;         // GO Lobby
    public GameObject inGameEnvironment;  // GO Ingame

    [Header("[Prefabs]")]
    public GameObject musicElement;
    public GameObject resultElement;

    [Header("[Origin Controller]")]
    public GameObject layLeftController;
    public GameObject layRightController;
    public GameObject handLeftController;
    public GameObject handRightController;

    [Header("[Music Info]")]
    public float playTime;
    public float playTimeOffset; // 패널 젠 시간
    public float offsetTimer;
    public int   bpm;
    public float secPerBeat;
    public float panelTimer; // BPM 계산 타이머
    public float moveSpeed = 2.0f;

    [Header("[Score & Kcal]")]
    public int   score = 0;
    public float kcal  = 0;

    [Header("[Audio Source]")]
    public AudioSource musicBackGround; // BGM
    public AudioSource musicSelected;   // Lobby Music
    public AudioSource musicPlayed;     // Ingame Music

    [Header("[InGame Data]")]
    public Text _IngameTextScore;
    public Text _IngameTextKacl;

    [Header("[Key]")]
    public Text _TextTitle;
    public Text _TextLevel;
    public Text _TextScore;
    public Text _TextKcal;

    [Header("[Mode]")]
    public float modePanelSpeed;
    public float modeHalfPlayTime;
    public float modeHalfPlayTimeOffset;

    // [Header("[플래그 변수]")]
    [HideInInspector] public bool isStart;       // Game Start
    [HideInInspector] public bool isPause;       // Game Pause
    [HideInInspector] public bool isHandChange;  // True : Hand Controller | False : Lay Controller
    [HideInInspector] public bool isSensorLeft;  // 패널 접촉 유/무 왼쪽
    [HideInInspector] public bool isSensorRight; // 패널 접촉 유/무 오른쪽

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

        Btn100();
        BtnObOn();
}

    public UnityEvent endEvent;
    private void Update()
    {
        if (isStart)
        {
            if (isPause) return;

            if (!musicPlayed.isPlaying && !isPause)
            {
                endEvent?.Invoke();
            }
        }
    }

    public void Btn70()
    {
        modePanelSpeed = 0.7f;
    }

    public void Btn100()
    {
        modePanelSpeed = 1f;
    }

    public void Btn130()
    {
        modePanelSpeed = 1.3f;
    }

    public void BtnObOn()
    {
    }

    public void BtnObOff()
    {
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

    // [Button] Original MusicList Selected
    public void BtnOriginalSelected()
    {
        originalScrollView.SetActive(true);
        customScrollView.SetActive(false);
        OriginalListRenewal();

        btnOriginal.interactable = false;
        btnCustom.interactable = true;

        musicSelected.Stop();
        musicBackGround.UnPause();
    }

    // [Button] Custom MusicList Selected
    public void BtnCustomSelected()
    {
        originalScrollView.SetActive(false);
        customScrollView.SetActive(true);
        CustomListRenewal();

        btnOriginal.interactable = true;
        btnCustom.interactable = false;

        musicSelected.Stop();
        musicBackGround.UnPause();
    }

    // [Button] Easy
    public void BtnLvEasy()
    {
        secPerBeat = 420f / bpm;

        PanelManager.instance.quizCool = 15;
        btnEasy.interactable   = false;
        btnNormal.interactable = true;
        btnHard.interactable   = true;
        btnPlay.interactable   = true; // 노래 재생(Play) 버튼 활성화
    }

    // [Button] Normal
    public void BtnLvNormal()
    {
        secPerBeat = 360f / bpm;

        PanelManager.instance.quizCool = 10;
        btnEasy.interactable   = true;
        btnNormal.interactable = false;
        btnHard.interactable   = true;
        btnPlay.interactable   = true; // 노래 재생(Play) 버튼 활성화
    }

    // [Button] Hard
    public void BtnLvHard()
    {
        secPerBeat = 300f / bpm;

        PanelManager.instance.quizCool = 5;
        btnEasy.interactable   = true;
        btnNormal.interactable = true;
        btnHard.interactable   = false;
        btnPlay.interactable   = true; // 노래 재생(Play) 버튼 활성화
    }

    // [Button] Play
    public void BtnPlay()
    {
        _UILobby.SetActive(false);
        baseGround.SetActive(false);
        _UIIngame.SetActive(true);
        inGameEnvironment.SetActive(true);

        isStart = true;

        musicBackGround.Pause();
        musicSelected.Stop();
        musicPlayed.Play();

        isHandChange = true;
        ControllerModeChange();
    }

    // [Button] Pause
    public void BtnInGamePause()
    {
        if (isStart)
        {
            // Music Paused UI On
            _UIPause.SetActive(true);

            // 플레이 중 노래 일시 정지
            Time.timeScale = 0;
            musicPlayed.Pause();

            isPause = true;
            isHandChange = false;
            ControllerModeChange();
        }
    }

    // [Button] UnPause
    public void BtnInGameUnPause()
    {
        if (isStart && isPause)
        {
            // Music Paused UI Off
            _UIPause.SetActive(false);

            // 플레이 중 노래 일시 정지 해제
            Time.timeScale = 1;
            musicPlayed.UnPause();

            isPause = false;
            isHandChange = true;
            ControllerModeChange();
        }
    }

    // [Button] Pause to Back to the Lobby
    public void BtnPauseBackLobby()
    {
        if (isStart && isPause)
        {
            // 패널 생성 된 거 삭제
            int numOfChild = PanelManager.instance.panelSpawnPoint.transform.childCount;
            if (numOfChild != 0)
                for (int i = 0; i < PanelManager.instance.panelSpawnPoint.transform.childCount; i++)
                    Destroy(PanelManager.instance.panelSpawnPoint.transform.GetChild(i).gameObject);

            panelTimer = 0;
            offsetTimer = 0;
            secPerBeat = 0;
            PanelManager.instance.panelSpawnCount = -1;
            PanelManager.instance.panelLastIndex  = -1;
            PanelManager.instance.isQuiz = false;
            PanelManager.instance.isCurLeft = false;
            PanelManager.instance.isCurRight = false;

            btnPlay.interactable = false;

            _UIIngame.SetActive(false);
            _UIPause.SetActive(false);
            inGameEnvironment.SetActive(false);
            baseGround.SetActive(true);

            Time.timeScale = 1;
            musicBackGround.UnPause();
            musicPlayed.Stop();

            score = 0;
            kcal = 0;
            ScoreManager.instance.SetScore();
            ScoreManager.instance.SetKcal();
            
            btnEasy.interactable = false;
            btnNormal.interactable = false;
            btnHard.interactable = false;

            isStart = false;
            isPause = false;
        }
    }

    public void EndEvent()
    {
        // 패널 생성 된 거 삭제
        int numOfChild = PanelManager.instance.panelSpawnPoint.transform.childCount;
        if (numOfChild != 0)
            for (int i = 0; i < PanelManager.instance.panelSpawnPoint.transform.childCount; i++)
                Destroy(PanelManager.instance.panelSpawnPoint.transform.GetChild(i).gameObject);

        panelTimer = 0;
        offsetTimer = 0;
        secPerBeat = 0;
        PanelManager.instance.panelSpawnCount = -1;
        PanelManager.instance.panelLastIndex  = -1;
        PanelManager.instance.isQuiz = false;
        PanelManager.instance.isCurLeft = false;
        PanelManager.instance.isCurRight = false;

        musicBackGround.UnPause();
        ResultData();
        _UIResult.SetActive(true);
        score = 0;
        kcal = 0;
        ScoreManager.instance.SetScore();
        ScoreManager.instance.SetKcal();

        btnEasy.interactable = false;
        btnNormal.interactable = false;
        btnHard.interactable = false;

        isStart = false;
        isPause = false;
        isHandChange = false;
        ControllerModeChange();
    }

    // [Button] End to Back to the Lobby
    public void BtnEndBackLobby()
    {
        AddReusltList();

        _UIIngame.SetActive(false);
        inGameEnvironment.SetActive(false);
        _UIResult.SetActive(false);
        _UILobby.SetActive(true);
        baseGround.SetActive(true);

        btnPlay.interactable = false;
    }

    void ControllerModeChange()
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
        _TextKcal.text = PlayerPrefs.GetString("Kcal", $"{_IngameTextKacl.text}");
    }

    void OriginalListRenewal()
    {
        foreach (Transform item in contentOriginal.transform) Destroy(item.gameObject);

        // Custom Music 폴더의 AudioClip 속성 파일 조회
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

    void CustomListRenewal()
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

    void AddReusltList()
    {
        GameObject resultElementPrefab = Instantiate(resultElement, contentResult.transform.position, contentResult.transform.rotation);
        resultElementPrefab.transform.parent = contentResult.transform;
        resultElementPrefab.transform.localScale = Vector3.one;

        btnReset.interactable = true;
    }

    // [Button] 결과 리스트 초기화
    public void ResultListReset()
    {
        foreach (Transform item in contentResult.transform) Destroy(item.gameObject);

        btnReset.interactable = false;
    }
}
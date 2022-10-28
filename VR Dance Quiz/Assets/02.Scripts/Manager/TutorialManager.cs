/// <summary>
/// TutorialManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// - 튜토리얼 과정을 담당하는 스크립트입니다.
/// - TextAnimatorPlayer 클래스를 통해 Text Animation 에셋을 사용합니다.
/// - TutorialStart() 인터페이스에서 각 함수들이 yield return new WaitWhile(() => tutorialStep < ?)의 제약동안 실행됩니다.
/// - Tuple방식(Vector3, Vector2, Quaternion)으로 리턴 값을 받는 XR_TutoCanvasSize() 메소드를 통해 캔버스 사이즈와 위치, 회전 값을 정의 했습니다.
/// </summary>

using Febucci.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : Singleton<TutorialManager>
{
    [Header("[UI]")]
    [SerializeField] RectTransform xrTutoCanvas;
    [SerializeField] Button btnTutoNext;
    [SerializeField] TextAnimatorPlayer textAnimatorPlayer;
    [SerializeField] TMP_Text textTutoOriginal;
    [SerializeField] TMP_Text textTutoEasy;
    [SerializeField] TMP_Text textTutoPlay;

    [Header("[Message]")]
    public int tutorialStep;
    public string[] textBox = {
        "<speed=0.5><rainb f=0.2>안녕하세요.</rainb> 반갑습니다." +
            "\n지금부터 플레이 방법을 안내해드리겠습니다.",
        "<speed=0.5>상단의 <rainb f=0.2>[Original]</rainb> 테마를 선택해주세요.",
        "<speed=0.5>노래<rainb f=0.2>[Cat Life]</rainb>를 선택해주세요.",
        "<speed=0.5>난이도 <rainb f=0.2>[Easy]</rainb>을 선택해주세요.",
        "<speed=0.5><rainb f=0.2>[Play]</rainb> 버튼을 눌러 게임을 시작합니다.",
        "<speed=0.5><size=7>게임으로 진입했습니다." +
            "\n\n좌측에는 획득한 <bounce a=0.3 f=0.3>점수</bounce>와 <bounce a=0.3 f=0.3>소모된 칼로리</bounce>가 표시됩니다." +
            "\n동작 또는 퀴즈에 성공하면 점수와 콤보가 오릅니다." +
            "\n동작에 성공하면 소모량이 오릅니다." +
            "\n\n우측 하단에서 <bounce a=0.3 f=0.3>자신의 동작을 확인</bounce>할 수 있습니다." +
            "\n\n바닥에는 <bounce a=0.3 f=0.3>노래의 길이</bounce>를 알 수 있습니다.",
        "<speed=0.5>모션 패널을 해결해보세요.",
        "<speed=0.5>장애물 패널이 나왔습니다." +
            "\n몸을 이동하여 피해주세요." +
            "\n피격 시 콤보와 점수를 잃습니다.",
        "<speed=0.5>퀴즈 패널이 나왔습니다." +
            "\n좌/우로 색상과 글귀를 제시 받습니다.\n동작을 해결하면서 외워주세요.",
        "<speed=0.5>정답을 맞추는 패널입니다." +
            "\n외웠던 색상과 글귀가 일치하는 방향을 선택하세요.",
        "<speed=0.5>노래를 완수하게 되면 결과창이 표시됩니다." +
            "\n노래 제목/난이도/점수/소모된 칼로리를 알 수 있습니다." +
            "\n메인으로 돌아갑시다.",
        "<speed=0.5>밝기와 키 조절은 로비의 왼쪽에 있습니다." +
            "\n튜토리얼을 마치겠습니다." };

    [Header("[Panel]")]
    public GameObject[] tutoPanels;
    public int tutoPanelSpawnCount;
    public int tutoPanelDestroyCount;
    public float tutoMoveSpeed = 2.0f;

    [Header("[Music]")]
    public float tutoSecPerBeat = 3.5f;
    public float tutoPanelTimer; // BPM 계산 타이머

    [Header("[플래그 변수]")]
    public bool isTutorial;
    public bool isMotionClear;
    public bool isObstacleClear;
    public bool isMotionQuizClear;
    public bool isQuizClear;

    private void Awake()
    {
        // Reset
        tutorialStep = 0;
        isMotionClear = false;
        isObstacleClear = false;
        isMotionQuizClear = false;
        isQuizClear = false;
    }

    private void FixedUpdate()
    {
        // 모션 패널 시작 했을 때
        if (tutorialStep == 7 && !isMotionClear)
        {
            PanelSpawn();
            if (tutoPanelDestroyCount == 3)
                tutorialStep++;
        }
        // 장애물 패널 시작했을 때
        else if (tutorialStep == 9 && !isObstacleClear)
        {
            PanelSpawn();
            if (tutoPanelDestroyCount == 4)
                tutorialStep++;
        }
        // 모션 퀴즈 패널 시작했을 때
        else if (tutorialStep == 11 && !isMotionQuizClear)
        {
            PanelSpawn();
            if (tutoPanelDestroyCount == 5)
                tutorialStep++;
        }
        // 퀴즈 패널 시작했을 때
        else if (tutorialStep == 13 && !isQuizClear)
        {
            PanelSpawn();
            if (tutoPanelDestroyCount == 6)
                tutorialStep++;
        }
    }

    public IEnumerator TutorialStart()
    {
        Step1();
        yield return new WaitWhile(() => tutorialStep < 1);

        Step2();
        yield return new WaitWhile(() => tutorialStep < 2);

        Step3();
        yield return new WaitWhile(() => tutorialStep < 3);

        Step4();
        yield return new WaitWhile(() => tutorialStep < 4);

        Step5();
        yield return new WaitWhile(() => tutorialStep < 5);

        Step6();
        yield return new WaitWhile(() => tutorialStep < 6);

        Step7();
        yield return new WaitWhile(() => tutorialStep < 7);

        Step8();
        yield return new WaitWhile(() => tutorialStep < 8);

        Step9();
        yield return new WaitWhile(() => tutorialStep < 9);

        Step10();
        yield return new WaitWhile(() => tutorialStep < 10);

        Step11();
        yield return new WaitWhile(() => tutorialStep < 11);

        Step12();
        yield return new WaitWhile(() => tutorialStep < 12);

        Step13();
        yield return new WaitWhile(() => tutorialStep < 13);

        Step14();
        yield return new WaitWhile(() => tutorialStep < 14);

        Step15();
        yield return new WaitWhile(() => tutorialStep < 15);

        Step16();
        yield return new WaitWhile(() => tutorialStep < 16);

        Step17();
        yield return new WaitWhile(() => tutorialStep < 17);
    }

    (Vector3, Vector2, Quaternion) XR_TutoCanvasSize(int tutorialStep)
    {
        if      (tutorialStep == 0) return (new Vector3(0f, 2f, 1f), new Vector2(200f, 50f), Quaternion.Euler(10f, 0f, 0f));
        else if (tutorialStep == 1) return (new Vector3(0f, 1.3f, 0.85f), new Vector2(180f, 20f), Quaternion.Euler(50f, 0f, 0f));
        else if (tutorialStep == 5) return (new Vector3(0f, 2.25f, 1f), new Vector2(200f, 110f), Quaternion.Euler(0f, 0f, 0f));
        else if (tutorialStep == 14) return (new Vector3(0f, 2.9f, 0.9f), new Vector2(250f, 70f), Quaternion.Euler(-15f, 0f, 0f));
        else if (tutorialStep == 15) return (new Vector3(0f, 1.3f, 0.85f), new Vector2(180f, 40f), Quaternion.Euler(50f, 0f, 0f));
        else return (new Vector3(0f, 0f, 0f), new Vector2(0f, 0f), Quaternion.identity);
    }

    // 튜토리얼 시작 안내
    void Step1()
    {
        Singleton<GameManager>.Instance.uiTutorial.SetActive(true);
        Singleton<GameManager>.Instance.uiLobby.SetActive(false);

        foreach (Transform item in Singleton<GameManager>.Instance.contentOriginal.transform)
            Destroy(item.gameObject);
        foreach (Transform item in Singleton<GameManager>.Instance.contentCustom.transform)  
            Destroy(item.gameObject);

        (xrTutoCanvas.transform.position, xrTutoCanvas.sizeDelta, xrTutoCanvas.transform.rotation) = XR_TutoCanvasSize(tutorialStep);
        textAnimatorPlayer.ShowText(textBox[0]);
    }

    // Theme(Original) 선택
    void Step2()
    {
        // Tutorial Button OFF
        btnTutoNext.interactable = false;
        // UI Lobby ON
        Singleton<GameManager>.Instance.uiLobby.SetActive(true);
        // UI Option(Lobby Left UI) OFF
        Singleton<GameManager>.Instance.uiLobbyOption.SetActive(false);
        // UI Result(Lobby Right UI) OFF
        Singleton<GameManager>.Instance.uiLobbyResult.SetActive(false);
        // Original Theme Select ON
        Singleton<GameManager>.Instance.btnMusicTheme[0].interactable = true;
        // Custom Theme Select OFF
        Singleton<GameManager>.Instance.btnMusicTheme[1].interactable = false;
        // Levels OFF
        for (int i = 0; i < Singleton<GameManager>.Instance.btnLevels.Length; i++)
            Singleton<GameManager>.Instance.btnLevels[i].interactable = false;
        // Play OFF
        Singleton<GameManager>.Instance.btnPlay.interactable = false;
        // Tutorial(Btn) OFF
        Singleton<GameManager>.Instance.bgTutorial.SetActive(false);
        // InfoTitle Reset
        Singleton<GameManager>.Instance.infoTitle.text = "※ Not Search";
        // 안내 문구 강조
        textTutoOriginal.text = "<bounce a=0.5 f=0.5>Original</bounce>";

        (xrTutoCanvas.transform.position, xrTutoCanvas.sizeDelta, xrTutoCanvas.transform.rotation) = XR_TutoCanvasSize(tutorialStep);

        textAnimatorPlayer.ShowText(textBox[1]);
    }

    // Music(Cat Life) 선택
    void Step3()
    {
        // Custom Theme Select OFF
        Singleton<GameManager>.Instance.btnMusicTheme[1].interactable = false;
        // 안내 문구 강조 OFF
        textTutoOriginal.text = "Original";

        textAnimatorPlayer.ShowText(textBox[2]);
    }

    // Level(Easy) 선택
    void Step4()
    {
        // Easy ON
        Singleton<GameManager>.Instance.btnLevels[0].interactable = true;
        // Normal OFF
        Singleton<GameManager>.Instance.btnLevels[1].interactable = false;
        // Hard OFF
        Singleton<GameManager>.Instance.btnLevels[2].interactable = false;
        // Music Element OFF
        Singleton<GameManager>.Instance.contentOriginal.transform.GetChild(0).GetComponent<Button>().interactable = false;
        // 안내 문구 강조 OFF
        Singleton<GameManager>.Instance.contentOriginal.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = "Cat Life";
        // 안내 문구 강조 ON
        textTutoEasy.text = "<bounce a=0.5 f=0.5>Easy</bounce>";

        textAnimatorPlayer.ShowText(textBox[3]);
    }

    // Play 버튼 안내
    void Step5()
    {
        // Levels OFF
        for (int i = 0; i < Singleton<GameManager>.Instance.btnLevels.Length; i++) Singleton<GameManager>.Instance.btnLevels[i].interactable = false;
        // 안내 문구 강조 OFF
        textTutoEasy.text = "Easy";
        // 안내 문구 강조 ON
        textTutoPlay.text = "<bounce a=0.5 f=0.5>Play</bounce>";

        textAnimatorPlayer.ShowText(textBox[4]);
    }

    // 인게임 요소 설명
    void Step6() 
    {
        // 안내 문구 강조 OFF
        textTutoPlay.text = "Play";

        // Tutorial Button ON
        btnTutoNext.interactable = true;

        // TimeStop, HandChange
        StartCoroutine(TimeStop());

        (xrTutoCanvas.transform.position, xrTutoCanvas.sizeDelta, xrTutoCanvas.transform.rotation) = XR_TutoCanvasSize(tutorialStep);
        textAnimatorPlayer.ShowText(textBox[5]);
    }

    // 모션 패널 안내
    void Step7()
    {
        textAnimatorPlayer.ShowText(textBox[6]);
    }

    // 모션 패널 시작
    void Step8()
    {
        // UI Tutorial OFF
        Singleton<GameManager>.Instance.uiTutorial.SetActive(false);
        // TimeStart, HandChange
        StartCoroutine(TimeStart());
    }

    // 장애물 패널 안내
    void Step9()
    {
        // 모션 패널 클리어
        isMotionClear = true;
        // UI Tutorial ON
        Singleton<GameManager>.Instance.uiTutorial.SetActive(true);
        // TimeStop, HandChange
        StartCoroutine(TimeStop());

        textAnimatorPlayer.ShowText(textBox[7]);
    }

    // 장애물 패널 시작
    void Step10()
    {
        // UI Tutorial OFF
        Singleton<GameManager>.Instance.uiTutorial.SetActive(false);
        // TimeStart, HandChange
        StartCoroutine(TimeStart());
    }

    // 모션 퀴즈 패널 안내
    void Step11() 
    {
        // 장애물 패널 클리어
        isObstacleClear = true;
        // UI Tutorial ON
        Singleton<GameManager>.Instance.uiTutorial.SetActive(true);
        // TimeStop, HandChange
        StartCoroutine(TimeStop());
        textAnimatorPlayer.ShowText(textBox[8]);
    }

    // 모션 퀴즈 패널 시작
    void Step12() 
    {
        // UI Tutorial OFF
        Singleton<GameManager>.Instance.uiTutorial.SetActive(false);
        // TimeStart, HandChange
        StartCoroutine(TimeStart());
    }

    // 퀴즈 패널 안내
    void Step13() 
    {
        // 모션 퀴즈 패널 클리어
        isMotionQuizClear = true;
        // UI Tutorial ON
        Singleton<GameManager>.Instance.uiTutorial.SetActive(true);
        // TimeStop, HandChange
        StartCoroutine(TimeStop());
        textAnimatorPlayer.ShowText(textBox[9]);
    }

    // 퀴즈 패널 시작
    void Step14() 
    {
        // UI Tutorial OFF
        Singleton<GameManager>.Instance.uiTutorial.SetActive(false);
        // TimeStart, HandChange
        StartCoroutine(TimeStart());
    }

    // 결과창 출력
    void Step15() 
    {
        (xrTutoCanvas.transform.position, xrTutoCanvas.sizeDelta, xrTutoCanvas.transform.rotation) = XR_TutoCanvasSize(tutorialStep);
        // 퀴즈 패널 클리어
        isQuizClear = true;
        // Tutorial Button OFF
        btnTutoNext.interactable = false;
        // UI Tutorial ON
        Singleton<GameManager>.Instance.uiTutorial.SetActive(true);
        // TimeStop, HandChange
        StartCoroutine(TimeStop());
        textAnimatorPlayer.ShowText(textBox[10]);

        Singleton<GameManager>.Instance.InGameEnd();
        foreach (Transform item in Singleton<GameManager>.Instance.contentOriginal.transform)
            Destroy(item.gameObject);
    }

    // 옵션 안내 후 종료
    void Step16()
    {
        (xrTutoCanvas.transform.position, xrTutoCanvas.sizeDelta, xrTutoCanvas.transform.rotation) = XR_TutoCanvasSize(tutorialStep);

        // Tutorial Button ON
        btnTutoNext.interactable = true;
        textAnimatorPlayer.ShowText(textBox[11]);
    }

    // 초기화 및 로비
    void Step17()
    {
        Time.timeScale = 1;
        // UI Tutorial OFF
        Singleton<GameManager>.Instance.uiTutorial.SetActive(false);
        // UI Option(Lobby Left UI) ON
        Singleton<GameManager>.Instance.uiLobbyOption.SetActive(true);
        // UI Result(Lobby Right UI) ON
        Singleton<GameManager>.Instance.uiLobbyResult.SetActive(true);
        // Original Theme Select ON
        Singleton<GameManager>.Instance.btnMusicTheme[0].interactable = true;
        // Custom Theme Select ON
        Singleton<GameManager>.Instance.btnMusicTheme[1].interactable = true;
        // Tutorial(Btn) ON
        Singleton<GameManager>.Instance.bgTutorial.SetActive(true);

        tutorialStep = 0;
        tutoPanelSpawnCount = 0;
        tutoPanelDestroyCount = 0;
        tutoPanelTimer = 0;
        isMotionClear = false;
        isObstacleClear = false;
        isMotionQuizClear = false;
        isQuizClear = false;

        isTutorial = false;
        StopCoroutine(TutorialStart());
    }

    // [OnClick] tutorialStep++
    public void TutorialStep()
    {
        if (isTutorial) tutorialStep++;
    }

    // [OnClick] 로비 ---> 튜토리얼 버튼
    public void BtnTuto()
    {
        isTutorial = true;
        Singleton<GameManager>.Instance.music[0].UnPause();
        Singleton<GameManager>.Instance.music[1].Stop();
        StartCoroutine(TutorialStart());
    }

    public IEnumerator TimeStart()
    {
        Time.timeScale = 1;
        Singleton<GameManager>.Instance.music[2].UnPause();
        Singleton<GameManager>.Instance.RayControllerMode(false);
        yield return null;
    }

    public IEnumerator TimeStop()
    {
        Time.timeScale = 0;
        Singleton<GameManager>.Instance.music[2].Pause();
        Singleton<GameManager>.Instance.RayControllerMode(true);
        yield return null;
    }

    public void PanelSpawn()
    {
        Singleton<PanelManager>.Instance.PanelCheck();
        tutoPanelTimer += Time.deltaTime;
        if (tutoPanelTimer > tutoSecPerBeat)
        {
            tutoPanelTimer -= tutoSecPerBeat;

            if (tutoPanelSpawnCount == 0)
            {
                GameObject _motion = Instantiate(tutoPanels[0], Singleton<PanelManager>.Instance.panelSpawnPoint);
                _motion.name = "MOTION";
                tutoPanelSpawnCount++;
            }
            else if (tutoPanelSpawnCount == 1)
            {
                GameObject _motion = Instantiate(tutoPanels[1], Singleton<PanelManager>.Instance.panelSpawnPoint);
                _motion.name = "MOTION";
                tutoPanelSpawnCount++;
            }
            else if (tutoPanelSpawnCount == 2)
            {
                GameObject _motion = Instantiate(tutoPanels[2], Singleton<PanelManager>.Instance.panelSpawnPoint);
                _motion.name = "MOTION";
                tutoPanelSpawnCount++;
            }
            else if (tutoPanelSpawnCount == 3)
            {
                GameObject _block = Instantiate(tutoPanels[3], Singleton<PanelManager>.Instance.panelSpawnPoint);
                _block.name = "BLOCK";
                tutoPanelSpawnCount++;
            }
            else if (tutoPanelSpawnCount == 4)
            {
                GameObject _motion = Instantiate(tutoPanels[4], Singleton<PanelManager>.Instance.panelSpawnPoint);
                _motion.name = "MOTION";
                _motion.transform.GetChild(4).gameObject.SetActive(true);
                tutoPanelSpawnCount++;
            }
            else if (tutoPanelSpawnCount == 5)
            {
                GameObject _quiz = Instantiate(tutoPanels[5], Singleton<PanelManager>.Instance.panelSpawnPoint);
                _quiz.name = "QUIZ";
                tutoPanelSpawnCount++;
            }
        }
    }
}
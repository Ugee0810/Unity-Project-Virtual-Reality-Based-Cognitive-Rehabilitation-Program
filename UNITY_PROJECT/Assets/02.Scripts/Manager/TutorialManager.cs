using Febucci.UI;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public RectTransform xrTutoCanvas;
    public Button btnTutoNext;
    public TextAnimatorPlayer textAnimatorPlayer;

    public int tutorialDialogNum;
    public string[] textBox = {
        "<speed=0.5><rainb f=0.2>안녕하세요.</rainb> 반갑습니다." +
            "\n지금부터 플레이 방법을 안내해드리겠습니다.",
        "<speed=0.5>아래의 Original 테마를 선택해주세요.",
        "<speed=0.5>노래<rainb f=0.2>[Cat Life]</rainb>를 선택해주세요.",
        "<speed=0.5>난이도 <rainb f=0.2>[쉬움]</rainb>을 선택해주세요.",
        "<speed=0.5><rainb f=0.2>플레이</rainb> 버튼을 눌러 게임을 시작합니다.",
        "<speed=0.5><size=7>게임으로 진입했습니다." +
            "\n\n좌측에는 획득한 <bounce a=0.3 f=0.3>점수</bounce>와 <bounce a=0.3 f=0.3>소모된 칼로리</bounce>가 표시됩니다." +
            "\n동작 또는 퀴즈에 성공하면 점수와 콤보가 오릅니다." +
            "\n동작에 성공하면 소모량이 오릅니다." +
            "\n\n우측에서 <bounce a=0.3 f=0.3>자신의 동작을 확인</bounce>할 수 있습니다." +
            "\n\n하단에는 <bounce a=0.3 f=0.3>노래의 길이</bounce>를 알 수 있습니다.",
        "<speed=0.5>모션 모션 패널을 해결해보세요.",
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

    public bool isTutoLobby;
    public bool isTutoStart;

    public static TutorialManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        tutorialDialogNum = 0;
        UnityEngine.Assertions.Assert.IsNotNull(textAnimatorPlayer, $"Text Animator Player component is null in {gameObject.name}");
        //textAnimatorPlayer.textAnimator.onEvent += OnEvent;
    }

    public GameObject[] tutoPanels;
    public int tutoPanelSpawnCount;
    public float tutoMoveSpeed = 2.0f;

    public float tutoSecPerBeat = 3.5f;
    public float tutoPanelTimer; // BPM 계산 타이머

    private void FixedUpdate()
    {
        if (tutorialDialogNum == 0)
        {

        }
        else if (tutorialDialogNum == 1)
        {
            GameManager.instance.uiLobby.SetActive(true);
            GameManager.instance.option.SetActive(false);
            GameManager.instance.result.SetActive(false);
            GameManager.instance.btnCustom.interactable = false;
            btnTutoNext.interactable = false;
        }
        else if (tutorialDialogNum == 2)
        {
            GameManager.instance.btnCustom.interactable = false;
        }
        else if (tutorialDialogNum == 3)
        {
            return;
        }
        else if (tutorialDialogNum == 4)
        {
            GameManager.instance.btnEasy.interactable = false;
            GameManager.instance.btnNormal.interactable = false;
            GameManager.instance.btnHard.interactable = false;
            GameManager.instance.contentOriginal.transform.GetChild(0).GetComponent<Button>().interactable = false;
        }
        else if (tutorialDialogNum == 5)
        {
            btnTutoNext.interactable = true;
        }
        else if (tutorialDialogNum == 6)
        {
            MotionPanelSpawn();
        }
        else if (tutorialDialogNum == 7)
        {
            MotionPanelSpawn();
        }
        else if (tutorialDialogNum == 8)
        {
            MotionPanelSpawn();
        }
        else if (tutorialDialogNum == 9)
        {
            MotionPanelSpawn();
        }
    }

    public void ShowText()
    {
        // Opening
        if (tutorialDialogNum == 0)
        {
            xrTutoCanvas.transform.position = new Vector3(0, 2f, 1);
            xrTutoCanvas.sizeDelta = new Vector2(200, 50);
            xrTutoCanvas.transform.rotation = Quaternion.Euler(10, 0, 0);

            textAnimatorPlayer.ShowText(textBox[0]);
            tutorialDialogNum++;
        }
        // Select Music Theme
        else if (tutorialDialogNum == 1)
        {
            xrTutoCanvas.transform.position = new Vector3(0, 2.65f, 1);
            xrTutoCanvas.sizeDelta = new Vector2(150, 20);
            xrTutoCanvas.transform.rotation = Quaternion.Euler(0, 0, 0);

            textAnimatorPlayer.ShowText(textBox[1]);
            tutorialDialogNum++;
        }
        // Select to Music in Music List
        else if (tutorialDialogNum == 2)
        {
            textAnimatorPlayer.ShowText(textBox[2]);
            tutorialDialogNum++;
        }
        // Select to Level
        else if (tutorialDialogNum == 3)
        {
            textAnimatorPlayer.ShowText(textBox[3]);
            tutorialDialogNum++;
        }
        // Play
        else if (tutorialDialogNum == 4)
        {
            textAnimatorPlayer.ShowText(textBox[4]);
            tutorialDialogNum++;
        }
        // 인게임 요소 설명
        else if (tutorialDialogNum == 5)
        {
            xrTutoCanvas.transform.position = new Vector3(0, 2.25f, 1);
            xrTutoCanvas.sizeDelta = new Vector2(200, 110);
            xrTutoCanvas.transform.rotation = Quaternion.Euler(0, 0, 0);

            textAnimatorPlayer.ShowText(textBox[5]);
            tutorialDialogNum++;
        }
        // Motion Panel(x3)
        else if (tutorialDialogNum == 6)
        {
            textAnimatorPlayer.ShowText(textBox[6]);
            tutorialDialogNum++;

            StartCoroutine(TimeStart());
        }
        // Obstacle Panel
        else if (tutorialDialogNum == 7)
        {
            textAnimatorPlayer.ShowText(textBox[7]);
            tutorialDialogNum++;

            StartCoroutine(TimeStart());
        }
        // Motion Quiz Panel
        else if (tutorialDialogNum == 8)
        {
            textAnimatorPlayer.ShowText(textBox[8]);
            tutorialDialogNum++;

            StartCoroutine(TimeStart());
        }
        // Quiz Answer Panel
        else if (tutorialDialogNum == 9)
        {
            textAnimatorPlayer.ShowText(textBox[9]);
            tutorialDialogNum++;

            StartCoroutine(TimeStart());
        }
    }

    // [OnClick] 로비 ---> 튜토리얼 버튼
    public void BtnTuto()
    {
        isTutoLobby = true;
    }

    // [Onclick] 오리지널 버튼 클릭
    public void TutorialListRenewal()
    {
        if (isTutoLobby)
        {
            object tutorialMusic = Resources.Load<AudioClip>("Original Music/Cat life");
            GameObject tutorialMusicElementPrefab = tutorialMusic as GameObject;
            tutorialMusicElementPrefab = Instantiate(GameManager.instance.musicElement, GameManager.instance.contentOriginal.transform.position, GameManager.instance.contentOriginal.transform.rotation);
            tutorialMusicElementPrefab.name = "Tutorial Music Element";
            tutorialMusicElementPrefab.transform.parent = GameManager.instance.contentOriginal.transform;
            tutorialMusicElementPrefab.transform.localScale = Vector3.one;

            // AudioSource.clip ← Resources-Custom Musics.AudioClip
            tutorialMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip = (AudioClip)tutorialMusic;
            // (float)MusicLength to (string)PlayTime
            tutorialMusicElementPrefab.transform.GetChild(2).gameObject.GetComponent<Text>().text = GameManager.instance.TimeFormatter(tutorialMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.length, false);
            // textTitle.text ← customMusicElements.AudioSource.text
            tutorialMusicElementPrefab.transform.GetChild(1).gameObject.GetComponent<Text>().text = tutorialMusicElementPrefab.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name;
        }
    }

    // [OnClick] 인게임 시작
    public void BtnTutoPlay()
    {
        if (isTutoLobby)
        {
            GameManager.instance.isHandChange = false;
        }
    }

    public IEnumerator TimeStart()
    {
        yield return null;
        Time.timeScale = 1;
        GameManager.instance.musicPlayed.UnPause();
        GameManager.instance.isHandChange = true;
        GameManager.instance.ControllerModeChange();
    }

    public IEnumerator TimeStop()
    {
        yield return null;
        Time.timeScale = 0;
        GameManager.instance.musicPlayed.Pause();
        GameManager.instance.isHandChange = false;
        GameManager.instance.ControllerModeChange();
    }

    public void MotionPanelSpawn()
    {
        if (6 <= tutorialDialogNum && tutorialDialogNum >= 9)
        {
            PanelManager.instance.PanelCheck();
            tutoPanelTimer += Time.deltaTime;
            if (tutoPanelTimer > tutoSecPerBeat)
            {
                tutoPanelTimer -= tutoSecPerBeat;

                if (tutoPanelSpawnCount == 0)
                {
                    GameObject _motion = Instantiate(tutoPanels[0], PanelManager.instance.panelSpawnPoint);
                    _motion.name = "MOTION";
                    tutoPanelSpawnCount++;
                }
                else if (tutoPanelSpawnCount == 1)
                {
                    GameObject _motion = Instantiate(tutoPanels[1], PanelManager.instance.panelSpawnPoint);
                    _motion.name = "MOTION";
                    tutoPanelSpawnCount++;
                }
                else if (tutoPanelSpawnCount == 2)
                {
                    GameObject _motion = Instantiate(tutoPanels[2], PanelManager.instance.panelSpawnPoint);
                    _motion.name = "MOTION";
                    tutoPanelSpawnCount++;
                }
                else if (tutoPanelSpawnCount == 3)
                {
                    GameObject _block = Instantiate(tutoPanels[3], PanelManager.instance.panelSpawnPoint);
                    _block.name = "BLOCK";
                    tutoPanelSpawnCount++;
                }
                else if (tutoPanelSpawnCount == 4)
                {
                    GameObject _motion = Instantiate(tutoPanels[4], PanelManager.instance.panelSpawnPoint);
                    _motion.name = "MOTION";
                    _motion.transform.GetChild(4).gameObject.SetActive(true);
                    tutoPanelSpawnCount++;
                }
                else if (tutoPanelSpawnCount == 5)
                {
                    Debug.Log("퀴즈 패널 생성");
                    GameObject _quiz = Instantiate(tutoPanels[5], PanelManager.instance.panelSpawnPoint);
                    _quiz.name = "QUIZ";
                    tutoPanelSpawnCount++;
                }
            }
        }
    }

    //void OnEvent(string text)
    //{
    //    switch (text)
    //    {
    //        case "bg":
    //            break;
    //    }
    //}
}
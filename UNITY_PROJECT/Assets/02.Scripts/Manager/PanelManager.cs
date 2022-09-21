/// <summary>
/// PanelManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (Eternal Light)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;
using Random = UnityEngine.Random;

public class PanelManager : MonoBehaviour
{
    [Header("[패널 상호작용 요소]")]
    public Transform  panelSpawnPoint; // 패널 생성 좌표
    public GameObject panelCheck;

    [Header("[패널 프리팹]")]
    public GameObject[] quiz;         // 패널 프리팹 배열
    public GameObject[] block;        // 패널 프리팹 배열
    public GameObject[] motion;       // 패널 프리팹 배열

    public List<GameObject> ballList = new List<GameObject>();
    static string[] _LetterArray =
    { "집중", "평화", "용서", "감사", "침착", "정의", "조화", "자유", "정직", "지혜",
        "친절", "이해", "활력", "영감", "공감", "겸손", "힘", "지성", "고요", "결단",
        "사랑", "관용", "자비", "부드러움", "축복", "생명", "믿음", "젊음", "자신감", "덕성",
        "행복", "영혼", "민첩성", "건강", "충만", "일관성", "끈기", "목적", "성취", "풍요",
        "성공", "숙달", "능력", "에너지", "소명", "침묵", "직관", "재생", "소생" };
    public List<string> _LetterList = new List<string>();

    public int panelSpawnCount;
    public int panelLastIndex;
    public bool isQuiz;
    public bool isCurLeft;
    public bool isCurRight;
    public int quizCool;

    public string curColor;
    public string curLetter;
    
    public static PanelManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        panelSpawnCount = -1;
        panelLastIndex  = -1;

        _LetterList.AddRange(_LetterArray);
    }

    private void FixedUpdate()
    {
        if (GameManager.instance.isStart && GameManager.instance.musicPlayed.isPlaying)
        {
            PanelCheck();

            GameManager.instance.offsetTimer += Time.deltaTime;
            if (GameManager.instance.playTimeOffset >= GameManager.instance.offsetTimer)
            {
                PanelInstance();
            }
        }
    }

    public void PanelInstance()
    {
        GameManager.instance.timer += Time.deltaTime;
        if (GameManager.instance.timer > GameManager.instance.secPerBeat)
        {
            GameManager.instance.timer -= GameManager.instance.secPerBeat;

            int panelIndex = Random.Range(0, 10); // <--- 전체 패널 확률
            int quizCool   = Random.Range(5, 25); // <--- 퀴즈 쿨타임

            /* QUIZ 10% */ if (panelIndex == 0)
            {
                if (!isQuiz)
                {
                    Debug.Log("퀴즈 패널 패턴이 아니므로 모션 패널 생성");
                    GameObject _motion = Instantiate(motion[Random.Range(0, 1)], panelSpawnPoint);
                    _motion.name = "MOTION";

                    panelSpawnCount++;
                    panelLastIndex++;

                    if (panelSpawnCount == quizCool && !isQuiz)
                    {
                        _motion.transform.GetChild(4).gameObject.SetActive(true);
                        panelSpawnCount -= quizCool + quizCool;
                    }
                }
                else if (isQuiz)
                {
                    Debug.Log("퀴즈 패널 생성");
                    GameObject _quiz = Instantiate(quiz[0], panelSpawnPoint);
                    _quiz.name = "QUIZ";

                    panelSpawnCount++;
                    panelLastIndex++;

                    isQuiz = false;
                }
            }
            /* BLOCK 10% */ else if (panelIndex == 1)
            {
                GameObject _block = Instantiate(block[Random.Range(0, 3)], panelSpawnPoint);
                _block.name = "BLOCK";
                float zScale = Random.Range(1.0f, 4.0f);
                _block.transform.localScale = new Vector3(1, 1, Random.Range(1, zScale));

                panelSpawnCount++;
                panelLastIndex++;
            }
            /* MOTION 80% */ else if (panelIndex > 1)
            {
                GameObject _motion = Instantiate(motion[Random.Range(0, 1)], panelSpawnPoint);
                _motion.name = "MOTION";

                panelSpawnCount++;
                panelLastIndex++;

                if (panelSpawnCount >= quizCool && !isQuiz)
                {
                    _motion.transform.GetChild(4).gameObject.SetActive(true);
                    panelSpawnCount -= quizCool + quizCool;
                }
            }
        }
    }

    void PanelCheck()
    {
        if (GameManager.instance.isSensorLeft && GameManager.instance.isSensorRight)
        {
            panelCheck.SetActive(true);
            if (panelCheck.activeSelf) StartCoroutine(ScoreManager.instance.Increase());
        }
        else if (!GameManager.instance.isSensorLeft || !GameManager.instance.isSensorRight)
        {
            panelCheck.SetActive(false);
        }
    }
}
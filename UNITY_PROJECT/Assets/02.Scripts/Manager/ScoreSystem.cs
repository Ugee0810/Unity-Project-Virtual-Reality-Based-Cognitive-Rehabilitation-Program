using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;


    public GameObject scoreObj;
    Text textScore;
    int score = 0;

    public GameObject kcalObj;
    Text textKcal;
    float kcal = 0;


    private void Awake()
    {
        instance = this;


        textScore = scoreObj.GetComponent<Text>();
        textKcal  = kcalObj.GetComponent<Text>();
    }

    private void OnEnable()
    {
        IncreaseScore();
        IncreaseKcal();
    }

    private void Start()
    {
        if (GameManager.instance.isStart)
        {
            SetScore();
            SetKcal();
        }
    }


    public void IncreaseScore()
    {
        score += 1000;
        SetScore();
    }

    public void SetScore()
    {
        textScore.text = score.ToString();
    }


    public void IncreaseKcal()
    {
        kcal += 0.4f;
        SetKcal();
    }

    public void SetKcal()
    {
        textKcal.text  = kcal.ToString("F1");
    }
}
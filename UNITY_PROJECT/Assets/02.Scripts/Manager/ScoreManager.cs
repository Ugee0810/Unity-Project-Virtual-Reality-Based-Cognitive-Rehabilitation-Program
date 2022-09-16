using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text textScore;
    public Text textKcal;

    public int score = 0;
    float kcal = 0;

    public static ScoreManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
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

    public void IncreaseKcal()
    {
        kcal += Random.Range(0.1f, 0.2f);
        SetKcal();
    }

    public void SetScore()
    {
        textScore.text = score.ToString();
    }

    public void SetKcal()
    {
        textKcal.text  = kcal.ToString("F2");
    }
}
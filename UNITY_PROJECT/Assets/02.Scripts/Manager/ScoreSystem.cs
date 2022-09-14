using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ScoreSystem : MonoBehaviour
{
    public static ScoreSystem instance;

    public GameObject scoreObj;
    Text textScore;
    int score = 0;

    private void Awake()
    {
        instance = this;
        textScore = scoreObj.GetComponent<Text>();
    }

    private void Start()
    {
        if (GameManager.instance.isStart)
        {
            SetText();
        }
    }

    public void IncreaseScore()
    {
        score += 1000;
        SetText();
    }

    public void SetText()
    {
        textScore.text = score.ToString();
    }
}

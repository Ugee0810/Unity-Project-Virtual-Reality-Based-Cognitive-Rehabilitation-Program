using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    [SerializeField] string comboWord;
    [SerializeField] GameObject comboObj;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField, Range(0.2f, 0.5f)] float initEffectDuration = 0.2f; //初期演出時間(초기 연출 시간)
    [SerializeField, Range(0.01f, 0.1f)] float durationIncrement = 0.03f; //増加時間(증가 시간)
    [SerializeField, Range(0.4f, 0.7f)] float maxEffectDuration = 0.4f; //最大演出時間(최대 연출 시간)
    [SerializeField, Range(1.2f, 1.5f)] float initMaxScale = 1.4f; //初期最大スケール(초기 후 큰 스케일)
    [SerializeField, Range(0.2f, 1f)] float scaleIncrement = 0.6f; //スケール増加量(스케일 증가량)
    [SerializeField, Range(5, 10)] float maxScale = 5f; //最大スケール(최대 스케일)
    [SerializeField, Range(0, 0.2f)] float basicScaleIncrement = 0.09f; //演出後の文字の大きさの増加量(연출 후 크기의 증가량)

    TMP_Text comboText;
    RectTransform comboRectTrans;

    public int combo = 0;
    public float timer = 0f;
    float scale;
    float basicScale;
    float effectDuration;
    bool playingEffect = false;
    Coroutine effectCol;
    public Queue<int> comboOrder = new Queue<int>();

    public static ComboManager instance;
    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        comboText = comboObj.GetComponent<TMP_Text>();
        comboRectTrans = comboObj.GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (comboOrder.Count == 0) return;

        timer += Time.deltaTime;
        var tempRate = Mathf.Clamp((1f - combo / 10f), 0.3f, 0.5f); //コンボ数が大きいほど短時間で次の表示(콤보 수가 클 수록 단기간에 다음을 표시)
        if (timer > effectDuration * tempRate)
        {
            timer = 0;
            UpdateCombo(comboOrder.Dequeue());
        }
    }

    public void IncreaseCombo()
    {
        combo++;
        comboOrder.Enqueue(combo);
        if (combo == 1) //初回のみ
            UpdateCombo(comboOrder.Dequeue());
    }

    public void Clear()
    {
        combo = 0;
        comboText.text = "";
        comboOrder.Clear();
    }

    // 콤보 갱신 함수
    // int comboCount 콤보 수
    void UpdateCombo(int comboCount)
    {
        comboText.text = comboCount + comboWord;
        comboRectTrans.localRotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));

        //前のコンボ演出が終了してない場合(전 콤보 연출이 종료하지 않았을 경우)
        if (playingEffect)
        {
            StopCoroutine(effectCol);
            if (effectDuration < maxEffectDuration)
                effectDuration += durationIncrement;
            if (scale < maxScale)
                scale += scaleIncrement;
            //7コンボまでは初期スケールを大きくする(7콤보까지는 초기 스케일을 크게 한다.)
            if (combo < 7)
                basicScale += basicScaleIncrement;
        }
        else
        {
            scale = initMaxScale;
            basicScale = 1;
            effectDuration = initEffectDuration;
        }

        effectCol = StartCoroutine(PlayEffect(effectDuration));
    }

    /// <summary>
    /// 演出.テキストの大きさを大→小にする.(연출. 텍스트의 크기를 작게 한다.)
    /// </summary>
    /// <param name="duration">期間(sec)</param>
    IEnumerator PlayEffect(float duration)
    {
        var timer = 0f;
        var rate = 0f;
        var startScale = new Vector3(scale, scale, 1);
        var endScale   = new Vector3(basicScale, basicScale, 1);

        playingEffect = true;
        while (rate < 1)
        {
            timer += Time.deltaTime;
            rate = Mathf.Clamp01(timer / duration);
            var curvePos = scaleCurve.Evaluate(rate);
            comboRectTrans.localScale = Vector3.Lerp(startScale, endScale, curvePos);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        playingEffect = false;
    }
}
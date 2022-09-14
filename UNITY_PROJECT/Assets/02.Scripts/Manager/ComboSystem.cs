using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem instance;

    [SerializeField] string comboWord;
    [SerializeField] GameObject comboObj;
    [SerializeField] AnimationCurve scaleCurve;
    [SerializeField, Range(0.2f, 0.5f)] float initEffectDuration = 0.2f; //初期演出時間
    [SerializeField, Range(0.01f, 0.1f)] float durationIncrement = 0.03f; //増加時間
    [SerializeField, Range(0.4f, 0.7f)] float maxEffectDuration = 0.4f; //最大演出時間
    [SerializeField, Range(1.2f, 1.5f)] float initMaxScale = 1.4f; //初期最大スケール
    [SerializeField, Range(0.2f, 1f)] float scaleIncrement = 0.6f; //スケール増加量
    [SerializeField, Range(5, 10)] float maxScale = 5f; //最大スケール
    [SerializeField, Range(0, 0.2f)] float basicScaleIncrement = 0.09f; //演出後の文字の大きさの増加量

    Text comboText;
    RectTransform comboRectTrans;

    int counter = 0;
    bool playingEffect = false;
    float scale;
    float basicScale;
    float effectDuration;
    float timer = 0f;
    Coroutine effectCol;
    Queue<int> comboOrder = new Queue<int>();

    void Awake()
    {
        instance = this;

        comboText = comboObj.GetComponent<Text>();
        comboRectTrans = comboObj.GetComponent<RectTransform>();
    }

    void Update()
    {
        if (comboOrder.Count == 0) return;

        timer += Time.deltaTime;
        var tempRate = Mathf.Clamp((1f - counter / 10f), 0.3f, 0.5f); //コンボ数が大きいほど短時間で次の表示
        if (timer > effectDuration * tempRate)
        {
            timer = 0;
            UpdateCombo(comboOrder.Dequeue());
        }
    }

    public void IncreaseCombo()
    {
        counter++;
        comboOrder.Enqueue(counter);
        if (counter == 1) //初回のみ
            UpdateCombo(comboOrder.Dequeue());
    }

    /// <summary>
    /// コンボの更新.
    /// </summary>
    /// <param name="comboCount">コンボ数</param>
    void UpdateCombo(int comboCount)
    {
        comboText.text = comboCount + comboWord;

        comboRectTrans.localRotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));

        if (playingEffect)
        { //前のコンボ演出が終了してない場合
            StopCoroutine(effectCol);
            if (effectDuration < maxEffectDuration)
            {
                effectDuration += durationIncrement;
            }
            if (scale < maxScale)
            {
                scale += scaleIncrement;
            }
            if (counter < 7)
            { //7コンボまでは初期スケールを大きくする
                basicScale += basicScaleIncrement;
            }
        }
        else
        {
            scale = initMaxScale;
            basicScale = 1;
            effectDuration = initEffectDuration;
        }

        effectCol = StartCoroutine(PlayEffect(effectDuration));
    }

    public void Clear()
    {
        counter = 0;
        comboOrder.Clear();
    }

    /// <summary>
    /// 演出.テキストの大きさを大→小にする.
    /// </summary>
    /// <param name="duration">期間(sec)</param>
    IEnumerator PlayEffect(float duration)
    {
        var timer = 0f;
        var rate = 0f;
        var startScale = new Vector3(scale, scale, 1);
        var endScale = new Vector3(basicScale, basicScale, 1);

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
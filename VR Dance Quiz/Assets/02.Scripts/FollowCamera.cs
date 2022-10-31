using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FollowCamera : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;

    Button btn;

    // 진행될 총 시간
    float lerpTime = 0.5f;
    // 경과 카운트
    float currentTime = 0f;

    private void Start()
    {
        this.transform.position = startPosition.position;
        
        // OnClick() Lambda
        btn.onClick.AddListener(() => { StartCoroutine(LerpTest()); });
    }

    IEnumerator LerpTest()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= lerpTime) currentTime = lerpTime;
        float t = currentTime / lerpTime;
        t = t * t * t * (t * (6f * t - 15f) + 10f);
        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, t);
        yield return null;
    }
}
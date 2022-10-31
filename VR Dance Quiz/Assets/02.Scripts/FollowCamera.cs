using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform startPosition;
    public Transform endPosition;

    // 진행될 총 시간
    float lerpTime = 0.5f;
    // 경과 카운트
    float currentTime = 0f;

    private void Start()
    {
        this.transform.position = startPosition.position;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= lerpTime)
        {
            currentTime = lerpTime;
        }
        // currentTime / lerpTime <--- 프레임마다 0부터 1까지 서서히 증가하는 형태
        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, currentTime / lerpTime);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFrequencyRing : MonoBehaviour
{
    public Runningtap.AnalyzeAudio audioData;

    public GameObject sampleCubePrefab;
    public Vector3 StartScale = new Vector3(0.2f, 0.2f, 0.2f);
    public float Radius = 100f;
    public float Sensitivity = 2f;

    private GameObject[] sampleCube;

	void Start()
    {
        sampleCube = new GameObject[audioData.FrequencyBands];
        float angle = 360f / audioData.FrequencyBands;

		for(int i = 0; i < audioData.FrequencyBands; i++)
        {
            GameObject instance = Instantiate(sampleCubePrefab);
            instance.transform.position = transform.position;       // 큐브 포지션 = 스크립트 오브젝트
            instance.transform.parent = transform;                  // 큐브를 자식화 시킨다.
            instance.name = "SampleCube_" + i;                      // 큐브 넘버링
            transform.eulerAngles = new Vector3(0, 0, -angle * i);  // -(360/64) * 1 ~ 64
            instance.transform.position = new Vector3(0, Radius, 20);
            instance.transform.eulerAngles = new Vector3(0, 90, 0);
            sampleCube[i] = instance;
        }
	}
	
	void Update()
    {
		for(int i = 0; i < audioData.FrequencyBands; i++)
        {
            sampleCube[i].transform.localScale = new Vector3(StartScale.x, audioData.AudioBandBuffer[i] * Sensitivity + StartScale.y, StartScale.z);
        }
    }
}
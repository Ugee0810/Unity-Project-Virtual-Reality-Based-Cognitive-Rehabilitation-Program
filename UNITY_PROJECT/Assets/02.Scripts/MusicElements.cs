using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicElements : MonoBehaviour
{
    GameObject selectedElement;

    public void Select()
    {
        GameManager.instance.btnPlay.GetComponent<Button>().interactable = true; // 노래 재생(Play) 버튼 활성화
        GameManager.instance.musicBackGround.Pause(); // BGM Pause

        GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject; // 방금 클릭한 게임 오브젝트를 가져와서 저장
        selectedElement = currentSelectedGameObject;

            print($"선택한 프리팹 이름 : {selectedElement.name}"); // 방금 클릭한 게임 오브젝트의 이름 출력
            print($"앨범 이미지 : {selectedElement.transform.GetChild(0)}");
            print($"타이틀 :      {selectedElement.transform.GetChild(1)}");
            print($"플레이 타임 : {selectedElement.transform.GetChild(2)}");
            print($"노래 제목 :   {selectedElement.transform.GetChild(3)}");
            print("설마 되냐?" + selectedElement.transform.GetChild(3).GetComponent<AudioSource>().clip); // 방금 클릭한 게임 오브젝트의 Audio Clip 출력
            print("이건 되나?" + GameManager.instance.musicSelected.GetComponent<AudioSource>().clip);

        // textTitle.text ← customMusicElements.AudioSource.text
        GameManager.instance.infoTitle.GetComponent<Text>().text =
            $"- {selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip.name}";

        // 프리팹 오디오 소스 클립 -> musicSelected의 클립
        GameManager.instance.musicSelected.GetComponent<AudioSource>().clip = 
            selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip;

        // 플레이에 사용 될 오디오 소스 대칭화
        GameManager.instance.musicPlayed.GetComponent<AudioSource>().clip = 
            GameManager.instance.musicSelected.GetComponent<AudioSource>().clip; 

        GameManager.instance.musicSelected.Play();
    }
}

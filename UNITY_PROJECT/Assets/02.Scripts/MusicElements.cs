using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicElements : MonoBehaviour
{
    [Header("[Scripts]")]
    GameManager _GameManager;

    GameObject selectedElement;

    public void Select()
    {
        GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject; // 방금 클릭한 게임 오브젝트를 가져와서 저장
        selectedElement = currentSelectedGameObject;
        print($"선택한 프리팹 이름 : {selectedElement.name}"); // 방금 클릭한 게임 오브젝트의 이름 출력
        print($"앨범 이미지 : {selectedElement.transform.GetChild(0)}");
        print($"타이틀 : {selectedElement.transform.GetChild(1)}");
        print($"플레이 타임 : {selectedElement.transform.GetChild(2)}");
        print($"노래 제목 :  {selectedElement.transform.GetChild(3)}");
        print("설마 되냐?" + selectedElement.transform.GetChild(3).GetComponent<AudioSource>().clip); // 방금 클릭한 게임 오브젝트의 Audio Clip 출력
        print(_GameManager.musicSelected.GetComponent<AudioSource>().clip);


        //_GameManager.musicSelected.GetComponent<AudioSource>().clip = selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip;
        //print("이것도 된다고?" + _GameManager.musicSelected.GetComponent<AudioSource>().clip.name);
        //_GameManager.musicSelected.Play();
    }

    //public void SelectElement()
    //{
    //    //// 방금 선택한 버튼의 정보를 받아옵니다.
    //    //GameObject _selectedElement = EventSystem.current.currentSelectedGameObject;
    //    selectedElement = _selectedElement;
    //}

    //public IEnumerator SelectElementInfo()
    //{
    //    selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Stop(); // Off 'Play On Awake'
    //    // Onclick이벤트가 발생한 오브젝트 하위의 오디오 소스 클립의 이름과 타이틀 이름을 대칭한다.
    //    selectedElement.transform.GetChild(1).gameObject.GetComponent<Text>().text = selectedElement.transform.GetChild(3).GetComponent<AudioSource>().clip.name;

    //    _GameManager.musicSelected.clip = selectedElement.transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip;
    //    _GameManager.musicSelected.Play();
    //    yield return null;
    //}

    //// [Button] Element Selected
    //public void BtnElementSelected()
    //{
    //    // Original Element
    //    if (isOriginal == true && isCustom == false)
    //    {
    //        for (int i = 0; i < originalElementPrefab.Length; i++)
    //        {
    //            musicSelected.clip = originalElementPrefab[i].transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip;
    //            musicSelected.Play();
    //        }
    //    }
    //    // Custom Element
    //    else if (isOriginal == false && isCustom == true)
    //    {
    //        for (int i = 0; i < customElementPrefab.Length; i++)
    //        {
    //            musicSelected.clip = customElementPrefab[i].transform.GetChild(3).gameObject.GetComponent<AudioSource>().clip;
    //            musicSelected.Play();
    //        }
    //    }
    //    return;
    //}
}

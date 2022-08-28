using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicElements : MonoBehaviour
{
    [Header("[Scripts]")]
    GameManager  _GameManager;

    public void SelectElement()
    {
        GameObject selectedElement = EventSystem.current.currentSelectedGameObject;

        selectedElement.transform.GetChild(1).gameObject.GetComponent<Text>().text = selectedElement.GetComponent<AudioSource>().clip.name;
        selectedElement.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
    }

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeyboardButtonController : MonoBehaviour
{
    [SerializeField] Image containerBorderImage;
    [SerializeField] Image containerFillImage;
    [SerializeField] Image containerIcon;
    [SerializeField] TextMeshProUGUI containerText;
    [SerializeField] TextMeshProUGUI containerActionText;
    InputManager inputManager;
    private void Start()
    {
        inputManager = GameObject.FindGameObjectWithTag("InputField").GetComponent<InputManager>();

        Debug.Log(inputManager);
        SetContainerBorderColor(ColorDataStore.GetKeyboardBorderColor());
        SetContainerFillColor(ColorDataStore.GetKeyboardFillColor());
        SetContainerTextColor(ColorDataStore.GetKeyboardTextColor());
        SetContainerActionTextColor(ColorDataStore.GetKeyboardActionTextColor());
    }

    public void SetContainerBorderColor(Color color) => containerBorderImage.color = color;
    public void SetContainerFillColor(Color color) => containerFillImage.color = color;
    public void SetContainerTextColor(Color color) => containerText.color = color;
    public void SetContainerActionTextColor(Color color)
    {
        containerActionText.color = color;
        containerIcon.color = color;
    }

    public void AddLetter()
    {
        if (InputManager.Instance.isActiveAndEnabled && InputManager.Instance.currentInputField != null)
        {
            if (KeyBoardSetManager.Instance != null)
            {
                KeyBoardSetManager.Instance.AddLetter(containerText.text);
            }
            else
            {
                Debug.Log(InputManager.Instance);
                Debug.Log(containerText);
                InputManager.Instance.currentInputField.text += containerText.text;
            }
        }
    }
    public void DeleteLetter()
    {
        if(InputManager.Instance.isActiveAndEnabled && InputManager.Instance.currentInputField != null)
        {
            if (KeyBoardSetManager.Instance != null)
            {
                KeyBoardSetManager.Instance.DeleteLetter();

            }

            else
            {
                if (InputManager.Instance.currentInputField.text.Length != 0)
                {
                    InputManager.Instance.currentInputField.text = InputManager.Instance.currentInputField.text.Remove(InputManager.Instance.currentInputField.text.Length - 1, 1);
                }
                Debug.Log("Last char deleted");
            }
        }
    }
    public void SubmitWord()
    {
        if (KeyBoardSetManager.Instance != null)
        {
            KeyBoardSetManager.Instance.SubmitWord();
        }
        else
        {
            Debug.Log("Submitted successfully!");
        }
    }
}
    
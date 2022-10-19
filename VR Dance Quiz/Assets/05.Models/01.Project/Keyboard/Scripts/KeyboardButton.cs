using TMPro;
using UnityEngine;

public class KeyboardButton : MonoBehaviour
{
    Keyboard Keyboard;
    TextMeshProUGUI buttonText;

    private void Start()
    {
        Keyboard = GetComponentInParent<Keyboard>();
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        if (buttonText.text.Length == 1 && buttonText.text != "@" && buttonText.text != ".")
        {
            NameToButtonText();
            GetComponentInChildren<ButtonVR>().onRelease.AddListener(delegate { Keyboard.InsertChar(buttonText.text); });
        }
    }

    public void NameToButtonText()
    {
        buttonText.text = gameObject.name;
    }
}
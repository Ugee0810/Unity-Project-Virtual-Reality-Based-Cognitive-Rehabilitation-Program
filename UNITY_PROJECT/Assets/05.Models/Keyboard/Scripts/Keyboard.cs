using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public TMP_InputField inputField;
    public GameObject normalButtons;
    public GameObject capsButtons;

    bool caps;

    private void Start()
    {
        caps = false;
    }

    public void InsertChar(string c)
    {
        inputField.text += c;
    }

    public void DeleteChar()
    {
        if (inputField.text.Length > 0)
        {
            inputField.text = inputField.text.Substring(0, inputField.text.Length - 1);
        }
    }

    public void InsertSpace()
    {
        inputField.text += " ";
    }

    public void CapsPressed()
    {
        if (!caps)
        {
            normalButtons.SetActive(false);
            capsButtons.SetActive(true);
            caps = true;
        }
        else
        {
            normalButtons.SetActive(true);
            capsButtons.SetActive(false);
            caps = false;
        }
    }
}
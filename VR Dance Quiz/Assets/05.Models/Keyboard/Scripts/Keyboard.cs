using UnityEngine;
using TMPro;

public class Keyboard : MonoBehaviour
{
    public TMP_InputField inputFieldEmail;
    public TMP_InputField inputFieldPassword;
    public GameObject normalButtons;
    public GameObject capsButtons;

    bool caps;

    private void Start()
    {
        caps = false;
    }

    public void InsertChar(string c)
    {
        if (GameManager.instance.isEmail)
            inputFieldEmail.text += c;
        if (GameManager.instance.isPassword)
            inputFieldPassword.text += c;
    }

    public void DeleteChar()
    {
        if (GameManager.instance.isEmail)
            if (inputFieldEmail.text.Length > 0)
                inputFieldEmail.text = inputFieldEmail.text.Substring(0, inputFieldEmail.text.Length - 1);
        if (GameManager.instance.isPassword)
            if (inputFieldPassword.text.Length > 0)
                inputFieldPassword.text = inputFieldPassword.text.Substring(0, inputFieldPassword.text.Length - 1);
    }

    public void InsertSpace()
    {
        if (GameManager.instance.isEmail)
            inputFieldEmail.text += " ";
        if (GameManager.instance.isPassword)
            inputFieldPassword.text += " ";
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
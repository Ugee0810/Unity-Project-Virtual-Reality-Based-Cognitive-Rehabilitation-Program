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
        if (Singleton<GameManager>.Instance.isEmail)
            inputFieldEmail.text += c;
        if (Singleton<GameManager>.Instance.isPassword)
            inputFieldPassword.text += c;
    }

    public void DeleteChar()
    {
        if (Singleton<GameManager>.Instance.isEmail)
            if (inputFieldEmail.text.Length > 0)
                inputFieldEmail.text = inputFieldEmail.text.Substring(0, inputFieldEmail.text.Length - 1);
        if (Singleton<GameManager>.Instance.isPassword)
            if (inputFieldPassword.text.Length > 0)
                inputFieldPassword.text = inputFieldPassword.text.Substring(0, inputFieldPassword.text.Length - 1);
    }

    public void InsertSpace()
    {
        if (Singleton<GameManager>.Instance.isEmail)
            inputFieldEmail.text += " ";
        if (Singleton<GameManager>.Instance.isPassword)
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

    public void At()
    {
        if (Singleton<GameManager>.Instance.isEmail)
            inputFieldEmail.text += "@";
        if (Singleton<GameManager>.Instance.isPassword)
            inputFieldPassword.text += "@";
    }

    public void Dot()
    {
        if (Singleton<GameManager>.Instance.isEmail)
            inputFieldEmail.text += ".";
        if (Singleton<GameManager>.Instance.isPassword)
            inputFieldPassword.text += ".";
    }
}
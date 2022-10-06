using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class LoginSystem : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;

    public UnityEvent loginEvent;
    public UnityEvent logoutEvent;

    //public TMP_Text stateText;

    private void Start()
    {
        FirebaseAuthManager.Instance.LoginState += OnChangeState;
        FirebaseAuthManager.Instance.Init();
    }

    void OnChangeState(bool sign)
    {
        loginEvent?.Invoke();
        //stateText.text = sign ? "로그인 : " : "로그아웃 : ";
        //stateText.text = FirebaseAuthManager.Instance.UserId;
    }

    public void Create() 
    {
        FirebaseAuthManager.Instance.Create(email.text, password.text);
    }

    public void Login() 
    {
        FirebaseAuthManager.Instance.Login(email.text, password.text);
    }

    public void Logout()
    {
        FirebaseAuthManager.Instance.Logout();
    }
}

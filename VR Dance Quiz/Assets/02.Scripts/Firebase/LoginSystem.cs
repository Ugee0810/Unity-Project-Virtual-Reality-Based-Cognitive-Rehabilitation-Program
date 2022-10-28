/// <summary>
/// LoginSystem.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Febucci.UI;

public class LoginSystem : Singleton<LoginSystem>
{
    public TMP_InputField email;
    public TMP_InputField password;

    public UnityEvent loginEvent;
    public UnityEvent logoutEvent;

    [SerializeField] TextAnimatorPlayer textAnimatorPlayer;

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

    public void LoginFailMessage()
    {
        textAnimatorPlayer.ShowText("로그인에 실패했습니다.");
    }

    public void SigninFailMessage()
    {
        textAnimatorPlayer.ShowText("회원가입에 실패했습니다.");
    }
}

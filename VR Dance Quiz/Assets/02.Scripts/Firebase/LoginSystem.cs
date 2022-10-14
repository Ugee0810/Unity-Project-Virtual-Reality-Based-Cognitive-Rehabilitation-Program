using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using Febucci.UI;

public class LoginSystem : MonoBehaviour
{
    public TMP_InputField email;
    public TMP_InputField password;

    public UnityEvent loginEvent;
    public UnityEvent logoutEvent;

    [SerializeField] TextAnimatorPlayer textAnimatorPlayer;

    public static LoginSystem instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using Unity.XR.CoreUtils;

public class FirebaseAuthManager
{
    static FirebaseAuthManager instance = null;

    public static FirebaseAuthManager Instance
    {
        get
        {
            if (instance == null)
                instance = new FirebaseAuthManager();
            return instance;
        }
    }

    // 로그인 / 회원가입 등에 사용
    FirebaseAuth auth;
    // 인증이 완료된 유저 정보
    FirebaseUser user;

    public string UserId => user.UserId;

    public Action<bool> LoginState;

    public void Init()
    {
        auth = FirebaseAuth.DefaultInstance;
        // 임시처리
        if (auth.CurrentUser != null)
        {
            Logout();
            //Debug.Log("임시처리");
        }
        // 이벤트 핸들러(계정 상태가 바뀔 때 마다 호출)
        auth.StateChanged += OnChanged;
    }

    void OnChanged(object sender, EventArgs e)
    {
        if (auth.CurrentUser != user)
        {
            bool signed = (auth.CurrentUser != user && auth.CurrentUser != null);
            if (!signed && user != null)
            {
                //Debug.Log("로그아웃");
                LoginState?.Invoke(false);
            }

            user = auth.CurrentUser;
            if (signed)
            {
                //Debug.Log("로그인");
                LoginState?.Invoke(true);
            }
        }
    }

    public void Create(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                //Debug.Log("회원가입 취소");
                return;
            }

            if (task.IsFaulted)
            {
                // 회원가입 실패 이유 ---> 이메일이 비정상 / 비밀번호가 너무 간단 / 이미 가입된 이메일 등
                //Debug.Log("회원가입 실패");
                Singleton<LoginSystem>.Instance.SigninFailMessage();
                return;
            }

            // 실패하지 않았을 경우 정보를 가지고 새로운 유저를 생성함
            FirebaseUser newUser = task.Result;
            //Debug.Log("회원가입 완료");
        });
    }

    public void Login(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                //Debug.Log("로그인 취소");
                return;
            }

            if (task.IsFaulted)
            {
                // 로그인 실패 이유 ---> 이메일이 비정상 / 비밀번호가 너무 간단 / 이미 가입된 이메일 등
                //Debug.Log("로그인 실패");
                Singleton<LoginSystem>.Instance.LoginFailMessage();
                return;
            }

            // 실패하지 않았을 경우 정보를 가지고 새로운 유저를 생성함
            FirebaseUser newUser = task.Result;
            //Debug.Log("로그인 완료");
        });
    }

    public void Logout()
    {
        auth.SignOut();
        //Debug.Log("로그아웃");
    }
}
/// <summary>
/// InputManager.cs
/// Copyright (c) 2022 VR-Based Cognitive Rehabilitation Program (V-Light Stutio)
/// This software is released under the GPL-2.0 license
/// 
/// </summary>

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InputManager : MonoBehaviour
{
    static InputManager instance;

    [SerializeField]
    TMP_InputField[] inputFields;
    [SerializeField]
    List<KeyboardButtonController> keyButtonList;

    public TMP_InputField currentInputField;

    public static InputManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);

        }
        else
            Destroy(gameObject);
    }

    private void Update()
    {
        foreach (TMP_InputField item in inputFields)
        {
            if (item.isFocused)
                currentInputField = item;
        }
    }
}

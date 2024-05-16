using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keypad : MonoBehaviour
{
    [SerializeField] private String passcode;
    [SerializeField] private TextMeshPro text;
    private String enteredCode = "";
    private bool stage1;
    private bool stage2;
    private bool defused;

    public void setKey(int keyValue)
    {
        if(keyValue >= 0)
        {
            if(enteredCode.Length == passcode.Length) { return; }
            enteredCode += keyValue.ToString();
        }
        // if # check for value
        else if (keyValue == -1)
        {
            validateCode();
        }
        // if * reset code
        else if (keyValue == -2)
        {
            resetCode();
        }

        updateText();
    }

    private void updateText()
    {
        text.text = enteredCode;
    }

    private void validateCode()
    {
        if(passcode == enteredCode)
        {
            enteredCode = "correct";
            //KeypadCode[] buttons = GetComponentsInChildren<KeypadCode>();
            //foreach(KeypadCode button in buttons)
            //{
            //    button.gameObject.SetActive(false);
            //}
        }
    }

    private void resetCode()
    {
        enteredCode = "";
    }
}

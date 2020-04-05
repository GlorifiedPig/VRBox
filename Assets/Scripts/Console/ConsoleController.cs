﻿using UnityEngine;
using Unity.Collections;
using TMPro;

public class ConsoleController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField = null;
    [ReadOnly] public bool consoleActive;
    private string consoleText = "";

    public void AddLineToConsole( string newText ) {
        if( inputField == null ) { return; }
        consoleText = consoleText + newText + "\n";
        inputField.text = consoleText;
    }

    void Update() {
        consoleActive = inputField.gameObject.activeSelf;
        if( Input.GetButtonDown( "OpenConsole" ) ) {
            inputField.gameObject.SetActive( !consoleActive );
        }

        if( consoleActive ) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
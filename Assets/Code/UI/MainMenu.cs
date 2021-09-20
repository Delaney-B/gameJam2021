using System;
using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public void OnStartGame() {
        GameStateManager.GetInstance().LoadGameScene();
    }

    public void OnQuitGame() {
        Application.Quit();
    }
}

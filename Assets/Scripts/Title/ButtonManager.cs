using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void ChangeScene(string _strGameLevel)
    {
        SceneManager.LoadScene(_strGameLevel);
    }

    //public void SceneChangeToMainMenu() { SceneManager.LoadScene("MainMenu"); }

    public void SceneChangeTogGameStart()
    {
        SceneManager.LoadScene("Prototype_ver_1.0");

        Time.timeScale = 1.0f;
    }

    public void SceneChangeToMovie_g() { SceneManager.LoadScene("Movie_g"); }

    public void SceneChangeToTitle() { SceneManager.LoadScene("Menu_Title"); }

    public void SceneChangeGameIntro() { SceneManager.LoadScene("GameInrto"); }

    public void EndGame() { Application.Quit(); }
}

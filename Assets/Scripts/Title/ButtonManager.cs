using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void ChangeScene(string _strGameLevel)
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(_strGameLevel);
    }

    //public void SceneChangeToMainMenu() { SceneManager.LoadScene("MainMenu"); }
    

    public void SceneChangeToGameStart()
    {
        Time.timeScale = 1.0f;

        if (GlobalManager.m_bTutorial_Check == false)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Prototype_ver_1.1");
        }        
    }

    public void SceneChangeToTitle()
    {
        Time.timeScale = 1.0f;

        if (GlobalManager.m_bGameClear == true)
        {
            SceneManager.LoadScene("Title_Moon");
        }
        else
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void SceneChangeToMovie_g() { SceneManager.LoadScene("Movie_g"); }

    

    public void SceneChangeGameIntro() { SceneManager.LoadScene("GameInrto"); }

    public void EndGame() { Application.Quit(); }
}

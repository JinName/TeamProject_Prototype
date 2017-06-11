using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void ChangeScene(string _strGameLevel)
    {
        SceneManager.LoadScene(_strGameLevel);
        Time.timeScale = 1.0f;
    }

    //public void SceneChangeToMainMenu() { SceneManager.LoadScene("MainMenu"); }
    

    public void SceneChangeToGameStart()
    {
        if (GlobalManager.m_bTutorial_Check == false)
        {
            SceneManager.LoadScene("Tutorial");
        }
        else
        {
            SceneManager.LoadScene("Prototype_ver_1.1");
        }

        Time.timeScale = 1.0f;
    }

    public void SceneChangeToTitle()
    {
        if(GlobalManager.m_bGameClear == true)
        {
            SceneManager.LoadScene("Title_Moon");
        }
        else
        {
            SceneManager.LoadScene("Title");
        }

        Time.timeScale = 1.0f;        
    }

    public void SceneChangeToMovie_g() { SceneManager.LoadScene("Movie_g"); }

    

    public void SceneChangeGameIntro() { SceneManager.LoadScene("GameInrto"); }

    public void EndGame() { Application.Quit(); }
}

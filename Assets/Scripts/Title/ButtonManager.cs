using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour {

    public void NewGame(string _strGameLevel)
    {
        SceneManager.LoadScene(_strGameLevel);
    }
}

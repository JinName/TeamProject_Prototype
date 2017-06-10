using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PauseMenuScript : MonoBehaviour {


	private bool pauseOn = false;

	private GameObject normalPanel;
	private GameObject pausePanel;
    private GameObject failPanel;

    void Awake()
	{
		normalPanel = GameObject.Find ("Canvas").transform.Find ("NormalUI").gameObject;
		pausePanel = GameObject.Find ("Canvas").transform.Find ("PauseUI").gameObject;
        failPanel = GameObject.Find("Canvas").transform.Find("FailUI").gameObject;
    }


	public void ActivePauseBt()
	{
		if (!pauseOn)
		{
			Time.timeScale = 0;
			pausePanel.SetActive (true);
			normalPanel.SetActive (false);
            
		} 

		else 
		{
			Time.timeScale = 1.0f;
			pausePanel.SetActive (false);
			normalPanel.SetActive (true);
        }

		pauseOn = !pauseOn;
	}



	public void RetryBt()
	{
		Debug.Log ("게임재시작");
		Time.timeScale = 1.0f;
		SceneManager.LoadScene("Prototype_ver_1.1");
	}

	public void QuitBt()
	{
		Debug.Log ("게임종료");
        SceneManager.LoadScene("Title");
    }

    public void ExitGame()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }

    public void FailGame()
    {
        // Time.timeScale = 0.0f;
        // failPanel.SetActive(true)
        // normalPanel.SetActive(false);
    }

    void update()
    {
        // if 게임성공
        // FailGame()
        // if 게임실패
        // SceneManager.LoadScene("EndingScene");
    }


}

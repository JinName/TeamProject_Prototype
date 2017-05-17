using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class PauseMenuScript : MonoBehaviour {


	private bool pauseOn = false;
	private GameObject normalPanel;
	private GameObject pausePanel;

	void Awake()
	{
		normalPanel = GameObject.Find ("Canvas").transform.FindChild ("NormalUI").gameObject;
		pausePanel = GameObject.Find ("Canvas").transform.FindChild ("PauseUI").gameObject;
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
		SceneManager.LoadScene("Prototype_ver_1.0");
	}

	public void QuitBt()
	{
		Debug.Log ("게임종료");
		Application.Quit();
	}

}

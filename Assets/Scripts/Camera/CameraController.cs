using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    int m_ScreenWidth = 1280;
    int m_ScreenHeight = 720;
	// Use this for initialization
	void Awake () {
        //this.GetComponent<Camera>().aspect = m_ScreenHeight / m_ScreenWidth;
        Screen.SetResolution(m_ScreenWidth, m_ScreenHeight, false);
	}
}

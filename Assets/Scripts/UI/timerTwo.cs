using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class timerTwo : MonoBehaviour {

    Image fillImg;
    public float timeAmt = 60f;
    public float time;

    bool m_bTime_Over = false;

    public bool Get_Time_Over() { return m_bTime_Over; }

	// Use this for initialization
	void Start () {
       
        fillImg = this.GetComponent<Image>();
        time = timeAmt;

	}
	
	// Update is called once per frame
	void Update () {
		
        if(time > 0)
        {
            time -= Time.deltaTime;
            fillImg.fillAmount = time / timeAmt;
        }

        if(time < 0)
        {
            m_bTime_Over = true;
        }

	}
}

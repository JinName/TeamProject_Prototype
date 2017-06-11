using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialCheck : MonoBehaviour {

    private void Awake()
    {
        GlobalManager.m_bTutorial_Check = true;
    }
}

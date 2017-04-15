using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    // 플레이어
    PlayerController playerCtrl;

    // 포탈 스위치
    bool m_bSwitch = false;

    public void Set_PortalSwitch(bool _bSwitch) { m_bSwitch = _bSwitch; }
    public bool Get_PortalSwitch() { return m_bSwitch; }

    private void Awake()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Enter the Trigger");
        if (other.name.Contains("Player") && playerCtrl.Get_usePortal() == false)
        {
            //Debug.Log("in Contains");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Debug.Log("in Portal");
                m_bSwitch = true;
            }
        }
    }
}

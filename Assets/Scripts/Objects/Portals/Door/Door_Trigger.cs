using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Trigger : MonoBehaviour {

    // 트리거 충돌시 포탈 스위치 on
    bool m_bPortal_is_On = false; // 애니매이션 셋팅용 스위치
    bool m_bLets_Teleport = false; // 이동용 스위치

    public bool Get_Trigger_is_On() { return m_bPortal_is_On; }

    public bool Get_Teleport_Switch() { return m_bLets_Teleport; }
    public void Set_Teleport_Switch(bool _bTeleport) { m_bLets_Teleport = _bTeleport; }


    private void OnTriggerEnter(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            m_bPortal_is_On = true;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_bLets_Teleport = true;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            m_bPortal_is_On = false;
        }
    }
}

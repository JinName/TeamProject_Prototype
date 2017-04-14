using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    // 포탈 스위치
    bool m_bSwitch = false;

    public void Set_PortalSwitch(bool _bSwitch) { m_bSwitch = _bSwitch; }
    public bool Get_PortalSwitch() { return m_bSwitch; }

    private void OnTriggerEnter(Collider other)
    {
        m_bSwitch = true;
    }
}

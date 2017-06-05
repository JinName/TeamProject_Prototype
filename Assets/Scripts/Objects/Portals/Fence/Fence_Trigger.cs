using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence_Trigger : MonoBehaviour {

    // Switch
    bool m_bSwitch = false;

    public bool Get_Switch() { return m_bSwitch; }
    public void Set_Switch(bool _bSwtich) { m_bSwitch = _bSwtich; }

    private void OnTriggerStay(Collider other)
    {
        if(other.name.Contains("Player"))
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                m_bSwitch = true;
            }
        }
    }
}

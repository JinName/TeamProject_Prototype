using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink_Trigger : MonoBehaviour {

    // Switch
    bool m_bSwitch = false;

    public bool Get_Switch() { return m_bSwitch; }
    public void Set_Switch(bool _bSwitch) { m_bSwitch = _bSwitch; }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                m_bSwitch = true;
            }
        }
    }
}

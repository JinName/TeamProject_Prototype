using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunnel_Trigger : MonoBehaviour {

    // Switch
    bool m_bSwitch = false;

    public bool Get_Switch() { return m_bSwitch; }
    public void Set_Switch(bool _bSwtich) { m_bSwitch = _bSwtich; }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            Debug.Log("Trigger_Stay");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("input");
                m_bSwitch = true;
            }
        }
    }
}

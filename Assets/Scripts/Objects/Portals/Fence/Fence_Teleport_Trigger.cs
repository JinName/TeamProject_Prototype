using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence_Teleport_Trigger : MonoBehaviour {

    bool m_bTeleport_Switch = false;
    bool m_bSetting = false;

    GameObject playerCtrl;
    GameObject m_Enemy;

    public bool Get_Teleport_Switch() { return m_bTeleport_Switch; }

    public void Set_Setting(bool _bSetting) { m_bSetting = _bSetting; }

    private void Awake()
    {
        playerCtrl = GameObject.Find("Player");
        m_Enemy = GameObject.Find("Enemy");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            if (m_bSetting == false)
            { 
                playerCtrl.GetComponent<PlayerController>().Set_Ready_to_Teleport(true);
                m_bTeleport_Switch = true;

                m_bSetting = true;
            }
        }

        if (other.name.Contains("Enemy"))
        {
            if (m_bSetting == false)
            {
                m_Enemy.GetComponent<EnemyAI>().Set_Ready_to_Teleport(true);
                m_bTeleport_Switch = true;

                m_bSetting = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            m_bTeleport_Switch = false;
            m_bSetting = false;
        }

        if (other.name.Contains("Enemy"))
        {
            m_bTeleport_Switch = false;
            m_bSetting = false;
        }
    }
}

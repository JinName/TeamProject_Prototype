using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blink_Trigger : MonoBehaviour {

    PlayerController m_Player;
    EnemyAI m_Enemy;
    // Switch
    bool m_bSwitch = false;
    bool m_bEnemy_Used = false;

    public void Set_Enemy_Used(bool _bEnemy_Used) { m_bEnemy_Used = _bEnemy_Used; }

    public bool Get_Switch() { return m_bSwitch; }
    public void Set_Switch(bool _bSwitch) { m_bSwitch = _bSwitch; }

    private void Awake()
    {
        m_Player = GameObject.Find("Player").GetComponent<PlayerController>();
        m_Enemy = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Enemy") && m_Enemy.Get_Trigger_is_Possible() == true)
        {
            if (m_bEnemy_Used == false)
            {
                m_bSwitch = true;
                m_Enemy.Set_Enemy_use_Portal(true);
            }
            Debug.Log("Enemy In Blink Trigger");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Player"))
        {
            if(Input.GetKeyDown(KeyCode.Z))
            {
                m_bSwitch = true;
                m_Player.Set_Player_use_Portal(true);
            }
        }
    }
}

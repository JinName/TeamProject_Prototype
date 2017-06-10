using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door_Trigger : MonoBehaviour {

    PlayerController m_Player;
    EnemyAI m_Enemy;

    // 트리거 충돌시 포탈 스위치 on
    bool m_bPortal_is_On = false; // 애니매이션 셋팅용 스위치
    bool m_bLets_Teleport = false; // 이동용 스위치

    bool m_bEnemy_Used = false;
    bool isPlayer = false;

    public bool Get_isPlayer() { return isPlayer; }

    public void Set_Enemy_Used(bool _bEnemy_Used) { m_bEnemy_Used = _bEnemy_Used; }

    public bool Get_Trigger_is_On() { return m_bPortal_is_On; }

    public bool Get_Switch() { return m_bLets_Teleport; }
    public void Set_Switch(bool _bTeleport) { m_bLets_Teleport = _bTeleport; }

    private void Awake()
    {
        m_Player = GameObject.Find("Player").GetComponent<PlayerController>();
        m_Enemy = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }


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
                isPlayer = true;
                m_Player.Set_Player_use_Portal(true);
            }
        }

        if (other.name.Contains("Enemy") && m_Enemy.Get_Trigger_is_Possible())
        {
            if (m_bEnemy_Used == false)
            {
                if (this.transform.position.x + 0.2f > other.transform.position.x &&
                    this.transform.position.x - 0.2f < other.transform.position.x)
                {
                    m_bLets_Teleport = true;
                    isPlayer = false;
                    m_Enemy.Set_Enemy_use_Portal(true);
                }
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

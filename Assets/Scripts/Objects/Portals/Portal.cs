using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    // 플레이어
    PlayerController playerCtrl;
    // Enemy
    EnemyAI enemyAI;

    // 포탈의 층
    int m_PortalFloor;

    // 포탈 스위치
    bool m_bSwitch = false;

    // 플레이어, 적 구분
    bool isPlayer = false;

    public void Set_PortalSwitch(bool _bSwitch) { m_bSwitch = _bSwitch; }
    public bool Get_PortalSwitch() { return m_bSwitch; }

    // 충돌한게 플레이어인지 적인지 매니저에서 구분하기 위한 변수
    public bool Get_Player_in_Portal() { return isPlayer; }

    private void Awake()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyAI = GameObject.Find("Enemy").GetComponent<EnemyAI>();

        PortalFloor();
    }

    void PortalFloor()
    {
        if (this.transform.position.y > 0f && this.transform.position.y < 2.55f)
            m_PortalFloor = 1;
        else if (this.transform.position.y > 0f && this.transform.position.y < 2.55f)
            m_PortalFloor = 2;
        else if (this.transform.position.y > 0f && this.transform.position.y < 2.55f)
            m_PortalFloor = 3;
        else if (this.transform.position.y > 0f && this.transform.position.y < 2.55f)
            m_PortalFloor = 4;
    }

    public int Get_PortalFloor() { return m_PortalFloor; }

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("Enter the Trigger");
        if (other.name.Contains("Player") && playerCtrl.Get_usePortal() == false)
        {
            //Debug.Log("in Contains");
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Debug.Log("in Portal");
                isPlayer = true;
                m_bSwitch = true;
            }
        }
        else if (other.name.Contains("Enemy") && enemyAI.Get_AI_State() == 2)
        {
            if (enemyAI.Get_usePortal() == false)
            {
                isPlayer = false;
                m_bSwitch = true;
            }
        }
    }
}

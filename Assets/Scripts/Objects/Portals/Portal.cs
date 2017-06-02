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
    // 파트너 포탈의 층
    int m_Partner_Portal_Floor;

    // 포탈 스위치
    bool m_bSwitch = false;
    bool m_bEnmey_Use = false;

    // 플레이어, 적 구분
    bool isPlayer = false;

    // Enemy가 마구잡이로 포탈 이용하는 것을 방지
    public void Set_Enemy_Use(bool _Enemy_Use) { m_bEnmey_Use = _Enemy_Use; }

    // 포탈 스위치 - 포탈 매니저에서 관리
    public void Set_PortalSwitch(bool _bSwitch) { m_bSwitch = _bSwitch; }
    public bool Get_PortalSwitch() { return m_bSwitch; }

    // 충돌한게 플레이어인지 적인지 매니저에서 구분하기 위한 변수
    public bool Get_Player_in_Portal() { return isPlayer; }

    // 포탈의 층 반환
    public int Get_PortalFloor() { return m_PortalFloor; }

    // 파트너 포탈의 층 셋팅
    public int Get_Partner_Portal_Floor() { return m_Partner_Portal_Floor; }
    public void Set_Partner_Portal_Floor(int _Partner_Floor) { m_Partner_Portal_Floor = _Partner_Floor; }

    private void Awake()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyAI = GameObject.Find("Enemy").GetComponent<EnemyAI>();

        PortalFloor();
    }

    void PortalFloor()
    {
        if (this.transform.position.y > -0.2f && this.transform.position.y < 2.3f)
            m_PortalFloor = 1;
        else if (this.transform.position.y > 2.3f && this.transform.position.y < 4.9f)
            m_PortalFloor = 2;
        else if (this.transform.position.y > 4.9f && this.transform.position.y < 7.5f)
            m_PortalFloor = 3;
        else if (this.transform.position.y > 7.5f && this.transform.position.y < 10.2f)
            m_PortalFloor = 4;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("Player") && playerCtrl.Get_usePortal() == false)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                isPlayer = true;
                m_bSwitch = true;
            }
        }
        else if (other.name.Contains("Enemy") && (enemyAI.Get_AI_State() == 3 || enemyAI.Get_AI_State() == 4))
        {
            if (m_bEnmey_Use == true)
            {
                if (enemyAI.Get_usePortal() == false)
                {
                    isPlayer = false;
                    m_bSwitch = true;
                }
            }
        }
    }
}

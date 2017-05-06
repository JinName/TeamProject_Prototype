using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    // 플레이어
    PlayerController playerCtrl;
    // Enemy
    EnemyAI enemyAI;

    // 포탈 / 문 / 개구멍 / 스프링 오브젝트
    // 일단 임시 포탈
    GameObject m_Red;
    GameObject m_Green;
    GameObject m_Blue;
    GameObject m_Yellow;

    GameObject m_PortalClone;

    // 포탈 리스트
    List<GameObject> m_PortalList;
    // 포탈 갯수
    int m_iPortal_Count = 8;

    private void Awake()
    {
        // 플레이어
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        enemyAI = GameObject.Find("Enemy").GetComponent<EnemyAI>();

        // 포탈
        m_Red = GameObject.FindGameObjectWithTag("Red1");
        m_Green = GameObject.FindGameObjectWithTag("Green1");
        m_Blue = GameObject.FindGameObjectWithTag("Blue1");
        m_Yellow = GameObject.FindGameObjectWithTag("Yellow1");

        m_PortalList = new List<GameObject>();

        m_PortalList.Add(m_Red);
        m_PortalList.Add(m_Green);
        m_PortalList.Add(m_Blue);
        m_PortalList.Add(m_Yellow);

        createPortals();
    }

    private void Update()
    {
        onPortalCheck();
    }

    void createPortals()
    {
        // 임시 포탈 생성
        m_PortalClone = Instantiate(m_Red, new Vector3(m_Red.transform.position.x + 3f, m_Red.transform.position.y + 5.1f, m_Red.transform.position.z), Quaternion.identity);
        m_PortalList.Add(m_PortalClone);

        m_PortalClone = Instantiate(m_Green, new Vector3(m_Green.transform.position.x + 6f, m_Green.transform.position.y + 5.1f, m_Green.transform.position.z), Quaternion.identity);
        m_PortalList.Add(m_PortalClone);

        m_PortalClone = Instantiate(m_Blue, new Vector3(m_Blue.transform.position.x + 6f, m_Blue.transform.position.y + 2.55f, m_Blue.transform.position.z), Quaternion.identity);
        m_PortalList.Add(m_PortalClone);

        m_PortalClone = Instantiate(m_Yellow, new Vector3(m_Yellow.transform.position.x - 3f, m_Yellow.transform.position.y + 2.55f, m_Yellow.transform.position.z), Quaternion.identity);
        m_PortalList.Add(m_PortalClone);
        
        //파트너 포탈 셋팅
        for(int i = 0; i < 8; ++i)
        {
            if( i < 4 )
                m_PortalList[i].GetComponent<Portal>().Set_Partner_Portal_Floor(m_PortalList[i + 4].GetComponent<Portal>().Get_PortalFloor());
            else if( i >= 4 )
                m_PortalList[i].GetComponent<Portal>().Set_Partner_Portal_Floor(m_PortalList[i - 4].GetComponent<Portal>().Get_PortalFloor());
        }
    }
    
    void onPortalCheck()
    {
        // 어떤 포탈이 on 됐는지 확인
        for(int i = 0; i < 8; ++i)
        {
            if(m_PortalList[i].GetComponent<Portal>().Get_PortalSwitch() == true)
            {
                moveOtherPortal(m_PortalList, i);

                m_PortalList[i].GetComponent<Portal>().Set_PortalSwitch(false);
            }
        }
    }

    void moveOtherPortal(List<GameObject> _portal, int _portalNum)
    {
        // 같은 종류의 다른 위치 포탈로 이동
        // 리스트 [i] 로 구분
        if (_portal[_portalNum].GetComponent<Portal>().Get_Player_in_Portal() == true)
        {
            if (_portalNum > 3)
            {
                playerCtrl.Set_PlayerPosition(_portal[_portalNum - 4].transform.position);
            }
            else
            {
                playerCtrl.Set_PlayerPosition(_portal[_portalNum + 4].transform.position);
            }

            playerCtrl.Set_usePortal(true);
        }
        else if (_portal[_portalNum].GetComponent<Portal>().Get_Player_in_Portal() == false)
        {
            if (_portalNum > 3)
            {
                enemyAI.Set_EnemyPosition(_portal[_portalNum - 4].transform.position);
            }
            else
            {
                enemyAI.Set_EnemyPosition(_portal[_portalNum + 4].transform.position);
            }

            enemyAI.Set_usePortal(true);
            //enemyAI.Set_AI_4_Setting(true);
            _portal[_portalNum].GetComponent<Portal>().Set_Enemy_Use(false);
        }
    }

    public void Reset_Portal_Useable()
    {
        for (int i = 0; i < m_iPortal_Count; ++i)
        {
            m_PortalList[i].GetComponent<Portal>().Set_Enemy_Use(false);
        }
    }

    // AI 용 입력한 층과 같은 층에 있는 포탈 중 플레이어에게 갈 수 있는 포탈 있는지, 없으면 거리가 먼 포탈을 반환
    public Vector3 Search_toPlayer_Portal(int _floor, Vector3 _position)
    {
        Vector3 portalVector = new Vector3(0f, 0f, 0f);
        // 한번에 플레이어에가 갈 수 있는 포탈 있는지 없는지
        bool toPlayer = false;
        // 해당 층의 포탈 넘버
        int[] portalNum = new int[2] { 0, 0 };
        int j = 0;
        // 포탈과 매개변수 포지션과의 거리
        float[] Distance = new float[2] { 0f, 0f };

        // 플레이어 한번에 갈 수 있는 포탈이 있는지 찾아봄
        for (int i = 0; i < m_iPortal_Count; ++i)
        {
            if(m_PortalList[i].GetComponent<Portal>().Get_PortalFloor() == _floor)
            {
                portalNum[j] = i;
                j = j + 1;
                if(m_PortalList[i].GetComponent<Portal>().Get_Partner_Portal_Floor() == playerCtrl.Get_PlayerFloor())
                {
                    Debug.Log("Enemy Floor's Portal -> Player Floor");
                    m_PortalList[i].GetComponent<Portal>().Set_Enemy_Use(true);
                    portalVector = m_PortalList[i].transform.position;
                    toPlayer = true;
                }
            }
        }

        // 플레이어에게 한번에 갈 수 없다면, 같은 층에 있는 포탈 중 거리가 먼 포탈 반환
        if (toPlayer == false)
        {
            Distance[0] = Mathf.Abs(m_PortalList[portalNum[0]].transform.position.x - _position.x);
            Distance[1] = Mathf.Abs(m_PortalList[portalNum[1]].transform.position.x - _position.x);

            if(Distance[0] > Distance[1])
            {
                portalVector = m_PortalList[portalNum[0]].transform.position;
                m_PortalList[portalNum[0]].GetComponent<Portal>().Set_Enemy_Use(true);
            }
            else
            {
                portalVector = m_PortalList[portalNum[1]].transform.position;
                m_PortalList[portalNum[1]].GetComponent<Portal>().Set_Enemy_Use(true);
            }
        }

        return portalVector;
    }

    // AI 용 포탈 위치 반환 - 타겟으로 이용, AI 움직이게 만듦
    public Vector3 Search_Close_Portal(int _floor, Vector3 _position)
    {
        Vector3 portalVector = new Vector3(0f, 0f, 0f);

        float[] Distance = new float[2] { 0f, 0f };
        int[] portalNum = new int[2] { 0, 0 };

        int j = 0;

        for (int i = 0; i < 8; ++i)
        {
            if (m_PortalList[i].GetComponent<Portal>().Get_PortalFloor() == _floor)
            {
                //Debug.Log("Enmey 와 같은 층의 포탈 번호 : " + i.ToString());
                if (j < 2)
                {
                    Distance[j] = m_PortalList[i].GetComponent<Portal>().transform.position.x - _position.x;
                    portalNum[j] = i;
                    j = j + 1;
                }
            }
        }

        if (Mathf.Abs(Distance[0]) < Mathf.Abs(Distance[1]))
        {
            portalVector = m_PortalList[portalNum[0]].transform.position;
            m_PortalList[portalNum[0]].GetComponent<Portal>().Set_Enemy_Use(true);
            Debug.Log("0 == true");
        }
        else if (Mathf.Abs(Distance[0]) > Mathf.Abs(Distance[1]))
        {
            portalVector = m_PortalList[portalNum[1]].transform.position;
            m_PortalList[portalNum[1]].GetComponent<Portal>().Set_Enemy_Use(true);
            Debug.Log("1 == true");
        }

        return portalVector;
    }
}

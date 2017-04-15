using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    // 플레이어
    PlayerController playerCtrl;

    // 포탈 / 문 / 개구멍 / 스프링 오브젝트
    // 일단 임시 포탈
    GameObject m_Red;
    GameObject m_Green;
    GameObject m_Blue;
    GameObject m_Yellow;

    GameObject m_PortalClone;

    // 포탈 리스트
    List<GameObject> m_PortalList;

    private void Awake()
    {
        // 플레이어
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();

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
    }
    
    void onPortalCheck()
    {
        // 어떤 포탈이 on 됐는지 확인
        for(int i = 0; i < 8; ++i)
        {
            if(m_PortalList[i].GetComponent<Portal>().Get_PortalSwitch() == true)
            {
                //Debug.Log("onPortalCheck");
                moveOtherPortal(m_PortalList, i);

                m_PortalList[i].GetComponent<Portal>().Set_PortalSwitch(false);
            }
        }
    }

    void moveOtherPortal(List<GameObject> _portal, int _portalNum)
    {
        // 같은 종류의 다른 위치 포탈로 이동
        // 리스트 [i] 로 구분
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
}

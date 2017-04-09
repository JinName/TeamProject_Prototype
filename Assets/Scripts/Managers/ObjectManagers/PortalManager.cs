using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    // 포탈 / 문 / 개구멍 / 스프링 오브젝트
    GameObject m_Portal;

    // 포탈 리스트
    List<GameObject> m_PortalList;

    private void Awake()
    {
        m_PortalList = new List<GameObject>();
    }

    void createObjects()
    {
        // 리스트로 포탈 생성
    }
    
    void onPortalCheck()
    {
        // 어떤 포탈이 on 됐는지 확인
    }

    void moveOtherPortal()
    {
        // 같은 종류의 다른 위치 포탈로 이동
        // 리스트 [i] 로 구분
    }
}

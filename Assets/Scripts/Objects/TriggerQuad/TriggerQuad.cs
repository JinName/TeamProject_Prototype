using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerQuad : MonoBehaviour {

    // 누가 점령 중인지 판별하기 위한 변수 / 초기값 = 1 (해의 땅)
    int m_iWhosTile;

    // 특수 타일에 해당하는 트리거
    bool m_bSpecial = false;

    // 트리거에 진입했을때 true
    bool m_bSwitch = false;

    // 트리거 위에 있을때 (특수타일 점령)
    bool m_bStay = false;

    // 연속회전 방지 변수
    bool m_bStopRotate = false;

    // 3초 점령 판별 변수
    float m_fStayTime = 5.0f;
    bool m_bConquer;


	// Use this for initialization
	void Awake () {
        m_iWhosTile = 1;
        m_bSwitch = false;
        m_bStopRotate = false;
        m_bConquer = false;

    }

    // 스페셜 타일의 트리거인지
    public bool Get_isSpecial() { return m_bSpecial; }
    public void Set_isSpecial(bool _bSpecial) { m_bSpecial = _bSpecial; }

    // 스페셜 타일이 점령되었는지
    public bool Get_isConquered() { return m_bConquer; }
    public void Set_isConquered(bool _bConquer) { m_bConquer = _bConquer; }

    // 타일매니저에서 충돌 판별을 위한 get, set
    // 회전해야할지
    public bool Get_TriggerSwitch() { return m_bSwitch; }
    public void Set_TriggerSwitch(bool _bSwitch) { m_bSwitch = _bSwitch; }

    // 연속 회전 방지
    public bool Get_StopRotate() { return m_bStopRotate; }
    public void Set_StopRotate(bool _bStopRotate) { m_bStopRotate = _bStopRotate; }

    // 누가 점령 중인지 리턴
    public int Get_WhosTile() { return m_iWhosTile; }

    // 트리거 위에 서있는지
    public bool Get_isStay() { return m_bStay; }

    // OnTrigger 함수들에선 other 이 Player 인지, Enemy 인지 판별할것.

    // 트리거에 진입했을때
    private void OnTriggerEnter(Collider other)
    {
        if (m_bSpecial == false)
        {
            if (m_iWhosTile == 1)
            {
                if (other.name.Contains("Player"))
                {
                    m_bSwitch = true;
                    m_iWhosTile = 2;
                    
                }
            }
            else if (m_iWhosTile == 2)
            {
                if (other.name.Contains("Enemy"))
                {
                    if (m_bConquer == false)
                    {
                        m_bSwitch = true;
                        m_iWhosTile = 1;
                    }
                }
            }
        }
        
    }

    // 트리거에 서있는 경우 (연속회전 방지 + 3초 이상 머무를시 고정 점령)
    private void OnTriggerStay(Collider other)
    {
        if (m_bSpecial == true)
        {
            if (other.name.Contains("Player") && m_iWhosTile == 1)
            {
                m_fStayTime -= Time.deltaTime;

                if (m_fStayTime <= 0f)
                {
                    m_bSwitch = true;
                    m_bConquer = true;
                    m_iWhosTile = 2;
                    m_fStayTime = 5.0f;
                    Debug.Log("Conquered !");
                }
            }
        }
    }

    // 트리거에서 빠져나올때
    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Player") && m_iWhosTile == 1)
        {
            m_fStayTime = 5.0f;
        }
    }
}

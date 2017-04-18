using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    // 적 속도
    private float m_fEnmeySpeed;

    // AI 4가지 상태
    // 1 : 한층 좌우 돌아다님
    // 2 : 가까운 포탈 이용 ( 적이 있는 층에 포탈 위치 받아와서 가까운곳 계산 )
    // 몇층에 어떤 포탈이 있는지 정보를 가지고 있어야 함 -> portal manager 필요
    // 3 :
    // 4 : 특수타일 점령 시 플레이어 있는 층으로
    private int m_iState_AI = 1;

    // AI State CoolTime : 같은 상태가 다시 나오지 않도록
    private bool m_bAI_On = false;
    // 각 상태 지속시간 -> 3 초마다 상태가 바뀜
    private float m_fState_Runtime = 5.0f;

    // 현재 적이 있는 층
    private int m_iEnemyFloor;

    // 포탈 매니저 이용
    PortalManager m_Portal_Manager;
    // 포탈 한번만 찾고 다음부터 안찾음
    Vector3 m_vTarget;
    float m_fOffset;
    bool m_bSetting_Complete = false;
    // Enemy 포탈 이용 쿨타임
    bool usePortal = false;
    float m_fCooltime = 3.0f;

    // 적의 시작 위치 값
    private float m_fx = 2.2f; // 유동적
    private float m_fy = 7.9f; // 층에 따라 고정적
    private float m_fz = -0.363f; // 고정
    // 적의 위치값 변경
    Vector3 m_vEnemyPosition;
    // 초기 방향 변수 > 0 : 오른쪽 / < 0 : 왼쪽
    private float m_fDirection; 

    // 적 좌우 이동 범위 제한
    private float min_x = 2.0f;
    private float max_x = 7.85f;

    // 캐릭터 Rigidbody
    Rigidbody m_Rigidbody;

    // 애니매이터
    Animator m_Animator;
    bool walking = false;

    // 초기화
    private void Awake()
    {
        // Portal Manager
        m_Portal_Manager = GameObject.Find("Portals").GetComponent<PortalManager>();

        // Animator
        m_Animator = GetComponent<Animator>();
        // Rigidbody
        m_Rigidbody = GetComponent<Rigidbody>();

        // 적 이동 속도
        m_fEnmeySpeed = 20.0f;

        // 상태값 랜덤 초기화 / 임시값 1
        //m_iState_AI = Random.Range(1, 3); 4는 특수 상태
        m_iState_AI = 1;
        m_bAI_On = true;
        Debug.Log(m_iState_AI);

        // 시작층 = 4
        m_iEnemyFloor = 4;

        // 위치 값
        this.transform.position = new Vector3(m_fx, m_fy, m_fz);

        // 초기 이동방향 = 오른쪽
        m_fDirection = 0.1f;

        // animator
        walking = true;
    }


    private void Update()
    {
        AI_Move();
        AI_Portal_Cooltime();

        Animating();
        Turning();

        EnemyFloor();
    }

    // Enemy 의 현재 층
    void EnemyFloor()
    {
        if (this.transform.position.y > 0f && this.transform.position.y < 2.55f)
            m_iEnemyFloor = 1;
        else if (this.transform.position.y > 2.55f && this.transform.position.y < 5.1f)
            m_iEnemyFloor = 2;
        else if (this.transform.position.y > 5.1f && this.transform.position.y < 7.65f)
            m_iEnemyFloor = 3;
        else if (this.transform.position.y > 7.65f && this.transform.position.y < 10.2f)
            m_iEnemyFloor = 4;
    }

    // 현재 층 반환
    public int Get_EnemyFloor() { return m_iEnemyFloor; }

    // 포탈 이용 했는지 반환
    public void Set_usePortal(bool _usePortal) { usePortal = _usePortal; }
    public bool Get_usePortal() { return usePortal; }

    // 현재 AI 번호 반환
    public int Get_AI_State() { return m_iState_AI; }

    // 적의 위치 변경
    public void Set_EnemyPosition(Vector3 _vPosition)
    { this.transform.position = _vPosition; }

    private void Animating()
    {
        m_Animator.SetBool("IsWalking", walking);
    }

    

    // AI 포탈 쿨타임
    private void AI_Portal_Cooltime()
    {
        if (usePortal == true)
        {
            if (m_fCooltime > 0f)
            {
                m_fCooltime -= Time.deltaTime;
                //Debug.Log(coolTime);
                if (m_fCooltime <= 0f)
                {
                    usePortal = false;
                    m_fCooltime = 3.0f;
                }
            }
        }
    }

    private void Turning()
    {
        if ( m_fDirection > 0 )
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
        }
    }

    // AI 상태에 따른 움직임
    private void AI_Move()
    {
        if (m_bAI_On == true)
        {
            if (m_iState_AI == 1)
            {
                Debug.Log("AI_1");
                AI_1();
            }
            else if (m_iState_AI == 2)
            {
                Debug.Log("AI_2");
                AI_2();
            }
        }

        // AI 가 중복되지않게 랜덤으로 선택
        if (m_bAI_On == false)
        {
            //int CurrentAI = m_iState_AI;
            //while (true)
            {
                Debug.Log("False");
                m_iState_AI = Random.Range(1, 3);
                //if (m_iState_AI != CurrentAI)
                {
                    m_bAI_On = true;
                    //break;
                }
                    
            }
        }
    }

    private void Move()
    {
        if (this.transform.position.x > max_x)
        {
            m_fDirection = -0.1f;
        }
        else if (this.transform.position.x < min_x)
        {
            m_fDirection = 0.1f;
        }

        m_vEnemyPosition.Set(m_fDirection, 0f, 0f);
        m_vEnemyPosition = m_vEnemyPosition * m_fEnmeySpeed * Time.deltaTime;
        m_Rigidbody.MovePosition(transform.position + m_vEnemyPosition);
    }
    // 1번 상태 : 좌우 움직임
    private void AI_1()
    {
        Move();

        if(m_fState_Runtime > 0)
        {
            m_fState_Runtime -= Time.deltaTime;

            if(m_fState_Runtime <= 0)
            {
                m_bAI_On = false;
                m_fState_Runtime = 5.0f;
            }
        }
    }

    // 2번 상태 : 가까운 포탈 찾아 다른층으로 이동
    private void AI_2()
    {
        // 매개변수로 들어간 층 수에 존재하는 가까운 포탈 위치 벡터 반환
        if (m_bSetting_Complete == false)
        {
            m_vTarget = m_Portal_Manager.Search_Close_Portal(m_iEnemyFloor, this.transform.position);
            m_fOffset = m_vTarget.x - this.transform.position.x;

            if (m_fOffset > 0) // 오른쪽
            {
                m_fDirection = 0.1f;
            }
            else if (m_fOffset < 0) // 왼쪽
            {
                m_fDirection = -0.1f;
            }
            else // 제자리
            { }

            m_bSetting_Complete = true;
        }

        Move();

        if (usePortal == true)
        {
            m_iState_AI = 1;
            m_bSetting_Complete = false;
            //m_bAI_On = false;
        }
    }
}

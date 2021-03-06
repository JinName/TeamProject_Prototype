﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour {

    // 적 속도
    private float m_fEnmeySpeed;

    // AI 4가지 상태
    // 1 : 한층 좌우 돌아다님
    // 2 : 가만히 좌우 두리번
    // 3 : 가까운 포탈 이용 ( 적이 있는 층에 포탈 위치 받아와서 가까운곳 계산 )
    // 4 : 특수타일 점령 시 플레이어 있는 층으로
    // 몇층에 어떤 포탈이 있는지 정보를 가지고 있어야 함 -> portal manager 필요
    private int m_iState_AI = 1; // 기본 상태
    List<int> stateList; // 리스트를 이용 상태 중복시 제거
    int m_iState_Count = 4; // 상태의 총 갯수

    // AI State CoolTime : 같은 상태가 다시 나오지 않도록
    private bool m_bAI_On = false;
    // 각 상태 지속시간 -> 3 초마다 상태가 바뀜
    private float m_fState_Runtime = 4.0f;

    // 현재 적이 있는 층
    private int m_iEnemyFloor;
    bool m_bTrigger_is_Possible = false;

    // 포탈 매니저 이용
    PortalManager m_Portal_Manager;
    // 포탈 한번만 찾고 다음부터 안찾음
    Vector3 m_vTarget;
    float m_fOffset;
    bool m_bSetting_Complete = false;
    // Enemy 포탈 이용 쿨타임
    bool usePortal = false;
    float m_fCooltime = 1.0f;

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
    // 3번 AI 애니매이션
    bool lookAround = false;

    // 플레이어가 특수타일 점령 중인지 확인
    // 추후 게임매니저의 업데이트 쪽에서 지속적으로 플레이어의 상태 업데이트
    // 현재는 그냥 플레이어객체를 직접 받아옴
    PlayerController playerCtrl;
    bool Player_in_Special = false;
    bool Setting_AI_4 = false;

    bool m_bEnemyLock = false;
    bool m_bEntering = false;
    bool m_bReady_to_Teleport = false;
    float m_fOffset_x = 0f;
    bool m_bEnemy_use_Portal = false;
    bool m_bPortal_Setting_Complete = false;

    public bool Get_Ready_to_Teleport() { return m_bReady_to_Teleport; }
    public void Set_Ready_to_Teleport(bool _bReady_to_Teleport) { m_bReady_to_Teleport = _bReady_to_Teleport; }

    public bool Get_Enemy_Lock() { return m_bEnemyLock; }

    public bool Get_Enemy_use_Portal() { return m_bEnemy_use_Portal; }
    public void Set_Enemy_use_Portal(bool _bEnemy_use_Portal) { m_bEnemy_use_Portal = _bEnemy_use_Portal; }

    public bool Get_Enter() { return m_bEntering; }
    public void Set_Enter(bool _bEntering) { m_bEntering = _bEntering; }

    // 초기화
    private void Awake()
    {
        // Player
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();

        // Portal Manager
        m_Portal_Manager = GameObject.Find("PortalManagerObj").GetComponent<PortalManager>();

        // Animator
        m_Animator = GetComponent<Animator>();
        // Rigidbody
        m_Rigidbody = GetComponent<Rigidbody>();

        // 적 이동 속도
        m_fEnmeySpeed = 12.0f;

        // 상태값 랜덤 초기화 / 임시값 1
        //m_iState_AI = Random.Range(1, 4); 4는 특수 상태
        stateList = new List<int>();
        for (int i = 0; i < m_iState_Count - 1; ++i)
        {
            stateList.Add(i + 1);
        }

        m_iState_AI = 1;
        m_bAI_On = true;

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
        if (m_bEnemyLock == false)
        {
            Animating();
            Turning();

            AI_Move();
        }

        EnemyFloor();
    }

    // Enemy 의 현재 층
    void EnemyFloor()
    {
        if (this.transform.position.y > -0.2f && this.transform.position.y < 2.3f)
            m_iEnemyFloor = 1;
        else if (this.transform.position.y > 2.3f && this.transform.position.y < 4.9f)
            m_iEnemyFloor = 2;
        else if (this.transform.position.y > 4.9f && this.transform.position.y < 7.5f)
            m_iEnemyFloor = 3;
        else if (this.transform.position.y > 7.5f && this.transform.position.y < 10.2f)
            m_iEnemyFloor = 4;
    }

    // 트리거와 충돌 가능 상태인지
    public bool Get_Trigger_is_Possible() { return m_bTrigger_is_Possible; }

    // 현재 층 반환
    public int Get_EnemyFloor() { return m_iEnemyFloor; }

    // 포탈 이용 했는지 반환
    public void Set_usePortal(bool _usePortal) { usePortal = _usePortal; }
    public bool Get_usePortal() { return usePortal; }

    // 플레이어가 특수타일에서 벗어나면 추적멈추고 상태 초기화
    public void Set_AI_On(bool _bAI_On) { m_bAI_On = _bAI_On; }
    public void Set_Setting(bool _Setting) { Setting_AI_4 = _Setting; }

    // 현재 AI 번호 반환
    public int Get_AI_State() { return m_iState_AI; }

    // 적의 위치 변경
    public void Set_EnemyPosition(Vector3 _vPosition)
    { this.transform.position = _vPosition; }

    // 스페셜 타일 점령 중일 경우 스위치 on / off
    public void Set_AI_4(bool _special) { Player_in_Special = _special; }

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
                if (m_fCooltime <= 0f)
                {
                    usePortal = false;
                    m_fCooltime = 1.0f;
                }
            }
        }
    }

    private void Turning()
    {
        if ( m_fDirection > 0 )
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 135f, 0f));
        }
        else if ( m_fDirection < 0 )
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 225f, 0f));
        }
        else if ( m_fDirection == 0 )
        {
            this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
        }
    }

    // AI 상태에 따른 움직임
    private void AI_Move()
    {
        // 플레이어가 특수타일 점령 중 이면 AI 번호 4로 바꿈
        if (Setting_AI_4 == false)
        {
            if (Player_in_Special == true)
            {
                m_iState_AI = 4;
                stateList.Remove(3);
                m_bSetting_Complete = false;
                m_bAI_On = true;
                Setting_AI_4 = true;
            }
        }

        // AI 가 중복되지않게 랜덤으로 선택
        if (m_bAI_On == false)
        {
            m_Portal_Manager.Reset_Portal_Useable();

            int temp = Random.Range(0, stateList.Count);
            m_iState_AI = stateList[temp];
            stateList.Add(3);

            m_fEnmeySpeed = 12f;
            m_bSetting_Complete = false;
            usePortal = false;
            m_bAI_On = true;
        }


        if (m_bAI_On == true)
        {
            if (m_iState_AI == 1)
            {
                AI_1();
            }
            else if (m_iState_AI == 2)
            {
                AI_2();
            }
            else if (m_iState_AI == 3)
            {
                AI_3();
            }
            else if (m_iState_AI == 4)
            {
                AI_4();
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
        walking = true;
        if (m_fDirection == 0f)
            m_fDirection = 0.1f;

        Move();

        if(m_fState_Runtime > 0)
        {
            m_fState_Runtime -= Time.deltaTime;

            if(m_fState_Runtime <= 0)
            {
                m_bAI_On = false;
                m_fState_Runtime = 4.0f;
            }
        }
    }

    // 2번 상태 : 제자리에서 두리번
    private void AI_2()
    {
        // 상태에 필요한 초기 셋팅
        if (m_bSetting_Complete == false)
        {
            m_fDirection = 0.0f;
            walking = false;
            m_bSetting_Complete = true;
        }

        // 상태 지속시간
        if (m_fState_Runtime > 0)
        {
            m_fState_Runtime -= Time.deltaTime;

            // 상태 끝날때 설정 초기화
            if (m_fState_Runtime <= 0)
            {
                m_bAI_On = false;
                m_fState_Runtime = 4.0f;
                m_bSetting_Complete = false;
            }
        }
        
    }

    // 3번 상태 : 가까운 포탈 이용
    private void AI_3()
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

            walking = true;
            m_bTrigger_is_Possible = true;
            m_bSetting_Complete = true;
            
        }

        Move();

        if (usePortal == true)
        {
            m_iState_AI = 1;
            m_bTrigger_is_Possible = false;
            m_bSetting_Complete = false;
            usePortal = false;
        }
    }

    // 4번 상태 : if (특수타일 점령 중이면)
    //                  쫓아감;
    private void AI_4()
    {
        if (m_bSetting_Complete == false)
        {
            m_fEnmeySpeed = 12f;

            if (m_iEnemyFloor == playerCtrl.Get_PlayerFloor())
            {
                m_vTarget = playerCtrl.transform.position;
            }
            else
            {
                m_vTarget = m_Portal_Manager.Search_toPlayer_Portal(m_iEnemyFloor, this.transform.position);
            }

            m_fOffset = m_vTarget.x - this.transform.position.x;

            if (m_fOffset > 0) // 오른쪽
            {
                m_fDirection = 0.1f;
            }
            else if (m_fOffset < 0) // 왼쪽
            {
                m_fDirection = -0.1f;
            }

            walking = true;
            m_bTrigger_is_Possible = true;
            m_bSetting_Complete = true;
        }

        Move();

        if (usePortal == true)
        {
            m_bTrigger_is_Possible = false;
            m_bSetting_Complete = false;

            usePortal = false;
        }
    }

    // DoorAction
    public void into_the_Door()
    {
        if (m_bEntering == true)
        {
            if (m_bPortal_Setting_Complete == false)
            {
                m_bEnemyLock = true;

                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 360f, 0f));
                m_bPortal_Setting_Complete = true;
            }

            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);

            if (this.transform.position.z >= 1.2f)
            {
                m_bPortal_Setting_Complete = false;
                m_bReady_to_Teleport = true;
                m_bEntering = false;
            }
        }
    }
    public void out_the_Door()
    {
        if (m_bEntering == false)
        {
            if (m_bPortal_Setting_Complete == false)
            {
                m_bReady_to_Teleport = false;
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

                m_bPortal_Setting_Complete = true;
            }

            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);

            if (this.transform.position.z <= m_fz)
            {
                m_bPortal_Setting_Complete = false;
                m_bEnemyLock = false;
            }
        }
    }

    // FenceAction
    public void into_the_Fence()
    {
        if (m_bEntering == true)
        {
            if (m_bPortal_Setting_Complete == false)
            {
                m_bEnemyLock = true;

                if (m_iEnemyFloor == 1 && m_fDirection > 0f) // 1층 울타리에서 오른쪽을 보고 탈 경우
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                }
                else if (m_iEnemyFloor == 2 && m_fDirection < 0f)
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                else if (m_iEnemyFloor == 1 && m_fDirection < 0f) // 1층 울타리에서 오른쪽을 보고 탈 경우
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                }
                else if (m_iEnemyFloor == 2 && m_fDirection > 0f)
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                m_bPortal_Setting_Complete = true;
            }
            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);

            if (m_bReady_to_Teleport == true)
            {
                m_bEntering = false;
                m_bPortal_Setting_Complete = false;
            }
        }
    }
    public void out_the_Fence()
    {
        if (m_bEntering == false)
        {
            if (m_bPortal_Setting_Complete == false)
            {
                m_fOffset_x = this.transform.position.x;
                m_bReady_to_Teleport = false;
                m_bPortal_Setting_Complete = true;
            }
            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);
            
            if (m_fOffset_x >= max_x)
            {
                if (this.transform.position.x <= max_x)
                {
                    m_bEnemyLock = false;
                    m_bPortal_Setting_Complete = false;
                }
            }
            else if (m_fOffset_x <= min_x)
            {
                if (this.transform.position.x >= min_x)
                {
                    m_bEnemyLock = false;
                    m_bPortal_Setting_Complete = false;
                }
            }
        }
    }

    // TunnelAction
    public void into_the_Tunnel()
    {
        if (m_bEntering == true)
        {
            if (m_bPortal_Setting_Complete == false)
            {
                m_bEnemyLock = true;

                if (m_iEnemyFloor == 2 && m_fDirection > 0f) // 2층 터널에서 오른쪽을 보고 탈 경우
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                }
                else if (m_iEnemyFloor == 4 && m_fDirection < 0f)
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                else if (m_iEnemyFloor == 2 && m_fDirection < 0f) // 2층 터널에서 왼쪽을 보고 탈 경우
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                }
                else if (m_iEnemyFloor == 4 && m_fDirection > 0f)
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                m_bPortal_Setting_Complete = true;
            }

            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);

            if (m_bReady_to_Teleport == true)
            {
                m_bEntering = false;
                m_bPortal_Setting_Complete = false;
            }
        }
    }
    public void out_the_Tunnel()
    {
        if (m_bEntering == false)
        {
            if (m_bPortal_Setting_Complete == false)
            {
                m_fOffset_x = this.transform.position.x;
                m_bReady_to_Teleport = false;
                m_bPortal_Setting_Complete = true;
            }

            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);

            if (m_fOffset_x >= max_x)
            {
                if (this.transform.position.x <= max_x)
                {
                    m_bEnemyLock = false;
                    m_bPortal_Setting_Complete = false;
                }
            }
            else if (m_fOffset_x <= min_x)
            {
                if (this.transform.position.x >= min_x)
                {
                    m_bEnemyLock = false;
                    m_bPortal_Setting_Complete = false;
                }
            }
        }
    }
}

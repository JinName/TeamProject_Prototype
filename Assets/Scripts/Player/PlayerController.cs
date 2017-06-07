using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    int m_bPlayer_Health = 3;
    // 플레이어 충돌
    bool m_bCollision;
    //bool m_bLock; // 충돌시 플레이어 입력 못하게
    bool m_bSetting_Complete = false;

    // 함수포인터화
    delegate void DoorAction();
    DoorAction doorAction;

    bool m_bReady_to_Teleport = false;
    bool m_bPlayerLock = false;
    bool m_bLets_Action = false;
    bool m_bEntering = false;
    float m_fOffset_x;

    // 플레이어 현재 층
    int m_iPlayer_Floor = 1;
    int m_iDirection = 0;

    bool walking;
    bool usePortal;

    float coolTime;

    // 플레이어 초기 위치
    float m_fx = 7.5f;
    float m_fy = 0.1f;
    float m_fz = -0.8f;

    // x축 max, min
    float max_x = 8.2f;
    float min_x = 1.7f;

    
    // 캐릭터 좌표 벡터
    Vector3 m_vCharPosition;
    float m_fCollision_Direction; // 밀려날 방향
    float m_fCollision_Power = 38.0f; // 밀려나는 힘

    // 캐릭터 스피드 밖에서 참조 가능
    public float m_fSpeed = 6f;

    // 캐릭터의 rigidbody
    Rigidbody m_PlayerRigidbody;

    Animator m_Animator;

    // 특수타일 카운터 스위치
    bool m_bSpecial_Counter = false;

    public bool Get_Player_Lock() { return m_bPlayerLock; }
    public void Set_Player_Lock(bool _bPlayer_Lock) { m_bPlayerLock = _bPlayer_Lock; }

    public bool Get_Enter_Door() { return m_bEntering; }
    public void Set_Enter_Door(bool _bEnter) { m_bEntering = _bEnter; }

    public bool Get_Ready_to_Teleport() { return m_bReady_to_Teleport; }
    public void Set_Ready_to_Teleport(bool _bReady_to_Teleport) { m_bReady_to_Teleport = _bReady_to_Teleport; }

    public bool Get_Do_Action() { return m_bLets_Action; }
    public void Set_Do_Action(bool _bAction) { m_bLets_Action = _bAction; }

    public bool Get_Special_Counter() { return m_bSpecial_Counter; }
    public void Set_Special_Counter(bool _Special_Counter) { m_bSpecial_Counter = _Special_Counter; }

    void Awake()
    {
        m_bCollision = false;
        coolTime = 1.5f;
        walking = false;
        usePortal = false;
        m_Animator = GetComponentInChildren<Animator>();
        
        m_PlayerRigidbody = GetComponent<Rigidbody>();

        this.transform.position = new Vector3(m_fx, m_fy, m_fz);
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        if (m_bPlayerLock == false)
        {
            float m_h = Input.GetAxis("Horizontal");
            // 캐릭터 이동
            Move(m_h);
            // 애니매이션
            Animating(m_h);
        }


        // 충돌시 밀림
        Player_Collision_Movement();
    }

    private void Update()
    {
        PlayerFloor();

        // 포탈 쿨타임
        PortalCoolDown();
    }

    // 현재 플레이어가 있는 층
    public int Get_PlayerFloor() { return m_iPlayer_Floor; }

    // 플레이어가 포탈 이용한지 체크
    public bool Get_usePortal() { return usePortal; }
    public void Set_usePortal(bool _usePortal) { usePortal = _usePortal; }

    // 외부에서 캐릭터 위치를 이동시키기위해
    public void Set_PlayerPosition(Vector3 _vPosition)
    { this.transform.position = _vPosition; }

    // 플레이어 포탈 쿨타임 업데이트
    private void PortalCoolDown()
    {
        // if 포탈을 사용한 경우
        // if 3초 지났을 경우
        // usePortal = false;

        if (usePortal == true)
        {
            if (coolTime > 0f)
            {
                coolTime -= Time.deltaTime;
                if (coolTime <= 0f)
                {
                    usePortal = false;
                    coolTime = 1.5f;
                }
            }
        }
       
    }

    void PlayerFloor()
    {
        if (this.transform.position.y > -0.2f && this.transform.position.y < 2.3f)
            m_iPlayer_Floor = 1;
        else if (this.transform.position.y > 2.3f && this.transform.position.y < 4.9f)
            m_iPlayer_Floor = 2;
        else if (this.transform.position.y > 4.9f && this.transform.position.y < 7.5f)
            m_iPlayer_Floor = 3;
        else if (this.transform.position.y > 7.5f && this.transform.position.y < 10.2f)
            m_iPlayer_Floor = 4;
    }

    // 플레이어 좌우 이동
    void Move(float _h)
    {
        m_vCharPosition.Set(_h, 0f, 0f);
        // 두개의 키조합을 사용하면 벡터가 1.4가 됨으로 nomalized 해줌
        // Time.deltaTime 은 Update() 를 호출하는 간격
        m_vCharPosition = m_vCharPosition * m_fSpeed * Time.deltaTime;
        
        
        // 트랜스폼
        if(transform.position.x + m_vCharPosition.x > max_x)
            m_PlayerRigidbody.MovePosition(transform.position);
        else if (transform.position.x + m_vCharPosition.x < min_x)
            m_PlayerRigidbody.MovePosition(transform.position);
        else
            m_PlayerRigidbody.MovePosition(transform.position + m_vCharPosition);
    }

    void Animating(float _h)
    {
        // _h 가 0 이냐 아니냐에 따른 true, false 반환
        // 수평축을 눌렀나? 수직축을 눌렀나? 를 알기위함
        walking = _h != 0f;

        m_Animator.SetBool("IsWalking", walking);

        if (walking == true)
        {
            if (_h < 0f)
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                m_iDirection = -1;
            }
            else if (_h > 0f)
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                m_iDirection = 1;
            }
        }
        else
        {
        }
    }

    // DoorAction
    public void into_the_Door()
    {
        if (m_bEntering == true)
        {
            if (m_bSetting_Complete == false)
            {
                m_bPlayerLock = true;

                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 360f, 0f));
                m_bSetting_Complete = true;
            }

            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);
            
            if (this.transform.position.z >= 1.2f)
            {
                m_bSetting_Complete = false;
                m_bReady_to_Teleport = true;
                m_bEntering = false;
            }
        }
    }
    public void out_the_Door()
    {
        if (m_bEntering == false)
        {
            if (m_bSetting_Complete == false)
            {
                m_bReady_to_Teleport = false;
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

                m_bSetting_Complete = true;
            }

            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);
            
            if (this.transform.position.z <= m_fz)
            {
                m_bSetting_Complete = false;
                m_bPlayerLock = false;
            }
        }
    }

    // FenceAction
    public void into_the_Fence()
    {
        if (m_bEntering == true)
        {
            if (m_bSetting_Complete == false)
            {
                Debug.Log("Setting_Fence");
                m_bPlayerLock = true;

                if(m_iPlayer_Floor == 1 && m_iDirection == 1) // 1층 울타리에서 오른쪽을 보고 탈 경우
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                }
                else if (m_iPlayer_Floor == 2 && m_iDirection == -1)
                {
                    Debug.Log("Rotate");
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                m_bSetting_Complete = true;
            }
            Debug.Log("into_the_Fence");
            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);

            if (m_bReady_to_Teleport == true)
            {
                m_bEntering = false;
                m_bSetting_Complete = false;
            }
        }
    }
    public void out_the_Fence()
    {
        if (m_bEntering == false)
        {
            if (m_bSetting_Complete == false)
            {
                m_fOffset_x = this.transform.position.x;
                m_bReady_to_Teleport = false;
                m_bSetting_Complete = true;
                Debug.Log("이동 후 좌표 : " + m_fOffset_x.ToString());
            }
            Debug.Log("out_the_Fence");
            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);

            Debug.Log("이동 후에 좌표 : " + m_fOffset_x.ToString());
            if (m_fOffset_x >= max_x)
            {
                if(this.transform.position.x <= max_x)
                {
                    m_bPlayerLock = false;
                    m_bSetting_Complete = false;
                }
            }    
            else if(m_fOffset_x <= min_x)
            {
                if(this.transform.position.x >= min_x)
                {
                    m_bPlayerLock = false;
                    m_bSetting_Complete = false;
                }
            }
        }
    }

    // TunnelAction
    public void into_the_Tunnel()
    {
        if (m_bEntering == true)
        {
            if (m_bSetting_Complete == false)
            {
                m_bPlayerLock = true;

                if (m_iPlayer_Floor == 2 && m_iDirection == 1) // 2층 터널에서 오른쪽을 보고 탈 경우
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                }
                else if (m_iPlayer_Floor == 4 && m_iDirection == -1)
                {
                    this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
                }
                m_bSetting_Complete = true;
            }

            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);

            if (m_bReady_to_Teleport == true)
            {
                m_bEntering = false;
                m_bSetting_Complete = false;
            }
        }
    }
    public void out_the_Tunnel()
    {
        if (m_bEntering == false)
        {
            if (m_bSetting_Complete == false)
            {
                m_fOffset_x = this.transform.position.x;
                m_bReady_to_Teleport = false;
                m_bSetting_Complete = true;
            }

            this.transform.Translate(transform.forward * 5f * Time.deltaTime, Space.World);
            
            if (m_fOffset_x >= max_x)
            {
                if (this.transform.position.x <= max_x)
                {
                    m_bPlayerLock = false;
                    m_bSetting_Complete = false;
                }
            }
            else if (m_fOffset_x <= min_x)
            {
                if (this.transform.position.x >= min_x)
                {
                    m_bPlayerLock = false;
                    m_bSetting_Complete = false;
                }
            }
        }
    }

    // 충돌시 움직임
    void Player_Collision_Movement()
    {
        if(m_bCollision == true)
        {
            m_bPlayerLock = true; // 플레이어 잠시 키입력 불가

            m_PlayerRigidbody.AddForce(new Vector3(m_fCollision_Direction * m_fCollision_Power, 0f, 0f));

            m_fCollision_Power -= 3.0f;
            if(m_fCollision_Power < 0f)
            {
                m_fCollision_Power = 0;
                m_bCollision = false;
                m_bPlayerLock = false;
            }
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(collision.gameObject.transform.position.x > this.transform.position.x)
            {
                m_fCollision_Direction = -1;
            }
            else if (collision.gameObject.transform.position.x < this.transform.position.x)
            {
                m_fCollision_Direction = 1;
            }
            m_bPlayer_Health -= 1;
            m_bCollision = true;
        }
    }
    
}

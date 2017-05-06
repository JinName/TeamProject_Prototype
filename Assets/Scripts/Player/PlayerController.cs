using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    // 플레이어 현재 층
    int m_iPlayer_Floor = 1;

    bool walking;
    bool usePortal;

    float coolTime;

    // 플레이어 초기 위치
    float m_fx = 7.5f;
    float m_fy = 0.1f;
    float m_fz = -0.4f;

    // x축 max, min
    float max_x = 8.2f;
    float min_x = 1.7f;

    
    // 캐릭터 좌표 벡터
    Vector3 m_vCharPosition;

    // 캐릭터 스피드 밖에서 참조 가능
    public float m_fSpeed = 6f;

    // 캐릭터의 rigidbody
    Rigidbody m_PlayerRigidbody;

    Animator m_Animator;

    // 특수타일 카운터 스위치
    bool m_bSpecial_Counter = false;

    public bool Get_Special_Counter() { return m_bSpecial_Counter; }
    public void Set_Special_Counter(bool _Special_Counter) { m_bSpecial_Counter = _Special_Counter; }

    void Awake()
    {
        coolTime = 1.5f;
        walking = false;
        usePortal = false;
        m_Animator = GetComponentInChildren<Animator>();
        
        m_PlayerRigidbody = GetComponent<Rigidbody>();

        this.transform.position = new Vector3(m_fx, m_fy, m_fz);
    }

    // Update is called once per frame
    void FixedUpdate () {
        float m_h = Input.GetAxis("Horizontal");
        //float m_v = Input.GetAxis("Vertical");

        // 캐릭터 이동
        Move(m_h);

        // 애니매이션
        Animating(m_h);

        // 포탈 쿨타임
        PortalCoolDown();
        
	}

    private void Update()
    {
        PlayerFloor();
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
                //Debug.Log(coolTime);
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

        Debug.Log("Player의 현재 층 : " + m_iPlayer_Floor.ToString());
    }

    // 플레이어 액션키 Z
    void ActionKey()
    {

    }

    // 플레이어 좌우 이동
    void Move(float _h)
    {
        m_vCharPosition.Set(_h, 0f, 0f);
        // 두개의 키조합을 사용하면 벡터가 1.4가 됨으로 nomalized 해줌
        // Time.deltaTime 은 Update() 를 호출하는 간격
        m_vCharPosition = m_vCharPosition * m_fSpeed * Time.deltaTime;

        //Debug.Log(m_vCharPosition);
        
        
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

        //Debug.Log("Ani");
        // _h 가 0 이냐 아니냐에 따른 true, false 반환
        // 수평축을 눌렀나? 수직축을 눌렀나? 를 알기위함
        walking = _h != 0f;

        m_Animator.SetBool("IsWalking", walking);

        if (walking == true)
        {
            if (_h < 0f)
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
            }
            else if (_h > 0f)
            {
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 90f, 0f));
            }
        }
        else
        {
            //this.transform.rotation = new Quaternion(0f, 90f, 0f, 0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    bool walking;
    bool usePortal;

    float coolTime;

    // 캐릭터 좌표 벡터
    Vector3 m_vCharPosition;

    // 캐릭터 스피드 밖에서 참조 가능
    public float m_fSpeed = 6f;

    // 캐릭터의 rigidbody
    Rigidbody m_PlayerRigidbody;

    Animator m_Animator;


    void Awake()
    {
        coolTime = 1.5f;
        walking = false;
        usePortal = false;
        m_Animator = GetComponent<Animator>();
        m_PlayerRigidbody = GetComponent<Rigidbody>();
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
                //Debug.Log(_h);
                this.transform.rotation = Quaternion.Euler(new Vector3(0f, 270f, 0f));
                //Debug.Log("rotate");
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

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
    private float m_iState_AI;

    // 각 상태 지속시간 -> 3 초마다 상태가 바뀜
    private float m_fState_Runtime = 3.0f;

    // 현재 적이 있는 층
    private int m_iEnemyFloor;

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
    bool isWalking = false;

    // 초기화
    private void Awake()
    {
        // Animator
        m_Animator = GetComponent<Animator>();
        // Rigidbody
        m_Rigidbody = GetComponent<Rigidbody>();

        // 적 이동 속도
        m_fEnmeySpeed = 20.0f;

        // 상태값 랜덤 초기화 / 임시값 1
        //m_iState_AI = Random.Range(1, 4);
        m_iState_AI = 1;
        Debug.Log(m_iState_AI);

        // 시작층 = 4
        m_iEnemyFloor = 4;

        // 위치 값
        this.transform.position = new Vector3(m_fx, m_fy, m_fz);

        // 초기 이동방향 = 오른쪽
        m_fDirection = 0.1f;

        // animator
        isWalking = true;
    }


    private void Update()
    {
        AI_Move();
        Animating();
        Turning();
    }

    private void Animating()
    {
        m_Animator.SetBool("isWalking", isWalking);
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
        if(m_iState_AI == 1)
        {
            AI_1();
        }
        else if(m_iState_AI == 2)
        {
            // AI_2();
        }
    }

    private void Move()
    {
        m_vEnemyPosition.Set(m_fDirection, 0f, 0f);
        m_vEnemyPosition = m_vEnemyPosition * m_fEnmeySpeed * Time.deltaTime;
        m_Rigidbody.MovePosition(transform.position + m_vEnemyPosition);
    }
    // 1번 상태
    private void AI_1()
    {
        if ( this.transform.position.x > max_x )
        {
            m_fDirection = -0.1f;
        }
        else if ( this.transform.position.x < min_x )
        {
            m_fDirection = 0.1f;
        }

        Move();
    }
}

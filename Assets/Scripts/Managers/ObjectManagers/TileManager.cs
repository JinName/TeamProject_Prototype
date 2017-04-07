using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class TileManager : MonoBehaviour {

    // Tile 16개 리스트로 저장 - 16개 쿼드와 비교 함수
    // 타일 해당 타일 로테이션 함수
    GameObject m_Tile;
    GameObject m_Trigger;
    //GameObject m_Tile2;
    //GameObject m_Trigger2;

    GameObject TileClone;
    GameObject TriggerClone;

    // 타일 배치 간격
    float height = 2.6f;
    float width = 2.0f;

    // 기본 타일 위치

    // 타일 하나를 복사해 리스트로 저장 / 트리거 또한 마찬가지
    // 0~3 = 1층 / 4~7 = 2층 / 8~11 = 3층 / 12~15 = 4층
    List<GameObject> m_TileList;
    List<GameObject> m_TriggerList;


    // 타일 스코어 리턴
    int m_iSunScore;
    int m_iMoonScore;

    private void Awake()
    {
        Init();
        createList();
    }

    private void Update()
    {
        Player_In_Trigger();
    }

    public void Init()
    {
        m_TileList = new List<GameObject>();
        m_TriggerList = new List<GameObject>();
        // 이후 스테이지 넘버를 받아서 스테이지 별 타일 갯수로 초기화
        m_iSunScore = 16;
        m_iMoonScore = 0;

        m_Tile = GameObject.FindGameObjectWithTag("Tile");
        m_Trigger = GameObject.FindGameObjectWithTag("TriggerQuad");

        //m_Tile2 = GameObject.FindGameObjectWithTag("Tile2");
        //m_Trigger2 = GameObject.FindGameObjectWithTag("TriggerQuad2");

        m_Tile.transform.position = new Vector3(2.2f, 1.3f, 10.0f);
        m_Trigger.transform.position = new Vector3(2.0f, 1.0f, 10.0f);

        m_TileList.Add(m_Tile);
        m_TriggerList.Add(m_Trigger);
    }

    private void createList()
    {
        // 클론 생성
        // 16 이 스테이지별 타일 갯수를 받아오는 변수로 바뀌어야함
        int floor = 1;
        for(int i = 0; i < 16; ++i)
        {
            if (i > floor * Math.Sqrt(16))
            {
                TileClone = Instantiate(m_Tile, new Vector3(m_TileList[i - 4].transform.position.x, m_TileList[i - 4].transform.position.y + height, 10.0f), Quaternion.identity) as GameObject;
                TriggerClone = Instantiate(m_Trigger, new Vector3(m_TriggerList[i].transform.position.x, m_TriggerList[i - 4].transform.position.y + height, 10.0f), Quaternion.identity) as GameObject;
                floor++;
            }
            else
            {
                // 다음 타일 좌표
                TileClone = Instantiate(m_Tile, new Vector3(m_TileList[i].transform.position.x + width, m_TileList[i].transform.position.y, 10.0f), Quaternion.identity) as GameObject;
                TriggerClone = Instantiate(m_Trigger, new Vector3(m_TriggerList[i].transform.position.x + width, m_TriggerList[i].transform.position.y, 10.0f), Quaternion.identity) as GameObject;
            }
            

            m_TileList.Add(TileClone);
            m_TriggerList.Add(TriggerClone);
            Debug.Log(m_TileList);
        }
    }

    // 쿼드와 충돌 체크 -> i 번째 쿼드 = i 번째 타일
    // Testcase
    private void Player_In_Trigger()
    {
        if ( m_Trigger.GetComponent<TriggerQuad>().Get_TriggerSwitch() == true )
        {
            m_Tile.GetComponent<Tile>().Set_TileSwitch(true);
            m_Trigger.GetComponent<TriggerQuad>().Set_TriggerSwitch(false);
        }
        /*
        if (m_Trigger2.GetComponent<TriggerQuad>().Get_TriggerSwitch() == true)
        {
            m_Tile2.GetComponent<Tile>().Set_TileSwitch(true);
            m_Trigger2.GetComponent<TriggerQuad>().Set_TriggerSwitch(false);
        }
        */
    }
}

using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;

public class TileManager : MonoBehaviour {

    // Tile 16개 리스트로 저장 - 16개 쿼드와 비교 함수
    // 타일 해당 타일 로테이션 함수
    GameObject m_Tile;
    GameObject m_Trigger;

    GameObject TileClone;
    GameObject TriggerClone;

    public GameObject SpecialCounter_Floor1;
    public GameObject SpecialCounter_Floor2;
    public GameObject SpecialCounter_Floor3;
    public GameObject SpecialCounter_Floor4;

    // 타일 배치 간격
    float height = 2.55f;
    float width = 1.9f;

    // 기본 타일 위치

    // 타일 하나를 복사해 리스트로 저장 / 트리거 또한 마찬가지
    // 0~3 = 1층 / 4~7 = 2층 / 8~11 = 3층 / 12~15 = 4층
    List<GameObject> m_TileList;
    List<GameObject> m_TriggerList;

    // 층 별 특수타일 번호(랜덤)
    int[] array_SpecialNum = new int[4] { 2, 4, 9, 15 };

    // 타일 스코어 리턴
    int m_iSunScore;
    int m_iMoonScore;

    public int Get_MoonScore() { return m_iMoonScore; }

    private void Awake()
    {
        Init();
        createList();
    }

    private void Update()
    {
        inTrigger();
        TileScore();
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

        m_Tile.transform.position = new Vector3(2.2f, 1.3f, 70.0f);
        m_Trigger.transform.position = new Vector3(2.0f, 1.0f, -0.65f);

        // 스페셜 타일 번호 추첨
        /*
        for (int i = 0; i < 4; ++i)
        {
            array_SpecialNum[i] = Random.Range((i*4), (i*4) + 3);
        }
        */

        m_Tile.GetComponent<Tile>().Set_Floor(1);

        m_TileList.Add(m_Tile);
        m_TriggerList.Add(m_Trigger);
    }

    private void createList()
    {
        // 클론 생성
        // 16 이 스테이지별 타일 갯수를 받아오는 변수로 바뀌어야함
        int floor = 1;
        for(int i = 1; i < 16; ++i)
        {
            if (i >= floor * Mathf.Sqrt(16))
            {
                floor++;
                m_Tile.GetComponent<Tile>().Set_Floor(floor);

                TileClone = Instantiate(m_Tile, new Vector3(m_TileList[i - (int)Mathf.Sqrt(16)].transform.position.x, m_TileList[i - (int)Mathf.Sqrt(16)].transform.position.y + height, 70.0f), Quaternion.identity) as GameObject;
                TriggerClone = Instantiate(m_Trigger, new Vector3(m_TriggerList[i - (int)Mathf.Sqrt(16)].transform.position.x, m_TriggerList[i - (int)Mathf.Sqrt(16)].transform.position.y + height, -0.65f), Quaternion.Euler(new Vector3(0f, 90f, 0f))) as GameObject;
            }
            else
            {
                // 다음 타일 좌표
                TileClone = Instantiate(m_Tile, new Vector3(m_TileList[i-1].transform.position.x + width, m_TileList[i-1].transform.position.y, 70.0f), Quaternion.identity) as GameObject;
                TriggerClone = Instantiate(m_Trigger, new Vector3(m_TriggerList[i-1].transform.position.x + width, m_TriggerList[i-1].transform.position.y, -0.65f), Quaternion.Euler(new Vector3(0f, 90f, 0f))) as GameObject;
            }

            if (i == array_SpecialNum[floor - 1])
            {
                TileClone.GetComponent<Tile>().Set_SpecialTile(true);
                TriggerClone.GetComponent<TriggerQuad>().Set_isSpecial(true);
            }
                            
            m_TileList.Add(TileClone);
            m_TriggerList.Add(TriggerClone);
        }
    }

    // 쿼드와 충돌 체크 -> i 번째 쿼드 = i 번째 타일
    // Testcase
    private void inTrigger()
    {
        for (int i = 0; i < 16; ++i)
        {
            if (m_TriggerList[i].GetComponent<TriggerQuad>().Get_isSpecial() == true)
            {
                if (m_TriggerList[i].GetComponent<TriggerQuad>().Get_isStay() == true)
                {

                    if (i < 4)
                        SpecialCounter_Floor1.SetActive(true);
                    else if (i >= 4 && i < 8)
                        SpecialCounter_Floor2.SetActive(true);
                    else if (i >= 8 && i < 12)
                        SpecialCounter_Floor3.SetActive(true);
                    else if (i >= 12 && i < 16)
                        SpecialCounter_Floor4.SetActive(true);
                }
                else
                {
                    if (i < 4)
                        SpecialCounter_Floor1.SetActive(false);
                    else if (i >= 4 && i < 8)
                        SpecialCounter_Floor2.SetActive(false);
                    else if (i >= 8 && i < 12)
                        SpecialCounter_Floor3.SetActive(false);
                    else if (i >= 12 && i < 16)
                        SpecialCounter_Floor4.SetActive(false);
                }

                if (m_TriggerList[i].GetComponent<TriggerQuad>().Get_isConquered() == true)
                {
                    if (i < 4)
                        SpecialCounter_Floor1.SetActive(false);
                    else if (i >= 4 && i < 8)
                        SpecialCounter_Floor2.SetActive(false);
                    else if (i >= 8 && i < 12)
                        SpecialCounter_Floor3.SetActive(false);
                    else if (i >= 12 && i < 16)
                        SpecialCounter_Floor4.SetActive(false);

                    // 특수타일이 점령된 층의 다른 타일들이 다시 돌아가지 않게 셋팅하는 for 문
                    for (int j = (int)Mathf.Sqrt(16) * (m_TileList[i].GetComponent<Tile>().Get_Floor() - 1);
                        j < (Mathf.Sqrt(16) * (m_TileList[i].GetComponent<Tile>().Get_Floor() - 1)) + Mathf.Sqrt(16); ++j)
                    {
                        m_TriggerList[j].GetComponent<TriggerQuad>().Set_isConquered(true);
                    }
                }                
            }

            if (m_TriggerList[i].GetComponent<TriggerQuad>().Get_TriggerSwitch() == true)
            {
                m_TileList[i].GetComponent<Tile>().Set_TileSwitch(true); 
                m_TriggerList[i].GetComponent<TriggerQuad>().Set_TriggerSwitch(false);

                if (m_TileList[i].GetComponent<Tile>().Get_Player_In() == false && m_TriggerList[i].GetComponent<TriggerQuad>().Get_WhosTile() == 1)
                    m_TileList[i].GetComponent<Tile>().Set_Player_In(true);
                else if (m_TileList[i].GetComponent<Tile>().Get_Player_In() == true && m_TriggerList[i].GetComponent<TriggerQuad>().Get_WhosTile() == 2)
                    m_TileList[i].GetComponent<Tile>().Set_Player_In(false);
            }
        }
    }

    void TileScore()
    {
        int tempScore = 0;
        for (int i = 0; i < 16; i ++)
        {            
            if(m_TriggerList[i].GetComponent<TriggerQuad>().Get_WhosTile() == 2)
            {
                tempScore += 1;
            }

            if(i == 15)
            {
                m_iMoonScore = tempScore;
                m_iSunScore = 16 - m_iMoonScore;
            }
        }
    }
}

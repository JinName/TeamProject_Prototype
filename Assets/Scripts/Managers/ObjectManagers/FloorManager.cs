using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// StageNum 받아서 층 갯수 조절
public class FloorManager : MonoBehaviour {

    // Floor Object
    GameObject m_Floor;
    GameObject FloorClone;

    // Floor List ( 스테이지에 따른 층 갯수 조절 )
    List<GameObject> m_FloorList;

    // 층간 거리
    float height = 2.55f;

    // 한 층 x 축 스케일
    float m_fWidthScale = 1f;

    private void Awake()
    {
        m_Floor = GameObject.FindGameObjectWithTag("Floor");

        m_FloorList = new List<GameObject>();

        // 층 크기, 위치 초기화
        m_Floor.transform.position = new Vector3(5f, 0f, -0.5f);

        // 리스트 삽입
        m_FloorList.Add(m_Floor);

        createFloor();
    }
    // Use this for initialization
    void createFloor () {
        // 16 은 나중에 Stage에 따른 층 타일갯수를 받아오는것으로 변경
		for(int i = 1; i < Math.Sqrt(16); ++i)
        {
            FloorClone = Instantiate(m_Floor, new Vector3(m_FloorList[i - 1].transform.position.x, m_FloorList[i-1].transform.position.y + height, m_FloorList[i - 1].transform.position.z), Quaternion.identity);

            m_FloorList.Add(FloorClone);
        }
	}
}

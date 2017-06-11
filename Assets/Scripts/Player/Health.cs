using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    GameObject m_Player;

    bool m_bSetting = false;
    bool m_bHP_Rotate_Complete = false; // 한번만 돌게
    int m_iHP = 3; // 1 부터 증가

    float m_fRun_Time = 3.0f;

    float rot = 0f;
    float m_fSpeed = 200f;

    public int Get_HP() { return m_iHP; }

    private void Awake()
    {
        m_Player = GameObject.Find("Player");
    }

    private void Update()
    {
        // 충돌하면
        // SetActive(true)
        if(m_Player.GetComponent<PlayerController>().Get_Player_Collision() == true)
        {
            if (m_bSetting == false)
            {               
                this.transform.Find("HP_TileObj").gameObject.SetActive(true);
                m_bSetting = true;
            }
        }

        HP_Rotate();

        if(m_bSetting == true)
        {
            m_fRun_Time -= Time.deltaTime;            

            if(m_fRun_Time <= 0f)
            {
                m_bSetting = false;
                m_bHP_Rotate_Complete = false;
                m_fRun_Time = 3.0f;
                this.transform.Find("HP_TileObj").gameObject.SetActive(false);
            }
        }

        Follow_Player();
    }

    void Follow_Player()
    {
        this.transform.SetPositionAndRotation(new Vector3(m_Player.transform.position.x, m_Player.transform.position.y + 2.0f, -9.0f), Quaternion.identity);
    }

    void HP_Rotate()
    {
        if (m_bHP_Rotate_Complete == false)
        {
            rot += m_fSpeed * Time.deltaTime;

            if (m_iHP == 3)
            {
                this.transform.Find("HP_TileObj").Find("HP_Tile_1").gameObject.transform.eulerAngles = new Vector3(0f, rot, 0);

                if (rot >= 180)
                {
                    this.transform.Find("HP_TileObj").Find("HP_Tile_1").gameObject.transform.eulerAngles = new Vector3(0f, 180, 0);
                    rot = 0f;
                    m_iHP--;
                    m_bHP_Rotate_Complete = true;
                }
            }
            else if (m_iHP == 2)
            {
                this.transform.Find("HP_TileObj").Find("HP_Tile_2").gameObject.transform.eulerAngles = new Vector3(0f, rot, 0);

                if (rot >= 180)
                {
                    this.transform.Find("HP_TileObj").Find("HP_Tile_2").gameObject.transform.eulerAngles = new Vector3(0f, 180, 0);
                    rot = 0f;
                    m_iHP--;
                    m_bHP_Rotate_Complete = true;
                }
            }
            else if (m_iHP == 1)
            {
                this.transform.Find("HP_TileObj").Find("HP_Tile_3").gameObject.transform.eulerAngles = new Vector3(0f, rot, 0);

                if (rot >= 180)
                {
                    this.transform.Find("HP_TileObj").Find("HP_Tile_3").gameObject.transform.eulerAngles = new Vector3(0f, 180, 0);
                    rot = 0f;
                    m_iHP--;
                    m_bHP_Rotate_Complete = true;
                }
            }
        }
    }
}

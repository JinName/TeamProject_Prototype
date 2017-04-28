using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    // 타일 소유자
    // false : Enemy / true : Player
    bool m_bPlayer_In = false;

    // 스페셜 타일 만들때
    bool m_bSpecial = false;
    // 스페셜 타일이 점령되었을때
    bool m_bConquer = false;

    // 회전 스피드
    float m_Speed = 200f;

    // 회전 스위치
    bool m_bRotateSwitch;

    float rot = 0f;

    private void Awake()
    {
        m_bRotateSwitch = false;
    }

    public bool Get_isConquered() { return m_bConquer; }
    public void Set_isConquered(bool _bConquer) { m_bConquer = _bConquer; }

    public bool Get_SpecialTile() { return m_bSpecial; }
    public void Set_SpecialTile( bool _bSpecial ) { m_bSpecial = _bSpecial; }

    public bool Get_TileSwitch() { return m_bRotateSwitch; }
    public void Set_TileSwitch( bool _bRotateSwitch ) { m_bRotateSwitch = _bRotateSwitch; }

    public bool Get_Player_In() { return m_bPlayer_In; }
    public void Set_Player_In(bool _bPlayerConquer) { m_bPlayer_In = _bPlayerConquer; }

    private void Update()
    {
        if (m_bConquer == false)
        {
            if (m_bRotateSwitch == true)
            {
                TileRotate();
            }
        }
    }

    public void TileRotate()
    {
        rot += m_Speed * Time.deltaTime;
        
        if (m_bPlayer_In == true)
        {
            this.transform.eulerAngles = new Vector3(0, 180 - rot, 0);
            if (rot >= 180)
            {
                this.transform.eulerAngles = new Vector3(0, 0, 0);
                m_bRotateSwitch = false;
                rot = 0f;
            }
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0, rot, 0);

            if (rot >= 180)
            {
                this.transform.eulerAngles = new Vector3(0, 180, 0);
                m_bRotateSwitch = false;
                rot = 0f;
            }
        }
    }
}

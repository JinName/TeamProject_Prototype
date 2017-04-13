using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    // 타일 소유자
    // false : Enemy / true : Player
    bool m_bPlayerConquer = false;

    // 회전 스피드

    // 회전 스위치
    bool m_bRotateSwitch;

    float rot = 0f;
    float TempSum = 0f;

    private void Awake()
    {
        m_bRotateSwitch = false;
    }

    public bool Get_TileSwitch() { return m_bRotateSwitch; }
    public void Set_TileSwitch( bool _bRotateSwitch ) { m_bRotateSwitch = _bRotateSwitch; }

    public bool Get_PlayerConquer() { return m_bPlayerConquer; }
    public void Set_PlayerConquer(bool _bPlayerConquer) { m_bPlayerConquer = _bPlayerConquer; }

    private void Update()
    {
        if ( m_bRotateSwitch == true )
        {
            TileRotate();
        }
    }

    public void TileRotate()
    {
        rot += 200 * Time.deltaTime;
        
        if (m_bPlayerConquer == true)
        {
            Debug.Log("rotate in");
            this.transform.eulerAngles = new Vector3(0, -rot, 0);
            Debug.Log(rot);
            if (rot >= 180)
            {
                Debug.Log("stop");
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

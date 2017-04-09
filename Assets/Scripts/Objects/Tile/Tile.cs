﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    // 회전 스피드

    // 회전 스위치
    bool m_bRotateSwitch;

    float rot = 0f;

    private void Awake()
    {
        m_bRotateSwitch = false;
    }

    public bool Get_TileSwitch() { return m_bRotateSwitch; }
    public void Set_TileSwitch( bool _bRotateSwitch ) { m_bRotateSwitch = _bRotateSwitch; }

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

        this.transform.eulerAngles = new Vector3(0, rot, 0);

        if(this.transform.eulerAngles.y >= 180)
        {
            this.transform.eulerAngles = new Vector3(0, 180, 0);
            m_bRotateSwitch = false;
        }
    }
}

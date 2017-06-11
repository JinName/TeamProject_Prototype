using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCounter : MonoBehaviour {

    float m_fTotal_Time = 3.0f;
    bool m_bRotate_Switch_1 = false;
    bool m_bRotate_Switch_2 = false;
    bool m_bRotate_Switch_3 = false;

    bool m_bSetting_1 = false;
    bool m_bSetting_2 = false;
    bool m_bSetting_3 = false;

    float m_fCurrent_Time = 0f;

    float m_fSpeed = 200f;
    float rot = 0f;

    //bool Get_Switch() { return m_bRotate_Switch; }
    //void Set_Switch(bool _bRotate_Switch) { m_bRotate_Switch = _bRotate_Switch; }

    private void Update()
    {
        m_fCurrent_Time += Time.deltaTime;

        if(m_fCurrent_Time >= 0.2f && m_fCurrent_Time < 1.2f)
        {
            if (m_bSetting_1 == false)
            {
                m_bRotate_Switch_1 = true;
                m_bSetting_1 = true;
            }
        }

        if (m_fCurrent_Time >= 1.2f && m_fCurrent_Time < 2.2f)
        {
            if (m_bSetting_2 == false)
            {
                m_bRotate_Switch_2 = true;
                m_bSetting_2 = true;
            }
        }

        if (m_fCurrent_Time >= 2.2f)
        {
            if (m_bSetting_3 == false)
            {
                m_bRotate_Switch_3 = true;
                m_bSetting_3 = true;
            }
        }

        Rotate();
    }

    private void OnDisable()
    {
        Reset_All();
    }

    void Reset_All()
    {
        m_bRotate_Switch_1 = false;
        m_bRotate_Switch_2 = false;
        m_bRotate_Switch_3 = false;

        m_bSetting_1 = false;
        m_bSetting_2 = false;
        m_bSetting_3 = false;

        this.transform.Find("SpecialCounter_1").gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        this.transform.Find("SpecialCounter_2").gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
        this.transform.Find("SpecialCounter_3").gameObject.transform.eulerAngles = new Vector3(0, 0, 0);

        rot = 0f;
        m_fCurrent_Time = 0f;
    }

    void Rotate()
    {
        if(m_bRotate_Switch_1)
        {
            rot += m_fSpeed * Time.deltaTime;

            this.transform.Find("SpecialCounter_1").gameObject.transform.eulerAngles = new Vector3(0, rot, 0);

            if(rot >= 180f)
            {
                m_bRotate_Switch_1 = false;
                rot = 0f;
            }
        }

        if (m_bRotate_Switch_2)
        {
            rot += m_fSpeed * Time.deltaTime;

            this.transform.Find("SpecialCounter_2").gameObject.transform.eulerAngles = new Vector3(0, rot, 0);

            if (rot >= 180f)
            {
                m_bRotate_Switch_2 = false;
                rot = 0f;
            }
        }

        if (m_bRotate_Switch_3)
        {
            rot += m_fSpeed * Time.deltaTime;

            this.transform.Find("SpecialCounter_3").gameObject.transform.eulerAngles = new Vector3(0, rot, 0);

            if (rot >= 180f)
            {
                m_bRotate_Switch_3 = false;
                rot = 0f;
            }
        }
    }
}

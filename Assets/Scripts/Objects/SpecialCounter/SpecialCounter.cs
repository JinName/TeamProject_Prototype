using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialCounter : MonoBehaviour {

    float m_fTotal_Time = 3.0f;
    bool m_bRotate_Switch = false;

    bool Get_Switch() { return m_bRotate_Switch; }
    void Set_Switch(bool _bRotate_Switch) { m_bRotate_Switch = _bRotate_Switch; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal_Door : MonoBehaviour {

    // 애니메이션용 변수
    Animator m_Animator;
    bool m_bAni_onPortal;

    public void Set_Animation(bool _bAni_onPortal) { m_bAni_onPortal = _bAni_onPortal; }

    private void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        m_bAni_onPortal = false;
    }

    private void Update()
    {
        Animating();
    }

    void Animating()
    {
        m_Animator.SetBool("isOpen", m_bAni_onPortal);
    }
}

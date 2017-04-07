using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedPortal_other : MonoBehaviour
{

    // 캐릭터가 포탈에 겹쳐있는걸 체크
    //bool onPortal;

    // 캐릭터 오브젝트 가져오기
    private GameObject player;
    private GameObject portal_1;
    private GameObject portal_2;

    PlayerController playerCtrl;


    // Use this for initialization
    void Start()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        portal_1 = GameObject.FindGameObjectWithTag("Red1");
        portal_2 = GameObject.FindGameObjectWithTag("Red2");
    }

    private void OnTriggerStay(Collider other)
    {
        // GetComponent로 플레이어가 포탈 사용했는지 체크
        if (other.name.Contains("Player") && playerCtrl.Get_usePortal() == false)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("InputCheck_Red1");
                other.transform.position = portal_1.transform.position;
                playerCtrl.Set_usePortal(true);
            }
        }
    }
}

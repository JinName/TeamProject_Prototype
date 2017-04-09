using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowPortal : MonoBehaviour {

    // Use this for initialization
    private GameObject player;
    // 포탈 오브젝트 가져오기
    private GameObject portal_1;
    private GameObject portal_2;

    PlayerController playerCtrl;


    // Use this for initialization
    void Awake()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        portal_1 = GameObject.FindGameObjectWithTag("Yellow1");
        portal_2 = GameObject.FindGameObjectWithTag("Yellow2");
    }

    private void OnTriggerStay(Collider other)
    {
        // GetComponent로 플레이어가 포탈 사용했는지 체크
        if (other.name.Contains("Player") && playerCtrl.Get_usePortal() == false)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                other.transform.position = portal_2.transform.position;
                playerCtrl.Set_usePortal(true);
            }
        }
    }
}

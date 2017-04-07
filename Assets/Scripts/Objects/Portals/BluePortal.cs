using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BluePortal : MonoBehaviour {

    // 캐릭터가 포탈에 겹쳐있는걸 체크
    //bool onPortal;

    // 캐릭터 오브젝트 가져오기
    private GameObject player;
    // 포탈 오브젝트 가져오기
    private GameObject bluePortal_1;
    private GameObject bluePortal_2;

    PlayerController playerCtrl;


    // Use this for initialization
    void Awake()
    {
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        bluePortal_1 = GameObject.FindGameObjectWithTag("Blue1");
        bluePortal_2 = GameObject.FindGameObjectWithTag("Blue2");
    }

    private void OnTriggerStay(Collider other)
    {
        // GetComponent로 플레이어가 포탈 사용했는지 체크
        if (other.name.Contains("Player") && playerCtrl.Get_usePortal() == false)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Debug.Log("InputCheck_Blue1");
                other.transform.position = bluePortal_2.transform.position;
                playerCtrl.Set_usePortal(true);
            }
        }
    }
}

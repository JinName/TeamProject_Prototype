using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager : MonoBehaviour {

    bool m_bPlayer_Setting_Complete = false;
    bool m_bEnemy_Setting_Complete = false;
    // 플레이어
    PlayerController playerCtrl;
    // Enemy
    EnemyAI m_Enemy;
    
    // 포탈
    GameObject m_Door;      // 문
    GameObject m_Fence;     // 울타리
    GameObject m_Tunnel;    // 터널
    GameObject m_Blink;     // 순간이동

    GameObject m_PortalClone;

    GameObject m_Fence_Teleport_Trigger;
    List<GameObject> m_Fence_Teleport_Trigger_List;

    GameObject m_Tunnel_Teleport_Trigger;
    List<GameObject> m_Tunnel_Teleport_Trigger_List;

    // 포탈 리스트
    // 0,1 : Door
    // 2,3 : Fence
    // 4,5 : Tunnel
    // 6,7 : Blink
    List<GameObject> m_PortalList;
    List<GameObject> m_FenceList;
    List<GameObject> m_TunnelList;
    List<GameObject> m_BlinkList;
    // 포탈 갯수
    int m_iPortal_Count = 2;

    private void Awake()
    {
        // 플레이어
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        m_Enemy = GameObject.Find("Enemy").GetComponent<EnemyAI>();

        m_PortalList = new List<GameObject>(); // 문
        m_FenceList = new List<GameObject>(); // 울타리
        m_TunnelList = new List<GameObject>(); // 터널
        m_BlinkList = new List<GameObject>(); // 순간이동

        m_Fence_Teleport_Trigger_List = new List<GameObject>();
        m_Tunnel_Teleport_Trigger_List = new List<GameObject>();

        // 문, 울타리
        m_Door = GameObject.Find("Portal_Door");
        m_Fence = GameObject.Find("Portal_Fence");
        m_Tunnel = GameObject.Find("Portal_Tunnel");
        m_Blink = GameObject.Find("Portal_Blink");

        m_Fence_Teleport_Trigger = GameObject.Find("Fence_Teleport_Trigger");
        m_Fence_Teleport_Trigger_List.Add(m_Fence_Teleport_Trigger);
        m_PortalClone = Instantiate(m_Fence_Teleport_Trigger, new Vector3(10.0f, 3.5f, -0.45f), Quaternion.identity);
        m_Fence_Teleport_Trigger_List.Add(m_PortalClone);

        m_Tunnel_Teleport_Trigger = GameObject.Find("Tunnel_Teleport_Trigger");
        m_Tunnel_Teleport_Trigger_List.Add(m_Tunnel_Teleport_Trigger);
        m_PortalClone = Instantiate(m_Tunnel_Teleport_Trigger, new Vector3(10.0f, 8.7f, -0.45f), Quaternion.identity);
        m_Tunnel_Teleport_Trigger_List.Add(m_PortalClone);

        // 포탈 복사 생성
        createPortals();
    }

    private void Update()
    {
        Door_Check();
        Fence_Check();
        Tunnel_Check();
        Blink_Check();
    }
    
    // Door
    void Door_Check()
    {
        #region Player
        if (playerCtrl.Get_Player_use_Portal() == true)
        {
            if (playerCtrl.Get_Enter() == false)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (m_PortalList[i].GetComponentInChildren<Door_Trigger>().Get_Trigger_is_On() == true)
                    {
                        m_PortalList[i].GetComponentInChildren<Door>().Set_Animation(true);
                    }
                    else
                    {
                        m_PortalList[i].GetComponentInChildren<Door>().Set_Animation(false);
                    }

                    if (m_PortalList[i].GetComponentInChildren<Door_Trigger>().Get_Switch() == true)
                    {
                        if (m_bPlayer_Setting_Complete == false)
                        {
                            playerCtrl.Set_Enter(true);
                            m_bPlayer_Setting_Complete = true;
                        }
                    }
                }
            }
            if (m_PortalList[0].GetComponentInChildren<Door_Trigger>().Get_Switch() == true ||
                m_PortalList[1].GetComponentInChildren<Door_Trigger>().Get_Switch() == true)
            {
                playerCtrl.into_the_Door();
                if (playerCtrl.Get_Ready_to_Teleport() == true)
                {
                    if (m_PortalList[0].GetComponentInChildren<Door_Trigger>().Get_Switch() == true)
                        playerCtrl.Set_PlayerPosition(m_PortalList[1].transform.position + new Vector3(0f, -0.6f, 1.5f));
                    else if (m_PortalList[1].GetComponentInChildren<Door_Trigger>().Get_Switch() == true)
                        playerCtrl.Set_PlayerPosition(m_PortalList[0].transform.position + new Vector3(0f, -0.6f, 1.5f));
                }

                playerCtrl.out_the_Door();

                if (playerCtrl.Get_Player_Lock() == false)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        m_PortalList[i].GetComponentInChildren<Door_Trigger>().Set_Switch(false);
                        
                        if (i == 1)
                        {
                            m_bPlayer_Setting_Complete = false;
                            playerCtrl.Set_Player_use_Portal(false);
                        }
                    }
                }
            }
        }
        #endregion

        #region Enemy
        if(m_Enemy.Get_Enemy_use_Portal() == true)
        {
            if (m_Enemy.Get_Enter() == false)
            {
                for (int i = 0; i < 2; i++)
                {
                    if (m_PortalList[i].GetComponentInChildren<Door_Trigger>().Get_Trigger_is_On() == true)
                    {
                        m_PortalList[i].GetComponentInChildren<Door>().Set_Animation(true);
                    }
                    else
                    {
                        m_PortalList[i].GetComponentInChildren<Door>().Set_Animation(false);
                    }

                    if (m_PortalList[i].GetComponentInChildren<Door_Trigger>().Get_Switch() == true)
                    {
                        if (m_bEnemy_Setting_Complete == false)
                        {
                            m_Enemy.Set_Enter(true);
                            m_bEnemy_Setting_Complete = true;
                        }
                    }
                }
            }
            if (m_PortalList[0].GetComponentInChildren<Door_Trigger>().Get_Switch() == true ||
                m_PortalList[1].GetComponentInChildren<Door_Trigger>().Get_Switch() == true)
            {
                m_Enemy.into_the_Door();
                if (m_Enemy.Get_Ready_to_Teleport() == true)
                {
                    if (m_PortalList[0].GetComponentInChildren<Door_Trigger>().Get_Switch() == true)
                        m_Enemy.Set_EnemyPosition(m_PortalList[1].transform.position + new Vector3(0f, -0.6f, 1.5f));
                    else if (m_PortalList[1].GetComponentInChildren<Door_Trigger>().Get_Switch() == true)
                        m_Enemy.Set_EnemyPosition(m_PortalList[0].transform.position + new Vector3(0f, -0.6f, 1.5f));
                }

                m_Enemy.out_the_Door();

                if (m_Enemy.Get_Enemy_Lock() == false)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        m_PortalList[i].GetComponentInChildren<Door_Trigger>().Set_Switch(false);
                        m_PortalList[i].GetComponentInChildren<Door_Trigger>().Set_Enemy_Used(true);

                        if (i == 1)
                        {
                            m_bEnemy_Setting_Complete = false;
                            m_Enemy.Set_Enemy_use_Portal(false);
                            m_Enemy.Set_usePortal(true);
                        }
                    }
                }
            }
        }
        #endregion
    }

    // Fence
    void Fence_Check()
    {
        #region Player
        if (playerCtrl.Get_Player_use_Portal() == true)
        {
            if (playerCtrl.Get_Enter() == false)
            {
                for (int i = 2; i < 4; i++)
                {
                    // 스위치 켜지면
                    if (m_PortalList[i].GetComponentInChildren<Fence_Trigger>().Get_Switch() == true)
                    {
                        if (m_bPlayer_Setting_Complete == false)
                        {
                            playerCtrl.Set_Enter(true);
                            m_bPlayer_Setting_Complete = true;
                        }
                    }
                }
            }

            if (m_PortalList[2].GetComponentInChildren<Fence_Trigger>().Get_Switch() == true ||
                   m_PortalList[3].GetComponentInChildren<Fence_Trigger>().Get_Switch() == true)
            {
                playerCtrl.into_the_Fence();
                if (playerCtrl.Get_Ready_to_Teleport() == true)
                {
                    if (m_Fence_Teleport_Trigger_List[0].GetComponent<Fence_Teleport_Trigger>().Get_Teleport_Switch() == true)
                    {
                        m_Fence_Teleport_Trigger_List[1].GetComponent<Fence_Teleport_Trigger>().Set_Setting(true);
                        playerCtrl.Set_PlayerPosition(m_Fence_Teleport_Trigger_List[1].transform.position);
                    }
                    else if (m_Fence_Teleport_Trigger_List[1].GetComponent<Fence_Teleport_Trigger>().Get_Teleport_Switch() == true)
                    {
                        m_Fence_Teleport_Trigger_List[0].GetComponent<Fence_Teleport_Trigger>().Set_Setting(true);
                        playerCtrl.Set_PlayerPosition(m_Fence_Teleport_Trigger_List[0].transform.position);
                    }
                }
                playerCtrl.out_the_Fence();

                if (playerCtrl.Get_Player_Lock() == false)
                {
                    for (int i = 2; i < 4; i++)
                    {
                        m_PortalList[i].GetComponentInChildren<Fence_Trigger>().Set_Switch(false);

                        if (i == 3)
                        {
                            m_bPlayer_Setting_Complete = false;
                            playerCtrl.Set_Player_use_Portal(false);
                        }
                    }
                }
            }
        }
        #endregion

        #region Enemy
        if (m_Enemy.Get_Enemy_use_Portal() == true)
        {
            if (m_Enemy.Get_Enter() == false)
            {
                for (int i = 2; i < 4; i++)
                {
                    // 스위치 켜지면
                    if (m_PortalList[i].GetComponentInChildren<Fence_Trigger>().Get_Switch() == true)
                    {
                        if (m_bEnemy_Setting_Complete == false)
                        {
                            m_Enemy.Set_Enter(true);
                            m_bEnemy_Setting_Complete = true;
                        }
                    }
                }
            }

            if (m_PortalList[2].GetComponentInChildren<Fence_Trigger>().Get_Switch() == true ||
                   m_PortalList[3].GetComponentInChildren<Fence_Trigger>().Get_Switch() == true)
            {
                m_Enemy.into_the_Fence();
                if (m_Enemy.Get_Ready_to_Teleport() == true)
                {
                    if (m_Fence_Teleport_Trigger_List[0].GetComponent<Fence_Teleport_Trigger>().Get_Teleport_Switch() == true)
                    {
                        m_Fence_Teleport_Trigger_List[1].GetComponent<Fence_Teleport_Trigger>().Set_Setting(true);
                        m_Enemy.Set_EnemyPosition(m_Fence_Teleport_Trigger_List[1].transform.position);
                    }
                    else if (m_Fence_Teleport_Trigger_List[1].GetComponent<Fence_Teleport_Trigger>().Get_Teleport_Switch() == true)
                    {
                        m_Fence_Teleport_Trigger_List[0].GetComponent<Fence_Teleport_Trigger>().Set_Setting(true);
                        m_Enemy.Set_EnemyPosition(m_Fence_Teleport_Trigger_List[0].transform.position);
                    }
                }
                m_Enemy.out_the_Fence();

                if (m_Enemy.Get_Enemy_Lock() == false)
                {
                    for (int i = 2; i < 4; i++)
                    {
                        m_PortalList[i].GetComponentInChildren<Fence_Trigger>().Set_Switch(false);
                        m_PortalList[i].GetComponentInChildren<Fence_Trigger>().Set_Enemy_Used(true);

                        if (i == 3)
                        {
                            m_bEnemy_Setting_Complete = false;
                            m_Enemy.Set_Enemy_use_Portal(false);
                            m_Enemy.Set_usePortal(true);
                        }
                    }
                }
            }
        }
        #endregion
    }

    // Tunnel
    void Tunnel_Check()
    {
        #region Player
        if (playerCtrl.Get_Player_use_Portal() == true)
        {
            if (playerCtrl.Get_Enter() == false)
            {
                for (int i = 4; i < 6; i++)
                {
                    // 스위치 켜지면
                    if (m_PortalList[i].GetComponentInChildren<Tunnel_Trigger>().Get_Switch() == true)
                    {
                        if (m_bPlayer_Setting_Complete == false)
                        {
                            Debug.Log("Setting");
                            playerCtrl.Set_Enter(true);
                            m_bPlayer_Setting_Complete = true;
                        }
                    }
                }
            }

            if (m_PortalList[4].GetComponentInChildren<Tunnel_Trigger>().Get_Switch() == true ||
                   m_PortalList[5].GetComponentInChildren<Tunnel_Trigger>().Get_Switch() == true)
            {
                Debug.Log("true");

                playerCtrl.into_the_Tunnel();
                if (playerCtrl.Get_Ready_to_Teleport() == true)
                {
                    Debug.Log("Ready_to_Teleport");
                    if (m_Tunnel_Teleport_Trigger_List[0].GetComponent<Tunnel_Teleport_Trigger>().Get_Teleport_Switch() == true)
                    {
                        m_Tunnel_Teleport_Trigger_List[1].GetComponent<Tunnel_Teleport_Trigger>().Set_Setting(true);
                        playerCtrl.Set_PlayerPosition(m_Tunnel_Teleport_Trigger_List[1].transform.position);
                    }
                    else if (m_Tunnel_Teleport_Trigger_List[1].GetComponent<Tunnel_Teleport_Trigger>().Get_Teleport_Switch() == true)
                    {
                        m_Tunnel_Teleport_Trigger_List[0].GetComponent<Tunnel_Teleport_Trigger>().Set_Setting(true);
                        playerCtrl.Set_PlayerPosition(m_Tunnel_Teleport_Trigger_List[0].transform.position);
                    }
                }
                playerCtrl.out_the_Tunnel();

                if (playerCtrl.Get_Player_Lock() == false)
                {
                    for (int i = 4; i < 6; i++)
                    {
                        m_PortalList[i].GetComponentInChildren<Tunnel_Trigger>().Set_Switch(false);

                        if (i == 5)
                        {
                            m_bPlayer_Setting_Complete = false;
                            playerCtrl.Set_Player_use_Portal(false);
                        }
                    }
                }
            }
        }
        #endregion

        #region Enemy
        if (m_Enemy.Get_Enemy_use_Portal() == true)
        {
            if (m_Enemy.Get_Enter() == false)
            {
                for (int i = 4; i < 6; i++)
                {
                    // 스위치 켜지면
                    if (m_PortalList[i].GetComponentInChildren<Tunnel_Trigger>().Get_Switch() == true)
                    {
                        if (m_bEnemy_Setting_Complete == false)
                        {
                            Debug.Log("Setting");
                            m_Enemy.Set_Enter(true);
                            m_bEnemy_Setting_Complete = true;
                        }
                    }
                }
            }

            if (m_PortalList[4].GetComponentInChildren<Tunnel_Trigger>().Get_Switch() == true ||
                   m_PortalList[5].GetComponentInChildren<Tunnel_Trigger>().Get_Switch() == true)
            {
                Debug.Log("true");

                m_Enemy.into_the_Tunnel();
                if (m_Enemy.Get_Ready_to_Teleport() == true)
                {
                    Debug.Log("Ready_to_Teleport");
                    if (m_Tunnel_Teleport_Trigger_List[0].GetComponent<Tunnel_Teleport_Trigger>().Get_Teleport_Switch() == true)
                    {
                        m_Tunnel_Teleport_Trigger_List[1].GetComponent<Tunnel_Teleport_Trigger>().Set_Setting(true);
                        m_Enemy.Set_EnemyPosition(m_Tunnel_Teleport_Trigger_List[1].transform.position);
                    }
                    else if (m_Tunnel_Teleport_Trigger_List[1].GetComponent<Tunnel_Teleport_Trigger>().Get_Teleport_Switch() == true)
                    {
                        m_Tunnel_Teleport_Trigger_List[0].GetComponent<Tunnel_Teleport_Trigger>().Set_Setting(true);
                        m_Enemy.Set_EnemyPosition(m_Tunnel_Teleport_Trigger_List[0].transform.position);
                    }
                }
                m_Enemy.out_the_Tunnel();

                if (m_Enemy.Get_Enemy_Lock() == false)
                {
                    for (int i = 4; i < 6; i++)
                    {
                        m_PortalList[i].GetComponentInChildren<Tunnel_Trigger>().Set_Switch(false);
                        m_PortalList[i].GetComponentInChildren<Tunnel_Trigger>().Set_Enemy_Used(true);

                        if (i == 5)
                        {
                            m_bEnemy_Setting_Complete = false;
                            m_Enemy.Set_Enemy_use_Portal(false);
                            m_Enemy.Set_usePortal(true);
                        }
                    }
                }
            }
        }
        #endregion
    }

    // Blink
    void Blink_Check()
    {
        #region Player
        if (playerCtrl.Get_Player_use_Portal() == true)
        {
            if (m_PortalList[6].GetComponentInChildren<Blink_Trigger>().Get_Switch() == true)
            {
                playerCtrl.Set_PlayerPosition(m_PortalList[7].transform.position);
                m_PortalList[6].GetComponentInChildren<Blink_Trigger>().Set_Switch(false);
                playerCtrl.Set_Player_use_Portal(false);
            }
            else if (m_PortalList[7].GetComponentInChildren<Blink_Trigger>().Get_Switch() == true)
            {
                playerCtrl.Set_PlayerPosition(m_PortalList[6].transform.position);
                m_PortalList[7].GetComponentInChildren<Blink_Trigger>().Set_Switch(false);
                playerCtrl.Set_Player_use_Portal(false);
            }
        }
        #endregion

        #region Enemy
        if (m_Enemy.Get_Enemy_use_Portal() == true)
        {
            if (m_PortalList[6].GetComponentInChildren<Blink_Trigger>().Get_Switch() == true)
            {
                m_Enemy.Set_EnemyPosition(m_PortalList[7].transform.position);
                m_Enemy.Set_Enemy_use_Portal(false);
                m_Enemy.Set_usePortal(true);
                m_PortalList[6].GetComponentInChildren<Blink_Trigger>().Set_Switch(false);
                m_PortalList[6].GetComponentInChildren<Blink_Trigger>().Set_Enemy_Used(true);
                m_PortalList[7].GetComponentInChildren<Blink_Trigger>().Set_Enemy_Used(true);
            }
            else if (m_PortalList[7].GetComponentInChildren<Blink_Trigger>().Get_Switch() == true)
            {
                m_Enemy.Set_EnemyPosition(m_PortalList[6].transform.position);
                m_Enemy.Set_Enemy_use_Portal(false);
                m_Enemy.Set_usePortal(true);
                m_PortalList[7].GetComponentInChildren<Blink_Trigger>().Set_Switch(false);
                m_PortalList[6].GetComponentInChildren<Blink_Trigger>().Set_Enemy_Used(true);
                m_PortalList[7].GetComponentInChildren<Blink_Trigger>().Set_Enemy_Used(true);

                Debug.Log("Enemy Blink!");
            }
        }
        #endregion
    }

    void createPortals()
    {
        m_PortalList.Add(m_Door); // 0
        m_PortalClone = Instantiate(m_Door, new Vector3(m_Door.transform.position.x + 3f, m_Door.transform.position.y + 5.1f, m_Door.transform.position.z), Quaternion.identity);
        m_PortalList.Add(m_PortalClone); // 1

        m_PortalList.Add(m_Fence); // 2
        m_PortalClone = Instantiate(m_Fence, new Vector3(m_Fence.transform.position.x + 5.8f, m_Fence.transform.position.y + 2.5f, m_Fence.transform.position.z), Quaternion.identity);
        m_PortalList.Add(m_PortalClone); // 3

        m_PortalList.Add(m_Tunnel); // 4
        m_PortalClone = Instantiate(m_Tunnel, new Vector3(m_Fence.transform.position.x + 5.8f, m_Fence.transform.position.y + 7.6f, m_Fence.transform.position.z), Quaternion.identity);
        m_PortalList.Add(m_PortalClone); // 5

        m_PortalList.Add(m_Blink); // 6
        m_PortalClone = Instantiate(m_Blink, new Vector3(m_Blink.transform.position.x - 2.8f, m_Blink.transform.position.y + 2.5f, m_Blink.transform.position.z), Quaternion.identity);
        m_PortalList.Add(m_PortalClone); // 7

        //파트너 포탈 셋팅
        for (int i = 0; i < 4; ++i)
        {
            m_PortalList[i * 2].GetComponent<Portal>().Set_Partner_Portal_Floor(m_PortalList[i * 2 + 1].GetComponent<Portal>().Get_PortalFloor());
            m_PortalList[i * 2 + 1].GetComponent<Portal>().Set_Partner_Portal_Floor(m_PortalList[i * 2].GetComponent<Portal>().Get_PortalFloor());
        }
    }
    
    void onPortalCheck()
    {
        // 어떤 포탈이 on 됐는지 확인
        for(int i = 0; i < 8; ++i)
        {
            if(m_PortalList[i].GetComponent<Portal>().Get_PortalSwitch() == true)
            {
                moveOtherPortal(m_PortalList, i);

                m_PortalList[i].GetComponent<Portal>().Set_PortalSwitch(false);
            }
        }
    }
    

    // 파트너 포탈로 이동하는 함수
    void moveOtherPortal(List<GameObject> _portal, int _portalNum)
    {
        // 같은 종류의 다른 위치 포탈로 이동
        // 리스트 [i] 로 구분
        if (_portal[_portalNum].GetComponent<Portal>().Get_Player_in_Portal() == true)
        {
            if (_portalNum > 3)
            {
                playerCtrl.Set_PlayerPosition(_portal[_portalNum - 4].transform.position);
            }
            else
            {
                playerCtrl.Set_PlayerPosition(_portal[_portalNum + 4].transform.position);
            }

            playerCtrl.Set_usePortal(true);
        }
        else if (_portal[_portalNum].GetComponent<Portal>().Get_Player_in_Portal() == false)
        {
            if (_portalNum > 3)
            {
                m_Enemy.Set_EnemyPosition(_portal[_portalNum - 4].transform.position);
            }
            else
            {
                m_Enemy.Set_EnemyPosition(_portal[_portalNum + 4].transform.position);
            }

            m_Enemy.Set_usePortal(true);
            //enemyAI.Set_AI_4_Setting(true);
            _portal[_portalNum].GetComponent<Portal>().Set_Enemy_Use(false);
        }
    }

    public void Reset_Portal_Useable()
    {
        m_PortalList[0].GetComponentInChildren<Door_Trigger>().Set_Enemy_Used(false);
        m_PortalList[1].GetComponentInChildren<Door_Trigger>().Set_Enemy_Used(false);
        m_PortalList[2].GetComponentInChildren<Fence_Trigger>().Set_Enemy_Used(false);
        m_PortalList[3].GetComponentInChildren<Fence_Trigger>().Set_Enemy_Used(false);
        m_PortalList[4].GetComponentInChildren<Tunnel_Trigger>().Set_Enemy_Used(false);
        m_PortalList[5].GetComponentInChildren<Tunnel_Trigger>().Set_Enemy_Used(false);
        m_PortalList[6].GetComponentInChildren<Blink_Trigger>().Set_Enemy_Used(false);
        m_PortalList[7].GetComponentInChildren<Blink_Trigger>().Set_Enemy_Used(false);
    }

    // AI 용 입력한 층과 같은 층에 있는 포탈 중 플레이어에게 갈 수 있는 포탈 있는지, 없으면 거리가 먼 포탈을 반환
    public Vector3 Search_toPlayer_Portal(int _floor, Vector3 _position)
    {
        Vector3 portalVector = new Vector3(0f, 0f, 0f);
        // 한번에 플레이어에가 갈 수 있는 포탈 있는지 없는지
        bool toPlayer = false;
        // 해당 층의 포탈 넘버
        int[] portalNum = new int[2] { 0, 0 };
        int j = 0;
        // 포탈과 매개변수 포지션과의 거리
        float[] Distance = new float[2] { 0f, 0f };

        // 플레이어 한번에 갈 수 있는 포탈이 있는지 찾아봄
        for (int i = 0; i < m_iPortal_Count; ++i)
        {
            if(m_PortalList[i].GetComponent<Portal>().Get_PortalFloor() == _floor)
            {
                portalNum[j] = i;
                j = j + 1;
                if(m_PortalList[i].GetComponent<Portal>().Get_Partner_Portal_Floor() == playerCtrl.Get_PlayerFloor())
                {
                    m_PortalList[i].GetComponent<Portal>().Set_Enemy_Use(true);
                    portalVector = m_PortalList[i].transform.position;
                    toPlayer = true;
                }
            }
        }

        // 플레이어에게 한번에 갈 수 없다면, 같은 층에 있는 포탈 중 거리가 먼 포탈 반환
        if (toPlayer == false)
        {
            Distance[0] = Mathf.Abs(m_PortalList[portalNum[0]].transform.position.x - _position.x);
            Distance[1] = Mathf.Abs(m_PortalList[portalNum[1]].transform.position.x - _position.x);

            if(Distance[0] > Distance[1])
            {
                portalVector = m_PortalList[portalNum[0]].transform.position;
                m_PortalList[portalNum[0]].GetComponent<Portal>().Set_Enemy_Use(true);
            }
            else
            {
                portalVector = m_PortalList[portalNum[1]].transform.position;
                m_PortalList[portalNum[1]].GetComponent<Portal>().Set_Enemy_Use(true);
            }
        }

        return portalVector;
    }

    // AI 용 포탈 위치 반환 - 타겟으로 이용, AI 움직이게 만듦
    public Vector3 Search_Close_Portal(int _floor, Vector3 _position)
    {
        Vector3 portalVector = new Vector3(0f, 0f, 0f);

        float[] Distance = new float[2] { 0f, 0f };
        int[] portalNum = new int[2] { 0, 0 };

        int j = 0;

        for (int i = 0; i < 8; ++i)
        {
            if (m_PortalList[i].GetComponent<Portal>().Get_PortalFloor() == _floor)
            {
                if (j < 2)
                {
                    Distance[j] = m_PortalList[i].GetComponent<Portal>().transform.position.x - _position.x;
                    portalNum[j] = i;
                    j = j + 1;
                }
            }
        }

        if (Mathf.Abs(Distance[0]) < Mathf.Abs(Distance[1]))
        {
            portalVector = m_PortalList[portalNum[0]].transform.position;
            m_PortalList[portalNum[0]].GetComponent<Portal>().Set_Enemy_Use(true);
        }
        else if (Mathf.Abs(Distance[0]) > Mathf.Abs(Distance[1]))
        {
            portalVector = m_PortalList[portalNum[1]].transform.position;
            m_PortalList[portalNum[1]].GetComponent<Portal>().Set_Enemy_Use(true);
        }

        return portalVector;
    }
}

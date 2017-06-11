using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // 스테이지 셋팅
    int m_iGameStage;

    public int Get_GameStage() { return m_iGameStage; }
    public void Set_GameStage(int _iGameStage) { m_iGameStage = _iGameStage; }

    GameObject ClearUI;
    GameObject FailUI;

    bool m_bSetting = false;

    // 클리어 조건 셋팅, 판별
    TileManager m_TileManager;
    Health m_Health;
    timerTwo m_Timer;

    PlayerController playerCtrl;
    EnemyAI enemy;
    
    // 카메라 셋팅

    // 오브젝트 셋팅

    // 플레이어 셋팅

    // 적 셋팅



    private void Awake()
    {
        m_iGameStage = 1;
        m_TileManager = GameObject.FindGameObjectWithTag("TileManager").GetComponent<TileManager>();

        ClearUI = GameObject.Find("Canvas").transform.Find("ClearUI").gameObject;
        FailUI = GameObject.Find("Canvas").transform.Find("FailUI").gameObject;

        m_Timer = GameObject.Find("UpperUI").transform.Find("Gage").gameObject.GetComponent<timerTwo>();
        m_Health = GameObject.Find("HealthManager").GetComponent<Health>();
        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        enemy = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }

    private void Update()
    {
        AI_4_Switch();

        if (m_bSetting == false)
        {
            Game_Win();
            Game_Fail();
        }
    }

    private void AI_4_Switch()
    {
        if (playerCtrl.Get_Special_Counter() == true)
            enemy.Set_AI_4(true);
    }

    // 게임 승리, 패배(피가 전부 달거나, 시간이 다되거나)
    void Game_Win()
    {
        if(m_TileManager.Get_MoonScore() == 16)
        {
            // 승리화면 로드
            Time.timeScale = 0.0f;

            GlobalManager.m_bGameClear = true;

            ClearUI.SetActive(true);

            m_bSetting = true;
        }
    }

    void Game_Fail()
    {
        if(m_Health.Get_HP() == 0 || m_Timer.Get_Time_Over() == true)
        {
            // 패배화면 로드
            Time.timeScale = 0.0f;

            FailUI.SetActive(true);

            m_bSetting = true;
        }
    }
}

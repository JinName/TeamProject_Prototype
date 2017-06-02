using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // 스테이지 셋팅
    int m_iGameStage;

    public int Get_GameStage() { return m_iGameStage; }
    public void Set_GameStage(int _iGameStage) { m_iGameStage = _iGameStage; }

    // 클리어 조건 셋팅, 판별
    GameObject m_TileManager;
    PlayerController playerCtrl;
    EnemyAI enemy;

    public void StageClear()
    { }

    // 카메라 셋팅

    // 오브젝트 셋팅

    // 플레이어 셋팅

    // 적 셋팅



    private void Awake()
    {
        m_iGameStage = 1;
        m_TileManager = GameObject.FindGameObjectWithTag("TileManager");

        playerCtrl = GameObject.Find("Player").GetComponent<PlayerController>();
        enemy = GameObject.Find("Enemy").GetComponent<EnemyAI>();
    }

    private void Update()
    {
        AI_4_Switch();
    }

    private void AI_4_Switch()
    {
        if (playerCtrl.Get_Special_Counter() == true)
            enemy.Set_AI_4(true);
    }
}

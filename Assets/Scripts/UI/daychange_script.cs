using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class daychange_script : MonoBehaviour
{
    TileManager m_Tile_Manager;
   Image fillImg2;
   public Tile daychanges;
   public int daychange_moonscore;

    // Use this for initialization
    void Start()
    {
        m_Tile_Manager = GameObject.Find("TileManagerObj").GetComponent<TileManager>();
        fillImg2 = this.GetComponent<Image>();
       fillImg2.fillAmount = 0;
        //daychange_moonscore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // var daychanges = GameObject.Find("Tiles").GetComponent<Tile>();// as TileManager;
        // daychange_moonscore = daychanges.MoonScore;
        fillImg2.fillAmount = m_Tile_Manager.Get_MoonScore() * 0.0625f; // fill 마운트값 0~1사이 에따라서 이미지 보이는 정도가 달라짐
                                                                                                // 타일최대 16장 1/16 = 0.0625  ex) 타일 8장 점령하면 0.5 만큼보임
    }
}
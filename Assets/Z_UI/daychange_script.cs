using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class daychange_script : MonoBehaviour
{
    Image fillImg2;
    public TileManager daychanges;
    public int daychange_moonscore;
   float daychange_divisionnumber =  0.0625F;

    // Use this for initialization
    void Start()
    {
       fillImg2 = this.GetComponent<Image>();
        fillImg2.fillAmount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        var daychanges = GameObject.Find("Tiles").GetComponent<TileManager>();// as TileManager;
       daychange_moonscore = daychanges.m_iMoonScore;
        fillImg2.fillAmount = daychange_moonscore * 0.0625f;
    }
}
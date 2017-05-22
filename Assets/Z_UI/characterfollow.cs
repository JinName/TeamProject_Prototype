using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterfollow : MonoBehaviour {

    
    // Use this for initialization
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.y += 0.1f;
        print(pos);
    }
   

}

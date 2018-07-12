using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightLineCollider : MonoBehaviour {

    HandMove handMove;
    
	// Use this for initialization
	void Start () {
        handMove = GetComponentInParent<HandMove>();
    }
	
	// Update is called once per frame
	void Update () {
       
	}

    void OnTriggerEnter2D(Collider2D col)
    {
       if(col.tag == "Player")
        {
            
            handMove.see = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            handMove.see = false;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMove : MonoBehaviour {

    public Transform righthand;
    public Transform lefthand;
    public Transform player;
    public bool fire;

    public Vector2 v4;//用來判斷方位
    private Vector2 yVelocity ;
    private Vector2 player_pos;
    // Use this for initialization
    void Start () {
        Attack();

    }
	
	// Update is called once per frame
	void Update () {
        Attack();
        ready();
        IKchange();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player_pos = player.position;
            fire = true;
            Invoke("over", 0.3f);
        }

    }

    public void IKchange()
    {
        if (v4.y > 0)
        {
            righthand.GetComponent<Anima2D.IkLimb2D>().flip = false;
            lefthand.GetComponent<Anima2D.IkLimb2D>().flip = true;
        }
        else
        {
            righthand.GetComponent<Anima2D.IkLimb2D>().flip = true;
            lefthand.GetComponent<Anima2D.IkLimb2D>().flip = false;
        }
    }

     void over()
    {
        fire = false;
    }

    public void Attack()
    {
        v4 =  player.position- transform.position;
        //Debug.Log(v4);

    } 

    public void ready()
    {
        if (v4.x > 0)
        {
            righthand.position = Vector2.SmoothDamp(righthand.position, new Vector2(transform.position.x+2.5f, player.position.y), ref yVelocity, 1);
            if (fire)
            {
                righthand.position = Vector2.SmoothDamp(righthand.position, player_pos, ref yVelocity, 0.3f);
                
            }
            
        }
        else
        {
            lefthand.position = Vector2.SmoothDamp(lefthand.position, new Vector2(transform.position.x -3f, player.position.y), ref yVelocity, 1);
            if (fire)
            {
                lefthand.position = Vector2.SmoothDamp(lefthand.position, player_pos, ref yVelocity, 0.3f);
                
            }
            
        }
    }

}

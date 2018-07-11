using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMove : MonoBehaviour {

    public Animator ani;
    public Transform righthand;
    public Transform lefthand;
    public Transform player;
    public bool fire;

    public Vector2 v4;//用來判斷方位
    private Vector2 yVelocity ;
    private Vector2 player_pos;
    private float StandbyposX =-3;

    public bool alert;
    public int mode =1;

    public RuntimeAnimatorController IdleAni;
    public RuntimeAnimatorController AttackAni;
    // Use this for initialization
    void Start () {
        Attack();
        GetComponent<Rigidbody2D>().AddForce(gun.forward * bulletSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        if (alert == true)
        {
            ani.runtimeAnimatorController = AttackAni;
            Attack();
            ready();
            //IKchange();
            runtotarget();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                player_pos = player.position;
                fire = true;
                Invoke("over", 0.3f);
            }
        }
        else
        {
            ani.runtimeAnimatorController = IdleAni;
        }

        
    }

    public void runtotarget()
    {




        if (v4.x > 1f)
        {
            if(v4.x > 2f)
                ani.SetFloat("runspeed", 1f);

            ani.transform.localScale = new Vector3(1, 1, 1);
            StandbyposX = 3;
            /* if (ani.transform.localScale.x > -1)
             {
                 ani.transform.localScale = new Vector3(ani.transform.localScale.x-0.1f, 1, 1);
             }*/

        }
        else if (v4.x < -1f)
        {
            
            if (v4.x < -2f)
                ani.SetFloat("runspeed", 1f);

            ani.transform.localScale = new Vector3(-1, 1, 1);
            StandbyposX = -3;
            /*if (ani.transform.localScale.x < 1)
            {
                ani.transform.localScale = new Vector3(ani.transform.localScale.x + 0.1f, 1, 1);
            }*/
        }
        else
        {
            ani.SetFloat("runspeed", 0f);
            /*if (v4.y > 3.5f)
            {
                ani.SetTrigger("JumpAttack");
            }*/

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
    float t;

    public void Attack()
    {
        v4 =  player.position- transform.position;
        if (Vector2.Distance(player.position,righthand.position)<0.5)
        {
            t += Time.deltaTime;
            if (t >1.5f)
            {
                player_pos = player.position;
                fire = true;
                Invoke("over", 0.3f);
                t = 0;
            }
        }
        else
        {
            t = 0;
        }

        
        //Debug.Log(v4);

    } 

    public void ready()
    {
        if (mode == 1)
        {
            lefthand.position = Vector2.SmoothDamp(lefthand.position, new Vector2(transform.position.x + StandbyposX, player.position.y), ref yVelocity, 1);
            if (fire)
            {
                lefthand.position = Vector2.SmoothDamp(lefthand.position, player_pos, ref yVelocity, 0.3f);

            }
        }
        if(mode == 2)
        {
            
            if (fire)
            {
                /*righthand.position = player.position;
                lefthand.position = player.position;*/
                righthand.position = Vector2.Lerp(righthand.position, player_pos, 0.2f);
                lefthand.position = Vector2.Lerp(lefthand.position, player_pos, 0.2f);
            }
            else
            {
                righthand.position = Vector2.Lerp(righthand.position, new Vector2(player.position.x - 0.5f, player.position.y), 0.01f);
                lefthand.position = Vector2.Lerp(lefthand.position, new Vector2(player.position.x + 0.5f, player.position.y) , 0.01f);
            }
        }
        /*
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
            
        }*/
    }

    public Rigidbody2D bullet;
    public Transform gun;
    public float bulletSpeed;
    public void GunFire()
    {
        Rigidbody2D br = Instantiate(bullet, gun.position, gun.rotation);
        br.AddForce((gun.transform.right+new Vector3(0,Random.Range(0.5f,-0.5f),0)) * bulletSpeed);
        Destroy(br, Random.Range(1f,3f));
    }





}

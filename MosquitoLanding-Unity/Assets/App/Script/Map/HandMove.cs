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

    public GameObject gunman;

    public int mode =1;

    public enum State
    {
        Idle,
        Attack,
        Underarm
    }

    public State _currentState;


    public RuntimeAnimatorController IdleAni;
    public RuntimeAnimatorController AttackAni;
    // Use this for initialization

    Vector3 starpos;
    void Start () {
        player = GameObject.Find("Mosquito").transform;
        Attack();
        starpos = transform.position;
       // GetComponent<Rigidbody2D>().AddForce(gun.forward * bulletSpeed);
    }
	
	// Update is called once per frame
	void Update () {
       
        check();
        switch (_currentState)
        {
            case State.Idle:
                ani.runtimeAnimatorController = IdleAni;
                break;

            case State.Attack:
                repos();
                ani.runtimeAnimatorController = AttackAni;
                Attack();
                ready();                
                runtotarget();
                if (v4.y > 3f)
                {
                    _currentState = State.Underarm;
                }
                break;

            case State.Underarm:
                repos();
                ani.runtimeAnimatorController = IdleAni;
                ani.SetTrigger("Underarm");
                gunman.SetActive(true);
                _currentState = State.Idle;

                break;
        }
    }

    public void repos()
    {
        transform.position = new Vector3(transform.position.x,starpos.y);
    }

    public void check()
    {
        v4 = player.position - transform.position;

    }


    public void runtotarget()
    {

        if (v4.x > 1f)
        {
            if(v4.x > 2f)
                ani.SetFloat("runspeed", 1f);

            ani.transform.localScale = new Vector3(1, 1, 1);
            StandbyposX = 3;       
        }
        else if (v4.x < -1f)
        {
            
            if (v4.x < -2f)
                ani.SetFloat("runspeed", 1f);

            ani.transform.localScale = new Vector3(-1, 1, 1);
            StandbyposX = -3;
            
        }
        else
        {
            ani.SetFloat("runspeed", 0f);
          
        }
    }


     void over()
    {
        fire = false;
    }
    float t;

    public void Attack()
    {        
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
                righthand.position = Vector2.Lerp(righthand.position, player_pos, 0.2f);
                lefthand.position = Vector2.Lerp(lefthand.position, player_pos, 0.2f);
            }
            else
            {
                righthand.position = Vector2.Lerp(righthand.position, new Vector2(player.position.x - 0.5f, player.position.y), 0.01f);
                lefthand.position = Vector2.Lerp(lefthand.position, new Vector2(player.position.x + 0.5f, player.position.y) , 0.01f);
            }
        }
       
    }

    public Rigidbody2D bullet;
    public Transform gun;
    public float bulletSpeed;
    public void GunFire()
    {
        Rigidbody2D br = Instantiate(bullet, gun.position, gun.rotation);
        br.AddForce((gun.transform.right+new Vector3(0,Random.Range(0.2f,-0.2f),0)) * bulletSpeed);
        Destroy(br, Random.Range(1f,3f));
    }





}

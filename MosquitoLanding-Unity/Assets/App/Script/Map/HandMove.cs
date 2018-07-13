using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandMove : MonoBehaviour {

    public Animator ani;
    public Transform righthand;
    public Transform attobj;
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
        Underarm,
        Wander
    }

    public State _currentState;


    public RuntimeAnimatorController IdleAni;
    public RuntimeAnimatorController AttackAni;

    public Collider2D rc;
    public Collider2D lc;
    // Use this for initialization

    Vector3 starpos;
    float ttt;
    public float sighttime;
    float tt;
    public SpriteRenderer EyeSprite;
    public bool see;

    public GameObject bigtext;

    void Start () {
        player = GameObject.Find("Mosquito").transform;
        Attack();
        tt = Time.time;
        starpos = transform.position;
        StartCoroutine(ModeChange(Random.Range(8f,12f)));
        
       // GetComponent<Rigidbody2D>().AddForce(gun.forward * bulletSpeed);
    }
	
	// Update is called once per frame
	void Update () {
       
        check();
        switch (_currentState)
        {
            case State.Idle:
                ani.runtimeAnimatorController = IdleAni;
                ani.SetBool("Wander", false);
                break;

            case State.Wander:
                ani.runtimeAnimatorController = IdleAni;
                ani.SetBool("Wander", true);
                wander();
                break;

            case State.Attack:
                repos();
                ani.runtimeAnimatorController = AttackAni;
                Attack();
                ready();                
                runtotarget();
                if (v4.y > 3f)
                {
                    if (v4.x>-0.4&&v4.x<0.4)
                    {
                        _currentState = State.Underarm;
                        
                    }
                }
                break;

            case State.Underarm:
                if (bigtext.activeSelf==false) {
                    bigtext.SetActive(true);
                   // Invoke("UnderarmFire", 3);
                    Time.timeScale = 0;
                }

                break;
        }

    
        if (see)
        {
            if (tt > sighttime)
            {
                toAttack();
            }
            tt += Time.deltaTime;
            EyeSprite.color = new Color(EyeSprite.color.r, EyeSprite.color.g, EyeSprite.color.b, Mathf.Lerp(EyeSprite.color.a, 1, 0.1f));
            if (EyeSprite.color.b > 0)
            {
                EyeSprite.color = new Color(EyeSprite.color.r, EyeSprite.color.g - ((1 / sighttime) * Time.deltaTime), EyeSprite.color.b - ((1 / sighttime) * Time.deltaTime), EyeSprite.color.a);
            }
        }
        else
        {
            if (_currentState == HandMove.State.Idle)
            {
                if (tt > 0)
                    tt -= Time.deltaTime;
                EyeSprite.color = Color.Lerp(EyeSprite.color, new Color(1, 1, 1, 0), 0.05f);
            }

        }

    }


    public void UnderarmFire()
    {
        Time.timeScale = 1;
        repos();
        ani.runtimeAnimatorController = IdleAni;
        ani.SetTrigger("Underarm");

        gunman.SetActive(true);
        see = false;
        tt = 0;
        _currentState = State.Idle;
    }

    IEnumerator ModeChange(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);
        switch (_currentState)
        {
            case State.Idle:
                _currentState = State.Wander;
                StartCoroutine(ModeChange(Random.Range(8f, 11f)));
                break;

            case State.Wander:
                _currentState = State.Idle;
                StartCoroutine(ModeChange(Random.Range(3f, 15f)));
                break;
        }
        
    }

    public void toAttack()
    {
        if (Time.time - ttt>5)
        {
            _currentState = State.Attack;
        }
        else
        {
            see = false;
        }
       
    }

    private void wander()
    {
        Vector2 RV2 = starpos + new Vector3(17, 0);
        Vector2 LV2 = starpos + new Vector3(-17, 0);
        float Rd = starpos.x + 17;
        float Ld = starpos.x - 17;
        if (transform.position.x > Rd)
        {
            ani.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (transform.position.x < Ld)
        {
            ani.transform.localScale = new Vector3(1, 1, 1);
        }


    }

    public void repos()
    {
        transform.position = new Vector3(transform.position.x,starpos.y);
    }


    public void rest()
    {
        _currentState = State.Idle;
        see = false;
        tt = 0;
        StopCoroutine(ModeChange(0));
        StopAllCoroutines();
        StartCoroutine(ModeChange(Random.Range(12f, 15f)));
        transform.position = new Vector3(starpos.x, starpos.y);
    }

    public void check()
    {
        v4 = player.position - transform.position;

    }


    public void runtotarget()
    {

        if (v4.x > 1f)
        {
            if(v4.x > 2.5f)
                ani.SetFloat("runspeed", 1f);

            ani.transform.localScale = new Vector3(1, 1, 1);
            StandbyposX = 3;       
        }
        else if (v4.x < -1f)
        {
            
            if (v4.x < -2.5f)
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
                
                //player.GetComponent<MosquitoHandler>().DeadAnimationHandler(EventFlag.Death.Squash);
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
                rc.enabled = true;
                lc.enabled = true;
            }
            else
            {
                righthand.position = Vector2.Lerp(righthand.position, new Vector2(player.position.x - 0.5f, player.position.y), 0.01f);
                lefthand.position = Vector2.Lerp(lefthand.position, new Vector2(player.position.x + 0.5f, player.position.y) , 0.01f);
                rc.enabled = false;
                lc.enabled = false;
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

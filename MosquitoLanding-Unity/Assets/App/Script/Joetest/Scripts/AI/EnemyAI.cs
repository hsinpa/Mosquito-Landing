﻿using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	// Custom status, change them in inspector
	public float speed = 3f ;		// move speed
	public float rSpeed = 10f ; 	// rotate speed
    public int HP = 3;
    public GameObject[] DeadEffect;	// 消滅特效
    

    // intial variables, DON'T change the value
    internal Player pl ;			 // player reference
	internal Vector3 targetPoint ; 	 // target point
	internal Quaternion look ;  	// target rotation
    internal bool onSphere = false ;  // is on player sphere?
    internal float flyspeed = 3;

    SpriteRenderer[] spriteRenderers;

	void Awake(){
        //pl = Object.FindObjectOfType<Player>() ;
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
	}
	
	// Moving function
	public void MoveTo(){
		//Vector3 target ; // target position
		Vector3 pos = transform.position ;	// my position
        transform.position = transform.position + transform.right * speed * Time.deltaTime; 
        //Vector3 pPos = pl.transform.position ; // player position
        /*
		if(onSphere){
			// spheric movement
			Vector3 dir = Vector3.RotateTowards(pos-pPos, targetPoint-pPos, speed*Time.deltaTime, 100) ;
			target = dir + pPos ;
            flyspeed = 1;
		}else{
			// direct movement
			target = targetPoint ;
            flyspeed = 3;
            
		}
		
		// face movement
		if(Vector3.Distance(target,pos) >= 0.08f){
            look = Quaternion.LookRotation(target - pos, pPos - pos);
           
        }
        
        transform.position = Vector3.MoveTowards(pos, target,  flyspeed*speed*Time.deltaTime) ;
		transform.rotation = Quaternion.Slerp(transform.rotation, look, rSpeed*Time.deltaTime) ;
		
		// if into player sphere, use Spheric Movement. Call once
		if(Vector3.Distance(pos, pPos) <= pl.sphere.radius && !onSphere){
			onSphere = true ;
			OnTouchSphere() ;
		}*/
    }
	
    // BeHurt function
    public void BeHurt(int amount)
    {
        HP -= amount;

        if(cf == 0)
        Invoke("Colorchange", 0.1f);
        if (HP <= 0)
        {
            Destroy(gameObject);
            Dead();
            //Instantiate(DeadCreate,transform.position,transform.rotation);//生成掉落物
        }
    }
    int cf;

    public void Colorchange()
    {
        foreach (SpriteRenderer sr in spriteRenderers)
        {
            sr.color = new Color(1, 1, 1, cf%2);
            
        }

        cf++;
        if (cf >5)
        {
            cf = 0;
            foreach (SpriteRenderer sr in spriteRenderers)
            {
                sr.color = new Color(1, 1, 1, 1);

            }
        }
        else
        {
            Invoke("Colorchange", 0.1f);
        }

    }


    public void Dead()
    {
        if (DeadEffect.Length>0)
        {
            GameObject effect = Instantiate(DeadEffect[Random.Range(0,DeadEffect.Length)], transform.position, transform.rotation) as GameObject;
            effect.transform.parent = Game.moveall;

        }
    }

    //=========
    // Events
    //=========
    // Call once when this enemy attached to player sphere
    public virtual void OnTouchSphere(){}

}
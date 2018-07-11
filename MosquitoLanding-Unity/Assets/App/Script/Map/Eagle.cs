using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour {
    public List<Transform> neck;
    public float sSpeed = 0.1f;//平滑速度
    public Transform target;
    public float attackspeed;
    public float t;
    public float d = 0.5f;
    public float atttime;
    CircleCollider2D cc2d;
    public int necknum;
    public Transform neckobj;
   
    // Use this for initialization
    void Start () {
        cc2d = GetComponent<CircleCollider2D>();
        
        for(int i = 0; i<necknum; i++)
        {
            Transform t = Instantiate(neckobj, transform.position, transform.rotation);
            
            neck.Add(t);
        }
       
        neck.Reverse(1,neck.Count-1);
    }

    private void Re()
    {
        
        cc2d.enabled = true;
    }

    // Update is called once per frame
    void Update () {


        


        if (target)
        {
            for (int i = 1; i < neck.Count; i++)
            {
                if (i + 1 < neck.Count)
                {
                    if (Vector2.Distance(neck[i].position, neck[i + 1].position) <= d)
                    {
                        neck[i].position = Vector3.Lerp(neck[i].position, neck[i - 1].position, sSpeed);
                        neck[i].up = Vector3.Lerp(neck[i].up, neck[i - 1].position - neck[i].position, 10f);
                    }
                }
               
            }
            transform.right = new Vector2 ((target.position - transform.position).x, (target.position - transform.position).y)*-1;
            t += Time.deltaTime;
            if (t > 3)
            {
                transform.position = Vector2.Lerp(transform.position, new Vector2(target.position.x,target.position.y), attackspeed);
            }
            if (t > 3+atttime)
            {
                target = null;
                t = 0;
                cc2d.enabled = false;
                Invoke("Re", 2);
            }
        }
        else
        {
            for (int i = 0; i < neck.Count; i++)
            {
                if (i + 1 < neck.Count)
                {
                    neck[i].position = Vector3.Lerp(neck[i].position, neck[i+1].position, sSpeed);
                    if(i>0)
                    neck[i].right = -(neck[i + 1].position - neck[i].position);
                }
                
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            target = col.transform;
            
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            target = null;
            t = 0;

        }
    }
}

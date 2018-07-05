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
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


        for (int i = 1; i < neck.Count; i++)
        {
            if (i + 1 < neck.Count)
            {
                if (Vector2.Distance(neck[i].position, neck[i + 1].position) <= d)
                {
                    neck[i].position = Vector3.Lerp(neck[i].position, neck[i - 1].position, sSpeed);

                }
            }
            neck[i].right = neck[i - 1].position - neck[i].position;
        }


        if (target)
        {
            transform.right = new Vector2 ((target.position - transform.position).x, (target.position - transform.position).y);
            t += Time.deltaTime;
            if (t > 3)
            {
                transform.position = Vector3.Lerp(transform.position, target.position, attackspeed);
            }
            if (t > 3+atttime)
            {
                target = null;
                t = 0;
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

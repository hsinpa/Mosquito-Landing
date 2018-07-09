using UnityEngine;
using System.Collections;

public class bullet : MonoBehaviour {
    public float DestroyTime = 3;
    public float speed = 5;
    public bool pb;
    void Start () {
        Destroy(gameObject, DestroyTime);
	}
	
	
	void Update () {
        transform.position += transform.right*speed*Time.deltaTime;
	}

    void OnTriggerEnter2D(Collider2D other)
    {
       
        if (pb) {
            if (other.tag == "Boss")
            {

                other.GetComponent<EnemyAI>().BeHurt(1);
            }
        }
        else
        {

        }
       
    }

}

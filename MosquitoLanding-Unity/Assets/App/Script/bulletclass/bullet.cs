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

    

}

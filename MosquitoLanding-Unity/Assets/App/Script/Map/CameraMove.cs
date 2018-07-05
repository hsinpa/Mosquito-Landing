using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position,new Vector3(transform.position.x,transform.position.y, Camera.main.transform.position.z), 0.01f);
    }
}

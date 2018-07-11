using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enobj : MonoBehaviour {
    public float time;
	// Use this for initialization
	void Start () {
        Invoke("en", time);
	}
    void OnEnable()
    {
        Invoke("en", time);
    }

    void en()
    {
        gameObject.SetActive(false);
    }
	
}

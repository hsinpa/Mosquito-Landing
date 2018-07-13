using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class enobj : MonoBehaviour {
    public float time;
    public UnityEvent ue;
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
	
    public void anity()
    {
        ue.Invoke();
        Invoke("en", 0.5f);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameView : MonoBehaviour {

	public T GetViewObject<T>() where T : Object {
		return transform.GetComponentInChildren<T>();
	}
	
	public Transform GetViewObject(string p_relative_path) {
		return transform.Find(p_relative_path);
	}

}

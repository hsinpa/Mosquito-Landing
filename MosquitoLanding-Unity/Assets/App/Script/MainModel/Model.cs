using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour {

	public T FindModel<T>() where T : Object { 
		return transform.GetComponentInChildren<T>();
	}

}

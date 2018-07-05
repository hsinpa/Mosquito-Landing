using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MosquitoBodyContainer  {
	public Transform bodyPart;
	public Vector3 originalLocalPositon;
	public Quaternion originalLocalRotation;
	
	public MosquitoBodyContainer(Transform p_bodyaPart, Vector3 p_localPosition, Quaternion p_localRotation) {
		bodyPart = p_bodyaPart;
		originalLocalPositon = p_localPosition;
		originalLocalRotation = p_localRotation;
	}
}

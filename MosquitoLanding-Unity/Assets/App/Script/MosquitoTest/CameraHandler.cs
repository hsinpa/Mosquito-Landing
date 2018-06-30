using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour {

	private Vector3 defaultPosition = new Vector3(0, 0, -10);
	private float defaultCameraSize = 5;
	
	private Camera _camera;

	private Transform targetTransform;
	private float targetCameraSize;

	public enum State {
		FollowTarget,
		Default,
		Idle
	}

	private State _currentState;

	// Use this for initialization
	void Start () {
		_camera = GetComponent<Camera>();
		_currentState = State.Idle;
	}

	public void SetAnimation(State p_state, Transform p_target = null, float p_camera_size = 0) {	
		targetTransform = p_target;
		targetCameraSize = p_camera_size;

		_currentState = p_state;
	}

	public bool PlayAnimation(Vector3 p_target_position, float p_camera_size) {
		Vector3 tempCameraPosition = Vector3.Lerp(transform.position, p_target_position, 0.06f);
		transform.position = tempCameraPosition;

		float tempCameraSize = Mathf.Lerp(_camera.orthographicSize, p_camera_size, 0.06f);
		_camera.orthographicSize = tempCameraSize;

		return ((p_target_position - transform.position).magnitude < 0.1f && tempCameraSize < 0.1f); 
	}

	public void AnimationHandler() {
		
		bool IsAnimationFinish = false;
		switch (_currentState)
		{
			case State.Default:
				IsAnimationFinish = PlayAnimation(defaultPosition, defaultCameraSize);
			break;

			case State.FollowTarget:
				IsAnimationFinish = PlayAnimation(new Vector3(targetTransform.position.x, targetTransform.position.y, -10),
												targetCameraSize);
			break;
		}
		
		if (IsAnimationFinish) _currentState = State.Idle;
	}


	// Update is called once per frame
	void Update () {
		if (_currentState != State.Idle) {
			
			AnimationHandler();
		}
	}

}

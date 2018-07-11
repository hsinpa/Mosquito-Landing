using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using MosquitoInput;

public class MosquitoMovement {

	private float defaultAccelerateSpeed = 0.008f;
	private float accelerateSpeed;
	private float headAngle;


	private Rigidbody2D _rigidBody;
	private Transform headObject;
	private MosquitoHandler _mosquitoHandler;
	private Transform _mosquitoBody;
	private CameraHandler _camera;

	private MosquitoInputManager inputManager;
	

	public MosquitoMovement(MosquitoHandler p_mosquito, CameraHandler p_camera) {
		_mosquitoHandler = p_mosquito;
		_mosquitoBody = p_mosquito.transform;
		_rigidBody = p_mosquito.GetComponent<Rigidbody2D>();
		headObject = _mosquitoBody.Find("head");
		_camera = p_camera;

		accelerateSpeed = defaultAccelerateSpeed;
		

		inputManager = new MosquitoInputManager();

		_mosquitoHandler.OnStatusChange += OnStatusChange;

	}

	public void OnStatusChange(MosquitoHandler.Status p_status) {
		switch (p_status)
		{
			case MosquitoHandler.Status.Idle:
				_rigidBody.gravityScale = 0.05f;
				_rigidBody.drag = 0.3f;
			break;

			case MosquitoHandler.Status.SuckBlood:
				_rigidBody.gravityScale = 0;
				_rigidBody.drag = 8;
			break;

			case MosquitoHandler.Status.Dead:

			break;
		}
	}

	public void OnUpdate() {
		if (_mosquitoHandler.currentStatus == MosquitoHandler.Status.Dead) return;
		
		RotateHeadToNearstTarget();
	}

	public void OnFixedUpdate() {
		if (_mosquitoHandler.currentStatus == MosquitoHandler.Status.Dead ||
			 _rigidBody.bodyType != RigidbodyType2D.Dynamic) return;

		float translation = ( inputManager.IsFrontClick() + 
						((inputManager.IsLeftClick() + inputManager.IsRightClick() == 2) ? 1 : 0) )
						* _mosquitoHandler.speed * Time.deltaTime * 10;

        float rotation = -(inputManager.IsLeftClick() - inputManager.IsRightClick()) * _mosquitoHandler.rotateSpeed * Time.deltaTime * 10;

		//HOW TO LEAVE SUCKBLOOD STATUS
		if (_mosquitoHandler.currentStatus == MosquitoHandler.Status.SuckBlood) {

			if ((headAngle <= 60 || headAngle >= 300) && rotation > 0) rotation = 0;

			if ((headAngle >= 150 || headAngle <= 250) && rotation < 0) rotation = 0;

			if (translation + Mathf.Abs(rotation) > 0) _mosquitoHandler.currentStatus = MosquitoHandler.Status.Idle;
		}

		VerticalMove(translation);
		HorizontalMove(rotation);
	}

	private void VerticalMove(float p_variable) {
		if (p_variable != 0) {
			DirectionalMove(p_variable, _mosquitoBody.up);

			_rigidBody.rotation = Mathf.Lerp(_rigidBody.rotation, 0, 0.1f);
			_rigidBody.angularVelocity = 0;

			//Debug.Log("Magnitude " + _rigidBody.velocity.magnitude);
		} else {
			accelerateSpeed = Mathf.Lerp(accelerateSpeed, defaultAccelerateSpeed, 0.01f);
		}
	}

	private void HorizontalMove(float p_variable) {
		if (p_variable != 0) {
			DirectionalMove(p_variable, _mosquitoBody.right);
		}
	}

	private void DirectionalMove(float p_variable, Vector3 p_local_direction) {
		Vector2 newVelocity = ( p_local_direction * p_variable * Time.deltaTime );
				newVelocity += _rigidBody.velocity;
		
		if (newVelocity.magnitude <= 4) {
			accelerateSpeed =  Mathf.Clamp(accelerateSpeed + (Time.deltaTime * 0.02f ), 0, 0.1f);
			_rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, newVelocity, accelerateSpeed);
		}
	}


	private List<Vector2> FindNearestBodySkin() {
		float lineRadius = 1.5f;
		float perDegree = 18;
		int totalLine = 20;

		List<Vector2> bodyPoint = new List<Vector2>();
		
		for (int i = 1; i <= totalLine; i++) {
			var x = lineRadius * Mathf.Cos((perDegree * i  )* Mathf.Deg2Rad);
			var y = lineRadius * Mathf.Sin((perDegree * i )* Mathf.Deg2Rad);

			Vector2 targetLocation = new Vector2( _mosquitoBody.position.x +  x, _mosquitoBody.position.y + y);

			// Gizmos.color = Color.blue;
            // Gizmos.DrawLine(transform.position, targetLocation);

			RaycastHit2D raycastHit = Physics2D.Linecast(_mosquitoBody.position, targetLocation, EventFlag.BodySkinRaycastLayer); 
			if (raycastHit.collider != null) {
				bodyPoint.Add(raycastHit.point);
			}
		}

		bodyPoint.OrderBy(x=>x.sqrMagnitude);

		return bodyPoint;
	}

	private void RotateHeadToNearstTarget() {
		List<Vector2> allTouchableSkin = FindNearestBodySkin();
		if (allTouchableSkin.Count > 0) {
			Vector3 faceDir = ((Vector3)allTouchableSkin[0] - headObject.transform.position).normalized; 
			headAngle = ((180 / Mathf.PI) * Mathf.Atan2(faceDir.y, faceDir.x)) + 90;

			headObject.rotation = Quaternion.Lerp(headObject.rotation, Quaternion.Euler(0, 0, (headAngle)), 0.3f);
			_camera.SetAnimation(CameraHandler.State.Enlarge, _mosquitoBody, 3);

		} else {
			headObject.rotation = Quaternion.Lerp(headObject.rotation, Quaternion.Euler(Vector3.zero), 0.5f);
			_camera.SetAnimation(CameraHandler.State.Default, _mosquitoBody);
			headAngle = 0;
		}
	}


	public void OnCollisionEnter(Vector2 p_velocity, GameObject p_target) {
		Vector2 maxLandingVelocity = new Vector2(0.5f, 0.5f);

	}
}

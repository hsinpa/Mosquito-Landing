using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MosquitoInput;
using System.Linq;

public class MosquitoHandler : MonoBehaviour {

	[SerializeField]
	private float _speed;

	[SerializeField]
	private float _rotateSpeed;

	private Rigidbody2D _rigidBody;
	private Transform headObject;

	private float defaultAccelerateSpeed = 0.008f;
	private float accelerateSpeed;

	private MosquitoInputManager inputManager;

	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		accelerateSpeed = defaultAccelerateSpeed;

		inputManager = new MosquitoInputManager();

		headObject = transform.Find("head");

	}

	void FixedUpdate() {
		float translation = ( inputManager.IsFrontClick() + 
						((inputManager.IsLeftClick() + inputManager.IsRightClick() == 2) ? 1 : 0) )
						* _speed;

        float rotation = -(inputManager.IsLeftClick() - inputManager.IsRightClick()) * _rotateSpeed;

		VerticalMove(translation);
		HorizontalMove(rotation);
	}

	void Update() {
		RotateHeadToNearstTarget();
	}

	private void VerticalMove(float p_variable) {
		if (p_variable != 0) {
			DirectionalMove(p_variable, transform.up);

			_rigidBody.rotation = Mathf.Lerp(_rigidBody.rotation, 0, 0.1f);
			_rigidBody.angularVelocity = 0;

			//Debug.Log("Magnitude " + _rigidBody.velocity.magnitude);
		} else {
			accelerateSpeed = Mathf.Lerp(accelerateSpeed, defaultAccelerateSpeed, 0.01f);
		}
	}

	private void HorizontalMove(float p_variable) {
		if (p_variable != 0) {
			DirectionalMove(p_variable, transform.right);
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
		float lineRadius = 2.5f;
		float perDegree = 18;
		int totalLine = 20;

		List<Vector2> bodyPoint = new List<Vector2>();

		for (int i = 1; i <= totalLine; i++) {
			var x = lineRadius * Mathf.Cos((perDegree * i  )* Mathf.Deg2Rad);
			var y = lineRadius * Mathf.Sin((perDegree * i )* Mathf.Deg2Rad);

			Vector2 targetLocation = new Vector2( transform.position.x +  x, transform.position.y + y);

			// Gizmos.color = Color.blue;
            // Gizmos.DrawLine(transform.position, targetLocation);

			RaycastHit2D raycastHit = Physics2D.Linecast(transform.position, targetLocation, EventFlag.BodySkinRaycastLayer); 
			if (raycastHit.collider != null) {
				bodyPoint.Add(raycastHit.point);
			}
		}

		bodyPoint.OrderBy(x=>x.magnitude);

		return bodyPoint;
	}

	private void RotateHeadToNearstTarget() {
		List<Vector2> allTouchableSkin = FindNearestBodySkin();
		if (allTouchableSkin.Count > 0) {
			Vector3 faceDir = ((Vector3)allTouchableSkin[0] - headObject.transform.position).normalized; 
			float angle = (180 / Mathf.PI) * Mathf.Atan2(faceDir.y, faceDir.x);

			headObject.rotation = Quaternion.Lerp(headObject.rotation, Quaternion.Euler(0, 0, (angle +90)), 0.3f);
		} else {
			headObject.rotation = Quaternion.Lerp(headObject.rotation, Quaternion.Euler(Vector3.zero), 0.5f);
		}
	}

    // void OnDrawGizmosSelected() {
	// 	FindNearestBodySkin();
    // }

	void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject.layer == 9) {
			Debug.Log(collision.gameObject.name);
			float xLandingPower = Mathf.Abs(_rigidBody.velocity.x);
			float yLandingPower = Mathf.Abs(_rigidBody.velocity.y);

		}
    }
 

}
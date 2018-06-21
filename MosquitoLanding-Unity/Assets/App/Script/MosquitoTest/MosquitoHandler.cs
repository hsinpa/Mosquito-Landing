using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MosquitoHandler : MonoBehaviour {

	[SerializeField]
	private float _speed;

	[SerializeField]
	private float _rotateSpeed;

	private Rigidbody2D _rigidBody;


	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// // Update is called once per frame
	// void Update () {
		
	// }

	void FixedUpdate()
	{
		float translation = Input.GetAxis("Vertical") * _speed;
        float rotation = -Input.GetAxis("Horizontal") * _rotateSpeed;

		if (translation != 0) {
			// _rigidBody.MoveRotation();
			Vector2 newVelocity = ( transform.up * translation * Time.deltaTime );
					newVelocity += _rigidBody.velocity;
			
			if (newVelocity.magnitude <= 4) {
				_rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, newVelocity, 0.05f);
			}

			Debug.Log("Magnitude " + _rigidBody.velocity.magnitude);
		} else {
			_rigidBody.velocity = Vector3.Lerp(_rigidBody.velocity, new Vector2(0, _rigidBody.velocity.y), 0.0001f);
		}

		// Quaternion rotationVelocity = Quaternion.Euler(0 ,0 ,rotation * );
		// _rigidBody.AddTorque(rotation * Time.deltaTime);
		transform.Rotate(new Vector3(0 ,0 ,rotation) * Time.deltaTime);
	}
}

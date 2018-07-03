using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MosquitoInput;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class MosquitoHandler : MonoBehaviour {

	#region Inspector Parameter
    [Header("Movement Setting")]
	[SerializeField]
	private float _speed;
	public float speed {
		get {
			return _speed;
		}
		set {
			_speed = value;
		}
	}

	[SerializeField]
	private float _rotateSpeed;
	public float rotateSpeed {
		get {
			return _rotateSpeed;
		}
		set {
			_rotateSpeed = value;
		}
	}

	private float _collsionResistance = 1.5f;

    [Header("Blood Related Setting")]
	public float _bloodSeekAmount = 0;
	public float _maxPerBloodAmount = 5;
	public int _totalBloodSeekAmount = 20;



	private CameraHandler _camera;
	private Rigidbody2D _rigidBody;

	private MosquitoMovement _mosquitoMovement;
	private MosquitoBloodSuck _mosquitoBloodSucker;

	public delegate void OnStatusChangeEvent(Status p_status);

	public enum Status {
		Idle,
		Landing,
		SuckBlood,
		Dead
	}

	public Status currentStatus {
		get {
			return _currentStatus;
		}
		set {
			
			if (value != _currentStatus) {
				if (OnStatusChange != null)
					OnStatusChange(value);
				_currentStatus = value;
			}
		}
	}
	
	private Status _currentStatus;
	public event OnStatusChangeEvent OnStatusChange;

	#endregion

	// Use this for initialization
	void Start () {
		_rigidBody = GetComponent<Rigidbody2D>();
		_camera = Camera.main.transform.GetComponent<CameraHandler>();
		_mosquitoMovement = new MosquitoMovement(this, _camera);
		_mosquitoBloodSucker = new MosquitoBloodSuck(this, 
		transform.Find("BUG_BODY DOWN").GetComponent<Anima2D.SpriteMeshInstance>());
		
		SetUp();
	}

	public void SetUp() {
		_bloodSeekAmount = 0;
		currentStatus = Status.Idle;
		_rigidBody.angularVelocity = 0;
		_rigidBody.rotation = 0;
	}

	void FixedUpdate() {
		_mosquitoMovement.OnFixedUpdate();
	}

	void Update() {
		
		_mosquitoMovement.OnUpdate();
		_mosquitoBloodSucker.OnUpdate();
	}

	void DeadAnimationHandler(string p_death_style) {
		// switch (p_death_style)
		// {
			
		// }

		currentStatus = Status.Dead;

		StartCoroutine(WaitAndRestart(2));
	}

	void OnCollisionEnter2D(Collision2D collision)
    {
		if (collision.gameObject != this.gameObject && currentStatus != Status.SuckBlood) {
			OnCollisionHandler(collision);
			// currentStatus = Status.SuckBlood;
		}
    }

	void OnCollisionHandler(Collision2D collision) {
		float velocity = collision.relativeVelocity.sqrMagnitude;

		if (velocity > _collsionResistance) {
			DeadAnimationHandler(EventFlag.Death.HitWall);
		}

		if (collision.gameObject.layer == 9) {
			currentStatus = Status.SuckBlood;
		}
		Debug.Log("Landing power : " + velocity);
	}
	

	//Only exist temporarily
	private IEnumerator WaitAndRestart(float waitTime)
    {
		yield return new WaitForSeconds(waitTime);
		transform.position = Vector2.zero;
		SetUp();
    }
}
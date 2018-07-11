using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MosquitoInput;
using System.Linq;

[RequireComponent(typeof(Rigidbody2D))]
public class MosquitoHandler : BaseCharacter
{

    #region Inspector Parameter
    [Header("Movement Setting")]
    [SerializeField]
    private float _speed;
    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
        }
    }

    [SerializeField]
    private float _rotateSpeed;
    public float rotateSpeed
    {
        get
        {
            return _rotateSpeed;
        }
        set
        {
            _rotateSpeed = value;
        }
    }

    private float _collsionResistance = 1f;
    private float _collsionSecResistance = 0.6f;

    [Header("Blood Related Setting")]
    public float _bloodSeekAmount = 0;
    public float _maxPerBloodAmount = 5;
    public int _totalBloodSeekAmount = 20;

    private CameraHandler _camera;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private MosquitoMovement _mosquitoMovement;
    private MosquitoBloodSuck _mosquitoBloodSucker;

    public delegate void OnStatusChangeEvent(Status p_status);

    public enum Status
    {
        Idle,
        Landing,
        SuckBlood,
        Dead
    }

    public Status currentStatus
    {
        get
        {
            return _currentStatus;
        }
        set
        {

            if (value != _currentStatus)
            {
                if (OnStatusChange != null)
                    OnStatusChange(value);
                _currentStatus = value;
            }
        }
    }

    private Status _currentStatus;
    public event OnStatusChangeEvent OnStatusChange;

    private Dictionary<string, MosquitoBodyContainer> bodyContainer = new Dictionary<string, MosquitoBodyContainer>();
    private List<CollisionReporter> _seperableReporters;


    #endregion

    // Use this for initialization
    public void SetUp(CameraHandler p_camera)
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _camera = p_camera;
        
        _mosquitoMovement = new MosquitoMovement(this, _camera);
        _mosquitoBloodSucker = new MosquitoBloodSuck(this,
        transform.Find("BUG_BODY DOWN").GetComponent<Anima2D.SpriteMeshInstance>());

        _seperableReporters = GetComponentsInChildren<CollisionReporter>().ToList().FindAll(x => x.isBreakable);

        foreach (Transform child in transform)
        {
            bodyContainer.Add(child.transform.name, new MosquitoBodyContainer(child.transform, child.localPosition, child.localRotation));
        }

        Init();
    }

    public void Init()
    {		
        ResumeBodyPosition();
		EnableChildRigidCollision(false);

        _bloodSeekAmount = 0;
        currentStatus = Status.Idle;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.angularVelocity = 0;
        _rigidBody.velocity = Vector2.zero;

        _rigidBody.rotation = 0;
        _animator.enabled = true;

		transform.position = Vector2.zero;
        
        _camera.SetAnimation(CameraHandler.State.Default, transform);
    }

    void FixedUpdate()
    {
        _mosquitoMovement.OnFixedUpdate();
    }

    void Update()
    {
        _mosquitoMovement.OnUpdate();
        _mosquitoBloodSucker.OnUpdate();
    }

    public void DeadAnimationHandler(string p_death_style)
    {
        if (currentStatus == Status.Dead) return;

		//Assign explosion power
        float explodePower = 0;
        switch (p_death_style)
        {
            case EventFlag.Death.HitWall:
                explodePower = 300;
                break;

            case EventFlag.Death.OverImbeded:
                explodePower = 500;
                break;

            case EventFlag.Death.Squash:
                explodePower = 100;
                break;
        }

        _animator.enabled = false;
        _rigidBody.bodyType = RigidbodyType2D.Static;

        EnableChildRigidCollision(true);

        foreach (Transform item in transform)
        {
            Vector2 force = (transform.position - (transform.up * 2) - item.position).normalized * explodePower;
            item.GetComponent<Rigidbody2D>().AddForceAtPosition(force, transform.position);
        }

        currentStatus = Status.Dead;

        StartCoroutine(WaitAndRestart(1));
    }

    private void ResumeBodyPosition()
    {
		foreach(KeyValuePair<string, MosquitoBodyContainer> entry in bodyContainer) {
			entry.Value.bodyPart.SetParent(this.transform);
            entry.Value.bodyPart.localPosition = entry.Value.originalLocalPositon;
            entry.Value.bodyPart.localRotation = entry.Value.originalLocalRotation;
		}
    }

    public override void OnCollisionHandler(Collision2D p_collision, Transform p_receiver)
    {
        if (p_collision.gameObject != this.gameObject && p_collision.gameObject.layer != 10
            && currentStatus == Status.Idle)
        {

            base.OnCollisionHandler(p_collision, p_receiver);

            float velocity = p_collision.relativeVelocity.sqrMagnitude;

            Debug.Log(velocity);

            if (velocity > _collsionSecResistance && velocity < _collsionResistance)
            {
                CollisionReporter seperableObject = _seperableReporters.Find(x => !x.IsBreakUp() &&
                                                   x.gameObject == p_receiver.gameObject);
                if (seperableObject != null)
                {
                    seperableObject.SeperateFromMainBody();
                }
                else
                {
                    DeadAnimationHandler(EventFlag.Death.HitWall);
                }

            }
            else if (velocity >= _collsionResistance)
            {
                DeadAnimationHandler(EventFlag.Death.HitWall);
            }

            if (p_collision.gameObject.layer == 9)
            {
                currentStatus = Status.SuckBlood;
            }

            // Debug.Log("Landing power : " + velocity);
        }
    }

    private void EnableChildRigidCollision(bool p_enable)
    {
        foreach (Transform item in transform)
        {
            Rigidbody2D itemRigid = item.GetComponent<Rigidbody2D>();
            if (!p_enable)
            {
                if (itemRigid != null) GameObject.Destroy(itemRigid);
            }
            else if (p_enable && itemRigid == null)
            {
                itemRigid = item.gameObject.AddComponent<Rigidbody2D>();
				itemRigid.drag = 0.3f;
            }

            Collider2D boxCollider = item.GetComponent<Collider2D>();
            if (boxCollider == null) boxCollider = item.gameObject.AddComponent<BoxCollider2D>();
            if (boxCollider.GetComponent<CollisionReporter>() == null) boxCollider.enabled = p_enable;
        }
    }

    //Only exist temporarily
    private IEnumerator WaitAndRestart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        float bloodEmbide = _bloodSeekAmount / _totalBloodSeekAmount;

        int brokenPartCount = _seperableReporters.Count(x=>x.IsBreakUp());

        MainApp.Instance.subject.notify(EventFlag.Game.GameEnd, bloodEmbide, brokenPartCount);
        // Init();
    }
}
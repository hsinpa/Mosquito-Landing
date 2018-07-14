using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MosquitoInput;
using System.Linq;
using UnityEngine.UI;

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

    private float _collsionResistance = 1.5f;
    private float _collsionSecResistance = 1f;

    [Header("Blood Related Setting")]
    public float _bloodSeekAmount = 0;
    public float _maxPerBloodAmount = 5;
    public int _totalBloodSeekAmount = 20;
    private float _bloodAmountPercentage {
        get {
            return (_bloodSeekAmount / _totalBloodSeekAmount);
        }
    }

    private CameraHandler _camera;
    private Rigidbody2D _rigidBody;
    private Animator _animator;

    private MosquitoMovement _mosquitoMovement;
    private MosquitoBloodSuck _mosquitoBloodSucker;
    private SpawnPoint _spawnPoint;

    public delegate void OnStatusChangeEvent(Status p_status);
    private Coroutine _assignWaitRestartCoroutine;
    public enum Status
    {
        Idle,
        Landing,
        SuckBlood,
        End
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
                if (_assignWaitRestartCoroutine != null)
                    // StopCoroutine(_assignWaitRestartCoroutine);

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
    private Vector3 _originalBugScaleSize;
    private Text _subtitleText;
    private TextEffect.TypeWriter _typeWriter;

    public SoundModel soundModel;
    public AudioSource audioSource;

    #endregion

    // Use this for initialization
    public void SetUp(CameraHandler p_camera, SpawnPoint p_spawnPoint, SoundModel p_soundModel)
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _camera = p_camera;
        _spawnPoint = p_spawnPoint;
        _originalBugScaleSize = transform.localScale;

        soundModel = p_soundModel;
        audioSource = GetComponent<AudioSource>();
        _mosquitoMovement = new MosquitoMovement(this, _camera);
        _mosquitoBloodSucker = new MosquitoBloodSuck(this,
        transform.Find("BUG_BODY DOWN").GetComponent<Anima2D.SpriteMeshInstance>());

        _seperableReporters = GetComponentsInChildren<CollisionReporter>().ToList().FindAll(x => x.isBreakable);

        foreach (Transform child in transform)
        {
            bodyContainer.Add(child.transform.name, new MosquitoBodyContainer(child.transform, child.localPosition, child.localRotation));
        }

        _camera.ForceAlignWithTarget(transform);

        _subtitleText = transform.Find("world_ui/subtitle").GetComponent<Text>();
        _typeWriter = GetComponentInChildren<TextEffect.TypeWriter>();
        _typeWriter.AddMessage(_subtitleText, "OK, I'm in");
        Init();
    }

    public void Init()
    {
        transform.localScale = _originalBugScaleSize;

        ResumeBodyPosition();
		EnableChildRigidCollision(false);

        _bloodSeekAmount = 0;
        currentStatus = Status.Idle;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;
        _rigidBody.angularVelocity = 0;
        _rigidBody.velocity = Vector2.zero;

        _rigidBody.rotation = 0;

        _animator.enabled = true;
        transform.rotation = Quaternion.identity;
		transform.position = _spawnPoint.transform.position;
        _camera.SetAnimation(CameraHandler.State.Default, transform);

        PlayAudio(soundModel.GetClip(EventFlag.Audio.MosquitoFlying), true);
    }

    void FixedUpdate()
    {
        _mosquitoMovement.OnFixedUpdate();
    }

    void Update()
    {
        _mosquitoMovement.OnUpdate();
        _mosquitoBloodSucker.OnUpdate();

        //Shrink mosquito
        if (currentStatus == Status.Landing) {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.08f);   
        }
    }

    public void DeadAnimationHandler(string p_death_style)
    {
        if (currentStatus == Status.End) return;

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

        PlayAudio(soundModel.GetClip(
            Utility.UtilityGroup.RollDice() == 0 ? EventFlag.Audio.Crash1 : EventFlag.Audio.Crash2),
            false
        );

        currentStatus = Status.End;
        _camera.SetAnimation(CameraHandler.State.Default, transform);
        StartCoroutine(WaitAndRestart(2));
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
        // _mosquitoCollision.OnCollision(p_collision, p_receiver);
        if (p_collision.gameObject == this.gameObject || p_collision.gameObject.layer == EventFlag.mosquitoLayer ||
             this._currentStatus == Status.End) return;
        float velocity = p_collision.relativeVelocity.sqrMagnitude;
        //Debug.Log(velocity);

        //If exit point, then game end here
        if (CheckIfGameComplete() && p_collision.gameObject.layer == EventFlag.exitLayer && 
            p_collision.gameObject.GetComponent<ExitPoint>() != null ) {
            if (currentStatus != Status.Landing) {
                _assignWaitRestartCoroutine = StartCoroutine(WaitAndRestart(0.8f));
                currentStatus = Status.Landing;
            }
            return;
        }

        //Broke leg or something
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
        //Die
        else if (velocity >= _collsionResistance)
        {
            DeadAnimationHandler(EventFlag.Death.HitWall);
        }

        if (currentStatus != Status.End) {
            switch (p_collision.gameObject.layer)
            {
                case EventFlag.humanBodyLayer : {
                    currentStatus = Status.SuckBlood;
                    _mosquitoBloodSucker.SetTarget(p_collision.transform);
                }break;

                case EventFlag.harmfulLayer : {
                    Debug.Log("Die harmful");

                    DeadAnimationHandler(EventFlag.Death.Squash);
                }break;
            }
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

    /// <summary>
    /// Check if player has accomplish the minimum requirement to pass game
    /// </summary>
    /// <returns></returns>
    private bool CheckIfGameComplete() {
        return (_bloodAmountPercentage >= GameModel.winCondition);
    }

    //Only exist temporarily
    private IEnumerator WaitAndRestart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (_currentStatus == Status.End || _currentStatus == Status.Landing) {
            float bloodEmbide = _bloodSeekAmount / _totalBloodSeekAmount;

            int brokenPartCount = _seperableReporters.Count(x=>x.IsBreakUp());

            if (_assignWaitRestartCoroutine != null) {
                StopCoroutine(_assignWaitRestartCoroutine);
                _assignWaitRestartCoroutine = null;
            }

            _currentStatus = Status.End;
            MainApp.Instance.subject.notify(EventFlag.Game.GameEnd, bloodEmbide, brokenPartCount);
        }
    }

    public void PlayAudio(AudioClip p_clip, bool p_isLoop) {
        audioSource.clip = p_clip;
        audioSource.Play();
        audioSource.loop = p_isLoop;
    }

}
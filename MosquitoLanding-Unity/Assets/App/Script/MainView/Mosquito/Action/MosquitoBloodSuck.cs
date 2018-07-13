using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Anima2D;

public class MosquitoBloodSuck {

	private MosquitoHandler _mosquito;

	private float timer;
	private float waitUntilTime;

	private float bloodSeekAmount = 0, bloodSeekPower = 5;

	private float maxFatSize = 2.5f, maxSpeedDiminish = 150;

	private SpriteMeshInstance _bodySprite;

	private float d_speed, d_rotate_speed;

	private Transform imbibeTarget;
	private Vector2 distanceBetweenTarget;

	public MosquitoBloodSuck(MosquitoHandler p_mosquito, SpriteMeshInstance p_body_sprite) {
		_mosquito = p_mosquito;
		_bodySprite = p_body_sprite;

		d_speed = p_mosquito.speed;
		d_rotate_speed = p_mosquito.rotateSpeed;

		_bodySprite.transform.localScale = Vector3.one;
		_mosquito.OnStatusChange += OnStatusChange;
	}

	public void SetTarget(Transform p_target) {
		imbibeTarget = p_target;
		distanceBetweenTarget = p_target.position - _mosquito.transform.position;
	}

	public void OnStatusChange(MosquitoHandler.Status p_status) {
		switch (p_status)
		{
			case MosquitoHandler.Status.SuckBlood:
				bloodSeekAmount = 0;
				ResetTimer();
			break;

			case MosquitoHandler.Status.End:

			break;
		}
	}

	public void OnUpdate() {
		SetBodyVisionEffect( _mosquito._bloodSeekAmount / _mosquito._totalBloodSeekAmount );

		if (_mosquito.currentStatus != MosquitoHandler.Status.SuckBlood) return;

		//imbibe too much blood.
		if (_mosquito._bloodSeekAmount >= _mosquito._totalBloodSeekAmount) {
			_mosquito.DeadAnimationHandler(EventFlag.Death.OverImbeded);		
			return;
		}

		if (imbibeTarget != null) {
			_mosquito.transform.position = new Vector2(imbibeTarget.position.x-distanceBetweenTarget.x ,_mosquito.transform.position.y);
		}

		float newAddedBlood = bloodSeekPower * Time.deltaTime * 0.4f;
		_mosquito._bloodSeekAmount += newAddedBlood;
	}

	public void SetBodyVisionEffect(float p_bloody_value) {
		_bodySprite.color = new Color(1, 1- p_bloody_value, 1-p_bloody_value);
		_bodySprite.transform.localScale = new Vector3( 1 + (maxFatSize * p_bloody_value),
											1, 1);

		_mosquito.speed = d_speed - (maxSpeedDiminish * p_bloody_value);
		_mosquito.rotateSpeed = d_rotate_speed - (maxSpeedDiminish * p_bloody_value);
	}

	public void ResetTimer(float p_append_time = 10) {
		timer = Time.time;
		waitUntilTime = timer + p_append_time;
	}

}
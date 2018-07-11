﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private SpriteRenderer bgSprite;

    private Vector3 defaultPosition = new Vector3(0, 0, -10);
    private float defaultCameraSize = 5;

    private Camera _camera;

    private Transform targetTransform;
    private float targetCameraSize;

    public enum State
    {
        Enlarge,
        Default,
        Idle
    }

    private State _currentState;

    private float _max_top, _max_bottom, _max_left, _max_right;
    private float _camera_height, _camera_width;

    // Use this for initialization
    public void SetUp(SpriteRenderer p_background)
    {
        bgSprite = p_background;

        _camera = GetComponent<Camera>();
		_currentState = State.Default;

        _max_top = bgSprite.size.y * 0.5f;
        _max_bottom = -_max_top;

        _max_right = bgSprite.size.x * 0.5f;
        _max_left = -_max_right;

        // float width = height * Screen.width / Screen.height;


        // Debug.Log("Top " + height +", Bottom " + (-height));
    }

    public void SetAnimation(State p_state, Transform p_target = null, float p_camera_size = 0)
    {
        targetTransform = p_target;
        targetCameraSize = p_camera_size;

        _camera_height = 2f * Camera.main.orthographicSize * 0.5f;
        _camera_width = _camera_height * Camera.main.aspect;

        _currentState = p_state;
    }

    private Vector3 RecalculateCameraPosition(Vector3 p_target_position)
    {
        //Calculate For Y
        float y = p_target_position.y;

        if (p_target_position.y + _camera_height > _max_top)
        {
            y = _max_top - _camera_height;
        }
        else if (p_target_position.y - _camera_height < _max_bottom)
        {
            y = _max_bottom + _camera_height;
        }

        //Calculate For X
        float x = p_target_position.x;

        if (p_target_position.x + _camera_width > _max_right)
        {
            x = _max_right - _camera_width;
        }
        else if (p_target_position.x - _camera_width < _max_left)
        {
            x = _max_left + _camera_width;
        }

        return new Vector3(x, y, p_target_position.z);
    }

    private bool PlayAnimation(Vector3 p_target_position, float p_camera_size)
    {
        Vector3 tempCameraPosition = Vector3.Lerp(transform.position, p_target_position, 0.06f);
        transform.position = RecalculateCameraPosition(tempCameraPosition);

        float tempCameraSize = Mathf.Lerp(_camera.orthographicSize, p_camera_size, 0.06f);
        _camera.orthographicSize = tempCameraSize;

        return ((p_target_position - transform.position).magnitude < 0.1f && tempCameraSize < 0.1f);
    }

    private void AnimationHandler()
    {

        bool IsAnimationFinish = false;
        switch (_currentState)
        {
            case State.Default:
                IsAnimationFinish = PlayAnimation(new Vector3(targetTransform.position.x, targetTransform.position.y, -10),
                                    defaultCameraSize);
                break;

            case State.Enlarge:
                IsAnimationFinish = PlayAnimation(new Vector3(targetTransform.position.x, targetTransform.position.y, -10),
                                    targetCameraSize);
                break;
        }

        if (IsAnimationFinish) _currentState = State.Idle;
    }


    // Update is called once per frame
    void Update()
    {
        if (_currentState != State.Idle && _camera != null)
        {

            AnimationHandler();
        }
    }

}
using System.Collections;
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
    // private float _camera_height, _camera_width;

    // Use this for initialization
    public void SetUp(SpriteRenderer p_background)
    {
        bgSprite = p_background;

        _camera = GetComponent<Camera>();
        _currentState = State.Default;

        _max_top = bgSprite.bounds.size.y * 0.5f;
        _max_bottom = -_max_top;

        _max_right = bgSprite.bounds.size.x * 0.5f;
        _max_left = -_max_right;

        // float width = height * Screen.width / Screen.height;


        // Debug.Log("Top " + height +", Bottom " + (-height));
    }

    public void ForceAlignWithTarget(Transform p_target)
    {
        if (p_target != null) _camera.transform.position = p_target.position;
    }

    public void SetAnimation(State p_state, Transform p_target = null, float p_camera_size = 0)
    {
        targetTransform = p_target;
        targetCameraSize = p_camera_size;

        // _camera_height = Camera.main.orthographicSize;
        // _camera_width = _camera_height * Camera.main.aspect;

        _currentState = p_state;
    }

    private Vector3 RecalculateCameraPosition(Vector3 p_target_position, float camera_height, float camera_width)
    {
        //Calculate For Y
        float y = p_target_position.y;

        if (p_target_position.y + camera_height >= _max_top)
        {
            y = _max_top - camera_height;
        }
        else if (p_target_position.y - camera_height <= _max_bottom)
        {
            y = _max_bottom + camera_height;
        }

        //Calculate For X
        float x = p_target_position.x;

        if (p_target_position.x + camera_width >= _max_right)
        {
            x = _max_right - camera_width;
        }
        else if (p_target_position.x - camera_width <= _max_left)
        {
            x = _max_left + camera_width;
        }

        return new Vector3(x, y, p_target_position.z);
    }

    private bool PlayAnimation(Vector3 p_target_position, float p_camera_size)
    {
        float tempCameraSize = Mathf.Lerp(_camera.orthographicSize, p_camera_size, 0.06f);
        _camera.orthographicSize = tempCameraSize;


        Vector3 tempCameraPosition = Vector3.Lerp(transform.position, p_target_position, 0.06f);
        transform.position = RecalculateCameraPosition(tempCameraPosition, tempCameraSize, tempCameraSize * Camera.main.aspect);

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

        // if (IsAnimationFinish) _currentState = State.Default;
    }


    // Update is called once per frame
    void Update()
    {

        AnimationHandler();

    }

}

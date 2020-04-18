using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;

public class CameraControl : MonoBehaviour
{
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _mouseVelocity = Vector2.zero;

    private float _zoomVelocity = 0;
    private Camera _camera;

    private const float MouseOffset = 200;

    private void Start()
    {
        _camera = GetComponent<Camera>();
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _velocity = Vector3.zero;
            return;
        }

        _velocity = context.ReadValue<Vector2>();
    }

    public void Zoom(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _zoomVelocity = 0;
            return;
        }

        var value = context.ReadValue<float>();

        _zoomVelocity = value < 0 ? -1 : value > 0 ? 1 : 0;
    }

    private void Update()
    {
        _mouseVelocity = Vector2.zero;

        var mouse = Input.mousePosition;

        if (mouse.x < MouseOffset)
        {
            _mouseVelocity.x = -1;
        }

        if (mouse.x > Screen.width - MouseOffset)
        {
            _mouseVelocity.x = 1;
        }

        if (mouse.y < MouseOffset)
        {
            _mouseVelocity.y = -1;
        }

        if (mouse.y > Screen.height - MouseOffset)
        {
            _mouseVelocity.y = 1;
        }
    }

    private void FixedUpdate()
    {
        var position = transform.position;
        var velocity = (_velocity + _mouseVelocity).normalized;

        position = Vector3.Lerp(
            position,
            position + new Vector3(velocity.x, 0, velocity.y) * 40,
            Time.fixedDeltaTime
        );
        
        transform.position = position;

        _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView + _zoomVelocity, 30, 100);
    }
}
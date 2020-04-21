using UnityEngine;
using UnityEngine.InputSystem;

public class CameraControl : MonoBehaviour
{
    private Vector2 _velocity = Vector2.zero;
    private Vector2 _mouseVelocity = Vector2.zero;

    private const float MouseOffset = 200;
    
    public void Move(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _velocity = Vector3.zero;
            return;
        }

        _velocity = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        _mouseVelocity = Vector2.zero;

        var mouse = Input.mousePosition;

        return;
        
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
            position + new Vector3(velocity.x, 0, velocity.y) * (TimeCheat.Instance.IsFastMode() ? 20 : 40),
            Time.fixedDeltaTime
        );

        transform.position = position;
    }
}
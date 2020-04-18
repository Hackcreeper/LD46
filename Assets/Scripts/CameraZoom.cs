using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    private float _velocity = 0f;
    private float _zoomLevel = 0;

    private const float Speed = 20f;

    public void Zoom(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            _velocity = 0;
            return;
        }

        var value = context.ReadValue<float>();

        _velocity = value < 0 ? -1 : value > 0 ? 1 : 0;
    }

    private void FixedUpdate()
    {
        var min = 0; // -10f / 20f * Speed
        var max = 35f / 20f * Speed;

        _zoomLevel = Mathf.Clamp(_zoomLevel + _velocity, min, max);

        var transform1 = transform;

        var position = transform1.forward * _zoomLevel * Speed * Time.fixedDeltaTime;
        transform1.localPosition = position;

        // level / max
        transform.localRotation = Quaternion.Euler(
            Mathf.Lerp(60, 45, _zoomLevel / max),
            0,
            0
        );
    }
}
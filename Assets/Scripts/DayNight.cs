using UnityEngine;

public class DayNight : MonoBehaviour
{
    public static DayNight Instance;

    private float _speed = 1f;
    private float _rotation = 90f;
    private bool _isDay = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        _rotation += Time.deltaTime / 1.5f * _speed;
        if (_rotation >= 360f)
        {
            _rotation = 0;
        }

        if (_rotation >= 180 && _isDay)
        {
            _isDay = false;
        } else if (_rotation <= 180 && !_isDay)
        {
            _isDay = true;
        }
        
        transform.rotation = Quaternion.Euler(_rotation, 0, 0);
    }

    public bool IsDay() => _isDay;
}
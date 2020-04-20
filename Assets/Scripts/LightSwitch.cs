using UnityEngine;

public class LightSwitch : MonoBehaviour
{
    private Light _light;

    private void Start()
    {
        _light = GetComponent<Light>();
    }
    
    private void Update()
    {
        _light.enabled = !DayNight.Instance.IsDay();
    }
}
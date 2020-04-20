using System;
using UnityEngine;

public class TimeCheat : MonoBehaviour
{
    public int currentFactor = 1;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            currentFactor *= 2;
            Time.timeScale = currentFactor;
        }
        
        if (Input.GetKeyDown(KeyCode.J))
        {
            currentFactor = Mathf.Clamp(currentFactor / 2, 1, Int32.MaxValue);
            Time.timeScale = currentFactor;
        }
    }
}
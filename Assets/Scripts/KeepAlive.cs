using System;
using UnityEngine;

public class KeepAlive : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
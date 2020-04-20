using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class GameOverRocket : MonoBehaviour
{
    public float speed = 10;
    public float moveSpeed = 10;
    public Transform model;

    private float _zSpeed, _ySpeed;

    private void Start()
    {
        _ySpeed = Random.Range(0, 100) <= 50 ? -speed : speed;
        _zSpeed = Random.Range(0, 100) <= 50 ? -speed : speed;
    }
    
    private void Update()
    {
        model.Rotate(new Vector3(0, _ySpeed * Time.deltaTime, _zSpeed * Time.deltaTime), Space.Self);
        
        transform.Translate(0, moveSpeed * Time.deltaTime, 0, Space.Self);
    }
}
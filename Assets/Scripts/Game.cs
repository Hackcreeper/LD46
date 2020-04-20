using System;
using Buildings;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance;
    
    public GameObject landingPlatformPrefab;

    private Building _landingPlatform;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        var platform = Instantiate(landingPlatformPrefab);
        var building = platform.GetComponent<Building>();
        BuildingRegistry.Instance.Register(building);

        _landingPlatform = building;
    }

    public Building GetLandingPlatform() => _landingPlatform;
}
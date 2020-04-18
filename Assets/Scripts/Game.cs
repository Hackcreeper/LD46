using Buildings;
using UnityEngine;

public class Game : MonoBehaviour
{
    public GameObject landingPlatformPrefab;
    
    private void Start()
    {
        var platform = Instantiate(landingPlatformPrefab);
        BuildingRegistry.Instance.Register(platform.GetComponent<Building>());
    }
}
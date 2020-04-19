using System.Collections.Generic;
using Buildings;
using Resource;
using UnityEngine;

public class ColonistManager : MonoBehaviour
{
    public GameObject colonistPrefab;

    private List<Colonist> _colonists = new List<Colonist>();
    
    public void Start()
    {
        // spawn 5 colonists
        for (var i = 0; i < 5; i++)
        {
            // SpawnColonist();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Spawing colonist!");
            SpawnColonist();
        }
    }

    public void SpawnColonist()
    {
        var colonist = Instantiate(colonistPrefab);
        colonist.transform.position = new Vector3(
            Random.Range(-3f, 3f),
            2f,
            Random.Range(-3f, 3f)
        );
            
        colonist.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        var component = colonist.GetComponent<Colonist>();
        
        _colonists.Add(component);
        component.taskCompleted += TaskCompleted;
        component.SetCurrentBuilding(BuildingRegistry.Instance.GetAll()[0]);

        ResourceManager.Instance.ForType(ResourceType.Colonists).Increase(1);
        
        AssignRandomTaskTo(component);
    }

    private void AssignRandomTaskTo(Colonist colonist)
    {
        var building = BuildingRegistry.Instance.GetRandom();
        colonist.SetTarget(building);
    }

    private void TaskCompleted(Colonist colonist)
    {
        Debug.Log("TODO: Assign new task to colonist: ");
    }
}
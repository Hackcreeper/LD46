using System.Collections.Generic;
using Buildings;
using Resource;
using UnityEngine;

public class ColonistManager : MonoBehaviour
{
    public GameObject colonistPrefab;
    public LayerMask obstacleMask;
    public LayerMask buildingMask;

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

        var i = 0;
        while (true)
        {
            Debug.Log($"Try {++i}");
            
            colonist.transform.position = new Vector3(
                building.transform.position.x - 2.5f + Random.Range(0, 5f), 
                0.72f, 
                building.transform.position.z - 2.5f + Random.Range(0, 5f)
            );

            var size = colonist.GetComponent<BoxCollider>().bounds.extents;

            if (!Physics.CheckBox(colonist.transform.position, size, Quaternion.identity, obstacleMask))
            {
                break;
            }
        }
    }

    private void TaskCompleted(Colonist colonist)
    {
        Debug.Log("TODO: Assign new task to colonist: ");
    }
}
using System.Collections.Generic;
using Buildings;
using Resource;
using UnityEngine;

namespace Colonists
{
    public class ColonistManager : MonoBehaviour
    {
        public GameObject colonistPrefab;
        public LayerMask obstacleMask;
        public LayerMask buildingMask;

        private readonly List<Colonist> _colonists = new List<Colonist>();
    
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
                SpawnColonist();
            }
        }

        public void SpawnColonist()
        {
            var colonist = Instantiate(colonistPrefab);
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

            var buildingSize = building.GetComponent<BoxCollider>().bounds.size;

            var i = 0;
            while (true)
            {
                var halfX = (buildingSize.x - 1f) / 2f;
                var halfZ = (buildingSize.z - 1f) / 2f;
            
                colonist.transform.position = new Vector3(
                    building.transform.position.x - halfX + Random.Range(0, buildingSize.x - 1f), 
                    0.72f, 
                    building.transform.position.z - halfZ + Random.Range(0, buildingSize.z - 1f)
                );

                var colonistSize = colonist.GetComponent<BoxCollider>().bounds.extents;

                if (!Physics.CheckBox(colonist.transform.position, colonistSize, Quaternion.identity, obstacleMask))
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
}
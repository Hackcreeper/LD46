using System.Collections.Generic;
using System.Linq;
using Buildings;
using Resource;
using UnityEngine;

namespace Colonists
{
    public class ColonistManager : MonoBehaviour
    {
        public static ColonistManager Instance;
        
        public GameObject colonistPrefab;
        public LayerMask obstacleMask;
        public LayerMask buildingMask;

        private readonly List<Colonist> _unemployedColonists = new List<Colonist>();

        private void Awake()
        {
            Instance = this;
        }
        
        public void SpawnColonist()
        {
            var colonist = Instantiate(colonistPrefab);
            colonist.transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            var component = colonist.GetComponent<Colonist>();
        
            _unemployedColonists.Add(component);
            ResourceManager.Instance.ForType(ResourceType.Colonists).Increase(1);
        
            AssignRandomSleepQuarter(component);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U))
            {
                MissingResources.Instance.Died("markus", 0, 0);
            }
        }

        private void AssignRandomSleepQuarter(Colonist colonist)
        {
            var building = BuildingRegistry.Instance.GetRandomSleepingQuarter();
            AssignBuilding(colonist, building);
        }
        
        private void AssignBuilding(Colonist colonist, Building building)
        {
            colonist.SetTarget(building);

            var buildingSize = building.GetComponent<BoxCollider>().bounds.extents;

            while (true)
            {
                var halfX = (buildingSize.x + 1f) / 2f;
                var halfZ = (buildingSize.z + 1f) / 2f;
            
                colonist.transform.position = new Vector3(
                    building.transform.position.x - halfX + Random.Range(0, buildingSize.x + 1f), 
                    0.72f, 
                    building.transform.position.z - halfZ + Random.Range(0, buildingSize.z + 1f)
                );

                var colonistSize = colonist.GetComponent<BoxCollider>().bounds.extents;

                if (!Physics.CheckBox(colonist.transform.position, colonistSize, Quaternion.identity, obstacleMask))
                {
                    break;
                }
            }
        }

        public int GetUnemployedAmount() => _unemployedColonists.Count;

        public void RequestColonist(Building building)
        {
            var colonist = _unemployedColonists.First();
            _unemployedColonists.Remove(colonist);
            
            AssignBuilding(colonist, building);
            building.AddColonist(colonist);
        }

        public void SetUnemployed(Colonist colonist)
        {
            _unemployedColonists.Add(colonist);
            AssignRandomSleepQuarter(colonist);
        }
    }
}
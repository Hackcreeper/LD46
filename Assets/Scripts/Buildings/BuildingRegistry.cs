using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Buildings
{
    public class BuildingRegistry : MonoBehaviour
    {
        public static BuildingRegistry Instance;

        private readonly List<Building> _buildings = new List<Building>();

        private void Awake()
        {
            Instance = this;
        }

        public void Register(Building building)
        {
            _buildings.Add(building);
        }

        public Building[] GetAll() => _buildings.ToArray();

        public Building[] GetInRange(Vector3 center, float range)
        {
            return _buildings
                .Where(building => Vector3.Distance(building.transform.position, center) <= range)
                .ToArray();
        }

        public Building GetRandom()
        {
            return _buildings[Random.Range(0, _buildings.Count)];
        }

        public Building GetRandomSleepingQuarter()
        {
            var quarters = _buildings.Where(building => building.buildingType == BuildingType.SleepQuarter).ToArray();

            if (quarters.Length == 0)
            {
                return Game.Instance.GetLandingPlatform();
            }

            return quarters[Random.Range(0, quarters.Length)];
        }
    }
}
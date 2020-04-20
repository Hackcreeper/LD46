using System.Collections.Generic;
using System.Linq;
using Colonists;
using Effects;
using Resource;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Buildings
{
    public class Building : MonoBehaviour
    {
        private static bool _isFirst = true;
        
        public BuildingType buildingType;
        public int maxColonists;
        public string title;
        public string description;
        public bool hasModal;
        
        private int _activePopups = 0;
        private ProducesResource[] _producer;
        private readonly List<Colonist> _colonists = new List<Colonist>();
        
        private void Start()
        {
            _producer = GetComponents<ProducesResource>();

            foreach (var producer in _producer)
            {
                producer.SetBuilding(this);
                producer.SetColonists();
            }
            
            if (buildingType == BuildingType.SleepQuarter && _isFirst)
            {
                ColonistManager.Instance.SpawnColonist();
                ColonistManager.Instance.SpawnColonist();
                ColonistManager.Instance.SpawnColonist();
                ColonistManager.Instance.SpawnColonist();

                ResourceManager.Instance.SpawnPopup(this).Set(ResourceType.Colonists, 4);
                _isFirst = false;
            }
        }

        public float AddPopup()
        {
            return .2f * _activePopups++;
        }

        public void RemovePopup()
        {
            --_activePopups;
        }

        public void OnMouseDown()
        {
            if (!hasModal || EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            
            BuildingRegistry.Instance.OpenModal(this);
        }

        public int GetAssignedColonists() => _colonists.Count;

        public ProducesResource[] GetProducer() => _producer;

        public void ChangeColonistAmount(int newAmount)
        {
            var difference = newAmount - GetAssignedColonists();
            if (difference == 0)
            {
                return;
            }
            
            if (difference > 0)
            {
                for (var i = 0; i < difference; i++)
                {
                    ColonistManager.Instance.RequestColonist(this);
                }
                
                return;
            }
            
            for (var i = 0; i < Mathf.Abs(difference); i++)
            {
                var colonist = _colonists.First();
                _colonists.Remove(colonist);
                
                ColonistManager.Instance.SetUnemployed(colonist);
            }
        }

        public void AddColonist(Colonist colonist)
        {
            _colonists.Add(colonist);

            foreach (var producer in _producer)
            {
                producer.SetColonists();
            }
        }

        public void RemoveColonist(Colonist colonist)
        {
            _colonists.Remove(colonist);
            
            foreach (var producer in _producer)
            {
                producer.SetColonists();
            }
        }
    }
}
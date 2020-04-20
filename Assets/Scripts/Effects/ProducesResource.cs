using System;
using System.Linq;
using Buildings;
using Resource;
using UnityEngine;

namespace Effects
{
    [Serializable]
    public struct ProduceAmount
    {
        public int colonists;
        public int amount;
        public int interval;
    }
    
    public class ProducesResource : MonoBehaviour
    {
        public ResourceType resourceType;
        public ProduceAmount[] amounts;
        
        private Building _building;
        private float _timer = 0f;
        private bool _active = false;

        public void SetBuilding(Building building)
        {
            _building = building;
        }
        
        public void SetColonists()
        {
            if (GetAmount().amount == 0)
            {
                _active = false;
                return;
            }

            _timer = GetAmount().interval;
            _active = true;
        }

        private ProduceAmount GetAmount()
        {
            return GetAmountFor(_building.GetAssignedColonists());
        }

        public ProduceAmount GetAmountFor(int colonists)
        {
            return amounts.Where(amount => amount.colonists == colonists).FirstOrDefault();
        }

        private void Update()
        {
            if (!_active)
            {
                return;
            }
            
            _timer -= Time.deltaTime;
            if (_timer > 0)
            {
                return;
            }

            var amount = GetAmount().amount;
            
            ResourceManager.Instance.ForType(resourceType).Increase(amount);
            ResourceManager.Instance.SpawnPopup(GetComponent<Building>()).Set(resourceType, amount);
            
            _timer = GetAmount().interval;
        }
    }
}
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
        public bool requiresDaylight;
        public int requiredEnergy;
        public int requiredEnergyInterval;
        
        private Building _building;
        private float _timer = 0f;
        private float _energyTimer = 0f;
        private bool _active = false;
        private bool _noEnergy = false;

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
            if (!_active || (requiresDaylight && !DayNight.Instance.IsDay()))
            {
                return;
            }
            
            _timer -= Time.deltaTime;
            _energyTimer -= Time.deltaTime;

            if (_energyTimer <= 0)
            {
                if (!ResourceManager.Instance.ForType(ResourceType.Energy).Decrease(requiredEnergy))
                {
                    if (!_noEnergy)
                    {
                        MissingResources.Instance.ReportEnergy();
                    }
                    
                    _noEnergy = true;
                }
                else
                {
                    if (_noEnergy)
                    {
                        MissingResources.Instance.ClearReportEnergy();
                    }
                    _noEnergy = false;
                }

                _energyTimer = requiredEnergyInterval;
            }
            
            if (_timer > 0 || _noEnergy)
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
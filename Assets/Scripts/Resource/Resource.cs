using System;
using UnityEngine;

namespace Resource
{
    public class Resource
    {
        private int _amount = 0;
        private int _max = 0;

        private bool _infinite = false;

        public Resource(int max, bool infinite = false)
        {
            _max = max;
            _infinite = infinite;
        }

        public int Get() => _amount;
        
        public int GetMax() => _max;

        public override string ToString()
        {
            if (_infinite)
            {
                return $"{_amount}";
            }
            
            return $"{_amount} / {_max}";
        }

        public void Increase(int amount)
        {
            if (_infinite)
            {
                _amount += amount;
                return;
            }
            
            _amount = Mathf.Clamp(_amount + amount, 0, _max);
        }
        
        public bool Decrease(int amount)
        {
            if (_amount - amount > 0)
            {
                _amount -= amount;
                return true;
            }
            
            _amount = Mathf.Clamp(_amount - amount, 0, Int32.MaxValue);
            return false;
        }

        public void IncreaseCapacity(int amount)
        {
            _max += amount;
        }
    }
}
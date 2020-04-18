using Buildings;
using Resource;
using UnityEngine;

namespace Effects
{
    public class ProducesResource : MonoBehaviour
    {
        public ResourceType resourceType;
        public float interval;
        public int amount;

        private float _timer = 0f;

        private void Start()
        {
            _timer = interval;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer > 0)
            {
                return;
            }

            ResourceManager.Instance.ForType(resourceType).Increase(amount);
            ResourceManager.Instance.SpawnPopup(GetComponent<Building>()).Set(resourceType, amount);
            
            _timer = interval;
        }
    }
}
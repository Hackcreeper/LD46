using Resource;
using UnityEngine;

namespace Effects
{
    public class IncreaseCapacity : MonoBehaviour
    {
        public ResourceType resourceType;
        public int amount;

        public void Awake()
        {
            ResourceManager.Instance.ForType(resourceType).IncreaseCapacity(amount);
        }
    }
}
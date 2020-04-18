using Resource;
using UnityEngine;

namespace Effects
{
    public class IncreaseCapacity : MonoBehaviour
    {
        public ResourceType resourceType;
        public int amount;

        public void Start()
        {
            ResourceManager.Instance.ForType(resourceType).IncreaseCapacity(amount);
        }
    }
}
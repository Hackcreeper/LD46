using Resource;
using UnityEngine;

namespace Effects
{
    public class GivesResource : MonoBehaviour
    {
        public ResourceType resourceType;
        public int amount;

        public void Start()
        {
            ResourceManager.Instance.ForType(resourceType).Increase(amount);
        }
    }
}
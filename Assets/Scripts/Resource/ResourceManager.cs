using System.Collections.Generic;
using UnityEngine;

namespace Resource
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;

        private Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();
    
        public void Awake()
        {
            Instance = this;
        
            _resources.Add(ResourceType.Titanium, new Resource(0, true));
            _resources.Add(ResourceType.Food, new Resource(0, true));
            _resources.Add(ResourceType.O2, new Resource(10));
            _resources.Add(ResourceType.Colonists, new Resource(5));
            _resources.Add(ResourceType.WubbelUbbelOre, new Resource(0, true));
        }

        public Resource ForType(ResourceType type)
        {
            return _resources[type];
        }
    }
}
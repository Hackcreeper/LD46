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
        
            _resources.Add(ResourceType.Money, new Resource(0, true));
            _resources.Add(ResourceType.Titanium, new Resource(0, true));
            _resources.Add(ResourceType.Food, new Resource(10));
            _resources.Add(ResourceType.O2, new Resource(10));
            _resources.Add(ResourceType.Colonists, new Resource(5));
            _resources.Add(ResourceType.WubbelUbbelOre, new Resource(0, true));
            
            _resources[ResourceType.Titanium].Increase(10);
            _resources[ResourceType.Food].Increase(10);
            _resources[ResourceType.O2].Increase(10);
            _resources[ResourceType.Colonists].Increase(5); // TODO: Actually spawn these
        }

        public Resource ForType(ResourceType type)
        {
            return _resources[type];
        }
    }
}
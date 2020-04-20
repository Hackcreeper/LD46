using System;
using System.Collections.Generic;
using Buildings;
using UnityEngine;

namespace Resource
{
    [Serializable]
    public struct ResourceImage
    {
        public ResourceType resourceType;
        public Sprite sprite;
    }

    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager Instance;

        public ResourceImage[] images;
        public Sprite defaultImage;
        public GameObject popupPrefab;

        private readonly Dictionary<ResourceType, Resource> _resources = new Dictionary<ResourceType, Resource>();
        private readonly Dictionary<ResourceType, Sprite> _sprites = new Dictionary<ResourceType, Sprite>();

        public void Awake()
        {
            Instance = this;

            _resources.Add(ResourceType.Money, new Resource(0, true));
            _resources.Add(ResourceType.Titanium, new Resource(0, true));
            _resources.Add(ResourceType.Food, new Resource(10));
            _resources.Add(ResourceType.O2, new Resource(10));
            _resources.Add(ResourceType.Colonists, new Resource(5));
            _resources.Add(ResourceType.WubbelUbbelOre, new Resource(0, true));

            _resources[ResourceType.Titanium].Increase(50);
            _resources[ResourceType.Food].Increase(10);
            _resources[ResourceType.O2].Increase(10);
            
            _resources[ResourceType.Money].Increase(1000);

            foreach (var image in images)
            {
                _sprites.Add(image.resourceType, image.sprite);
            }
        }

        public Resource ForType(ResourceType type)
        {
            return _resources[type];
        }

        public Sprite GetSpriteForType(ResourceType type)
        {
            if (_sprites.ContainsKey(type))
            {
                return _sprites[type];
            }

            return defaultImage;
        }

        public ResourcePopup SpawnPopup(Building building)
        {
            var height = building.AddPopup();

            var popup = Instantiate(popupPrefab, building.transform);
            popup.transform.localPosition = new Vector3(0, 2.5f, height);

            var resourcePopup = popup.GetComponent<ResourcePopup>();
            resourcePopup.onDeath = () => building.RemovePopup();

            return resourcePopup;
        }
    }
}
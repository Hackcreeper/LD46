using System;
using TMPro;
using UnityEngine;

namespace Resource
{
    public class ResourcePopup : MonoBehaviour
    {
        public TextMeshPro text;
        public SpriteRenderer spriteRenderer;
        public Action onDeath;

        public void Set(ResourceType resource, int amount)
        {
            text.text = $"+{amount}";
            spriteRenderer.sprite = ResourceManager.Instance.GetSpriteForType(resource);
        }

        public void Kill()
        {
            onDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
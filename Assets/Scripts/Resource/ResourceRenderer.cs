using TMPro;
using UnityEngine;

namespace Resource
{
    public class ResourceRenderer : MonoBehaviour
    {
        public ResourceType resType;

        public void Update()
        {
            GetComponent<TextMeshProUGUI>().text = ResourceManager.Instance.ForType(resType).ToString();
        }
    }
}
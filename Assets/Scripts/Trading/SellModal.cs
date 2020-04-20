using System;
using Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trading
{
    public class SellModal : MonoBehaviour
    {
        public TextMeshProUGUI description;
        public Button confirmButton;

        private bool _open;

        public void Open()
        {
            gameObject.SetActive(true);
            _open = true;
        }

        public void Close()
        {
            _open = false;
            gameObject.SetActive(false);
        }
        
        private void Update()
        {
            if (!_open)
            {
                return;
            }
            
            var totalWubbel = ResourceManager.Instance.ForType(ResourceType.WubbelUbbelOre).Get();
            var totalMoney = totalWubbel * 20;
            
            description.text = $"Sell {totalWubbel} wubbelubbel ore for\n<b>{totalMoney}</b> money?";

            confirmButton.interactable = totalWubbel > 0;
        }

        public void Sell()
        {
            var totalWubbel = ResourceManager.Instance.ForType(ResourceType.WubbelUbbelOre).Get();
            var totalMoney = totalWubbel * 20;

            ResourceManager.Instance.ForType(ResourceType.WubbelUbbelOre).Decrease(totalWubbel);
            ResourceManager.Instance.ForType(ResourceType.Money).Increase(totalMoney);

            Close();
        }
    }
}
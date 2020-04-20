using Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trading
{
    public class SellModal : MonoBehaviour
    {
        private const int WubbelUbbelPrice = 21;
        
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
            var totalMoney = totalWubbel * WubbelUbbelPrice;
            
            description.text = $"Sell {totalWubbel} wubbelubbel ore for<br>$<b>{totalMoney}</b>?";

            confirmButton.interactable = totalWubbel > 0;
        }

        public void Sell()
        {
            var totalWubbel = ResourceManager.Instance.ForType(ResourceType.WubbelUbbelOre).Get();
            var totalMoney = totalWubbel * WubbelUbbelPrice;

            ResourceManager.Instance.ForType(ResourceType.WubbelUbbelOre).Decrease(totalWubbel);
            ResourceManager.Instance.ForType(ResourceType.Money).Increase(totalMoney);

            Close();
        }
    }
}
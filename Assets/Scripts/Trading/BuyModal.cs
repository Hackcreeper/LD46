using System;
using Resource;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Trading
{
    public class BuyModal : MonoBehaviour
    {
        private const int TitaniumCosts = 8;
        private const int ColonistsCosts = 900;

        private int _buyTitanium;
        private int _buyColonists;

        public TextMeshProUGUI titaniumAmountText;
        public TextMeshProUGUI titaniumAmountCostsText;
        public TextMeshProUGUI titaniumSingleCostsText;

        public TextMeshProUGUI colonistAmountText;
        public TextMeshProUGUI colonistAmountCostsText;
        public TextMeshProUGUI colonistSingleCostsText;

        public GameObject reservedBox;
        public TextMeshProUGUI titaniumReservedText;
        public TextMeshProUGUI colonistReservedText;

        public TextMeshProUGUI errorText;
        public Button buyButton;

        private void Start()
        {
            Recalculate(); // Set everything to zero
        }

        public void Open()
        {
            if (Tutorial.Instance.IsActive())
            {
                return;
            }
            
            gameObject.SetActive(true);
            Recalculate();
        }

        public void Close()
        {
            if (gameObject.activeSelf)
            {
                PauseModal.Handled = true;
            }
            
            gameObject.SetActive(false);   
            _buyTitanium = _buyColonists = 0;
        }

        public void IncreaseTitanium(int amount)
        {
            _buyTitanium += amount;
            Recalculate();
        }

        public void DecreaseTitanium(int amount)
        {
            _buyTitanium = Mathf.Clamp(_buyTitanium - amount, 0, Int32.MaxValue);
            Recalculate();
        }

        public void IncreaseColonists(int amount)
        {
            _buyColonists += amount;
            Recalculate();
        }

        public void DecreaseColonists(int amount)
        {
            _buyColonists = Mathf.Clamp(_buyColonists - amount, 0, Int32.MaxValue);
            Recalculate();
        }

        private void Recalculate()
        {
            titaniumAmountText.text = _buyTitanium.ToString();
            titaniumAmountCostsText.text = $"${_buyTitanium * TitaniumCosts}";
            titaniumSingleCostsText.text = $"Costs: ${TitaniumCosts}";

            colonistAmountText.text = _buyColonists.ToString();
            colonistAmountCostsText.text = $"${_buyColonists * ColonistsCosts}";
            colonistSingleCostsText.text = $"Costs: ${ColonistsCosts}";

            var hasError = false;
            var error = "";

            var totalPrice = _buyTitanium * TitaniumCosts + _buyColonists * ColonistsCosts;
            if (ResourceManager.Instance.ForType(ResourceType.Money).Get() < totalPrice)
            {
                hasError = true;
                error = "You don't have enough money to request those resources.";
            }

            var colRes = ResourceManager.Instance.ForType(ResourceType.Colonists);
            var sumColonists = colRes.Get() + _buyColonists + Trader.Instance.GetReservedColonists();

            if (colRes.GetMax() < sumColonists)
            {
                hasError = true;
                error =
                    $"You can only have {colRes.GetMax()} colonists total! Build more sleep quarters to buy more colonists!";
            }

            errorText.text = error;
            errorText.gameObject.SetActive(hasError);

            buyButton.interactable = !hasError && totalPrice > 0;

            reservedBox.SetActive(Trader.Instance.GetReservedColonists() > 0 || Trader.Instance.GetReservedTitanium() > 0);
            titaniumReservedText.text = Trader.Instance.GetReservedTitanium().ToString();
            colonistReservedText.text = Trader.Instance.GetReservedColonists().ToString();
        }

        public void PlaceOrder()
        {
            ResourceManager.Instance.ForType(ResourceType.Money).Decrease(
                _buyTitanium * TitaniumCosts + _buyColonists * ColonistsCosts
            );

            Trader.Instance.Reserve(_buyTitanium, _buyColonists);
            Close();
        }
    }
}
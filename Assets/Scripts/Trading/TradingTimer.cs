using Resource;
using TMPro;
using UnityEngine;

namespace Trading
{
    public class TradingTimer : MonoBehaviour
    {
        public TextMeshProUGUI timer;
        public GameObject requestButton;
        public GameObject tradeButton;

        public SellModal sellModal;

        private void Update()
        {
            switch (Trader.Instance.GetState())
            {
                case TradeState.OnEarth:
                    HandleOnEarth();
                    break;

                case TradeState.Landing:
                    HandleLanding();
                    break;

                case TradeState.Active:
                    HandleActive();
                    break;
                
                case TradeState.Going:
                    HandleGoing();
                    break;
            }
        }

        private void HandleGoing()
        {
            sellModal.Close();
            
            timer.text = "Returning to earth";
            timer.color = Color.black;
            
            tradeButton.gameObject.SetActive(false);
        }

        private void HandleActive()
        {
            var remainingTimeUntilTradeEnds = Trader.Instance.GetRemainingTimeUntilTradeEnds();
            
            timer.text = ((int) remainingTimeUntilTradeEnds).ToString();
            timer.color = remainingTimeUntilTradeEnds <= 10 ? Color.red : Color.black;
            
            tradeButton.gameObject.SetActive(true);
        }

        private void HandleLanding()
        {
            timer.text = "Landing";
            timer.color = Color.red;

            requestButton.gameObject.SetActive(false);
        }

        private void HandleOnEarth()
        {
            var remainingTime = Trader.Instance.GetRemainingTime();

            timer.text = ((int) remainingTime).ToString();
            timer.color = remainingTime <= 10 ? Color.red : Color.black;

            requestButton.gameObject.SetActive(true);
        }

        public void SellWubbelubbel()
        {
            sellModal.Open();
        }
    }
}
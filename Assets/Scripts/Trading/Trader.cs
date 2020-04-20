using Colonists;
using Resource;
using UnityEngine;

namespace Trading
{
    public class Trader : MonoBehaviour
    {
        public static Trader Instance;

        private float _timeUntilTrade = 100;
        private float _timeUntilTradeEnds;
        
        private TradeState _state = TradeState.OnEarth;

        private int _reservedTitanium = 0;
        private int _reservedColonists = 0;

        private void Awake()
        {
            Instance = this;
        }
    
        private void Update()
        {
            if (Tutorial.Instance.IsActive())
            {
                return;
            }
            
            switch (_state)
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
            
        }

        private void HandleActive()
        {
            _timeUntilTradeEnds -= Time.deltaTime;

            if (_timeUntilTradeEnds > 0)
            {
                return;
            }
            
            SetState(TradeState.Going);
        }

        private void HandleLanding()
        {
            
        }

        private void HandleOnEarth()
        {
            _timeUntilTrade -= Time.deltaTime;

            if (_timeUntilTrade <= 0)
            {
                SetState(TradeState.Landing);
            }
        }

        public float GetRemainingTime() => _timeUntilTrade;

        public float GetRemainingTimeUntilTradeEnds() => _timeUntilTradeEnds;
        
        public TradeState GetState() => _state;

        private void SetState(TradeState state)
        {
            _state = state;

            switch (state)
            {
                case TradeState.Landing:
                    Rocket.Instance.Land();
                    break;
                
                case TradeState.Active:
                    ResourceManager.Instance.ForType(ResourceType.Titanium).Increase(_reservedTitanium);
                    for (var i = 0; i < _reservedColonists; i++)
                    {
                        ColonistManager.Instance.SpawnColonist();
                    }

                    if (_reservedTitanium > 0)
                    {
                        ResourceManager.Instance.SpawnPopup(Game.Instance.GetLandingPlatform()).Set(ResourceType.Titanium, _reservedTitanium);
                    }
                    
                    if (_reservedColonists > 0)
                    {
                        ResourceManager.Instance.SpawnPopup(Game.Instance.GetLandingPlatform()).Set(ResourceType.Colonists, _reservedColonists);
                    }
                    
                    _timeUntilTradeEnds = 45f;
                    _reservedColonists = 0;
                    _reservedTitanium = 0;
                    
                    break;
                
                case TradeState.Going:
                    Rocket.Instance.Go();
                    break;
            }
        }

        public void RocketHasLanded()
        {
            SetState(TradeState.Active);
        }

        public void RocketIsGone()
        {
            SetState(TradeState.OnEarth);
            _timeUntilTrade = 100f;
        }

        public void Reserve(int titanium, int colonists)
        {
            _reservedTitanium += titanium;
            _reservedColonists += colonists;
        }

        public int GetReservedColonists() => _reservedColonists;
        
        public int GetReservedTitanium() => _reservedTitanium;
    }
}
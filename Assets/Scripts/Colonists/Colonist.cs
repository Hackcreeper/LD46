using System;
using System.Collections.Generic;
using Buildings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Colonists
{
    public class Colonist : MonoBehaviour
    {
        public Action<Colonist> taskCompleted;
        public Animator animator;

        private List<Transform> _path = new List<Transform>();
        private List<Building> _checked = new List<Building>();
        private Building _currentBuilding;
        private ColonistState _state = ColonistState.Walking;

        private float _stateTimer;

        public void SetCurrentBuilding(Building current)
        {
            _currentBuilding = current;
        }

        public void SetTarget(Building building)
        {
            Debug.Log("Going to " + building.name);
            
            SetState(ColonistState.Walking);
        }

        private void SetState(ColonistState state)
        {
            switch (state)
            {
                case ColonistState.Idle:
                    break;
                
                case ColonistState.Working:
                    break;
                
                default:
                    _stateTimer = Random.Range(5, 30);
                    animator.SetBool("walking", true);
                    break;
            }

            _state = state;
        }
        
        private void Update()
        {
            switch (_state)
            {
                case ColonistState.Idle:
                    HandleIdleState();
                    break;
                
                case ColonistState.Working:
                    HandleWorkingState();
                    break;
                
                default:
                    HandleWalkingState();
                    break;
            }
            
            // State: Idle standing
            // Unit stands there and players the idle Animation every 2 seconds
            // Ends after 4 - 20 seconds
            
            // State: Idle working
            // Plays the idle working animation
            // When this is finished, this state is finished
        }

        private void HandleWalkingState()
        {
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0)
            {
                animator.SetBool("walking", false);
                SelectRandomState();

                return;
            }

            // transform.Translate(1 * Time.deltaTime, 0, 0, Space.Self);

            // State: walking
            // Unit moves randomlly for 5 to 30 seconds
            // If it hits a wall or obstacle (building collider) -> rotate in random direction and try again
        }

        private void SelectRandomState()
        {
            // TODO
        }

        private void HandleWorkingState()
        {
            
        }

        private void HandleIdleState()
        {
            
        }
    }
}
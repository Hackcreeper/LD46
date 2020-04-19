using System;
using System.Collections.Generic;
using Buildings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Colonists
{
    public class Colonist : MonoBehaviour
    {
        private List<Building> _checked = new List<Building>();
        private Building _currentBuilding;

        private List<Transform> _path = new List<Transform>();
        private ColonistState _state = ColonistState.Walking;

        private float _stateTimer;
        private Vector2? _targetPosition = null;
        public Animator animator;
        public LayerMask buildingMask;
        public Action<Colonist> taskCompleted;

        public void SetCurrentBuilding(Building current)
        {
            _currentBuilding = current;
        }

        public void SetTarget(Building building)
        {
            _currentBuilding = building;
            SelectRandomState();
        }

        private void SetState(ColonistState state)
        {
            switch (state)
            {
                case ColonistState.Idle:
                    _stateTimer = Random.Range(4, 15);
                    animator.SetBool("idle", true);
                    break;

                case ColonistState.Working:
                    _stateTimer = Random.Range(3, 10);
                    animator.SetBool("working", true);
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

            var body = GetComponent<Rigidbody>();
            body.MovePosition(body.position + transform.right * 1f * Time.fixedDeltaTime);

            if (Physics.Raycast(transform.position - new Vector3(0, .4f, 0), transform.right, .8f, buildingMask))
                transform.Rotate(0, Random.Range(90, 270), 0);
        }

        private void SelectRandomState()
        {
            var states = new[] {ColonistState.Idle, ColonistState.Walking, ColonistState.Working};
            
            SetState(states[Random.Range(0, states.Length)]);
        }

        private void HandleWorkingState()
        {
            _stateTimer -= Time.deltaTime;

            if (_stateTimer > 0) return;

            animator.SetBool("working", false);
            SelectRandomState();
        }

        private void HandleIdleState()
        {
            _stateTimer -= Time.deltaTime;

            if (_stateTimer > 0) return;

            animator.SetBool("idle", false);
            SelectRandomState();
        }
    }
}
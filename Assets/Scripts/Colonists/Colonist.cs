using System;
using System.Collections.Generic;
using Buildings;
using Resource;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Colonists
{
    public class Colonist : MonoBehaviour
    {
        private const float O2Interval = 3;
        private const float FoodInterval = 10;

        private List<Building> _checked = new List<Building>();
        private Building _currentBuilding;

        private List<Transform> _path = new List<Transform>();
        private ColonistState _state = ColonistState.Walking;

        private float _stateTimer;
        private Vector2? _targetPosition = null;
        private float _o2Timer = O2Interval;
        private float _foodTimer = FoodInterval;

        private int _missingO2Intervals = 0;
        private int _missingFoodIntervals = 0;

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
            _o2Timer -= Time.deltaTime;
            _foodTimer -= Time.deltaTime;

            if (_o2Timer <= 0)
            {
                if (!ResourceManager.Instance.ForType(ResourceType.O2).Decrease(1))
                {
                    _missingO2Intervals++;
                    MissingResources.Instance.ReportO2();
                }
                else
                {
                    MissingResources.Instance.ClearReportO2(_missingO2Intervals);
                    _missingO2Intervals = 0;
                }

                _o2Timer = O2Interval;
            }

            if (_foodTimer <= 0)
            {
                if (!ResourceManager.Instance.ForType(ResourceType.Food).Decrease(1))
                {
                    _missingFoodIntervals++;
                    MissingResources.Instance.ReportFood();
                }
                else
                {
                    MissingResources.Instance.ClearReportFood(_missingFoodIntervals);
                    _missingFoodIntervals = 0;
                }

                if (_missingFoodIntervals > 3)
                {
                    MissingResources.Instance.Died("starvation", _missingO2Intervals, _missingFoodIntervals);
                    Die();
                    return;
                }

                if (_missingO2Intervals > 20)
                {
                    MissingResources.Instance.Died("suffocation", _missingO2Intervals, _missingFoodIntervals);
                    Die();
                    return;
                }

                _foodTimer = FoodInterval;
            }

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

        private void Die()
        {
            _currentBuilding.RemoveColonist(this);
            ResourceManager.Instance.ForType(ResourceType.Colonists).Decrease(1);
            
            Destroy(gameObject);
            
            if (ResourceManager.Instance.ForType(ResourceType.Colonists).Get() == 0)
            {
                SceneManager.LoadScene("GameOver");
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
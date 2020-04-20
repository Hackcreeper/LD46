using System;
using UnityEngine;

namespace Trading
{
    public class Rocket : MonoBehaviour
    {
        public static Rocket Instance;
        
        public Animator animator;

        private void Awake()
        {
            Instance = this;
        }

        public void Land()
        {
            gameObject.SetActive(true);
            animator.SetBool("landing", true);
        }

        public void Landed()
        {
            animator.SetBool("landing", false);
            Trader.Instance.RocketHasLanded();
        }

        public void Go()
        {
            animator.SetBool("going", true);
        }

        public void Gone()
        {
            animator.SetBool("going", false);
            Trader.Instance.RocketIsGone();
        }
    }
}
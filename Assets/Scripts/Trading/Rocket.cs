using UnityEngine;
using UnityEngine.Rendering;

namespace Trading
{
    public class Rocket : MonoBehaviour
    {
        public static Rocket Instance;
        
        public Animator animator;
        public MeshRenderer meshRenderer;

        private void Awake()
        {
            Instance = this;
        }

        public void Land()
        {
            meshRenderer.shadowCastingMode = ShadowCastingMode.On;
            
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
            meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            animator.SetBool("going", false);
            Trader.Instance.RocketIsGone();
        }
    }
}
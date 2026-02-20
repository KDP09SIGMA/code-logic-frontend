using UnityEngine;

namespace SoccerSim.Animation
{
    /// <summary>
    /// Bridges gameplay events to Animator params/triggers.
    /// Pattern: Animation Facade to decouple gameplay systems from Animator setup.
    /// </summary>
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        // Hashes prevent repeated string lookups and are a common optimization pattern.
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int PassTrigger = Animator.StringToHash("Pass");
        private static readonly int ShootTrigger = Animator.StringToHash("Shoot");
        private static readonly int TackleTrigger = Animator.StringToHash("Tackle");

        private void Reset()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public void SetMovement(float normalizedSpeed)
        {
            if (animator == null) return;
            animator.SetFloat(Speed, Mathf.Clamp01(normalizedSpeed));
        }

        public void TriggerPass()
        {
            if (animator == null) return;
            animator.SetTrigger(PassTrigger);
        }

        public void TriggerShoot()
        {
            if (animator == null) return;
            animator.SetTrigger(ShootTrigger);
        }

        public void TriggerTackle()
        {
            if (animator == null) return;
            animator.SetTrigger(TackleTrigger);
        }
    }
}

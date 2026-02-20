using UnityEngine;

namespace SoccerSim.Core
{
    /// <summary>
    /// Handles football movement using Rigidbody physics.
    /// Pattern: Component-based simulation with force/impulse commands.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class BallPhysicsController : MonoBehaviour
    {
        [Header("Ball Tuning")]
        [SerializeField] private float passImpulse = 8.5f;
        [SerializeField] private float shootImpulse = 14f;
        [SerializeField] private float maxLinearSpeed = 24f;
        [SerializeField] private float rollingDrag = 0.12f;
        [SerializeField] private float airDrag = 0.02f;

        [Header("Ownership")]
        [SerializeField] private PlayerMovementController currentOwner;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }

        private void FixedUpdate()
        {
            // Apply contextual drag so the ball naturally slows over the pitch.
            rb.drag = IsGrounded() ? rollingDrag : airDrag;

            // Safety clamp avoids unrealistic velocities from stacked impulses.
            if (rb.velocity.magnitude > maxLinearSpeed)
            {
                rb.velocity = rb.velocity.normalized * maxLinearSpeed;
            }
        }

        /// <summary>
        /// Pass = flatter, controlled impulse.
        /// </summary>
        public void Pass(Vector3 direction, float powerMultiplier = 1f)
        {
            ReleaseOwner();
            var impulse = direction.normalized * passImpulse * Mathf.Clamp(powerMultiplier, 0.4f, 1.3f);
            rb.AddForce(impulse, ForceMode.Impulse);
        }

        /// <summary>
        /// Shoot = stronger impulse with optional upward lift.
        /// </summary>
        public void Shoot(Vector3 direction, float lift = 0.08f, float powerMultiplier = 1f)
        {
            ReleaseOwner();
            var shotDirection = (direction + Vector3.up * Mathf.Clamp(lift, 0f, 0.35f)).normalized;
            var impulse = shotDirection * shootImpulse * Mathf.Clamp(powerMultiplier, 0.5f, 1.5f);
            rb.AddForce(impulse, ForceMode.Impulse);
        }

        public void SetOwner(PlayerMovementController owner)
        {
            currentOwner = owner;
        }

        public PlayerMovementController GetOwner() => currentOwner;

        public void ReleaseOwner()
        {
            currentOwner = null;
        }

        private bool IsGrounded()
        {
            // Simplified grounded check for ball surface contact.
            return Physics.Raycast(transform.position, Vector3.down, 0.6f);
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Optional: richer collisions (crowd boards, goal posts) can be expanded here.
            if (collision.collider.CompareTag("GoalNet"))
            {
                // Leave as hook for SFX/VFX/goal logic.
            }
        }
    }
}

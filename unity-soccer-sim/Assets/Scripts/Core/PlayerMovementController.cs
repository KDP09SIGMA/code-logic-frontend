using SoccerSim.Animation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SoccerSim.Core
{
    /// <summary>
    /// Handles user/AI-controlled locomotion and ball actions.
    /// Pattern: Command-driven character motor over Rigidbody.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveAcceleration = 26f;
        [SerializeField] private float maxMoveSpeed = 6f;
        [SerializeField] private float sprintMultiplier = 1.45f;
        [SerializeField] private float turnSpeed = 10f;

        [Header("Ball Interaction")]
        [SerializeField] private Transform dribblePivot;
        [SerializeField] private float ballControlDistance = 1.2f;
        [SerializeField] private float controlMagnetForce = 10f;

        [Header("Input Actions (New Input System)")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference sprintAction;
        [SerializeField] private InputActionReference passAction;
        [SerializeField] private InputActionReference shootAction;
        [SerializeField] private InputActionReference tackleAction;

        [Header("Refs")]
        [SerializeField] private BallPhysicsController matchBall;
        [SerializeField] private PlayerAnimationController animationController;

        private Rigidbody rb;
        private Vector2 moveInput;
        private bool isSprinting;

        public bool IsUserControlled = true;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        }

        private void OnEnable()
        {
            if (!IsUserControlled) return;

            moveAction?.action.Enable();
            sprintAction?.action.Enable();
            passAction?.action.Enable();
            shootAction?.action.Enable();
            tackleAction?.action.Enable();

            if (passAction != null) passAction.action.performed += OnPass;
            if (shootAction != null) shootAction.action.performed += OnShoot;
            if (tackleAction != null) tackleAction.action.performed += OnTackle;
        }

        private void OnDisable()
        {
            if (!IsUserControlled) return;

            if (passAction != null) passAction.action.performed -= OnPass;
            if (shootAction != null) shootAction.action.performed -= OnShoot;
            if (tackleAction != null) tackleAction.action.performed -= OnTackle;
        }

        private void Update()
        {
            if (!IsUserControlled) return;

            moveInput = moveAction != null ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;
            isSprinting = sprintAction != null && sprintAction.action.IsPressed();
        }

        private void FixedUpdate()
        {
            MoveCharacter(moveInput, isSprinting);
            ApplyDribbleMagnet();
        }

        /// <summary>
        /// Shared motor logic used by both player input and AI steering.
        /// </summary>
        public void MoveCharacter(Vector2 input, bool sprint)
        {
            var moveVector = new Vector3(input.x, 0f, input.y);
            float speedCap = maxMoveSpeed * (sprint ? sprintMultiplier : 1f);

            if (moveVector.sqrMagnitude > 0.001f)
            {
                // Steering pattern: rotate toward desired heading.
                var targetRotation = Quaternion.LookRotation(moveVector.normalized, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime);

                rb.AddForce(moveVector.normalized * moveAcceleration, ForceMode.Acceleration);
            }

            // Horizontal speed clamp keeps control responsive and predictable.
            var horizontalVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            if (horizontalVelocity.magnitude > speedCap)
            {
                horizontalVelocity = horizontalVelocity.normalized * speedCap;
                rb.velocity = new Vector3(horizontalVelocity.x, rb.velocity.y, horizontalVelocity.z);
            }

            animationController?.SetMovement(horizontalVelocity.magnitude / (maxMoveSpeed * sprintMultiplier));
        }

        private void ApplyDribbleMagnet()
        {
            if (matchBall == null || matchBall.GetOwner() != this) return;

            var ballTransform = matchBall.transform;
            var pivot = dribblePivot != null ? dribblePivot.position : transform.position + transform.forward;
            Vector3 toPivot = pivot - ballTransform.position;

            if (toPivot.magnitude > ballControlDistance)
            {
                var ballRb = matchBall.GetComponent<Rigidbody>();
                ballRb.AddForce(toPivot.normalized * controlMagnetForce, ForceMode.Acceleration);
            }
        }

        private void OnPass(InputAction.CallbackContext _)
        {
            if (matchBall == null || matchBall.GetOwner() != this) return;
            matchBall.Pass(transform.forward, 1f);
            animationController?.TriggerPass();
        }

        private void OnShoot(InputAction.CallbackContext _)
        {
            if (matchBall == null || matchBall.GetOwner() != this) return;
            matchBall.Shoot(transform.forward, 0.12f, 1f);
            animationController?.TriggerShoot();
        }

        private void OnTackle(InputAction.CallbackContext _)
        {
            animationController?.TriggerTackle();
            // Hook for tackle overlap checks / dispossession logic.
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.TryGetComponent(out BallPhysicsController ball))
            {
                // Possession pattern: nearest valid touch claims temporary ownership.
                ball.SetOwner(this);
            }
        }
    }
}

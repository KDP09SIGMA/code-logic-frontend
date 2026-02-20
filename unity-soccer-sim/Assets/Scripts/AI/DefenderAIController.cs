using SoccerSim.Core;
using UnityEngine;

namespace SoccerSim.AI
{
    /// <summary>
    /// Basic defensive behavior: chase attacker, hold pressure radius, attempt dispossession.
    /// Pattern: Finite-state style utility logic for NPC locomotion.
    /// </summary>
    public class DefenderAIController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementController controlledPlayer;
        [SerializeField] private Transform attackerTarget;
        [SerializeField] private BallPhysicsController matchBall;

        [Header("Defensive Tuning")]
        [SerializeField] private float pressDistance = 2.4f;
        [SerializeField] private float sprintEngageDistance = 6f;
        [SerializeField] private float tackleDistance = 1.5f;

        private void Reset()
        {
            controlledPlayer = GetComponent<PlayerMovementController>();
        }

        private void FixedUpdate()
        {
            if (controlledPlayer == null || attackerTarget == null) return;

            Vector3 toAttacker = attackerTarget.position - transform.position;
            float distance = toAttacker.magnitude;

            bool shouldSprint = distance > sprintEngageDistance;
            Vector3 desiredDirection;

            if (distance > pressDistance)
            {
                // Chase to close down ball carrier.
                desiredDirection = toAttacker.normalized;
            }
            else
            {
                // Contain once close: keep body between goal and attacker.
                Vector3 containOffset = -attackerTarget.forward * 0.8f;
                desiredDirection = ((attackerTarget.position + containOffset) - transform.position).normalized;
            }

            controlledPlayer.MoveCharacter(new Vector2(desiredDirection.x, desiredDirection.z), shouldSprint);

            if (matchBall != null && distance <= tackleDistance && matchBall.GetOwner() != controlledPlayer)
            {
                // Simple dispossession attempt.
                matchBall.SetOwner(controlledPlayer);
            }
        }
    }
}

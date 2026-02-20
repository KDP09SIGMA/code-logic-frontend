using UnityEngine;

namespace SoccerSim.Core
{
    /// <summary>
    /// Lightweight match wiring helper for scene setup.
    /// Pattern: Bootstrapper/Composition Root.
    /// </summary>
    public class MatchBootstrap : MonoBehaviour
    {
        [SerializeField] private BallPhysicsController ball;
        [SerializeField] private PlayerMovementController userPlayer;
        [SerializeField] private PlayerMovementController[] allPlayers;

        private void Start()
        {
            if (ball == null || userPlayer == null) return;

            // Start in user possession for quick gameplay testing.
            ball.SetOwner(userPlayer);

            foreach (var player in allPlayers)
            {
                if (player == null) continue;
                player.IsUserControlled = (player == userPlayer);
            }
        }
    }
}

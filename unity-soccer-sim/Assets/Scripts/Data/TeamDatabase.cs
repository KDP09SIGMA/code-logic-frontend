using System;
using UnityEngine;

namespace SoccerSim.Data
{
    /// <summary>
    /// Loads team/player data from JSON.
    /// Pattern: Data-driven design via serializable DTOs.
    /// </summary>
    public class TeamDatabase : MonoBehaviour
    {
        [SerializeField] private TextAsset teamsJson;

        public TeamCollection Teams { get; private set; }

        private void Awake()
        {
            if (teamsJson == null)
            {
                Debug.LogWarning("TeamDatabase: teamsJson is not assigned.");
                return;
            }

            Teams = JsonUtility.FromJson<TeamCollection>(teamsJson.text);
        }
    }

    [Serializable]
    public class TeamCollection
    {
        public TeamData[] teams;
    }

    [Serializable]
    public class TeamData
    {
        public string id;
        public string name;
        public string formation;
        public PlayerData[] players;
    }

    [Serializable]
    public class PlayerData
    {
        public string name;
        public string position;
        public int pace;
        public int shooting;
        public int passing;
        public int defending;
        public int stamina;
    }
}

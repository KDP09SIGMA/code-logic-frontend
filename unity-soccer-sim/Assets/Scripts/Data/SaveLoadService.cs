using System;
using UnityEngine;

namespace SoccerSim.Data
{
    /// <summary>
    /// Stores lightweight user settings and progression.
    /// Pattern: Service wrapper around PlayerPrefs persistence.
    /// </summary>
    public class SaveLoadService : MonoBehaviour
    {
        private const string SaveKey = "soccer_sim_save_v1";

        public void Save(PlayerProfile profile)
        {
            var json = JsonUtility.ToJson(profile);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public PlayerProfile LoadOrDefault()
        {
            var json = PlayerPrefs.GetString(SaveKey, string.Empty);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new PlayerProfile
                {
                    preferredTeamId = "blue-lions",
                    difficulty = 1,
                    matchesPlayed = 0
                };
            }

            return JsonUtility.FromJson<PlayerProfile>(json);
        }
    }

    [Serializable]
    public class PlayerProfile
    {
        public string preferredTeamId;
        public int difficulty;
        public int matchesPlayed;
    }
}

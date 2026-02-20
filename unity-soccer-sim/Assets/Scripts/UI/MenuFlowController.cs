using UnityEngine;

namespace SoccerSim.UI
{
    /// <summary>
    /// Controls menu stage transitions.
    /// Pattern: Simple state machine for UI flow.
    /// </summary>
    public class MenuFlowController : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject teamSelectPanel;
        [SerializeField] private GameObject matchHudPanel;

        public void ShowMainMenu()
        {
            SetPanels(main: true, teamSelect: false, match: false);
        }

        public void ShowTeamSelection()
        {
            SetPanels(main: false, teamSelect: true, match: false);
        }

        public void StartMatch()
        {
            SetPanels(main: false, teamSelect: false, match: true);
        }

        private void SetPanels(bool main, bool teamSelect, bool match)
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(main);
            if (teamSelectPanel != null) teamSelectPanel.SetActive(teamSelect);
            if (matchHudPanel != null) matchHudPanel.SetActive(match);
        }
    }
}

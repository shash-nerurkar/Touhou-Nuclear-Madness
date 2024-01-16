using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    
    #region Serializable Fields

    [ SerializeField ] private Toggle chaosDifficultyToggleOption;

    [ SerializeField ] private TextMeshProUGUI [ ] timerScoresLabels;

    #endregion


    #region Action

    public static event Action<bool> OnChaosDifficultyToggledAction;

    #endregion


    #region Methods

    public void UpdateScore ( int fightIndex, string fightTimeScore ) {
        timerScoresLabels [ fightIndex ].text = "Fight " + ( fightIndex + 1 ) + " - " + fightTimeScore;
    }

    public void OnChaosDifficultyUnlocked ( ) => chaosDifficultyToggleOption.gameObject.SetActive ( true );

    public void OnChaosDifficultyToggled ( bool toggleFlag ) => OnChaosDifficultyToggledAction?.Invoke ( toggleFlag );

    #endregion
}

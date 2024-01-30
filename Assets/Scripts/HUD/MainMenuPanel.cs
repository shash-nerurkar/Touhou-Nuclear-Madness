using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    
    #region Serializable Fields

    [ SerializeField ] private Toggle tryhardModeToggleOption;

    [ SerializeField ] private Toggle chaosDifficultyToggleOption;

    [ SerializeField ] private TextMeshProUGUI [ ] bestTimeScoreLabels;

    [ SerializeField ] private TextMeshProUGUI [ ] newTimeScoreLabels;

    [ SerializeField ] private TextMeshProUGUI [ ] newRecordLabels;

    #endregion


    #region Action

    public static event Action<bool> OnTryhardModeToggledAction;

    public static event Action<bool> OnChaosDifficultyToggledAction;

    #endregion


    #region Methods

    public void UpdateScore ( int fightIndex, string newTimeScore, string bestTimeScore ) {
        newTimeScoreLabels [ fightIndex ].text = newTimeScore;
        bestTimeScoreLabels [ fightIndex ].text = bestTimeScore;

        newRecordLabels [ fightIndex ].enabled = newTimeScore != Constants.TIMER_EMPTY && newTimeScore.Equals ( bestTimeScore );
    }

    public void OnTryhardModeUnlocked ( ) => tryhardModeToggleOption.gameObject.SetActive ( true );

    public void OnChaosDifficultyUnlocked ( ) => chaosDifficultyToggleOption.gameObject.SetActive ( true );

    public void OnTryhardModeToggled ( bool toggleFlag ) => OnTryhardModeToggledAction?.Invoke ( toggleFlag );

    public void OnChaosDifficultyToggled ( bool toggleFlag ) => OnChaosDifficultyToggledAction?.Invoke ( toggleFlag );

    #endregion
}

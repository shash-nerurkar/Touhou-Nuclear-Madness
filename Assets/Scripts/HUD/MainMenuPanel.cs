using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    
    #region Serializable Fields

    [ SerializeField ] private Toggle chaosDifficultyToggleOption;

    #endregion


    #region Action

    public static event Action<bool> OnChaosDifficultyToggledAction;

    #endregion


    #region Methods


    #region Event subscriptions

    private void Awake ( ) {
        SceneManager.OnChaosDifficultyUnlocked += OnChaosDifficultyUnlocked;
    }

    private void OnDestroy ( ) {
        SceneManager.OnChaosDifficultyUnlocked -= OnChaosDifficultyUnlocked;
    }

    #endregion


    private void OnChaosDifficultyUnlocked ( ) => chaosDifficultyToggleOption.gameObject.SetActive ( true );

    public void OnChaosDifficultyToggled ( bool toggleFlag ) => OnChaosDifficultyToggledAction?.Invoke ( toggleFlag );

    #endregion
}

using System;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuPanel : MonoBehaviour
{
    
    #region Serializable Fields

    [ SerializeField ] private Toggle easyDifficultyToggleOption;

    #endregion


    #region Action

    public static event Action<bool> OnEasyDifficultyToggledAction;

    #endregion


    #region Methods


    #region Event subscriptions

    private void Awake ( ) {
        SceneManager.OnEasyDifficultyUnlocked += OnEasyDifficultyUnlocked;
    }

    private void OnDestroy ( ) {
        SceneManager.OnEasyDifficultyUnlocked -= OnEasyDifficultyUnlocked;
    }

    #endregion


    private void OnEasyDifficultyUnlocked ( ) => easyDifficultyToggleOption.gameObject.SetActive ( true );

    public void OnEasyDifficultyToggled ( bool toggleFlag ) => OnEasyDifficultyToggledAction?.Invoke ( toggleFlag );

    #endregion
}

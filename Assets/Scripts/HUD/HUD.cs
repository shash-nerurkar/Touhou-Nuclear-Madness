using System;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] private BackgroundImage backgroundImage;

    [ SerializeField ] private MainMenuPanel mainMenuPanel;

    [ SerializeField ] private InGamePanel inGamePanel;

    [ SerializeField ] private EndGamePanel endGamePanel;

    [ SerializeField ] private DialogueBox dialogueBox;

    [ SerializeField ] private Transition transition;

    #endregion

    
    #region Fields

    private List<Dialogue> currentDialogues;

    private int currentDialogueIndex;

    #endregion

    
    #region Actions

    public static event Action OnDialogueSequenceCompleted;

    #endregion

    
    #region Methods


    #region Event Subscriptions

    private void Awake ( ) {
        SceneManager.OnGameStateChanged += OnGameStateChanged;
        SceneManager.OnInGameStateChanged += OnInGameStateChanged;
        SceneManager.ContinueDialogueSequence += ContinueDialogueSequence;
        SceneManager.ChangeGameStateOnHUD += ChangeGameStateOnHUD;

        SceneManager.OnChaosDifficultyUnlocked += mainMenuPanel.OnChaosDifficultyUnlocked;
        SceneManager.OnTryhardModeUnlocked += mainMenuPanel.OnTryhardModeUnlocked;
        SceneManager.UpdateScore += mainMenuPanel.UpdateScore;

        SceneManager.ShowTutorial += inGamePanel.ShowTutorial;
        SceneManager.OnFightStarted += inGamePanel.SetPanelValues;
        SceneManager.OnCurrentPlayerShootEnergyChanged += inGamePanel.UpdatePlayerShootEnergy;
        SceneManager.OnCurrentPlayerHit += inGamePanel.UpdatePlayerHealth;
        SceneManager.OnCurrentPlayerGrazed += inGamePanel.UpdatePlayerGraze;
        SceneManager.OnCurrentPlayerDamageMultiplierChanged += inGamePanel.UpdatePlayerDamageMultiplier;
        SceneManager.OnCurrentPlayerFiredAbility1 += inGamePanel.UpdatePlayerAbility1;
        SceneManager.OnCurrentPlayerFiredAbility2 += inGamePanel.UpdatePlayerAbility2;
        SceneManager.OnCurrentEnemyHit += inGamePanel.UpdateEnemyHealth;
        SceneManager.UpdateFightTimer += inGamePanel.UpdateFightTimer;
        
        SceneManager.OnEndGame += endGamePanel.ChangeBackgroundScene;
        
        SceneManager.ChangeBackgroundScene += backgroundImage.ChangeBackgroundScene;
        SceneManager.PlayExplosion += backgroundImage.PlayExplosion;

        SceneManager.TransitionFadeIn += transition.FadeIn;
        SceneManager.TransitionFadeOut += transition.FadeOut;
        SceneManager.TransitionSetNextPlayerCharacter += transition.SetNextPlayerCharacter;
        SceneManager.TransitionRemoveNextPlayerCharacter += transition.SetNextPlayerCharacter;
    }

    private void OnDestroy ( ) {
        SceneManager.OnGameStateChanged -= OnGameStateChanged;
        SceneManager.OnInGameStateChanged -= OnInGameStateChanged;
        SceneManager.ContinueDialogueSequence -= ContinueDialogueSequence;
        SceneManager.ChangeGameStateOnHUD -= ChangeGameStateOnHUD;

        SceneManager.OnChaosDifficultyUnlocked -= mainMenuPanel.OnChaosDifficultyUnlocked;
        SceneManager.OnTryhardModeUnlocked -= mainMenuPanel.OnTryhardModeUnlocked;
        SceneManager.UpdateScore -= mainMenuPanel.UpdateScore;

        SceneManager.ShowTutorial -= inGamePanel.ShowTutorial;
        SceneManager.OnFightStarted -= inGamePanel.SetPanelValues;
        SceneManager.OnCurrentPlayerShootEnergyChanged -= inGamePanel.UpdatePlayerShootEnergy;
        SceneManager.OnCurrentPlayerHit -= inGamePanel.UpdatePlayerHealth;
        SceneManager.OnCurrentPlayerGrazed -= inGamePanel.UpdatePlayerGraze;
        SceneManager.OnCurrentPlayerDamageMultiplierChanged -= inGamePanel.UpdatePlayerDamageMultiplier;
        SceneManager.OnCurrentPlayerFiredAbility1 -= inGamePanel.UpdatePlayerAbility1;
        SceneManager.OnCurrentPlayerFiredAbility2 -= inGamePanel.UpdatePlayerAbility2;
        SceneManager.OnCurrentEnemyHit -= inGamePanel.UpdateEnemyHealth;
        SceneManager.UpdateFightTimer -= inGamePanel.UpdateFightTimer;
        
        SceneManager.OnEndGame -= endGamePanel.ChangeBackgroundScene;
        
        SceneManager.ChangeBackgroundScene -= backgroundImage.ChangeBackgroundScene;
        SceneManager.PlayExplosion -= backgroundImage.PlayExplosion;

        SceneManager.TransitionFadeIn -= transition.FadeIn;
        SceneManager.TransitionFadeOut -= transition.FadeOut;
        SceneManager.TransitionSetNextPlayerCharacter -= transition.SetNextPlayerCharacter;
        SceneManager.TransitionRemoveNextPlayerCharacter -= transition.SetNextPlayerCharacter;
    }

    #endregion


    #region On-States-Changed Managers

    private void OnGameStateChanged ( GameState gameState ) {
        if ( gameState == GameState.BlockingInput ) {
            dialogueBox.ToggleInputPrompt ( false );
        }
        else {
            dialogueBox.ToggleInputPrompt ( true );
            
            mainMenuPanel.gameObject.SetActive ( gameState == GameState.MainMenu );
            inGamePanel.gameObject.SetActive ( gameState == GameState.Playing );
            endGamePanel.gameObject.SetActive ( gameState == GameState.Ended );
        }
    }
 
    private void OnInGameStateChanged ( InGameState inGameState ) {
        switch ( inGameState ) {
            case InGameState.MainMenu:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_MAIN_MENU, isInGame: false );
                break;

            case InGameState.PreExplosion:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_1 );
                break;

            case InGameState.PostExplosion:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_2 );
                break;
            
            case InGameState.PostFight1Branch1:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_3_1);
                break;
            
            case InGameState.PostFight1Branch2:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_3_2 );
                break;
            
            case InGameState.PreFight2:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_4 );
                break;
            
            case InGameState.PostFight2Branch1:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_5_1 );
                break;
            
            case InGameState.PostFight2Branch2:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_5_2 );
                break;
                
            case InGameState.EndGame:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_GAME_ENDED, isInGame: false );
                break;
        }
    }

    #endregion


    #region Dialogue Sequence Managers

    private void BeginDialogueSequence ( List<Dialogue> dialogues, bool isInGame = true ) {
        if ( dialogues.Count == 0 || (isInGame && SceneManager.IsTryhardModeActive ) ) {
            OnDialogueSequenceComplete ( );
        }
        else {
            currentDialogueIndex = 0;
            currentDialogues = dialogues;
            dialogueBox.Show ( dialogues [ currentDialogueIndex++ ] );
        }
    }

    private void ContinueDialogueSequence ( ) {
        if ( currentDialogues == null ) return;

        if ( currentDialogueIndex == currentDialogues.Count )
            OnDialogueSequenceComplete ( );
        else
            dialogueBox.Show ( currentDialogues [ currentDialogueIndex++ ] );
    }

    private void OnDialogueSequenceComplete ( ) {
        dialogueBox.Hide ( );
        currentDialogues = null;

        OnDialogueSequenceCompleted?.Invoke ( );
    }

    #endregion

    private void ChangeGameStateOnHUD ( GameState gameState ) => OnGameStateChanged ( gameState );

    #endregion
}

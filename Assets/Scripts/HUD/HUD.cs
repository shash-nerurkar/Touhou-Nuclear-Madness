using System;
using UnityEngine;

public class HUD : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] private BackgroundImage backgroundImage;

    [ SerializeField ] private GameObject mainMenuPanel;

    [ SerializeField ] private InGamePanel inGamePanel;

    [ SerializeField ] private EndGamePanel endGamePanel;

    [ SerializeField ] private DialogueBox dialogueBox;

    [ SerializeField ] private Transition transition;

    [ SerializeField ] private Sprite scene1BackgroundSprite;
    [ SerializeField ] private Sprite scene2BackgroundSprite;

    [ SerializeField ] private Sprite ending1BackgroundSprite;
    [ SerializeField ] private Sprite ending2BackgroundSprite;
    [ SerializeField ] private Sprite ending3BackgroundSprite;

    #endregion

    
    #region Fields

    private Dialogue [ ] currentDialogues;

    private int currentDialogueIndex;

    #endregion

    
    #region Actions

    public static event Action OnExplosionComplete;

    public static event Action OnPostExplosionSequenceComplete;

    public static event Action OnTransitionFadeInComplete;

    public static event Action OnTransitionFadeOutComplete;

    public static event Action EndGame;

    public static event Action<int> OnBackgroundSceneChanged;

    public static event Action<float, float> ShakeCamera;

    #endregion

    
    #region Methods

    private void Awake ( ) {
        Transition.OnFadeIn += OnTransitionFadeIn;
        Transition.OnFadeOut += OnTransitionFadeOut;

        BackgroundImage.OnExplosionAnimationComplete += OnExplosionAnimationComplete;

        SceneManager.OnGameStateChanged += OnGameStateChanged;
        SceneManager.OnInGameStateChanged += OnInGameStateChanged;
        SceneManager.OnFightStarted += inGamePanel.SetPanelValues;
        SceneManager.OnCurrentPlayerHit += inGamePanel.UpdatePlayerHealth;
        SceneManager.OnCurrentEnemyHit += inGamePanel.UpdateEnemyHealth;
        SceneManager.HideDialogueBox += dialogueBox.Hide;
        SceneManager.ContinueDialogueSequence += ContinueDialogueSequence;
    }

    private void OnGameStateChanged ( GameState gameState ) {
        mainMenuPanel.SetActive ( gameState == GameState.MainMenu );
        inGamePanel.gameObject.SetActive ( gameState == GameState.Playing );
        endGamePanel.gameObject.SetActive ( gameState == GameState.Ended );
        
        switch ( SceneManager.CurrentGameState ) {
            case GameState.MainMenu:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_MAIN_MENU );
                break;
            
            case GameState.Ended:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_GAME_ENDED );
                break;
        }
    }
 
    private void OnInGameStateChanged ( InGameState inGameState ) {
        switch ( inGameState ) {
            case InGameState.PreExplosion:
                ChangeBackgroundScene ( 0 );
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
            
            case InGameState.PostFight2Branch1:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_4_1 );
                break;
            
            case InGameState.PostFight2Branch2:
                BeginDialogueSequence ( Constants.DIALOGUE_SEQUENCE_4_2 );
                break;
        }
    }

    private void BeginDialogueSequence ( Dialogue [ ] dialogues ) {
        if ( dialogues.Length == 0 ) {
            OnDialogueSequenceComplete ( );
        }
        else {
            currentDialogueIndex = 0;
            currentDialogues = dialogues;
            dialogueBox.Show ( dialogues [ currentDialogueIndex++ ] );
        }
    }

    private void ContinueDialogueSequence ( ) {
        if ( currentDialogueIndex == currentDialogues.Length )
            OnDialogueSequenceComplete ( );
        else
            dialogueBox.Show ( currentDialogues [ currentDialogueIndex++ ] );
    }

    private void OnDialogueSequenceComplete ( ) {
        dialogueBox.Hide ( );
        currentDialogues = null;
        
        switch ( SceneManager.CurrentInGameState ) {
            case InGameState.PreExplosion:
                SoundManager.instance.Stop ( Constants.CHATTING_MUSIC );
                SoundManager.instance.Play ( Constants.ON_EXPLOSION_SOUND );
                ShakeCamera?.Invoke ( 0.5f, 0.5f );
                backgroundImage.PlayExplosion ( );

                break;

            case InGameState.PostExplosion:
                inGamePanel.ShowTutorial ( );
                OnPostExplosionSequenceComplete?.Invoke ( );
                
                break;
            
            case InGameState.PostFight1Branch1:
                SoundManager.instance.Stop( Constants.SCENE_01_MUSIC );
                SoundManager.instance.Play( Constants.ON_END_GAME_LOSS_SOUND );
                endGamePanel.SetPanelValues ( ending1BackgroundSprite );
                EndGame?.Invoke ( );

                break;
            
            case InGameState.PostFight1Branch2:
                transition.FadeIn ( );
                
                break;
            
            case InGameState.PostFight2Branch1:
                SoundManager.instance.Play( Constants.ON_END_GAME_LOSS_SOUND );
                endGamePanel.SetPanelValues ( ending2BackgroundSprite );
                EndGame?.Invoke ( );

                break;
            
            case InGameState.PostFight2Branch2:
                SoundManager.instance.Play( Constants.ON_END_GAME_WIN_SOUND );
                endGamePanel.SetPanelValues ( ending3BackgroundSprite );
                EndGame?.Invoke ( );

                break;
        }
    }

    void ChangeBackgroundScene ( int sceneIndex ) {
        if ( sceneIndex == 0)
            backgroundImage.SetPanelValues ( scene1BackgroundSprite );
        else if ( sceneIndex == 1)
            backgroundImage.SetPanelValues ( scene2BackgroundSprite );

        OnBackgroundSceneChanged?.Invoke ( sceneIndex );
    }

    private void OnExplosionAnimationComplete ( ) {
        OnExplosionComplete?.Invoke ( );
    }

    private void OnTransitionFadeIn ( ) {
        ChangeBackgroundScene ( 1 );
        transition.FadeOut ( );

        OnTransitionFadeInComplete?.Invoke ( );
    }

    private void OnTransitionFadeOut ( ) {
        OnTransitionFadeOutComplete?.Invoke ( );
    }

    private void OnDestroy ( ) {
        Transition.OnFadeIn -= OnTransitionFadeInComplete;
        Transition.OnFadeOut -= OnTransitionFadeOutComplete;

        BackgroundImage.OnExplosionAnimationComplete -= OnExplosionAnimationComplete;

        SceneManager.OnGameStateChanged -= OnGameStateChanged;
        SceneManager.OnInGameStateChanged -= OnInGameStateChanged;
        SceneManager.OnFightStarted -= inGamePanel.SetPanelValues;
        SceneManager.OnCurrentPlayerHit -= inGamePanel.UpdatePlayerHealth;
        SceneManager.OnCurrentEnemyHit -= inGamePanel.UpdateEnemyHealth;
        SceneManager.HideDialogueBox -= dialogueBox.Hide;
        SceneManager.ContinueDialogueSequence -= ContinueDialogueSequence;
    }

    #endregion
}

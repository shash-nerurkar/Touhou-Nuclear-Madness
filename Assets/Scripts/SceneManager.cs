using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] private GameObject playerSagumeObject;

    [ SerializeField ] private GameObject enemyUtsuhoObject;

    [ SerializeField ] private GameObject playerAyaObject;

    [ SerializeField ] private GameObject enemySagumeObject;

    #endregion


    #region Fields

    private GameState CurrentGameState;

    private InGameState CurrentInGameState;

    private Player currentPlayer;

    private Enemy currentEnemy;

    private int currentFightIndex;

    private List<Character> otherCharacters = new List<Character> ( );

    
    #region AI Cheats

    public static Vector3 PlayerPosition;

    #endregion


    #endregion


    #region Actions

    public static event Action<GameState> OnGameStateChanged;

    public static event Action<InGameState> OnInGameStateChanged;

    public static event Action PlayExplosion;

    public static event Action ShowTutorial;

    public static event Action<float, float> OnFightStarted;

    public static event Action<float> OnCurrentPlayerHit;

    public static event Action<float> OnCurrentEnemyHit;

    public static event Action ContinueDialogueSequence;

    public static event Action HideDialogueBox;

    public static event Action<int> ChangeBackgroundScene;

    public static event Action TransitionFadeIn;

    public static event Action TransitionFadeOut;

    public static event Action<Characters> TransitionSetNextPlayerCharacter;

    public static event Action TransitionRemoveNextPlayerCharacter;

    public static event Action<float, float> ShakeCamera;

    public static event Action<string> PlaySound;

    public static event Action<string> StopSound;

    public static event Action<int> OnEndGame;

    #endregion


    #region Methods

    private void Start ( ) => ChangeGameState ( newState: GameState.MainMenu );

    private void Update ( ) {
        if ( currentPlayer != null )
            PlayerPosition = currentPlayer.transform.position;
    }


    #region Event Subscriptions

    private void Awake ( ) {
        InputManager.PopDialogueAction += OnDialoguePopped;

        BackgroundImage.OnExplosionAnimationComplete += OnExplosionComplete;

        Transition.OnFadeInBeginning += OnTransitionFadeInStart;
        Transition.OnFadeInCompleted += OnTransitionFadeInEnd;
        Transition.OnFadeOutBeginning += OnTransitionFadeOutStart;
        Transition.OnFadeOutCompleted += OnTransitionFadeOutEnd;

        HUD.OnDialogueSequenceCompleted += OnDialogueSequenceComplete;
    }

    private void OnDestroy ( ) {
        InputManager.PopDialogueAction -= OnDialoguePopped;

        BackgroundImage.OnExplosionAnimationComplete -= OnExplosionComplete;

        Transition.OnFadeInBeginning -= OnTransitionFadeInStart;
        Transition.OnFadeInCompleted -= OnTransitionFadeInEnd;
        Transition.OnFadeOutBeginning -= OnTransitionFadeOutStart;
        Transition.OnFadeOutCompleted -= OnTransitionFadeOutEnd;

        HUD.OnDialogueSequenceCompleted -= OnDialogueSequenceComplete;
    }

    #endregion


    #region State Handlers

    private void ChangeGameState ( GameState newState ) {
        CurrentGameState = newState;

        switch ( CurrentGameState ) {
            case GameState.MainMenu:
                currentFightIndex = 0;

                break;

            case GameState.Chatting:
                HideDialogueBox?.Invoke ( );
                
                break;
        }

        OnGameStateChanged?.Invoke ( CurrentGameState );
    }

    private void ChangeInGameState ( InGameState newState ) {
        CurrentInGameState = newState;

        switch ( CurrentInGameState ) {
            case InGameState.PreExplosion:
                ChangeBackgroundScene?.Invoke ( 0 );
                break;
        }

        OnInGameStateChanged?.Invoke ( CurrentInGameState );
    }
    
    #endregion


    #region Transition

    private void OnTransitionFadeInStart ( ) { }

    private void OnTransitionFadeInEnd ( ) {
        switch ( CurrentGameState ) {
            case GameState.MainMenu:
                ChangeGameState ( newState: GameState.Chatting );

                PlaySound?.Invoke ( Constants.CHATTING_MUSIC );

                AddPlayer ( playerSagumeObject );
                AddEnemy ( enemyUtsuhoObject );
                AddBystander1 ( playerAyaObject );

                ChangeInGameState ( newState: InGameState.PreExplosion );

                break;

            case GameState.Playing:
            case GameState.Chatting:
                StopSound?.Invoke ( Constants.SCENE_01_MUSIC );

                ClearAllCharacters ( );
                AddPlayer ( playerAyaObject );
                AddEnemy ( enemySagumeObject );

                ChangeBackgroundScene?.Invoke ( 1 );
                
                break;

            case GameState.Ended:
                ChangeGameState ( newState: GameState.MainMenu );
                
                StopSound?.Invoke ( Constants.ON_END_GAME_LOSS_SOUND );
                StopSound?.Invoke ( Constants.ON_END_GAME_WIN_SOUND );

                break;
        }

        TransitionFadeOut?.Invoke ( );
    }

    private void OnTransitionFadeOutStart ( ) { }

    private void OnTransitionFadeOutEnd ( ) {
        switch ( CurrentInGameState ) {
            case InGameState.PostFight1Branch2: 
                StartNextFight ( );
                
                break;
        }
    }

    private void StartTransition ( ) {
        // ChangeGameState ( GameState.Transitioning );

        TransitionRemoveNextPlayerCharacter?.Invoke ( );

        TransitionFadeIn?.Invoke ( );
    }

    private void StartTransition ( Characters nextPlayerCharacter ) {
        // ChangeGameState ( GameState.Transitioning );

        TransitionSetNextPlayerCharacter?.Invoke ( nextPlayerCharacter );

        TransitionFadeIn?.Invoke ( );
    }

    #endregion


    #region Dialogue Handlers

    private void OnDialoguePopped ( ) {
        switch ( CurrentGameState ) {
            case GameState.MainMenu:
                PlaySound?.Invoke ( Constants.ON_DIALOGUE_POP_SOUND );

                StartTransition ( );

                break;
                
            case GameState.Chatting:
                PlaySound?.Invoke ( Constants.ON_DIALOGUE_POP_SOUND );

                ContinueDialogueSequence?.Invoke ( );

                break;

            case GameState.Ended:
                PlaySound?.Invoke ( Constants.ON_DIALOGUE_POP_SOUND );

                StartTransition ( );

                break;
        }
    }

    private void OnDialogueSequenceComplete ( ) {
        switch ( CurrentInGameState ) {
            case InGameState.PreExplosion:
                StopSound?.Invoke ( Constants.CHATTING_MUSIC );

                StartExplosion ( );

                break;

            case InGameState.PostExplosion:
                StartNextFight ( );
                
                break;
            
            case InGameState.PostFight1Branch1:
                StopSound?.Invoke ( Constants.SCENE_01_MUSIC );
                PlaySound?.Invoke ( Constants.ON_END_GAME_LOSS_SOUND );

                EndGame ( endingIndex: 0 );

                break;
            
            case InGameState.PostFight1Branch2:
                StartTransition ( nextPlayerCharacter: Characters.Aya );
                
                break;
            
            case InGameState.PostFight2Branch1:
                StopSound?.Invoke ( Constants.SCENE_02_MUSIC );
                PlaySound?.Invoke ( Constants.ON_END_GAME_LOSS_SOUND );

                EndGame ( endingIndex: 1 );

                break;
            
            case InGameState.PostFight2Branch2:
                StopSound?.Invoke ( Constants.SCENE_02_MUSIC );
                PlaySound?.Invoke ( Constants.ON_END_GAME_WIN_SOUND );

                EndGame ( endingIndex: 2 );

                break;
        }
    }

    #endregion


    #region Fight Handlers

    private void StartNextFight ( ) {
        ChangeGameState ( newState: GameState.Playing );

        foreach ( Character character in otherCharacters )
            Destroy ( character.gameObject );
        otherCharacters.Clear ( );

        switch ( currentFightIndex ) {
            case 0:
                ShowTutorial?.Invoke ( );
                PlaySound?.Invoke ( Constants.SCENE_01_MUSIC );
                break;

            case 1:
                PlaySound?.Invoke ( Constants.SCENE_02_MUSIC );
                break;
        }

        currentPlayer.SetSelected ( true );
        currentEnemy.SetActive ( true );
        
        OnFightStarted ( currentPlayer.Health, currentEnemy.Health );
    }

    private void OnFightComplete ( bool didWin ) {
        ChangeGameState ( newState: GameState.Chatting );

        switch ( currentFightIndex ) {
            case 0:
                if ( didWin )
                    ChangeInGameState ( newState: InGameState.PostFight1Branch2 );
                else
                    ChangeInGameState ( newState: InGameState.PostFight1Branch1 );
                
                AddBystander1 ( playerAyaObject );
                
                break;
                
            case 1:
                if ( didWin )
                    ChangeInGameState ( newState: InGameState.PostFight2Branch2 );
                else
                    ChangeInGameState ( newState: InGameState.PostFight2Branch1 );
                
                break;
        }

        ++currentFightIndex;

        currentPlayer.SetSelected ( false );
        currentEnemy.SetActive ( false );
        currentEnemy.OnLose -= ( ) => { OnFightComplete ( didWin: true ); };
        currentEnemy.OnHit -= CurrentEnemyHit;
        currentPlayer.OnLose -= ( ) => { OnFightComplete ( didWin: false ); };
        currentPlayer.OnPlayerShoot -= CurrentPlayerShoot;
        currentPlayer.OnHit -= CurrentPlayerHit;
    }

    private void CurrentPlayerShoot ( ) {
        PlaySound?.Invoke ( Constants.ON_PLAYER_SHOOT_SOUND );
    }

    private void CurrentPlayerHit ( float health ) {
        PlaySound?.Invoke ( Constants.ON_PLAYER_HIT_SOUND );

        OnCurrentPlayerHit?.Invoke ( health );
    }

    private void CurrentEnemyHit ( float health ) {
        OnCurrentEnemyHit?.Invoke ( health );
    }
  
    #endregion


    #region Explosion Handlers

    private void StartExplosion ( ) {
        ChangeGameState ( GameState.Animating );

        PlaySound?.Invoke ( Constants.ON_EXPLOSION_SOUND );
        
        ShakeCamera?.Invoke ( 0.5f, 0.5f );

        PlayExplosion?.Invoke ( );
    }

    private void OnExplosionComplete ( ) {
        ChangeGameState ( GameState.Chatting );

        ChangeInGameState ( newState: InGameState.PostExplosion );
    }
  
    #endregion


    #region Character Managers

    private Player AddPlayer ( GameObject playerObject ) {
        GameObject playerInstance = Instantiate ( playerObject, Constants.BASE_POSITION_PLAYER, Quaternion.identity );
        
        currentPlayer = playerInstance.GetComponent<Player> ( );
        if ( currentPlayer != null ) {
            currentPlayer.OnLose += ( ) => { OnFightComplete ( didWin: false ); };
            currentPlayer.OnPlayerShoot += CurrentPlayerShoot;
            currentPlayer.OnHit += CurrentPlayerHit;
        }

        return currentPlayer;
    }

    private Enemy AddEnemy ( GameObject enemyObject ) {
        GameObject enemyInstance = Instantiate ( enemyObject, Constants.BASE_POSITION_ENEMY, Quaternion.identity );

        currentEnemy = enemyInstance.GetComponent<Enemy> ( );
        if ( currentEnemy != null ) {
            currentEnemy.OnLose += ( ) => { OnFightComplete ( didWin: true ); };
            currentEnemy.OnHit += CurrentEnemyHit;
        }

        return currentEnemy;
    }

    private Character AddBystander1 ( GameObject bystanderObject ) {
        GameObject bystanderInstance = Instantiate ( bystanderObject, Constants.BASE_POSITION_BYSTANDER_1, Quaternion.identity );
        Character bystander = bystanderInstance.GetComponent<Character> ( );
        if ( bystander != null )
            otherCharacters.Add ( bystander );

        return bystander;
    }

    private void ClearAllCharacters ( ) {
        foreach ( Character character in otherCharacters )
            Destroy ( character.gameObject );
        otherCharacters.Clear ( );

        Destroy ( currentPlayer.gameObject );
        currentPlayer = null;

        Destroy ( currentEnemy.gameObject );
        currentEnemy = null;
    }

    #endregion


    private void EndGame ( int endingIndex ) {
        ChangeGameState ( newState: GameState.Ended );
        
        ClearAllCharacters ( );

        OnEndGame?.Invoke ( endingIndex );
    }

    #endregion
}

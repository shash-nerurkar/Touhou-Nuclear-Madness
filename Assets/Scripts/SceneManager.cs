using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region Serialized Fields

    [ Header ( "Transition" ) ]
    [ SerializeField ] private float transitionFadeOutDelay;

    [ Header ( "Combat" ) ]
    [ SerializeField ] private static Vector3 BASE_POSITION_ENEMY  = new Vector2 ( 7.5f, 0f );
    
    [ SerializeField ] private static Vector3 BASE_POSITION_PLAYER = new Vector2 ( -7.5f, 0f );
    
    [ SerializeField ] private static Vector3 BASE_POSITION_BYSTANDER_1 = new Vector2 ( -7.5f, 3f );


    [ Header ( "Player Sagume" ) ]
    [ SerializeField ] private GameObject playerSagumeObject;

    [ SerializeField ] private PlayerData playerSagumeData;


    [ Header ( "Enemy Utsuho" ) ]
    [ SerializeField ] private GameObject enemyUtsuhoObject;

    [ SerializeField ] private EnemyData enemyUtsuhoData;


    [ Header ( "Player Aya" ) ]
    [ SerializeField ] private GameObject playerAyaObject;

    [ SerializeField ] private PlayerData playerAyaData;


    [ Header ( "Enemy Sagume" ) ]
    [ SerializeField ] private GameObject enemySagumeObject;

    [ SerializeField ] private EnemyData enemySagumeData;

    #endregion


    #region Fields

    private GameState CurrentGameState;

    private InGameState CurrentInGameState;

    private Timer transitionDelayTimer;

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

    public static event Action<float, int, int, int, float, float> OnFightStarted;

    public static event Action<float> OnCurrentPlayerHit;

    public static event Action<int> OnCurrentPlayerGrazed;

    public static event Action<float> OnCurrentPlayerDamageMultiplierChanged;

    public static event Action<int> OnCurrentPlayerFiredAbility1;

    public static event Action<int> OnCurrentPlayerFiredAbility2;

    public static event Action<float> OnCurrentEnemyHit;

    public static event Action ContinueDialogueSequence;

    public static event Action HideDialogueBox;

    public static event Action<int> ChangeBackgroundScene;

    public static event Action TransitionFadeIn;

    public static event Action TransitionFadeOut;

    public static event Action<PlayerData> TransitionSetNextPlayerCharacter;

    public static event Action TransitionRemoveNextPlayerCharacter;

    public static event Action<float, float> ShakeCamera;

    public static event Action<string> PlaySound;

    public static event Action<string> StopSound;

    public static event Action<Ending, float> OnEndGame;

    public static event Action ClearAllBullets;

    #endregion


    #region Methods

    private void Start ( ) => ChangeGameState ( newState: GameState.MainMenu );

    private void FixedUpdate ( ) {
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

        transitionDelayTimer = gameObject.AddComponent<Timer> ( );
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
                ChangeInGameState ( InGameState.MainMenu );
                
                PlaySound?.Invoke ( Constants.MAIN_MENU_MUSIC );
                
                currentFightIndex = 0;

                break;

            case GameState.Chatting:
                HideDialogueBox?.Invoke ( );
                
                break;
                
            case GameState.Ended:
                ChangeInGameState ( InGameState.EndGame );

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

    private void OnTransitionFadeOutStart ( ) { }

    private void OnTransitionFadeInEnd ( ) {
        switch ( CurrentInGameState ) {
            case InGameState.MainMenu:
                StopSound?.Invoke ( Constants.MAIN_MENU_MUSIC );

                transitionDelayTimer.StartTimer ( maxTime: transitionFadeOutDelay, onTimerFinish: ( ) => {
                    ChangeGameState ( newState: GameState.Chatting );

                    PlaySound?.Invoke ( Constants.CHATTING_MUSIC );

                    ChangeInGameState ( newState: InGameState.PreExplosion );

                    AddPlayer ( playerSagumeObject );
                    AddEnemy ( enemyUtsuhoObject );
                    AddBystander1 ( playerAyaObject );

                    TransitionFadeOut?.Invoke ( );
                } );

                break;

            case InGameState.PostFight1Branch2:
                transitionDelayTimer.StartTimer ( maxTime: transitionFadeOutDelay, onTimerFinish: ( ) => {           
                    StopSound?.Invoke ( Constants.SCENE_01_MUSIC );

                    ChangeBackgroundScene?.Invoke ( 1 );

                    ClearAllCharacters ( );
                    AddPlayer ( playerAyaObject );
                    AddEnemy ( enemySagumeObject );

                    TransitionFadeOut?.Invoke ( );
                } );
                
                break;

            case InGameState.EndGame:
                ChangeGameState ( newState: GameState.MainMenu );
                
                StopSound?.Invoke ( Constants.ON_END_GAME_LOSS_MUSIC );
                StopSound?.Invoke ( Constants.ON_END_GAME_WIN_MUSIC );

                TransitionFadeOut?.Invoke ( );

                break;
        }

    }

    private void OnTransitionFadeOutEnd ( ) {
        switch ( CurrentInGameState ) {
            case InGameState.MainMenu:
                ChangeGameState ( newState: GameState.MainMenu );
                
                break;

            case InGameState.PostFight1Branch2:
                ChangeGameState ( newState: GameState.Chatting );

                ChangeInGameState ( newState: InGameState.PreFight2 );
                
                break;
                
            case InGameState.EndGame:
                ChangeGameState ( newState: GameState.Ended );
                
                break;
        }
    }

    private void StartTransition ( ) {
        ChangeGameState ( newState: GameState.Transitioning );

        TransitionRemoveNextPlayerCharacter?.Invoke ( );

        TransitionFadeIn?.Invoke ( );
    }

    private void StartTransition ( Characters nextPlayerCharacter ) {
        ChangeGameState ( newState: GameState.Transitioning );

        float nextPlayerHealth, nextPlayerSpeed;
        switch ( nextPlayerCharacter ) {
            case Characters.Sagume:
                nextPlayerHealth = playerSagumeData.Health;
                nextPlayerSpeed = playerSagumeData.Speed;

                TransitionSetNextPlayerCharacter?.Invoke ( playerSagumeData );
                
                break;

            case Characters.Aya:
                nextPlayerHealth = playerAyaData.Health;
                nextPlayerSpeed = playerAyaData.Speed;

                TransitionSetNextPlayerCharacter?.Invoke ( playerAyaData );
                
                break;
            
            default:
                nextPlayerHealth = 0;
                nextPlayerSpeed = 0;

                TransitionRemoveNextPlayerCharacter?.Invoke ( );
                
                break;
        }

        TransitionFadeIn?.Invoke ( );
    }

    #endregion


    #region Dialogue Handlers

    private void OnDialoguePopped ( ) {
        PlaySound?.Invoke ( Constants.ON_DIALOGUE_POP_SOUND );

        ContinueDialogueSequence?.Invoke ( );
    }

    private void OnDialogueSequenceComplete ( ) {
        switch ( CurrentInGameState ) {
            case InGameState.MainMenu:
                StartTransition ( nextPlayerCharacter: Characters.Sagume );
                
                break;

            case InGameState.PreExplosion:
                StopSound?.Invoke ( Constants.CHATTING_MUSIC );

                StartExplosion ( );

                break;

            case InGameState.PostExplosion:
                StartNextFight ( );
                
                break;
            
            case InGameState.PostFight1Branch1:
                StopSound?.Invoke ( Constants.SCENE_01_MUSIC );
                PlaySound?.Invoke ( Constants.ON_END_GAME_LOSS_MUSIC );

                EndGame ( ending: Ending.UtsuhoWin );

                break;
            
            case InGameState.PostFight1Branch2:
                StartTransition ( nextPlayerCharacter: Characters.Aya );
                
                break;
            
            case InGameState.PreFight2:
                StartNextFight ( );
                
                break;
            
            case InGameState.PostFight2Branch1:
                StopSound?.Invoke ( Constants.SCENE_02_MUSIC );
                PlaySound?.Invoke ( Constants.ON_END_GAME_LOSS_MUSIC );

                EndGame ( ending: Ending.SagumeWin );

                break;
            
            case InGameState.PostFight2Branch2:
                StopSound?.Invoke ( Constants.SCENE_02_MUSIC );
                PlaySound?.Invoke ( Constants.ON_END_GAME_WIN_MUSIC );

                EndGame ( ending: Ending.AyaWin );

                break;

            case InGameState.EndGame:
                StartTransition ( );
                
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

        ClearAllBullets?.Invoke ( );

        switch ( currentFightIndex ) {
            case 0:
                ShowTutorial?.Invoke ( );
                PlaySound?.Invoke ( Constants.SCENE_01_MUSIC );
                break;

            case 1:
                PlaySound?.Invoke ( Constants.SCENE_02_MUSIC );
                break;
        }

        currentPlayer.ToggleAsCurrent ( true );
        currentPlayer.OnLose += ( ) => { OnFightComplete ( didWin: false ); };
        currentPlayer.OnPlayerShoot += CurrentPlayerShoot;
        currentPlayer.OnPlayerFiredAbility1 += CurrentPlayerFiredAbility1;
        currentPlayer.OnPlayerFiredAbility2 += CurrentPlayerFiredAbility2;
        currentPlayer.OnHit += CurrentPlayerHit;
        currentPlayer.OnGrazed += CurrentPlayerGrazed;
        currentPlayer.OnDamageMultiplierIncreased += OnCurrentPlayerDamageMultiplierIncreased;
        currentPlayer.OnDamageMultiplierDecreased += OnCurrentPlayerDamageMultiplierDecreased;

        currentEnemy.ToggleAsCurrent ( true );
        currentEnemy.OnLose += ( ) => { OnFightComplete ( didWin: true ); };
        currentEnemy.OnHit += CurrentEnemyHit;
        
        OnFightStarted ( currentPlayer.Data.Health, currentPlayer.Data.BombCount, currentPlayer.Data.Ability2Count, 0, 1, currentEnemy.Data.Health );
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

        currentPlayer.ToggleAsCurrent ( false );
        currentPlayer.OnLose -= ( ) => { OnFightComplete ( didWin: false ); };
        currentPlayer.OnPlayerShoot -= CurrentPlayerShoot;
        currentPlayer.OnPlayerFiredAbility1 -= CurrentPlayerFiredAbility1;
        currentPlayer.OnPlayerFiredAbility2 -= CurrentPlayerFiredAbility2;
        currentPlayer.OnHit -= CurrentPlayerHit;
        currentPlayer.OnGrazed -= CurrentPlayerGrazed;
        currentPlayer.OnDamageMultiplierIncreased -= OnCurrentPlayerDamageMultiplierIncreased;
        currentPlayer.OnDamageMultiplierDecreased -= OnCurrentPlayerDamageMultiplierDecreased;

        currentEnemy.ToggleAsCurrent ( false );
        currentEnemy.OnLose -= ( ) => { OnFightComplete ( didWin: true ); };
        currentEnemy.OnHit -= CurrentEnemyHit;
    }

    private void CurrentPlayerShoot ( ) {
        PlaySound?.Invoke ( Constants.ON_PLAYER_SHOOT_SOUND );
    }

    private void CurrentPlayerFiredAbility1 ( int abilityCount ) {
        PlaySound?.Invoke ( Constants.ON_PLAYER_ABILITY1_SOUND );

        ShakeCamera?.Invoke ( 0.4f, 0.2f );

        OnCurrentPlayerFiredAbility1?.Invoke ( abilityCount );
    }

    private void CurrentPlayerFiredAbility2 ( int abilityCount ) {
        PlaySound?.Invoke ( Constants.ON_PLAYER_ABILITY2_SOUND );

        OnCurrentPlayerFiredAbility2?.Invoke ( abilityCount );
    }

    private void CurrentPlayerHit ( float health ) {
        PlaySound?.Invoke ( Constants.ON_PLAYER_HIT_SOUND );

        OnCurrentPlayerHit?.Invoke ( health );
    }

    private void CurrentPlayerGrazed ( int grazeCount ) {
        OnCurrentPlayerGrazed?.Invoke ( grazeCount );
    }

    private void OnCurrentPlayerDamageMultiplierIncreased ( float damageMultiplier ) {
        StopSound?.Invoke ( Constants.ON_PLAYER_DAMAGE_MULTIPLIER_DECREASED_SOUND );
        PlaySound?.Invoke ( Constants.ON_PLAYER_DAMAGE_MULTIPLIER_INCREASED_SOUND );

        OnCurrentPlayerDamageMultiplierChanged?.Invoke ( damageMultiplier );
    }

    private void OnCurrentPlayerDamageMultiplierDecreased ( float damageMultiplier ) {
        StopSound?.Invoke ( Constants.ON_PLAYER_DAMAGE_MULTIPLIER_INCREASED_SOUND );
        PlaySound?.Invoke ( Constants.ON_PLAYER_DAMAGE_MULTIPLIER_DECREASED_SOUND );

        OnCurrentPlayerDamageMultiplierChanged?.Invoke ( damageMultiplier );
    }

    private void CurrentEnemyHit ( float health ) {
        OnCurrentEnemyHit?.Invoke ( health );
    }
  
    #endregion


    #region Explosion Handlers

    private void StartExplosion ( ) {
        ChangeGameState ( newState: GameState.Transitioning );

        PlaySound?.Invoke ( Constants.ON_EXPLOSION_SOUND );
        
        ShakeCamera?.Invoke ( 1.75f, 0.5f );

        PlayExplosion?.Invoke ( );
    }

    private void OnExplosionComplete ( ) {
        ChangeGameState ( newState: GameState.Chatting );

        ChangeInGameState ( newState: InGameState.PostExplosion );
    }
  
    #endregion


    #region Character Managers

    private Player AddPlayer ( GameObject playerObject ) {
        GameObject playerInstance = Instantiate ( playerObject, BASE_POSITION_PLAYER, Quaternion.identity );
        
        currentPlayer = playerInstance.GetComponent<Player> ( );
        return currentPlayer;
    }

    private Enemy AddEnemy ( GameObject enemyObject ) {
        GameObject enemyInstance = Instantiate ( enemyObject, BASE_POSITION_ENEMY, Quaternion.identity );

        currentEnemy = enemyInstance.GetComponent<Enemy> ( );
        return currentEnemy;
    }

    private Character AddBystander1 ( GameObject bystanderObject ) {
        GameObject bystanderInstance = Instantiate ( bystanderObject, BASE_POSITION_BYSTANDER_1, Quaternion.identity );
        
        Character bystander = bystanderInstance.GetComponent<Character> ( );
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


    private void EndGame ( Ending ending ) {
        ChangeGameState ( newState: GameState.Ended );

        float winnerHP;
        switch ( ending ) {
            case Ending.UtsuhoWin:
                winnerHP = currentEnemy.Health;
                break;
            
            case Ending.SagumeWin:
                winnerHP = currentEnemy.Health;
                break;
            
            case Ending.AyaWin:
                winnerHP = currentPlayer.Health;
                break;

            default:
                winnerHP = -1;
                break;
        }

        OnEndGame?.Invoke ( ending, winnerHP );
        
        ClearAllCharacters ( );

        ClearAllBullets?.Invoke ( );
    }

    #endregion
}

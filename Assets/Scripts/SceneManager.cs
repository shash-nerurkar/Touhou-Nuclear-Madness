using System;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    #region Serialized Fields

    [ Header ( "Dialogue" ) ]
    [ SerializeField ] [ Range ( 2.0f, 5.0f ) ] private float AchievementDialogueShowDelay;


    [ Header ( "Transition" ) ]
    [ SerializeField ] [ Range ( 0.0f, 4.0f ) ] private float transitionFadeOutDelay;


    [ Header ( "Combat" ) ]
    [ SerializeField ] private Vector3 BASE_POSITION_ENEMY  = new Vector2 ( 7.5f, 0f );
    
    [ SerializeField ] private Vector3 BASE_POSITION_PLAYER = new Vector2 ( -7.5f, 0f );
    
    [ SerializeField ] private Vector3 BASE_POSITION_BYSTANDER_1 = new Vector2 ( -7.5f, 3f );


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

    private Timer transitionDelayTimer;

    private Timer delayTimer;

    private Timer fightTimer;

    private const float fightMaxTime = 9999f;

    private float [ ] fightScores;


    #region Current run data

    private int currentFightIndex;

    private Player currentPlayer;

    private Enemy currentEnemy;

    private List<Character> currentOtherCharacters = new List<Character> ( );

    #endregion
    

    #region Current instance data

    private GameState currentGameState;

    private InGameState currentInGameState;

    private int currentRunCount = 0;

    private int currentWinCount = 0;

    private static GameDifficulty currentGameDifficulty;
    public static GameDifficulty CurrentGameDifficulty { get => currentGameDifficulty; }

    #endregion


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

    public static event Action OnChaosDifficultyUnlocked;

    public static event Action<GameState> ChangeGameStateOnHUD;

    public static event Action<string> UpdateFightTimer;

    public static event Action<int, string> UpdateScore;

    #endregion


    #region Methods

    private void Start ( ) {
        ChangeGameState ( newState: GameState.MainMenu );
        ChangeInGameState ( newState: InGameState.MainMenu );
    }

    private void FixedUpdate ( ) {
        PlayerPosition = currentPlayer != null ? currentPlayer.transform.position : BASE_POSITION_PLAYER;

        if ( currentGameState == GameState.Playing && fightTimer != null && fightTimer.IsRunning )
            UpdateFightTimer?.Invoke ( Timer.DisplayTime ( fightMaxTime - fightTimer.TimeRemaining ) );
    }


    #region Event Subscriptions

    private void Awake ( ) {
        transitionDelayTimer = gameObject.AddComponent<Timer> ( );
        delayTimer = gameObject.AddComponent<Timer> ( );
        fightTimer = gameObject.AddComponent<Timer> ( );
        fightScores = new float [ ] {
            Mathf.Infinity,
            Mathf.Infinity,
        };

        InputManager.PopDialogueAction += OnDialoguePopped;

        BackgroundImage.OnExplosionAnimationComplete += OnExplosionComplete;

        Transition.OnFadeInBeginning += OnTransitionFadeInStart;
        Transition.OnFadeInCompleted += OnTransitionFadeInEnd;
        Transition.OnFadeOutBeginning += OnTransitionFadeOutStart;
        Transition.OnFadeOutCompleted += OnTransitionFadeOutEnd;

        HUD.OnDialogueSequenceCompleted += OnDialogueSequenceComplete;

        MainMenuPanel.OnChaosDifficultyToggledAction += OnChaosDifficultyToggled;
    }

    private void OnDestroy ( ) {
        InputManager.PopDialogueAction -= OnDialoguePopped;

        BackgroundImage.OnExplosionAnimationComplete -= OnExplosionComplete;

        Transition.OnFadeInBeginning -= OnTransitionFadeInStart;
        Transition.OnFadeInCompleted -= OnTransitionFadeInEnd;
        Transition.OnFadeOutBeginning -= OnTransitionFadeOutStart;
        Transition.OnFadeOutCompleted -= OnTransitionFadeOutEnd;

        HUD.OnDialogueSequenceCompleted -= OnDialogueSequenceComplete;

        MainMenuPanel.OnChaosDifficultyToggledAction -= OnChaosDifficultyToggled;
    }

    #endregion


    #region State Handlers

    private void ChangeGameState ( GameState newState ) {
        currentGameState = newState;

        OnGameStateChanged?.Invoke ( currentGameState );
    }

    private void ChangeInGameState ( InGameState newState ) {
        currentInGameState = newState;

        switch ( currentInGameState ) {
            case InGameState.MainMenu:
                PlaySound?.Invoke ( Constants.MAIN_MENU_MUSIC );
                
                currentFightIndex = 0;
                
                break;

            case InGameState.PreExplosion:
                ChangeBackgroundScene?.Invoke ( 0 );

                break;
        }

        OnInGameStateChanged?.Invoke ( currentInGameState );
    }
    
    #endregion


    #region Transition

    private void OnTransitionFadeInStart ( ) { }

    private void OnTransitionFadeOutStart ( ) { }

    private void OnTransitionFadeInEnd ( ) {
        switch ( currentInGameState ) {
            case InGameState.MainMenu:
                StopSound?.Invoke ( Constants.MAIN_MENU_MUSIC );

                transitionDelayTimer.StartTimer ( maxTime: transitionFadeOutDelay, onTimerFinish: ( ) => {             
                    PlaySound?.Invoke ( Constants.CHATTING_MUSIC );

                    ChangeInGameState ( newState: InGameState.PreExplosion );

                    ChangeGameStateOnHUD?.Invoke ( GameState.Chatting );

                    AddPlayer ( playerSagumeObject );
                    AddEnemy ( enemyUtsuhoObject );
                    AddBystander1 ( playerAyaObject );

                    TransitionFadeOut?.Invoke ( );
                } );

                break;

            case InGameState.PostFight1Branch2:
                ClearAllCharacters ( );

                transitionDelayTimer.StartTimer ( maxTime: transitionFadeOutDelay, onTimerFinish: ( ) => {           
                    StopSound?.Invoke ( Constants.SCENE_01_MUSIC );

                    ChangeBackgroundScene?.Invoke ( 1 );

                    AddPlayer ( playerAyaObject );
                    AddEnemy ( enemySagumeObject );

                    TransitionFadeOut?.Invoke ( );
                } );
                
                break;

            case InGameState.EndGame:
                ChangeInGameState ( newState: InGameState.MainMenu );
                
                ChangeGameStateOnHUD?.Invoke ( GameState.MainMenu );

                StopSound?.Invoke ( Constants.ON_END_GAME_LOSS_MUSIC );
                StopSound?.Invoke ( Constants.ON_END_GAME_WIN_MUSIC );

                TransitionFadeOut?.Invoke ( );

                break;
        }

    }

    private void OnTransitionFadeOutEnd ( ) {
        switch ( currentInGameState ) {
            case InGameState.MainMenu:
                ChangeGameState ( newState: GameState.MainMenu );
                
                break;

            case InGameState.PreExplosion:
                ChangeGameState ( newState: GameState.Chatting );
                
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
        ChangeGameState ( newState: GameState.BlockingInput );

        TransitionRemoveNextPlayerCharacter?.Invoke ( );

        TransitionFadeIn?.Invoke ( );
    }

    private void StartTransition ( Characters nextPlayerCharacter ) {
        ChangeGameState ( newState: GameState.BlockingInput );

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
        switch ( currentInGameState ) {
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

        foreach ( Character character in currentOtherCharacters )
            Destroy ( character.gameObject );
        currentOtherCharacters.Clear ( );

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

        fightTimer.StartTimer ( maxTime: fightMaxTime, onTimerFinish: ( ) => { } );

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
        
        OnFightStarted ( currentPlayer.Health, currentPlayer.Data.BombCount, currentPlayer.Data.Ability2Count, 0, 1, currentEnemy.Data.Health );
    }

    private void OnFightComplete ( bool didWin ) {
        ChangeGameState ( newState: GameState.Chatting );

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

        fightTimer.PauseTimer ( );
        if ( didWin ) {
            float newScore = fightMaxTime - fightTimer.TimeRemaining;
            
            if ( newScore < fightScores [ currentFightIndex ] ) {
                fightScores [ currentFightIndex ] = newScore;
                UpdateScore ( currentFightIndex, Timer.DisplayTime ( fightScores [ currentFightIndex ] ) );
            }
        }
        
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
        ChangeGameState ( newState: GameState.BlockingInput );

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
        currentOtherCharacters.Add ( bystander );

        return bystander;
    }

    private void ClearAllCharacters ( ) {
        foreach ( Character character in currentOtherCharacters )
            Destroy ( character.gameObject );
        currentOtherCharacters.Clear ( );

        Destroy ( currentPlayer.gameObject );
        currentPlayer = null;

        Destroy ( currentEnemy.gameObject );
        currentEnemy = null;
    }

    #endregion


    #region Achievements

    private Dictionary<Achievement, bool> AchievementStatuses = new Dictionary<Achievement, bool> {
        { Achievement.Chaos_Difficulty, false },
        { Achievement.Skip_Dialogues_Mode, false }
    };

    private Dictionary<Achievement, bool> AchievementUnlockableStatuses { get {
        return new Dictionary<Achievement, bool> {
            { 
                Achievement.Chaos_Difficulty, 
                currentRunCount == Constants.SCENEMANAGER_CHAOS_MODE_RUN_COUNT_THRESHOLD && 
                    !AchievementStatuses [ Achievement.Chaos_Difficulty ]
            },
            { 
                Achievement.Skip_Dialogues_Mode, 
                currentRunCount >= Constants.SCENEMANAGER_SKIP_DIALOGUES_MODE_RUN_COUNT_THRESHOLD && 
                    currentWinCount > 0 && 
                        !AchievementStatuses [ Achievement.Skip_Dialogues_Mode ] 
            },
        };
    } }
    
    private Dictionary<Achievement, Action> OnAchievementUnlockActions { get {
        return new Dictionary<Achievement, Action> {
            { 
                Achievement.Chaos_Difficulty, 
                ( ) => { 
                    OnChaosDifficultyUnlocked?.Invoke ( ); 
                }
            },
            { 
                Achievement.Skip_Dialogues_Mode, 
                ( ) => {
                    foreach ( List<Dialogue> dialogueSequence in Constants.INGAME_DIALOGUE_SEQUENCES )
                        dialogueSequence.Clear ( );
                    transitionFadeOutDelay = 0;
                } 
            },
        };
    } }

    private void ShowAchievement ( Achievement achievement ) {
        StopSound?.Invoke ( Constants.ON_END_GAME_LOSS_MUSIC );
        StopSound?.Invoke ( Constants.ON_END_GAME_WIN_MUSIC );
        PlaySound?.Invoke ( Constants.ON_ACHIEVEMENT_UNLOCKED_SOUND );
        
        Constants.DIALOGUE_SEQUENCE_GAME_ENDED.Insert ( 0, Constants.DIALOGUES_ACHIEVEMENTS [ achievement ] );

        OnAchievementUnlockActions [ achievement ]?.Invoke ( );
    }

    private void HideAchievement ( Achievement achievement ) {
        ChangeGameState ( newState: GameState.BlockingInput );
        delayTimer.StartTimer (
            maxTime: AchievementDialogueShowDelay, onTimerFinish: ( ) => { 
                ChangeGameState ( newState: GameState.Ended );
                ChangeInGameState ( newState: InGameState.EndGame );
            }
        );

        Constants.DIALOGUE_SEQUENCE_GAME_ENDED.Remove ( Constants.DIALOGUES_ACHIEVEMENTS [ achievement ] );

        AchievementStatuses [ achievement ] = true;
    }

    #endregion


    private void OnChaosDifficultyToggled ( bool chaosDifficultyToggleFlag ) {
        currentGameDifficulty = chaosDifficultyToggleFlag ? GameDifficulty.Chaos : GameDifficulty.Default;
    }

    private void EndGame ( Ending ending ) {
        ++currentRunCount;
        if ( ending == Ending.AyaWin) 
            ++currentWinCount;

        foreach ( KeyValuePair<Achievement, bool> achievement in AchievementUnlockableStatuses )
            if ( achievement.Value ) ShowAchievement ( achievement.Key );

        ChangeGameState ( newState: GameState.Ended );
        ChangeInGameState ( newState: InGameState.EndGame );
        
        foreach ( KeyValuePair<Achievement, bool> achievement in AchievementUnlockableStatuses )
            if ( achievement.Value ) HideAchievement ( achievement.Key );

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

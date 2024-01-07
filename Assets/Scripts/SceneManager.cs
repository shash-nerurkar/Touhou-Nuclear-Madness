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

    public static GameState CurrentGameState;

    public static InGameState CurrentInGameState;

    public static Vector3 PlayerPosition;

    private Player currentPlayer;

    private Enemy currentEnemy;

    private List<Character> otherCharacters;

    #endregion


    #region Actions

    public static event Action<GameState> OnGameStateChanged;

    public static event Action<InGameState> OnInGameStateChanged;

    public static event Action<float, float> OnFightStarted;

    public static event Action<float> OnCurrentPlayerHit;

    public static event Action<float> OnCurrentEnemyHit;

    public static event Action ContinueDialogueSequence;

    public static event Action HideDialogueBox;

    #endregion


    #region Methods

    private void Awake ( ) {
        otherCharacters = new List<Character> ( );
        
        InputManager.PopDialogueAction += OnDialoguePopped;

        HUD.OnExplosionComplete += OnExplosionComplete;
        HUD.OnPostExplosionSequenceComplete += () => { StartFight ( 0 ); };
        HUD.OnTransitionFadeInComplete += OnTransitionFadeIn;
        HUD.OnTransitionFadeOutComplete += OnTransitionFadeOut;
        HUD.EndGame += EndGame;
    }

    private void Start ( ) {
        ChangeGameState ( newState: GameState.MainMenu );
    }

    private void Update ( ) {
        if ( currentPlayer != null )
            PlayerPosition = currentPlayer.transform.position;
    }

    private void OnDialoguePopped ( ) {
        SoundManager.instance.Play ( Constants.ON_DIALOGUE_POP_SOUND );

        switch ( CurrentGameState ) {
            case GameState.MainMenu:
                ChangeGameState ( newState: GameState.Chatting );
                SoundManager.instance.Play ( Constants.CHATTING_MUSIC );

                GameObject playerSagumeInstance = Instantiate ( playerSagumeObject, Constants.BASE_POSITION_PLAYER, Quaternion.identity );
                currentPlayer = playerSagumeInstance.GetComponent<Player> ( );
                if ( currentPlayer != null ) {
                    currentPlayer.OnLose += ( ) => { OnFightComplete ( didWin: false, fightIndex: 0 ); };
                    currentPlayer.OnPlayerShoot += CurrentPlayerShoot;
                    currentPlayer.OnHit += CurrentPlayerHit;
                }

                GameObject enemyUtsuhoInstance = Instantiate ( enemyUtsuhoObject, Constants.BASE_POSITION_ENEMY, Quaternion.identity );
                currentEnemy = enemyUtsuhoInstance.GetComponent<Enemy> ( );
                if ( currentEnemy != null ) {
                    currentEnemy.OnLose += ( ) => { OnFightComplete ( didWin: true, fightIndex: 0 ); };
                    currentEnemy.OnHit += CurrentEnemyHit;
                }

                AddBystanderAya ( );

                ChangeInGameState ( newState: InGameState.PreExplosion );
                break;
                
            case GameState.Chatting:
                ContinueDialogueSequence?.Invoke ( );
                break;

            case GameState.Ended:
                SoundManager.instance.Stop ( Constants.ON_END_GAME_LOSS_SOUND );
                SoundManager.instance.Stop ( Constants.ON_END_GAME_WIN_SOUND );
                ChangeGameState ( newState: GameState.MainMenu );
                break;
        }
    }

    private void OnExplosionComplete ( ) {
        ChangeInGameState ( newState: InGameState.PostExplosion );
    }

    private void OnTransitionFadeIn ( ) {
        SoundManager.instance.Stop( Constants.SCENE_01_MUSIC );

        ClearAllCharacters ( );

        GameObject playerAyaInstance = Instantiate ( playerAyaObject, Constants.BASE_POSITION_PLAYER, Quaternion.identity );
        currentPlayer = playerAyaInstance.GetComponent<Player> ( );
        if ( currentPlayer != null ) {
            currentPlayer.OnLose += ( ) => { OnFightComplete ( didWin: false, fightIndex: 1 ); };
            currentPlayer.OnPlayerShoot += CurrentPlayerShoot;
            currentPlayer.OnHit += CurrentPlayerHit;
        }
        
        GameObject enemySagumeInstance = Instantiate ( enemySagumeObject, Constants.BASE_POSITION_ENEMY, Quaternion.identity );
        currentEnemy = enemySagumeInstance.GetComponent<Enemy> ( );
        if ( currentEnemy != null ) {
            currentEnemy.OnLose += ( ) => { OnFightComplete ( didWin: true, fightIndex: 1 ); };
            currentEnemy.OnHit += CurrentEnemyHit;
        }
    }

    private void OnTransitionFadeOut ( ) {
        StartFight ( 1 );
    }

    private void StartFight ( int fightIndex ) {
        ChangeGameState ( newState: GameState.Playing );
        SoundManager.instance.Play( fightIndex == 0 ? Constants.SCENE_01_MUSIC : Constants.SCENE_02_MUSIC );

        foreach ( Character character in otherCharacters )
            Destroy ( character.gameObject );
        otherCharacters.Clear ( );

        currentPlayer.SetSelected ( true );
        currentEnemy.SetActive ( true );
        
        OnFightStarted ( currentPlayer.Health, currentEnemy.Health );
    }

    private void OnFightComplete ( bool didWin, int fightIndex ) {
        ChangeGameState ( newState: GameState.Chatting );

        currentPlayer.SetSelected ( false );
        currentEnemy.SetActive ( false );

        if ( fightIndex == 0 )
            AddBystanderAya ( );
        else if ( fightIndex == 1 )
            SoundManager.instance.Stop( Constants.SCENE_02_MUSIC );

        if ( didWin )
            ChangeInGameState ( newState: fightIndex == 0 ? InGameState.PostFight1Branch2 : InGameState.PostFight2Branch2 );
        else 
            ChangeInGameState ( newState: fightIndex == 0 ? InGameState.PostFight1Branch1 : InGameState.PostFight2Branch1 );

        currentEnemy.OnLose -= ( ) => { OnFightComplete ( didWin: true, fightIndex: fightIndex ); };
        currentEnemy.OnHit -= CurrentEnemyHit;
        currentPlayer.OnLose -= ( ) => { OnFightComplete ( didWin: false,  fightIndex: fightIndex ); };
        currentPlayer.OnPlayerShoot -= CurrentPlayerShoot;
        currentPlayer.OnHit -= CurrentPlayerHit;
    }

    private void EndGame ( ) {
        ChangeGameState ( newState: GameState.Ended );
        
        ClearAllCharacters ( );
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

    private void ChangeGameState ( GameState newState ) {
        CurrentGameState = newState;

        switch ( CurrentGameState ) {
            case GameState.Chatting:
                HideDialogueBox?.Invoke ( );
                break;
        }

        OnGameStateChanged?.Invoke ( CurrentGameState );
    }

    private void ChangeInGameState ( InGameState newState ) {
        CurrentInGameState = newState;

        OnInGameStateChanged?.Invoke ( CurrentInGameState );
    }

    private void CurrentPlayerHit ( float health ) {
        SoundManager.instance.Play ( Constants.ON_PLAYER_HIT_SOUND );
        OnCurrentPlayerHit?.Invoke ( health );
    }

    private void CurrentPlayerShoot ( ) {
        SoundManager.instance.Play ( Constants.ON_PLAYER_SHOOT_SOUND );
    }

    private void CurrentEnemyHit ( float health ) => OnCurrentEnemyHit?.Invoke ( health );
    
    private void AddBystanderAya ( ) {
        GameObject bystanderAyaInstance = Instantiate ( playerAyaObject, Constants.BASE_POSITION_BYSTANDER_1, Quaternion.identity );
        Character bystanderAya = bystanderAyaInstance.GetComponent<Character> ( );
        if ( bystanderAya != null )
            otherCharacters.Add ( bystanderAya );
    }

    private void OnDestroy ( ) {
        InputManager.PopDialogueAction -= OnDialoguePopped;

        HUD.OnExplosionComplete -= OnExplosionComplete;
        HUD.OnPostExplosionSequenceComplete -= () => { StartFight ( 0 ); };
        HUD.OnTransitionFadeInComplete -= OnTransitionFadeIn;
        HUD.OnTransitionFadeOutComplete -= OnTransitionFadeOut;
        HUD.EndGame -= EndGame;
    }

    #endregion
}

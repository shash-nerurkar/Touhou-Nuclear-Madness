using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Fields

    private PlayerInputActions.PlayerFlyActions FlyActions;

    private PlayerInputActions.PlayerUIActions UIActions;

    #endregion


    #region Actions
    
    public static event Action<Vector2> OnPlayerMoveAction;

    public static event Action<bool> OnPlayerShootAction;

    public static event Action<bool> OnPlayerAbility1Action;

    public static event Action<bool> OnPlayerAbility2Action;

    public static event Action PopDialogueAction;

    #endregion


    #region Methods

    private void Awake ( ) {
        PlayerInputActions playerInputActions = new PlayerInputActions();
        FlyActions = playerInputActions.PlayerFly;
        UIActions = playerInputActions.PlayerUI;

        SceneManager.OnGameStateChanged += OnGameStateChanged;

		UIActions.PopDialogue.performed += context => {
			PopDialogueAction?.Invoke( );
		};

		FlyActions.Ability1.performed += context => {
            OnPlayerAbility1Action?.Invoke ( true );
		};

		FlyActions.Ability2.performed += context => {
			// OnPlayerAbility2Action?.Invoke( true );
		};
    }

    private void FixedUpdate ( ) {
        OnPlayerMoveAction?.Invoke ( FlyActions.Movement.ReadValue<Vector2>( ) );

        OnPlayerShootAction?.Invoke ( FlyActions.Shoot.ReadValue<float>( ) != 0 );
    }

    private void OnGameStateChanged ( GameState gameState ) {
        switch ( gameState ) {
            case GameState.MainMenu:
                FlyActions.Disable ( );
                UIActions.Enable ( );
                break;
            
            case GameState.Playing:
                FlyActions.Enable ( );
                UIActions.Disable ( );
                break;
            
            case GameState.Chatting:
                FlyActions.Disable ( );
                UIActions.Enable ( );
                break;
            
            case GameState.Ended:
                FlyActions.Disable ( );
                UIActions.Enable ( );
                break;
            
            case GameState.BlockingInput:
                FlyActions.Disable ( );
                UIActions.Disable ( );
                break;
        }
    }

    private void OnDestroy ( ) {
        SceneManager.OnGameStateChanged -= OnGameStateChanged;
    }

    #endregion
}

using UnityEngine;

public abstract class Enemy : Character
{
    #region Serialized Fields

    [ SerializeField ] private float moveCooldownTime;

    [ SerializeField ] [ Range ( 0.0f, 4.0f ) ] private float moveDistanceRange;

    #endregion


    #region Fields

    protected enum State {
        Chatting,
        Idle,
        Move
    }

    protected State currentState;

    private Timer moveCooldownTimer;

    private Vector3 moveDestination;

    #endregion


    #region Methods

    protected override void Awake ( ) {
        base.Awake ( );

        moveCooldownTimer = gameObject.AddComponent<Timer> ( );
    }

    protected virtual void Start ( ) => ChangeState ( State.Chatting );

    protected virtual void ChangeState ( State newState ) {
        currentState = newState;

        switch ( currentState ) {
            case State.Chatting:
                cd.enabled = false;
                if ( moveCooldownTimer != null )
                    moveCooldownTimer.PauseTimer ( );
                break;
            
            case State.Idle:
                cd.enabled = true;
                moveCooldownTimer.StartTimer ( maxTime: moveCooldownTime, onTimerFinish: OnMoveCooldownTimerFinished );
                animator.SetBool ( "isMoving", false );
                break;
            
            case State.Move:
                animator.SetBool ( "isMoving", true );
                break;
        }
    }

    protected override void OnGetHit ( ) {
        base.OnGetHit();
        
        ChangeState ( State.Idle );
    }

    protected override void OnLoseFight ( ) {
        base.OnLoseFight();
        
        ChangeState ( State.Chatting );
    }

    private void Update ( ) {
        switch ( currentState ) {
            case State.Move:
                rb.MovePosition( transform.position + ( moveDestination - transform.position ).normalized * speed * Time.deltaTime );
                
                if ( Vector2.Distance ( transform.position, moveDestination ) < 0.2f ) {
                    ChangeState ( State.Idle );
                }
                break;
        }
    }

    private void OnMoveCooldownTimerFinished ( ) {
        moveDestination = Constants.BASE_POSITION_ENEMY + new Vector3 ( Random.Range ( -0.5f, 0.5f ), Random.Range ( -moveDistanceRange, moveDistanceRange ) );

        ChangeState ( State.Move );
    }

    public void SetActive ( bool isActive ) => ChangeState ( isActive ? State.Idle : State.Chatting );

    #endregion
}
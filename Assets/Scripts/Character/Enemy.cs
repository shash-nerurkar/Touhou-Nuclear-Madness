using UnityEngine;

public abstract class Enemy : Character
{
    #region Serialized Fields

    [ SerializeField ] protected EnemyData data;
    public EnemyData Data { get => data; }

    [ SerializeField ] protected LineRenderer heightIndicatorTop;

    [ SerializeField ] protected LineRenderer heightIndicatorBottom;

    [ SerializeField ] protected LineRenderer attackLineIndicator;

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

        speed = Data.Speed;
        health = Data.Health;
        onHitIDuration = Data.OnHitIDuration;

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
                moveCooldownTimer.StartTimer ( maxTime: Data.MoveCooldownTime, onTimerFinish: OnMoveCooldownTimerFinished );
                animator.SetBool ( "isMoving", false );
                break;
            
            case State.Move:
                animator.SetBool ( "isMoving", true );
                break;
        }
    }

    private void FixedUpdate ( ) {
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
        moveDestination = basePosition + new Vector3 ( Random.Range ( -0.5f, 0.5f ), Random.Range ( -Data.MoveDistanceRange, Data.MoveDistanceRange ) );

        ChangeState ( State.Move );
    }

    protected override void OnGetHit ( ) {
        base.OnGetHit();
        
        ChangeState ( State.Idle );
    }

    protected override void OnLoseFight ( ) {
        base.OnLoseFight();
        
        ChangeState ( State.Chatting );
    }

    public override void ToggleAsCurrent ( bool isCurrent ) {
        base.ToggleAsCurrent ( isCurrent );

        heightIndicatorTop.gameObject.SetActive ( isCurrent );
        heightIndicatorBottom.gameObject.SetActive ( isCurrent );

        ChangeState ( this.isCurrent ? State.Idle : State.Chatting );
    }

    #endregion
}

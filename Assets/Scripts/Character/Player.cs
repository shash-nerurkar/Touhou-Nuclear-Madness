using System;
using UnityEngine;

public class Player : Character
{
    #region Serialized Fields

    [ SerializeField ] private PlayerData data;
    public PlayerData Data { get => data; }


    [ Header ("Player graze") ]
    [ SerializeField ] private PlayerGrazeDetector grazeDetector;


    [ Header ("Player ability 1: Bomb") ]
    [ SerializeField ] private ParticleSystem bombParticleSystem;

    #endregion


    #region Fields

    private Vector3 moveDirection;

    private bool canShoot = true;

    private bool canFireAbility1 = true;

    private bool canFireAbility2 = true;

    private Timer shootCooldownTimer;

    private Timer ability1CooldownTimer;

    private Timer ability2CooldownTimer;

    private int ability1Count;

    private int ability2Count;

    private int grazeCount;

    #endregion


    #region Actions

    public event Action OnPlayerShoot;

    public event Action<int> OnPlayerFiredAbility1;

    public event Action<int> OnPlayerFiredAbility2;

    public static event Action FireAbility1Event;

    public event Action<int> OnGrazed;

    #endregion


    #region Methods


    #region Event Subscriptions

    protected override void Awake ( ) {
        base.Awake ( );

        InputManager.OnPlayerMoveAction += OnMove;
        InputManager.OnPlayerShootAction += OnShoot;
        InputManager.OnPlayerAbility1Action += OnAbility1;
        InputManager.OnPlayerAbility2Action += OnAbility2;

        grazeDetector.OnGrazed += OnGetGrazed;

        speed = Data.Speed;
        health = Data.Health;
        onHitIDuration = Data.OnHitIDuration;
        ability1Count = Data.BombCount;
        ability2Count = Data.Ability2Count;

        shootCooldownTimer = gameObject.AddComponent<Timer> ( );
        ability1CooldownTimer = gameObject.AddComponent<Timer> ( );
        ability2CooldownTimer = gameObject.AddComponent<Timer> ( );
    }

    protected virtual void OnDestroy ( ) {
        InputManager.OnPlayerMoveAction -= OnMove;
        InputManager.OnPlayerShootAction -= OnShoot;
        InputManager.OnPlayerAbility1Action -= OnAbility1;
        InputManager.OnPlayerAbility2Action -= OnAbility2;

        grazeDetector.OnGrazed -= OnGetGrazed;
    }

    #endregion


    #region Movement related

    private void OnMove ( Vector2 moveDirection ) {
        if ( moveDirection.magnitude == 0 ) {
            speed = Mathf.Clamp ( speed - 1.0f, 0, Data.Acceleration + Data.Speed );

            if ( speed == 0 )
                this.moveDirection = moveDirection;
        }
        else {
            speed = Mathf.Clamp ( speed + 1.0f, 0, Data.Acceleration + Data.Speed );

            this.moveDirection = moveDirection;
        }

        Vector3 velocity = this.moveDirection.normalized * speed;
        rb.MovePosition ( transform.position + velocity * Time.deltaTime );

        animator.SetBool ( "isMoving", this.moveDirection.magnitude > 0 );
    }

    #endregion


    #region Shoot related

    private void OnShoot ( bool isShooting ) {
        if ( isShooting && canShoot ) {
            Shoot ( );

            canShoot = false;
            shootCooldownTimer.StartTimer ( maxTime: Data.ShootCooldown, onTimerFinish: () => {
                canShoot = true;
            } );

            OnPlayerShoot?.Invoke ( );
        }
    }

    public void Shoot ( ) {
        GameObject bulletInstance = Instantiate ( original: Data.BulletObjects [ UnityEngine.Random.Range ( 0, Data.BulletObjects.Length ) ], position: pivot.position, rotation: Quaternion.identity );

        Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
        bullet.Init ( 
            pathType: BulletPathType.Straight, 
            shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ), 
            speed: Data.BulletSpeed,
            damage: Data.BulletDamage
        );
    }

    #endregion


    #region Graze related

    protected virtual void OnGetGrazed ( Collider2D collided ) {
        ++grazeCount;

        OnGrazed?.Invoke ( grazeCount );
    }

    #endregion


    #region Ability1 related

    private void OnAbility1 ( bool isFiringAbility1 ) {
        if ( isFiringAbility1 && canFireAbility1 && ability1Count > 0 ) {
            FireAbility1 ( );
            
            canFireAbility1 = false;
            ability1CooldownTimer.StartTimer ( maxTime: Data.BombCooldown, onTimerFinish: () => {
                canFireAbility1 = true;
            } );

            OnPlayerFiredAbility1?.Invoke ( ability1Count );
        }
    }

    private void FireAbility1 ( ) {
        --ability1Count;
        
        bombParticleSystem.Play ( );

        FireAbility1Event?.Invoke ( );
    }

    #endregion


    #region Ability2 related

    private void OnAbility2 ( bool isFiringAbility2 ) {
        if ( isFiringAbility2 && canFireAbility2 && ability2Count > 0  ) {
            FireAbility2 ( );
            
            canFireAbility2 = false;
            ability2CooldownTimer.StartTimer ( maxTime: Data.Ability2Cooldown, onTimerFinish: () => {
                canFireAbility2 = true;
            } );

            OnPlayerFiredAbility2?.Invoke ( ability2Count );
        }
    }

    private void FireAbility2 ( ) {
        --ability2Count;
    }

    #endregion


    public override void ToggleAsCurrent ( bool isCurrent ) {
        base.ToggleAsCurrent ( isCurrent );
        
        grazeDetector.ToggleEnabled ( this.isCurrent );
    }
 
    protected override void OnLoseFight ( ) {
        grazeDetector.ToggleEnabled ( false );

        base.OnLoseFight ( );
    }

    #endregion
}

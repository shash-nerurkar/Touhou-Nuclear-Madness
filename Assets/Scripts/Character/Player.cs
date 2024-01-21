using System;
using UnityEngine;

public class Player : Character
{
    #region Serialized Fields

    [ SerializeField ] private PlayerData data;
    public PlayerData Data { get => data; }


    [ Header ("Player graze") ]
    [ SerializeField ] private PlayerGrazeDetector grazeDetector;

    [ SerializeField ] private ParticleSystem grazeIncreaseEffectParticleSystem;

    [ SerializeField ] private ParticleSystem grazeDecreaseEffectParticleSystem;


    [ Header ("Player ability 1: Bomb") ]
    [ SerializeField ] private ParticleSystem bombEffectParticleSystem;

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

    private Timer grazeDamageMultiplierTimer;

    private float damageMultiplier;

    private bool isGrazeDamageMultiplierActive;

    #endregion


    #region Actions

    public event Action OnPlayerShoot;

    public event Action<int> OnPlayerFiredAbility1;

    public event Action<int> OnPlayerFiredAbility2;

    public static event Action FireAbility1Event;

    public event Action<int> OnGrazed;

    public event Action<float> OnDamageMultiplierIncreased;

    public event Action<float> OnDamageMultiplierDecreased;

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
        damageMultiplier = 1;
        health = SceneManager.CurrentGameDifficulty == GameDifficulty.Chaos ? 1 : Data.Health;
        onHitIDuration = Data.OnHitIDuration;
        ability1Count = Data.BombCount;
        ability2Count = Data.Ability2Count;

        shootCooldownTimer = gameObject.AddComponent<Timer> ( );
        ability1CooldownTimer = gameObject.AddComponent<Timer> ( );
        ability2CooldownTimer = gameObject.AddComponent<Timer> ( );
        grazeDamageMultiplierTimer = gameObject.AddComponent<Timer> ( );
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
            damage: Data.BulletDamage * damageMultiplier,
            bulletColor: Constants.CalculateColorForDamageMultiplier ( Constants.COLOR_PLAYER_BULLET_BASE, Constants.COLOR_PLAYER_BULLET_FINAL, damageMultiplier ),
            bulletBorderColor: Constants.CalculateColorForDamageMultiplier ( Constants.COLOR_PLAYER_BASE, Constants.COLOR_PLAYER_FINAL, damageMultiplier )
        );
    }

    #endregion


    #region Graze related

    protected virtual void OnGetGrazed ( Collider2D collided ) {
        ++grazeCount;

        if ( SceneManager.CurrentGameDifficulty == GameDifficulty.Chaos ) {
            ToggleDamageMultiplier ( true );
            
            Timer grazeDamageMultiplierInstanceTimer = gameObject.AddComponent<Timer> ( );
            grazeDamageMultiplierInstanceTimer.StartTimer ( maxTime: Data.GrazeDamageMultiplierDuration, onTimerFinish: ( ) => {
                ToggleDamageMultiplier ( false );

                Destroy ( grazeDamageMultiplierInstanceTimer );
            } );
        }
        else {
            if ( !isGrazeDamageMultiplierActive )
                ToggleDamageMultiplier ( true );
            
            grazeDamageMultiplierTimer.StartTimer ( maxTime: Data.GrazeDamageMultiplierDuration, onTimerFinish: ( ) => ToggleDamageMultiplier ( false ) );
        }
        

        OnGrazed?.Invoke ( grazeCount );
    }

    private void ToggleDamageMultiplier ( bool toggleFlag ) {
        isGrazeDamageMultiplierActive = toggleFlag;

        if ( isGrazeDamageMultiplierActive ) {
            damageMultiplier *= Data.GrazeDamageMultiplier;
            
            grazeDecreaseEffectParticleSystem.Stop ( );
            grazeIncreaseEffectParticleSystem.Play ( );

            OnDamageMultiplierIncreased?.Invoke ( damageMultiplier );
        }
        else {
            damageMultiplier /= Data.GrazeDamageMultiplier;
            
            grazeIncreaseEffectParticleSystem.Stop ( );
            grazeDecreaseEffectParticleSystem.Play ( );

            OnDamageMultiplierDecreased?.Invoke ( damageMultiplier );
        }
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
        
        bombEffectParticleSystem.Play ( );

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


    protected override void OnGetHit ( ) {
        base.OnGetHit();
        
        grazeDetector.ToggleEnabled ( false );
    }

    protected override void OnHitTimerFinish ( ) {
        base.OnHitTimerFinish();
        
        grazeDetector.ToggleEnabled ( true );
    }

    protected override void OnLoseFight ( ) {
        grazeDetector.ToggleEnabled ( false );

        base.OnLoseFight ( );
    }
   
    public override void ToggleAsCurrent ( bool isCurrent ) {
        base.ToggleAsCurrent ( isCurrent );
        
        grazeDetector.ToggleEnabled ( this.isCurrent );
    }
  
    #endregion
}

using System;
using UnityEngine;

public class Player : Character
{
    #region Serialized Fields

    [ SerializeField ] private PlayerData data;
    public PlayerData Data { get => data; }


    [ Header ( "Player Shoot" ) ]

    [ SerializeField ] [ Range ( 1.0f, 20.0f ) ] private float shootEnergyRegenMultiplierMax = 10.0f;

    [ SerializeField ] [ Range ( 0.0f, 2.0f ) ] private float shootEnergyRegenMultiplierTickValue = 0.75f;


    [ Header ( "Player graze" ) ]

    [ SerializeField ] private PlayerGrazeDetector grazeDetector;

    [ SerializeField ] private ParticleSystem grazeIncreaseEffectParticleSystem;

    [ SerializeField ] private ParticleSystem grazeDecreaseEffectParticleSystem;


    [ Header ( "Player ability 1: Bomb" ) ]

    [ SerializeField ] private ParticleSystem bombEffectParticleSystem;

    #endregion


    #region Fields

    private Vector3 moveDirection;

    private bool CanShoot { get { return !shootCooldownTimer.IsRunning && shootEnergy >= Data.ShootEnergyPerBullet ; } }

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

    protected float shootEnergy;
    public float ShootEnergy { get => shootEnergy; }

    private Timer shootEnergyRegenTimer;

    private float currentShootEnergyRegenMultiplier;

    #endregion


    #region Actions

    public event Action OnShootAction;

    public event Action<float> OnShootEnergyChanged;

    public event Action<int> OnFiredAbility1;

    public event Action<int> OnFiredAbility2;

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
        shootEnergy = Data.ShootEnergy;
        currentShootEnergyRegenMultiplier = 1;
        onHitIDuration = Data.OnHitIDuration;
        ability1Count = Data.BombUseCount;
        ability2Count = Data.Ability2UseCount;

        shootCooldownTimer = gameObject.AddComponent<Timer> ( );
        ability1CooldownTimer = gameObject.AddComponent<Timer> ( );
        ability2CooldownTimer = gameObject.AddComponent<Timer> ( );
        grazeDamageMultiplierTimer = gameObject.AddComponent<Timer> ( );
        shootEnergyRegenTimer = gameObject.AddComponent<Timer> ( );
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
        if ( isShooting ) {
            if ( CanShoot ) {
                shootCooldownTimer.StartTimer ( maxTime: Data.ShootCooldown, onTimerFinish: () => { } );

                Shoot ( );

                OnShootAction?.Invoke ( );
            }

            currentShootEnergyRegenMultiplier = 1;
            if ( shootEnergy < Data.ShootEnergyPerBullet && !shootEnergyRegenTimer.IsRunning )
                    shootEnergyRegenTimer.StartTimer ( maxTime: Data.ShootEnergyRegenCycleSizeInSeconds, onTimerFinish: OnShootEnergyRegenTimerFinished );
        }
        else {
            if ( !shootEnergyRegenTimer.IsRunning )
                shootEnergyRegenTimer.StartTimer ( maxTime: Data.ShootEnergyRegenCycleSizeInSeconds / currentShootEnergyRegenMultiplier, onTimerFinish: ( ) => {
                    currentShootEnergyRegenMultiplier = Mathf.Clamp ( currentShootEnergyRegenMultiplier + shootEnergyRegenMultiplierTickValue, 1, shootEnergyRegenMultiplierMax );

                    OnShootEnergyRegenTimerFinished ( );
                } );    
        }
    }

    public void Shoot ( ) {
        SetShootEnergy ( shootEnergy - Data.ShootEnergyPerBullet );

        GameObject bulletInstance = Instantiate ( original: Data.BulletObjects [ UnityEngine.Random.Range ( 0, Data.BulletObjects.Length ) ], position: pivot.position, rotation: Quaternion.identity );

        Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
        bullet.Init ( 
            pathType: BulletPathType.Straight, 
            shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ), 
            speed: Data.BulletSpeed,
            damage: Data.BulletDamage * damageMultiplier,
            bulletColor: Constants.CalculateColorForDamageMultiplier ( Data.BulletColorBaseDamage, Data.BulletColorMaxDamage, damageMultiplier ),
            bulletBorderColor: Constants.CalculateColorForDamageMultiplier ( Data.BulletBorderColorBaseDamage, Data.BulletBorderColorMaxDamage, damageMultiplier )
        );
    }

    private void OnShootEnergyRegenTimerFinished ( ) => SetShootEnergy ( shootEnergy + Data.ShootEnergyRegenPerCycle );

    private void SetShootEnergy ( float newShootEnergy ) {
        shootEnergy = Mathf.Clamp ( newShootEnergy, 0, Data.ShootEnergy );

        OnShootEnergyChanged?.Invoke ( shootEnergy );
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

            OnFiredAbility1?.Invoke ( ability1Count );
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

            OnFiredAbility2?.Invoke ( ability2Count );
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

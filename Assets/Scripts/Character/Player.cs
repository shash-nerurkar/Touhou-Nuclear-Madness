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

    [ SerializeField ] [ Range ( 1.25f, 3.0f ) ] private float grazeDamageMultiplier;

    [ SerializeField ] private float grazeDamageMultiplierDuration;


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
        health = Data.Health;
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
            damage: Data.BulletDamage * damageMultiplier
        );
    }

    #endregion


    #region Graze related

    protected virtual void OnGetGrazed ( Collider2D collided ) {
        ++grazeCount;

        if ( !isGrazeDamageMultiplierActive ) {
            isGrazeDamageMultiplierActive = true;

            damageMultiplier *= grazeDamageMultiplier;
            
            grazeDecreaseEffectParticleSystem.Stop ( );
            grazeIncreaseEffectParticleSystem.Play ( );

            OnDamageMultiplierIncreased?.Invoke ( damageMultiplier );
        }
        grazeDamageMultiplierTimer.StartTimer ( maxTime: grazeDamageMultiplierDuration, onTimerFinish: ( ) => { 
            isGrazeDamageMultiplierActive = false;

            damageMultiplier /= grazeDamageMultiplier;
            
            grazeIncreaseEffectParticleSystem.Stop ( );
            grazeDecreaseEffectParticleSystem.Play ( );

            OnDamageMultiplierDecreased?.Invoke ( damageMultiplier );
        } );

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


    public override void ToggleAsCurrent ( bool isCurrent ) {
        base.ToggleAsCurrent ( isCurrent );
        
        grazeDetector.ToggleEnabled ( this.isCurrent );
    }
  
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
   
    #endregion
}

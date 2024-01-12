using System;
using UnityEngine;

public class Player : Character
{
    #region Serialized Fields

    [ SerializeField ] private float shootCooldown;

    [ SerializeField ] private float bombCooldown;

    [ SerializeField ] private float ability2Cooldown;

    [ SerializeField ] private int bombCount;

    [ SerializeField ] private int ability2Count;

    [ SerializeField ] private Material unselectedMaterial;

    [ SerializeField ] private Material selectedMaterial;

    #endregion


    #region Fields

    private bool canShoot = true;

    private bool canFireAbility1 = true;

    private bool canFireAbility2 = true;

    private Timer shootCooldownTimer;

    private Timer ability1CooldownTimer;

    private Timer ability2CooldownTimer;

    public int BombCount { get => bombCount; }

    public int Ability2Count { get => ability2Count; }

    #endregion


    #region Actions

    public event Action OnPlayerShoot;

    public event Action<int> OnPlayerFiredAbility1;

    public event Action<int> OnPlayerFiredAbility2;

    public static event Action FireAbility1Event;

    #endregion


    #region Methods


    #region Event Subscriptions

    protected override void Awake ( ) {
        base.Awake ( );

        InputManager.OnPlayerMoveAction += OnMove;
        InputManager.OnPlayerShootAction += OnShoot;
        InputManager.OnPlayerAbility1Action += OnAbility1;
        InputManager.OnPlayerAbility2Action += OnAbility2;

        shootCooldownTimer = gameObject.AddComponent<Timer> ( );
        ability1CooldownTimer = gameObject.AddComponent<Timer> ( );
        ability2CooldownTimer = gameObject.AddComponent<Timer> ( );
    }

    private void OnDestroy ( ) {
        InputManager.OnPlayerMoveAction -= OnMove;
        InputManager.OnPlayerShootAction -= OnShoot;
        InputManager.OnPlayerAbility1Action -= OnAbility1;
        InputManager.OnPlayerAbility2Action -= OnAbility2;
    }

    #endregion


    #region Movement related

    private void OnMove ( Vector2 moveDirection ) {
        rb.MovePosition( new Vector2 ( transform.position.x, transform.position.y ) + moveDirection.normalized * speed * Time.deltaTime );
        animator.SetBool ( "isMoving", moveDirection.magnitude > 0 );
    }

    #endregion


    #region Shoot related

    private void OnShoot ( bool isShooting ) {
        if ( isShooting && canShoot ) {
            Shoot ( );

            canShoot = false;
            shootCooldownTimer.StartTimer ( maxTime: shootCooldown, onTimerFinish: () => {
                canShoot = true;
            } );

            OnPlayerShoot?.Invoke ( );
        }
    }

    public void Shoot ( ) {
        GameObject bulletInstance = Instantiate ( original: bulletObjects [ UnityEngine.Random.Range ( 0, bulletObjects.Length ) ], position: pivot.position, rotation: Quaternion.identity );

        Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
        bullet?.Init ( BulletPathType.Straight, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ) );
    }

    #endregion


    #region Ability1 related

    private void OnAbility1 ( bool isFiringAbility1 ) {
        if ( isFiringAbility1 && canFireAbility1 && bombCount > 0 ) {
            FireAbility1 ( );
            
            canFireAbility1 = false;
            ability1CooldownTimer.StartTimer ( maxTime: bombCooldown, onTimerFinish: () => {
                canFireAbility1 = true;
            } );

            OnPlayerFiredAbility1?.Invoke ( bombCount );
        }
    }

    private void FireAbility1 ( ) {
        --bombCount;

        FireAbility1Event?.Invoke ( );
    }

    #endregion


    #region Ability2 related

    private void OnAbility2 ( bool isFiringAbility2 ) {
        if ( isFiringAbility2 && canFireAbility2 && ability2Count > 0  ) {
            FireAbility2 ( );
            
            canFireAbility2 = false;
            ability2CooldownTimer.StartTimer ( maxTime: ability2Cooldown, onTimerFinish: () => {
                canFireAbility2 = true;
            } );

            OnPlayerFiredAbility2?.Invoke ( ability2Count );
        }
    }

    private void FireAbility2 ( ) {
        --ability2Count;
    }

    #endregion


    public void SetPlayerAsCurrent ( bool isSelected ) {
        spriteRenderer.material = isSelected ? selectedMaterial : unselectedMaterial;
        cd.enabled = isSelected;
    }

    #endregion
}

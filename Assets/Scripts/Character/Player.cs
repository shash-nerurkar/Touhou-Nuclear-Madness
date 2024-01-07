using System;
using UnityEngine;

public class Player : Character
{
    #region Serialized Fields

    [ SerializeField ] private float shootCooldown;

    [ SerializeField ] private float bombCooldown;

    [ SerializeField ] private Material unselectedMaterial;

    [ SerializeField ] private Material selectedMaterial;

    #endregion


    #region Fields

    private bool canShoot = true;

    private bool canBomb = true;

    private Timer shootCooldownTimer;

    private Timer bombCooldownTimer;

    #endregion


    #region Actions

    public event Action OnPlayerShoot;

    #endregion


    #region Methods

    protected override void Awake ( ) {
        base.Awake ( );

        InputManager.OnPlayerMoveAction += OnMove;
        InputManager.OnPlayerShootAction += OnShoot;
        InputManager.OnPlayerBombAction += OnBomb;

        shootCooldownTimer = gameObject.AddComponent<Timer> ( );
        bombCooldownTimer = gameObject.AddComponent<Timer> ( );
    }

    private void OnMove ( Vector2 moveDirection ) {
        rb.MovePosition( new Vector2 ( transform.position.x, transform.position.y ) + moveDirection.normalized * speed * Time.deltaTime );
        animator.SetBool ( "isMoving", moveDirection.magnitude > 0 );
    }

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

    private void OnBomb ( bool isBombing ) {
        if ( isBombing && canBomb ) {
            Bomb ( );
            
            canBomb = false;
            bombCooldownTimer.StartTimer ( maxTime: bombCooldown, onTimerFinish: () => {
                canBomb = true;
            } );
        }
    }

    private void Bomb ( ) {

    }

    public void Shoot ( ) {
        GameObject bulletInstance = Instantiate ( original: bulletObjects [ UnityEngine.Random.Range ( 0, bulletObjects.Length ) ], position: pivot.position, rotation: Quaternion.identity );

        Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
        bullet?.Init ( BulletPathType.Straight, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ) );
    }

    public void SetSelected ( bool isSelected ) {
        spriteRenderer.material = isSelected ? selectedMaterial : unselectedMaterial;
        cd.enabled = isSelected;
    }

    private void OnDestroy ( ) {
        InputManager.OnPlayerMoveAction -= OnMove;
        InputManager.OnPlayerShootAction -= OnShoot;
        InputManager.OnPlayerBombAction -= OnBomb;
    }

    #endregion
}

using System.Collections;
using UnityEngine;

public class EnemyUtsuho : Enemy
{
    #region Serialized Fields

    [ Header ("Attack") ]

    [ SerializeField ] private EnemyUtsuhoData utsuhoData;
    public EnemyUtsuhoData UtsuhoData { get => utsuhoData; }

    #endregion


    #region Fields

    private Timer sunAttackCooldownTimer;

    private Timer basicAttackCooldownTimer;

    private IEnumerator sunAttackCoroutine;

    private IEnumerator basicAttackCoroutine;

    #endregion


    #region Methods

    protected override void Awake ( ) {
        base.Awake ( );

        sunAttackCooldownTimer = gameObject.AddComponent<Timer> ( );
        basicAttackCooldownTimer = gameObject.AddComponent<Timer> ( );
    }

    protected override void ChangeState ( State newState ) {
        base.ChangeState ( newState );

        switch ( currentState ) {
            case State.Idle:
                if ( !sunAttackCooldownTimer.IsRunning )
                    sunAttackCooldownTimer.StartTimer ( maxTime: UtsuhoData.SunAttackCooldownTime, onTimerFinish: OnSunAttackCooldownTimerFinished );
                if ( !basicAttackCooldownTimer.IsRunning )
                    basicAttackCooldownTimer.StartTimer ( maxTime: UtsuhoData.BasicAttackCooldownTime, onTimerFinish: OnBasicAttackCooldownTimerFinished );
                break;
            
            case State.Chatting:
                sunAttackCooldownTimer.PauseTimer ( );
                basicAttackCooldownTimer.PauseTimer ( );
                break;
        }
    }

    protected override void OnLoseFight ( ) {
        base.OnLoseFight();

        sunAttackCooldownTimer.PauseTimer ( );
        if ( sunAttackCoroutine != null ) StopCoroutine ( sunAttackCoroutine );
        
        basicAttackCooldownTimer.PauseTimer ( );
        if ( basicAttackCoroutine != null ) StopCoroutine ( basicAttackCoroutine );
    }

    private void OnBasicAttackCooldownTimerFinished ( ) {
        basicAttackCoroutine = ReleaseBasicAttack ( );
        StartCoroutine ( basicAttackCoroutine );

        basicAttackCooldownTimer.StartTimer ( maxTime: UtsuhoData.BasicAttackCooldownTime, onTimerFinish: OnBasicAttackCooldownTimerFinished );
    }

    private void OnSunAttackCooldownTimerFinished ( ) {
        sunAttackCoroutine = ReleaseSunAttack ( );
        StartCoroutine ( sunAttackCoroutine );

        sunAttackCooldownTimer.StartTimer ( maxTime: UtsuhoData.SunAttackCooldownTime, onTimerFinish: OnSunAttackCooldownTimerFinished );
    }

    IEnumerator ReleaseBasicAttack (  ) {
        for ( int i = 0; i < UtsuhoData.BasicAttackBurstBulletCount; i++ ) {
            GameObject bulletInstance = Instantiate ( original: Data.BulletObjects [ 0 ], position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet.Init ( 
                BulletPathType.Sinusoidal, 
                shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ),
                speed: UtsuhoData.BasicAttackBulletSpeed,
                damage: UtsuhoData.BasicAttackBulletDamage,
                bulletColor: UtsuhoData.BasicAttackBulletColor1,
                bulletBorderColor: UtsuhoData.BasicAttackBulletBorderColor1,
                scale: UtsuhoData.BasicAttackBulletScale
            );

            GameObject bulletInstance2 = Instantiate ( original: Data.BulletObjects [ 0 ], position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet2 = bulletInstance2.GetComponent<Bullet> ( );
            bullet2.Init ( 
                BulletPathType.Cosinusoidal, 
                shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ),
                speed: UtsuhoData.BasicAttackBulletSpeed,
                damage: UtsuhoData.BasicAttackBulletDamage,
                bulletColor: UtsuhoData.BasicAttackBulletColor2,
                bulletBorderColor: UtsuhoData.BasicAttackBulletBorderColor2,
                scale: UtsuhoData.BasicAttackBulletScale
            );
            
            yield return new WaitForSeconds ( UtsuhoData.BasicAttackBurstDelayInSeconds );
        }
    }

    IEnumerator ReleaseSunAttack (  ) {
        void ShootSunBullet ( Vector3 shootDir, float dampingValue ) {
            GameObject bulletInstance = Instantiate ( original: Data.BulletObjects [ 1 ], position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet.Init ( 
                BulletPathType.Straight, 
                shootDir: shootDir,
                speed: UtsuhoData.SunAttackBulletSpeed,
                damage: UtsuhoData.SunAttackBulletDamage,
                bulletColor: UtsuhoData.SunAttackBulletColor,
                bulletBorderColor: UtsuhoData.SunAttackBulletBorderColor,
                scale: UtsuhoData.SunAttackBulletScale,
                scaleGradually: true,
                scalingUpDuration: UtsuhoData.SunAttackBulletScalingUpDuration,
                isDamping: true,
                dampingValue: dampingValue
            );
        }
        
        float dampingUnit = ( UtsuhoData.SunAttackBulletDampingValueRange.y - UtsuhoData.SunAttackBulletDampingValueRange.x ) / ( UtsuhoData.SunAttackBulletCount - 1 );
        
        for ( int i = 0; i < UtsuhoData.SunAttackBulletCount; i++ ) {    
            float dampingValue = UtsuhoData.SunAttackBulletDampingValueRange.x + ( i * dampingUnit );

            ShootSunBullet (
                shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), Random.Range ( -UtsuhoData.SunAttackSpreadRange, UtsuhoData.SunAttackSpreadRange ), 0 ),
                dampingValue
            );

            yield return new WaitForSeconds ( UtsuhoData.SunAttackBulletDelay );
        }    
    }

    #endregion
}

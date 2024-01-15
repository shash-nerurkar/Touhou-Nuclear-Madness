using System.Collections;
using UnityEngine;

public class EnemyUtsuho : Enemy
{
    #region Serialized Fields

    [ Header ("Attack") ]

    [ SerializeField ] protected EnemyUtsuhoData utsuhoData;
    public EnemyUtsuhoData UtsuhoData { get => utsuhoData; }

    #endregion


    #region Fields

    protected Timer sunAttackCooldownTimer;

    protected Timer basicAttackCooldownTimer;

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
        basicAttackCooldownTimer.PauseTimer ( );
    }

    private void OnBasicAttackCooldownTimerFinished ( ) {
        StartCoroutine ( ReleaseBasicAttack ( ) );

        basicAttackCooldownTimer.StartTimer ( maxTime: UtsuhoData.BasicAttackCooldownTime, onTimerFinish: OnBasicAttackCooldownTimerFinished );
    }

    private void OnSunAttackCooldownTimerFinished ( ) {
        StartCoroutine ( ReleaseSunAttack ( ) );

        sunAttackCooldownTimer.StartTimer ( maxTime: UtsuhoData.SunAttackCooldownTime, onTimerFinish: OnSunAttackCooldownTimerFinished );
    }

    IEnumerator ReleaseBasicAttack (  ) {
        for ( int i = 0; i < UtsuhoData.BasicAttackBurstBulletCount; i++ ) {
            GameObject randomBulletObject = Data.BulletObjects [ Random.Range ( 0, Data.BulletObjects.Length ) ];
            
            GameObject bulletInstance = Instantiate ( original: randomBulletObject, position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet.Init ( 
                BulletPathType.Sinusoidal, 
                shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ),
                speed: UtsuhoData.BasicAttackBulletSpeed,
                damage: UtsuhoData.BasicAttackBulletDamage
            );

            GameObject bulletInstance2 = Instantiate ( original: randomBulletObject, position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet2 = bulletInstance2.GetComponent<Bullet> ( );
            bullet2.Init ( 
                BulletPathType.Cosinusoidal, 
                shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ),
                speed: UtsuhoData.BasicAttackBulletSpeed,
                damage: UtsuhoData.BasicAttackBulletDamage
            );
            
            yield return new WaitForSeconds ( UtsuhoData.BasicAttackBurstDelayInSeconds );
        }
    }

    IEnumerator ReleaseSunAttack (  ) {
        GameObject randomBulletObject = Data.BulletObjects [ Random.Range ( 0, Data.BulletObjects.Length ) ];

        GameObject bulletInstance = Instantiate ( original: randomBulletObject, position: pivot.position, rotation: Quaternion.identity );
        Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
        bullet.Init ( 
            BulletPathType.Straight, 
            shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ),
            speed: UtsuhoData.SunAttackBulletSpeed,
            damage: UtsuhoData.SunAttackBulletDamage,
            isDamping: true
        );

        yield return new WaitForSeconds ( UtsuhoData.SunAttackFirstBulletDelay );
        
        for ( int i = 0; i < UtsuhoData.SunAttackBulletCount; i++ ) {
            bulletInstance = Instantiate ( original: randomBulletObject, position: pivot.position, rotation: Quaternion.identity );
            bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet.Init ( 
                BulletPathType.Straight, 
                shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), Random.Range ( -UtsuhoData.SunAttackSpreadRange, UtsuhoData.SunAttackSpreadRange ), 0 ),
                speed: UtsuhoData.SunAttackBulletSpeed,
                damage: UtsuhoData.SunAttackBulletDamage,
                isDamping: true
            );
        }
    }

    #endregion
}

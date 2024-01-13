using System.Collections;
using UnityEngine;

public class EnemyUtsuho : Enemy
{
    #region Serialized Fields

    [ Header ("Attack type 1: Basic") ]
    [ SerializeField ] protected float basicAttackCooldownTime;

    [ SerializeField ] protected float basicAttackBurstBulletCount;

    [ SerializeField ] protected float basicAttackBurstDelayInSeconds;


    [ Header ("Attack type 2: Sun") ]
    [ SerializeField ] protected float sunAttackCooldownTime;

    [ SerializeField ] protected float sunAttackBulletCount;

    [ SerializeField ] protected float sunAttackFirstBulletDelay;

    [ SerializeField ] protected float sunAttackSpreadRange;

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
                    sunAttackCooldownTimer.StartTimer ( maxTime: sunAttackCooldownTime, onTimerFinish: OnSunAttackCooldownTimerFinished );
                if ( !basicAttackCooldownTimer.IsRunning )
                    basicAttackCooldownTimer.StartTimer ( maxTime: basicAttackCooldownTime, onTimerFinish: OnBasicAttackCooldownTimerFinished );
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

        basicAttackCooldownTimer.StartTimer ( maxTime: basicAttackCooldownTime, onTimerFinish: OnBasicAttackCooldownTimerFinished );
    }

    IEnumerator ReleaseBasicAttack (  ) {
        for ( int i = 0; i < basicAttackBurstBulletCount; i++ ) {
            GameObject randomBulletObject = Data.BulletObjects [ Random.Range ( 0, Data.BulletObjects.Length ) ];
            
            GameObject bulletInstance = Instantiate ( original: randomBulletObject, position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet?.Init ( BulletPathType.Sinusoidal, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ) );

            GameObject bulletInstance2 = Instantiate ( original: randomBulletObject, position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet2 = bulletInstance2.GetComponent<Bullet> ( );
            bullet2?.Init ( BulletPathType.Cosinusoidal, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ) );
            
            yield return new WaitForSeconds ( basicAttackBurstDelayInSeconds );
        }
    }

    private void OnSunAttackCooldownTimerFinished ( ) {
        StartCoroutine ( ReleaseSunAttack ( ) );

        sunAttackCooldownTimer.StartTimer ( maxTime: sunAttackCooldownTime, onTimerFinish: OnSunAttackCooldownTimerFinished );
    }

    IEnumerator ReleaseSunAttack (  ) {
        GameObject randomBulletObject = Data.BulletObjects [ Random.Range ( 0, Data.BulletObjects.Length ) ];

        GameObject bulletInstance = Instantiate ( original: randomBulletObject, position: pivot.position, rotation: Quaternion.identity );
        Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
        bullet?.Init ( BulletPathType.Straight, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ), isDamping: true );

        yield return new WaitForSeconds ( sunAttackFirstBulletDelay );
        
        for ( int i = 0; i < sunAttackBulletCount; i++ ) {
            bulletInstance = Instantiate ( original: randomBulletObject, position: pivot.position, rotation: Quaternion.identity );
            bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet?.Init ( BulletPathType.Straight, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), Random.Range ( -sunAttackSpreadRange, sunAttackSpreadRange ), 0 ), isDamping: true );
        }
    }

    #endregion
}

using System.Collections;
using UnityEngine;

public class EnemySagume : Enemy
{
    #region Serialized Fields

    [ SerializeField ] protected float legendAttackCooldownTime;

    [ SerializeField ] protected float legend1AttackBulletCount;

    [ SerializeField ] protected float legend2AttackBulletCount;

    [ SerializeField ] protected float legend3AttackBulletCount;

    [ SerializeField ] protected float legend3AttackDelayInSeconds;

    [ SerializeField ] protected float legend3AttackSpread;

    [ SerializeField ] protected float legend3SpeedScale;

    #endregion


    #region Fields

    protected Timer legendAttackCooldownTimer;

    #endregion


    #region Methods

    protected override void Awake ( ) {
        base.Awake ( );

        legendAttackCooldownTimer = gameObject.AddComponent<Timer> ( );
    }

    protected override void ChangeState ( State newState ) {
        base.ChangeState ( newState );

        switch ( currentState ) {
            case State.Chatting:
                legendAttackCooldownTimer.PauseTimer ( );
                break;

            case State.Idle:
                if ( !legendAttackCooldownTimer.IsRunning )
                    legendAttackCooldownTimer.StartTimer ( maxTime: legendAttackCooldownTime, onTimerFinish: OnLegendAttackCooldownTimerFinished );
                break;
        }
    }

    protected override void OnLoseFight ( ) {
        base.OnLoseFight();

        legendAttackCooldownTimer.PauseTimer ( );
    }

    private void OnLegendAttackCooldownTimerFinished ( ) {
        int randomLegendIndex = Random.Range ( 0, 3 );
        switch ( randomLegendIndex ) {
            case 0:
                StartCoroutine ( ReleaseLegend1Attack ( ) );
                break;

            case 1:
                StartCoroutine ( ReleaseLegend2Attack ( ) );
                break;

            case 2:
                StartCoroutine ( ReleaseLegend3Attack ( ) );
                break;
        }


        legendAttackCooldownTimer.StartTimer ( maxTime: legendAttackCooldownTime, onTimerFinish: OnLegendAttackCooldownTimerFinished );
    }

    IEnumerator ReleaseLegend1Attack (  ) {
        for ( int i = 0; i < legend1AttackBulletCount; i++ ) {
            GameObject bulletInstance = Instantiate ( original: bulletObjects [ 1 ], position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet?.Init ( BulletPathType.Straight, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ), isDamping: true );

            yield return null;
        }
    }

    IEnumerator ReleaseLegend2Attack (  ) {
        for ( int i = 0; i < legend2AttackBulletCount; i++ ) {
            int curveDir;
            if ( SceneManager.PlayerPosition != null )
                curveDir = ( int ) Mathf.Sign ( SceneManager.PlayerPosition.y - transform.position.y );
            else
                curveDir = Random.Range ( 0, 2 ) == 1 ? 1 : -1;
            float curveInitialAngle = Random.Range ( -22.5f, 22.5f );

            GameObject bulletInstance = Instantiate ( original: bulletObjects [ 3 ], position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet?.Init ( BulletPathType.Curve, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ), curveDir: curveDir, angle: curveInitialAngle );

            yield return null;
        }
    }

    IEnumerator ReleaseLegend3Attack (  ) {
        for ( int i = 0; i < legend3AttackBulletCount; i++ ) {
            int shootDirY;
            if ( SceneManager.PlayerPosition != null )
                shootDirY = ( int ) Mathf.Sign ( SceneManager.PlayerPosition.y - transform.position.y );
            else
                shootDirY = Random.Range ( 0, 2 ) == 1 ? 1 : -1;

            GameObject bulletInstance = Instantiate ( original: bulletObjects [ 2 ], position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet?.Init ( BulletPathType.Straight, shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), shootDirY * Random.Range ( 0, legend3AttackSpread ), 0 ), isDamping: true, speedScale: legend3SpeedScale );

            yield return new WaitForSeconds ( legend3AttackDelayInSeconds );
        }
    }

    #endregion
}

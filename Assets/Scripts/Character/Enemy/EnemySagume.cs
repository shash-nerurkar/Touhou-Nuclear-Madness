using System.Collections;
using TMPro;
using UnityEngine;

public class EnemySagume : Enemy
{
    #region Serialized Fields

    [ Header ("Attack stats") ]

    [ SerializeField ] protected EnemySagumeData sagumeData;
    public EnemySagumeData SagumeData { get => sagumeData; }

    [ SerializeField ] protected TextMeshProUGUI legendDialogueLabel;

    [ SerializeField ] private LineRenderer legendLineIndicator;

    #endregion


    #region Fields

    protected Timer legendAttackCooldownTimer;

    protected Timer legendAttackDelayTimer;

    private IEnumerator legend1AttackCoroutine;

    private IEnumerator legend2AttackCoroutine;

    private IEnumerator legend3AttackCoroutine;

    #endregion


    #region Methods

    protected override void Awake ( ) {
        base.Awake ( );

        legendAttackCooldownTimer = gameObject.AddComponent<Timer> ( );
        legendAttackDelayTimer = gameObject.AddComponent<Timer> ( );
    }

    protected override void ChangeState ( State newState ) {
        base.ChangeState ( newState );

        switch ( currentState ) {
            case State.Chatting:
                legendAttackCooldownTimer.PauseTimer ( );
                legendAttackDelayTimer.PauseTimer ( );
                break;

            case State.Idle:
                if ( !legendAttackCooldownTimer.IsRunning )
                    legendAttackCooldownTimer.StartTimer ( maxTime: SagumeData.LegendAttackCooldownTime, onTimerFinish: OnLegendAttackCooldownTimerFinished );
                break;
        }
    }

    protected override void OnLoseFight ( ) {
        base.OnLoseFight();

        legendAttackCooldownTimer.PauseTimer ( );
        legendAttackDelayTimer.PauseTimer ( );
        legendDialogueLabel.gameObject.SetActive ( false );
        legendLineIndicator.gameObject.SetActive ( false );

        if ( legend1AttackCoroutine != null ) StopCoroutine ( legend1AttackCoroutine );
        if ( legend2AttackCoroutine != null ) StopCoroutine ( legend2AttackCoroutine );
        if ( legend3AttackCoroutine != null ) StopCoroutine ( legend3AttackCoroutine );
    }

    private void OnLegendAttackCooldownTimerFinished ( ) {
        int randomLegendIndex = Random.Range ( 0, 3 );

        legendDialogueLabel.gameObject.SetActive ( true );
        switch ( randomLegendIndex ) {
            case 0:
                legendDialogueLabel.text = SagumeData.Legend1DialogueText;
                break;
                
            case 1:
                legendDialogueLabel.text = SagumeData.Legend2DialogueText;
                break;
                
            case 2:
                legendDialogueLabel.text = SagumeData.Legend3DialogueText;
                break;
        }
        
        legendAttackDelayTimer.StartTimer ( maxTime: SagumeData.LegendAttackDelayTime, onTimerFinish: () => {
            OnLegendAttackDelayTimerFinished ( randomLegendIndex );
        } );
    }

    private void OnLegendAttackDelayTimerFinished ( int chosenLegendIndex ) {
        legendDialogueLabel.gameObject.SetActive ( false );

        switch ( chosenLegendIndex ) {
            case 0:
                legend1AttackCoroutine = ReleaseLegend1Attack ( );
                StartCoroutine ( legend1AttackCoroutine );
                break;

            case 1:
                legend2AttackCoroutine = ReleaseLegend2Attack ( );
                StartCoroutine ( legend2AttackCoroutine );
                break;

            case 2:
                legend3AttackCoroutine = ReleaseLegend3Attack ( );
                StartCoroutine ( legend3AttackCoroutine );
                break;
        }

        legendAttackCooldownTimer.StartTimer ( maxTime: SagumeData.LegendAttackCooldownTime, onTimerFinish: OnLegendAttackCooldownTimerFinished );
    }

    IEnumerator ReleaseLegend1Attack (  ) {
        for ( int i = 0; i < SagumeData.Legend1AttackBulletCount; i++ ) {
            GameObject bulletInstance = Instantiate ( original: Data.BulletObjects [ 1 ], position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet.Init ( 
                BulletPathType.Straight, 
                shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ),
                speed: SagumeData.Legend1BulletSpeed, 
                damage: SagumeData.Legend1BulletDamage,
                bulletColor: Constants.COLOR_ENEMY_BULLET_BASE,
                bulletBorderColor: Constants.COLOR_ENEMY_FINAL,
                scale: SagumeData.Legend1AttackBulletScale,
                scaleGradually: true,
                scalingUpDuration: SagumeData.Legend1AttackBulletScalingUpDuration,
                shouldDisappearOnTouchingScreenColliders: false
            );

            yield return new WaitForSeconds ( SagumeData.Legend1AttackDelayInSeconds );
        }
    }

    IEnumerator ReleaseLegend2Attack (  ) {
        int [ ] curveDirs = new int [ SagumeData.Legend2AttackBulletCountPerWave ];
        float [ ] curveInitialAngles = new float [ SagumeData.Legend2AttackBulletCountPerWave ];
        for ( int i = 0; i < SagumeData.Legend2AttackBulletCountPerWave; i++ ) {
            curveDirs [ i ] = Random.Range ( 0, 2 ) == 1 ? 1 : -1;
            curveInitialAngles [ i ] = Random.Range ( -SagumeData.Legend2CurveAngle, SagumeData.Legend2CurveAngle );
        }
        
        for ( int i = 0; i < SagumeData.Legend2AttackWaveCount; i++ ) {
            for ( int j = 0; j < SagumeData.Legend2AttackBulletCountPerWave; j++ ) {
                GameObject bulletInstance = Instantiate ( original: Data.BulletObjects [ 2 ], position: pivot.position, rotation: Quaternion.identity );
                Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
                bullet.Init ( 
                    BulletPathType.Curve, 
                    shootDir: new Vector3 ( Mathf.Sign ( transform.localScale.x ), 0, 0 ),
                    speed: SagumeData.Legend2BulletSpeed,  
                    damage: SagumeData.Legend2BulletDamage,
                    bulletColor: Constants.COLOR_ENEMY_BULLET_BASE,
                    bulletBorderColor: Constants.COLOR_ENEMY_FINAL,
                    scale: SagumeData.Legend2AttackBulletScale,
                    curveDir: curveDirs [ j ], 
                    angle: curveInitialAngles [ j ]
                );
            }
            
            yield return new WaitForSeconds ( SagumeData.Legend2AttackDelayBetweenWavesInSeconds );
        }

    }

    IEnumerator ReleaseLegend3Attack (  ) {
        for ( int i = 0; i < SagumeData.Legend3AttackBulletCount; i++ ) {
            Vector3 shootDir;
            if ( SceneManager.PlayerPosition != null )
                shootDir = ( SceneManager.PlayerPosition - transform.position ).normalized;
            else
                shootDir = new Vector3 ( Mathf.Sign ( transform.localScale.x ), Random.Range ( 0, 2 ) == 1 ? 1 : -1, 0 );
            shootDir += new Vector3 ( 0, Random.Range ( -SagumeData.Legend3AttackSpread, SagumeData.Legend3AttackSpread ), 0 );

            legendLineIndicator.SetPosition ( 0, pivot.position );
            legendLineIndicator.SetPosition ( 1, shootDir * SagumeData.Legend3BulletSpeed );
            
            yield return new WaitForSeconds ( SagumeData.Legend3AttackDelayInSeconds );
      
            legendLineIndicator.SetPosition ( 0, Vector3.zero );
            legendLineIndicator.SetPosition ( 1, Vector3.zero );
        
            GameObject bulletInstance = Instantiate ( original: Data.BulletObjects [ 3 ], position: pivot.position, rotation: Quaternion.identity );
            Bullet bullet = bulletInstance.GetComponent<Bullet> ( );
            bullet.Init ( 
                BulletPathType.Straight, 
                shootDir: shootDir,
                speed: SagumeData.Legend3BulletSpeed, 
                damage: SagumeData.Legend3BulletDamage,
                bulletColor: Constants.COLOR_ENEMY_BULLET_BASE,
                bulletBorderColor: Constants.COLOR_ENEMY_FINAL,
                scale: SagumeData.Legend3AttackBulletScale
            );

        }
    }

    #endregion
}

using System;
using UnityEngine;

public class EnemyBullet : Bullet
{
    #region Actions
    
    public static event Action<EnemyBullet> OnCreated;
    
    public static event Action<EnemyBullet> OnDestroyed;

    #endregion


    #region Methods

    public override void Init ( BulletPathType pathType, Vector3 shootDir, int curveDir = 1, float angle = 0, int scale = 1, bool isDamping = false, float speedScale = 1 ) {
        OnCreated?. Invoke ( this );

        base.Init ( pathType, shootDir, curveDir, angle, scale, isDamping, speedScale );
    }

    public override void OnHitAnimationComplete ( ) {
        OnDestroyed?.Invoke ( this );

        base.OnHitAnimationComplete ( );
    }

    #endregion
}

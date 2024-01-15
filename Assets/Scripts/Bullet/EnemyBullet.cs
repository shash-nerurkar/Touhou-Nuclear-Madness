using System;
using UnityEngine;

public class EnemyBullet : Bullet
{
    #region Actions
    
    public static event Action<EnemyBullet> OnCreated;
    
    public static event Action<EnemyBullet> OnDestroyed;

    #endregion


    #region Methods

    public override void Init ( 
        BulletPathType pathType, 
        Vector3 shootDir, 
        float speed, 
        float damage, 
        float scale = 1, 
        float curveDir = 1, 
        float angle = 0, 
        bool isDamping = false,
        bool shouldDisappearOnTouchingScreenColliders = true
    ) {
        OnCreated?. Invoke ( this );

        base.Init ( pathType, shootDir, speed, damage, scale, curveDir, angle, isDamping, shouldDisappearOnTouchingScreenColliders );
    }

    public override void OnHitAnimationComplete ( ) {
        OnDestroyed?.Invoke ( this );

        base.OnHitAnimationComplete ( );
    }

    #endregion
}

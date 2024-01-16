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
        float scale = 1, bool scaleGradually = false, float scalingUpDuration = 0f,
        float curveDir = 1, 
        float angle = 0, 
        bool isDamping = false, float dampingValue = 0.0f,
        bool shouldDisappearOnTouchingScreenColliders = true
    ) {
        OnCreated?. Invoke ( this );

        base.Init ( pathType, shootDir, speed, damage, scale, scaleGradually, scalingUpDuration, curveDir, angle, isDamping, dampingValue, shouldDisappearOnTouchingScreenColliders );
    }

    public override void OnHitAnimationComplete ( ) {
        OnDestroyed?.Invoke ( this );

        base.OnHitAnimationComplete ( );
    }

    #endregion
}

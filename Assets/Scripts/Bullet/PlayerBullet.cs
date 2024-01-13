using System;
using UnityEngine;

public class PlayerBullet : Bullet
{
    #region Actions
    
    public static event Action<PlayerBullet> OnCreated;
    
    public static event Action<PlayerBullet> OnDestroyed;

    #endregion


    #region Methods

    public override void Init ( 
        BulletPathType pathType, 
        Vector3 shootDir, 
        float speed, 
        float damage, 
        int scale = 1, 
        int curveDir = 1, 
        float angle = 0, 
        bool isDamping = false
    ) {
        OnCreated?. Invoke ( this );

        base.Init ( pathType, shootDir, speed, damage, scale, curveDir, angle, isDamping );
    }

    public override void OnHitAnimationComplete ( ) {
        OnDestroyed?.Invoke ( this );

        base.OnHitAnimationComplete ( );
    }

    #endregion
}

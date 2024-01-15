using System.Collections.Generic;
using UnityEngine;

public class BulletsContainer : MonoBehaviour
{
    #region Fields

    protected List<Bullet> bullets = new List<Bullet> ( );

    #endregion


    #region Methods


    #region Event Subscriptions

    protected virtual void Awake ( ) {
        SceneManager.ClearAllBullets += ClearAllBullets;
    }

    protected virtual void OnDestroy ( ) {
        SceneManager.ClearAllBullets -= ClearAllBullets;
    }

    #endregion


    protected void AddBullet ( Bullet bullet ) {
        bullet.transform.SetParent ( transform );

        bullets.Add ( bullet );
    }

    protected void RemoveBullet ( Bullet bullet ) {
        bullets.Remove ( bullet );
    }

    protected void ClearAllBullets ( ) {
        foreach ( Bullet bullet in bullets )
            bullet.OnHit ( );
        
        bullets.Clear ( );
    }

    #endregion
}

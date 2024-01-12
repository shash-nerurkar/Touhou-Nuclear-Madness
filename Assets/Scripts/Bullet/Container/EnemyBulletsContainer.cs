using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletsContainer : BulletsContainer
{
    #region Methods


    #region Event Subscriptions

    protected virtual void Awake ( ) {
        EnemyBullet.OnCreated += AddBullet;
        EnemyBullet.OnDestroyed += RemoveBullet;
        Player.FireAbility1Event += ClearAllBullets;
    }

    protected virtual void OnDestroy ( ) {
        EnemyBullet.OnCreated -= AddBullet;
        EnemyBullet.OnDestroyed -= RemoveBullet;
        Player.FireAbility1Event -= ClearAllBullets;
    }

    #endregion


    #endregion
}

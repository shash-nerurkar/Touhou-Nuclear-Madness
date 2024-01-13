using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletsContainer : BulletsContainer
{
    #region Methods


    #region Event Subscriptions

    protected override void Awake ( ) {
        base.Awake ( );

        EnemyBullet.OnCreated += AddBullet;
        EnemyBullet.OnDestroyed += RemoveBullet;
        Player.FireAbility1Event += ClearAllBullets;
    }

    protected override void OnDestroy ( ) {
        base.OnDestroy ( );

        EnemyBullet.OnCreated -= AddBullet;
        EnemyBullet.OnDestroyed -= RemoveBullet;
        Player.FireAbility1Event -= ClearAllBullets;
    }

    #endregion


    #endregion
}

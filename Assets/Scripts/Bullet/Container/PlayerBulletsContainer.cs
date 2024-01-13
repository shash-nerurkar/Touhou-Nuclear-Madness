using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletsContainer : BulletsContainer
{
    #region Methods


    #region Event Subscriptions

    protected override void Awake ( ) {
        base.Awake ( );

        PlayerBullet.OnCreated += AddBullet;
        PlayerBullet.OnDestroyed += RemoveBullet;
    }

    protected override void OnDestroy ( ) {
        base.OnDestroy ( );

        PlayerBullet.OnCreated -= AddBullet;
        PlayerBullet.OnDestroyed -= RemoveBullet;
    }

    #endregion


    #endregion
}

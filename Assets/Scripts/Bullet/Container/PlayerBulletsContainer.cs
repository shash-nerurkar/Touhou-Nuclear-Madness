using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletsContainer : BulletsContainer
{
    #region Methods


    #region Event Subscriptions

    protected virtual void Awake ( ) {
        PlayerBullet.OnCreated += AddBullet;
        PlayerBullet.OnDestroyed += RemoveBullet;
    }

    protected virtual void OnDestroy ( ) {
        PlayerBullet.OnCreated -= AddBullet;
        PlayerBullet.OnDestroyed -= RemoveBullet;
    }

    #endregion


    #endregion
}

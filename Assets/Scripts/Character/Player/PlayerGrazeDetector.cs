using System;
using UnityEngine;

public class PlayerGrazeDetector : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] private CircleCollider2D cd;

    #endregion


    #region Actions

    public event Action<Collider2D> OnGrazed;

    #endregion


    #region Methods

    public void ToggleEnabled ( bool enabled ) => cd.enabled = enabled;

    private void OnTriggerEnter2D ( Collider2D collided ) {
        int collidedLayer = collided.gameObject.layer;
        if ( collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_ENEMY_WEAPON ) )
            OnGrazed?.Invoke ( collided );
    }

    #endregion
}

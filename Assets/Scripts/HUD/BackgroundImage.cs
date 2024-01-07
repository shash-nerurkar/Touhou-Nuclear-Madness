using System;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImage : MonoBehaviour
{
    #region Serializable Fields

    [ SerializeField ] private Image backgroundImage;

    [ SerializeField ] private Animator animator;

    #endregion


    #region Actions

    public static event Action OnExplosionAnimationComplete;

    #endregion


    #region Methods
    
    public void SetPanelValues ( Sprite backgroundImageSprite ) {
        backgroundImage.sprite = backgroundImageSprite;
    }

    public void PlayExplosion ( ) {
        animator.enabled = true;
        animator.SetBool ( "isExploding" , true );
    }

    public void OnExplosionComplete ( ) {
        animator.SetBool ( "isExploding" , false );
        animator.enabled = false;

        OnExplosionAnimationComplete?.Invoke ( );
    }

    #endregion
}

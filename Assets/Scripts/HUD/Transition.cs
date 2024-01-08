using System;
using UnityEngine;

public class Transition : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] private Animator animator;

    #endregion


    #region Actions

    public static event Action OnFadeIn;

    public static event Action OnFadeOut;

    #endregion

    
    #region Methods

    public void FadeIn ( ) {
        gameObject.SetActive ( true );
        animator.SetBool ( "fadeIn", true );
    }

    public void OnFadeInComplete ( ) {
        OnFadeIn?.Invoke ( );
    }

    public void FadeOut ( ) {
        animator.SetBool ( "fadeOut", true );
    }

    public void OnFadeOutComplete ( ) {
        gameObject.SetActive ( false );
        animator.SetBool ( "fadeIn", false );
        animator.SetBool ( "fadeOut", false );

        OnFadeOut?.Invoke ( );
    }

    #endregion
}

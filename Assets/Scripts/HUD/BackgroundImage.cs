using System;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundImage : MonoBehaviour
{
    #region Serializable Fields

    [ SerializeField ] private Image backgroundImage;

    [ SerializeField ] private Animator animator;

    [ SerializeField ] private Sprite [ ] sceneBackgroundSprites;

    #endregion


    #region Actions

    public static event Action OnExplosionAnimationComplete;

    public static event Action<int> OnBackgroundSceneChanged;

    #endregion


    #region Methods
    
    public void ChangeBackgroundScene ( int sceneIndex ) {
        gameObject.SetActive ( true );

        if ( sceneIndex < 0 || sceneIndex >= sceneBackgroundSprites.Length ) {
            Debug.Log ( "No scene background sprite found for index: " + sceneIndex );
            return;
        }

        backgroundImage.sprite = sceneBackgroundSprites [ sceneIndex ];

        OnBackgroundSceneChanged?.Invoke ( sceneIndex );
    }

    public void PlayExplosion ( ) {
        gameObject.SetActive ( true );
        
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

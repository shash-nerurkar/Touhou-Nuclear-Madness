using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] private Animator animator;

    [ SerializeField ] private TextMeshProUGUI infoTextLabel;

    [ SerializeField ] private Image nextPlayerCharacterImage;

    [ SerializeField ] private Sprite sagumeCharacterSprite;
    [ SerializeField ] private Sprite ayaCharacterSprite;
    [ SerializeField ] private Sprite utsuhoCharacterSprite;
    [ SerializeField ] private Sprite narratorCharacterSprite;

    #endregion


    #region Actions

    public static Action OnFadeInBeginning;

    public static event Action OnFadeInCompleted;

    public static Action OnFadeOutBeginning;

    public static event Action OnFadeOutCompleted;

    #endregion

    
    #region Methods


    #region Next Character Manager

    public void SetNextPlayerCharacter ( ) {
        infoTextLabel.gameObject.SetActive ( false );
        nextPlayerCharacterImage.gameObject.SetActive ( false );
    }

    public void SetNextPlayerCharacter ( Characters character ) {
        infoTextLabel.gameObject.SetActive ( true );
        nextPlayerCharacterImage.gameObject.SetActive ( true );

        infoTextLabel.text = "You will now play as ";

        switch ( character ) {
            case Characters.Sagume:
                infoTextLabel.text += "Sagume";
                nextPlayerCharacterImage.sprite = sagumeCharacterSprite;
                break;

            case Characters.Aya:
                infoTextLabel.text += "Aya";
                nextPlayerCharacterImage.sprite = ayaCharacterSprite;
                break;

            case Characters.Utsuho:
                infoTextLabel.text += "Utsuho";
                nextPlayerCharacterImage.sprite = utsuhoCharacterSprite;
                break;

            case Characters.Narrator:
                infoTextLabel.text += "Narrator";
                nextPlayerCharacterImage.sprite = narratorCharacterSprite;
                break;
        }
    }

    #endregion
    

    #region Fade Managers

    public void FadeIn ( ) {
        gameObject.SetActive ( true );

        animator.SetBool ( "fadeIn", true );

        OnFadeInBeginning?.Invoke ( );
    }

    public void OnFadeInComplete ( ) {
        OnFadeInCompleted?.Invoke ( );
    }

    public void FadeOut ( ) {
        animator.SetBool ( "fadeOut", true );

        OnFadeOutBeginning?.Invoke ( );
    }

    public void OnFadeOutComplete ( ) {
        gameObject.SetActive ( false );
        
        animator.SetBool ( "fadeIn", false );
        animator.SetBool ( "fadeOut", false );

        OnFadeOutCompleted?.Invoke ( );
    }

    #endregion
    

    #endregion
}

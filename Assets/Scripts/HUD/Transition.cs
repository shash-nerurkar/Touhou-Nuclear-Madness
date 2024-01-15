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

    [ SerializeField ] private GameObject nextPlayerCharacterStats;

    [ SerializeField ] private TextMeshProUGUI healthValueLabel;

    [ SerializeField ] private TextMeshProUGUI speedValueLabel;

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
        nextPlayerCharacterStats.SetActive ( false );
    }

    public void SetNextPlayerCharacter ( PlayerData playerCharacterData ) {
        infoTextLabel.gameObject.SetActive ( true );
        nextPlayerCharacterImage.gameObject.SetActive ( true );
        nextPlayerCharacterStats.SetActive ( true );

        infoTextLabel.text = "You are " + playerCharacterData.Name;

        nextPlayerCharacterImage.sprite = playerCharacterData.Sprite;

        healthValueLabel.text = ( ( int ) playerCharacterData.Health ).ToString ( );
        
        speedValueLabel.text = ( ( int ) ( playerCharacterData.Speed + playerCharacterData.Acceleration ) ).ToString ( );
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

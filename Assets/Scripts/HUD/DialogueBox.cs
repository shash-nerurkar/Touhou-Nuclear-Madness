using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] private TextMeshProUGUI dialogueLabel;

    [ SerializeField ] private Image boxBackgroundImage;

    [ SerializeField ] private Image speakerPortrait;

    [ SerializeField ] private TextMeshProUGUI speakerNameLabel;

    [ SerializeField ] private Sprite narratorPortrait;
    [ SerializeField ] private Sprite sagumePortrait;
    [ SerializeField ] private Sprite utsuhoPortrait;
    [ SerializeField ] private Sprite ayaPortrait;
    [ SerializeField ] private Sprite sagumeCryPortrait;
    [ SerializeField ] private Sprite utsuhoCryPortrait;
    [ SerializeField ] private Sprite ayaCryPortrait;

    #endregion

    #region Methods

    public void Show ( Dialogue dialogue ) {
        gameObject.SetActive ( true );

        if ( dialogue.Text == null ) return;

        dialogueLabel.text = dialogue.Text;
        
        switch ( dialogue.Speaker ) {
            case Characters.Narrator:
                boxBackgroundImage.color = Constants.COLOR_NARRATOR;
                speakerPortrait.sprite = narratorPortrait;
                speakerNameLabel.text = Constants.NARRATOR_NAME;
                break;
            
            case Characters.Sagume:
                boxBackgroundImage.color = Constants.COLOR_SAGUME;
                speakerPortrait.sprite = sagumePortrait;
                speakerNameLabel.text = Constants.SAGUME_NAME;
                break;
            
            case Characters.Utsuho:
                boxBackgroundImage.color = Constants.COLOR_UTSUHO;
                speakerPortrait.sprite = utsuhoPortrait;
                speakerNameLabel.text = Constants.UTSUHO_NAME;
                break;
            
            case Characters.Aya:
                boxBackgroundImage.color = Constants.COLOR_AYA;
                speakerPortrait.sprite = ayaPortrait;
                speakerNameLabel.text = Constants.AYA_NAME;
                break;
            
            case Characters.SagumeCry:
                boxBackgroundImage.color = Constants.COLOR_SAGUME;
                speakerPortrait.sprite = sagumeCryPortrait;
                speakerNameLabel.text = Constants.SAGUME_NAME;
                break;
            
            case Characters.UtsuhoCry:
                boxBackgroundImage.color = Constants.COLOR_UTSUHO;
                speakerPortrait.sprite = utsuhoCryPortrait;
                speakerNameLabel.text = Constants.UTSUHO_NAME;
                break;
            
            case Characters.AyaCry:
                boxBackgroundImage.color = Constants.COLOR_AYA;
                speakerPortrait.sprite = ayaCryPortrait;
                speakerNameLabel.text = Constants.AYA_NAME;
                break;
            
        }
        
        // speakerNameLabel.color = boxBackgroundImage.color;
    }

    public void Hide ( ) {
        gameObject.SetActive ( false );
        
        dialogueLabel.text = "";
        boxBackgroundImage.color = Constants.COLOR_NARRATOR;
    }

    #endregion
}

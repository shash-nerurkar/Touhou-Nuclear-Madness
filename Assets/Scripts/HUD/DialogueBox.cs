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

    [ SerializeField ] private Image inputPrompt;

    [ SerializeField ] private Sprite narratorPortrait;
    [ SerializeField ] private Sprite sagumePortrait;
    [ SerializeField ] private Sprite utsuhoPortrait;
    [ SerializeField ] private Sprite ayaPortrait;
    [ SerializeField ] private Sprite sagumeCryPortrait;
    [ SerializeField ] private Sprite utsuhoCryPortrait;
    [ SerializeField ] private Sprite ayaCryPortrait;
    [ SerializeField ] private Sprite creatorsPortrait;

    #endregion

    #region Methods

    public void ToggleInputPrompt ( bool toggleFlag ) => inputPrompt.gameObject.SetActive ( toggleFlag );

    public void Show ( Dialogue dialogue ) {
        if ( dialogue.Text == null ) return;

        gameObject.SetActive ( true );

        dialogueLabel.text = dialogue.Text;
        
        switch ( dialogue.Speaker ) {
            case Characters.Narrator:
                boxBackgroundImage.color = Constants.COLOR_NARRATOR;
                dialogueLabel.color = Color.white;
                speakerPortrait.sprite = narratorPortrait;
                speakerNameLabel.text = Constants.NARRATOR_NAME;
                break;
            
            case Characters.Sagume:
                boxBackgroundImage.color = Constants.COLOR_SAGUME;
                dialogueLabel.color = Color.white;
                speakerPortrait.sprite = sagumePortrait;
                speakerNameLabel.text = Constants.SAGUME_NAME;
                break;
            
            case Characters.Utsuho:
                boxBackgroundImage.color = Constants.COLOR_UTSUHO;
                dialogueLabel.color = Color.white;
                speakerPortrait.sprite = utsuhoPortrait;
                speakerNameLabel.text = Constants.UTSUHO_NAME;
                break;
            
            case Characters.Aya:
                boxBackgroundImage.color = Constants.COLOR_AYA;
                dialogueLabel.color = Color.white;
                speakerPortrait.sprite = ayaPortrait;
                speakerNameLabel.text = Constants.AYA_NAME;
                break;
            
            case Characters.SagumeCry:
                boxBackgroundImage.color = Constants.COLOR_SAGUME;
                dialogueLabel.color = Color.white;
                speakerPortrait.sprite = sagumeCryPortrait;
                speakerNameLabel.text = Constants.SAGUME_NAME;
                break;
            
            case Characters.UtsuhoCry:
                boxBackgroundImage.color = Constants.COLOR_UTSUHO;
                dialogueLabel.color = Color.white;
                speakerPortrait.sprite = utsuhoCryPortrait;
                speakerNameLabel.text = Constants.UTSUHO_NAME;
                break;
            
            case Characters.AyaCry:
                boxBackgroundImage.color = Constants.COLOR_AYA;
                dialogueLabel.color = Color.white;
                speakerPortrait.sprite = ayaCryPortrait;
                speakerNameLabel.text = Constants.AYA_NAME;
                break;
            
            case Characters.Creators:
                boxBackgroundImage.color = Constants.COLOR_CREATORS;
                dialogueLabel.color = Color.black;
                speakerPortrait.sprite = creatorsPortrait;
                speakerNameLabel.text = Constants.CREATORS_NAMES;
                break;
        }
    }

    public void Hide ( ) {
        gameObject.SetActive ( false );
        
        dialogueLabel.text = "";
        boxBackgroundImage.color = Constants.COLOR_NARRATOR;
    }

    #endregion
}

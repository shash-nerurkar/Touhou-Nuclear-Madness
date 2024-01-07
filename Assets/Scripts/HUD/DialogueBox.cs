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
        if ( dialogue.Text == null ) return;

        dialogueLabel.text = dialogue.Text;
        switch ( dialogue.Speaker ) {
            case Speakers.Narrator:
                boxBackgroundImage.color = Constants.COLOR_DEFAULT;
                speakerPortrait.sprite = narratorPortrait;
                speakerNameLabel.text = Constants.NARRATOR_NAME;
                break;
            
            case Speakers.Sagume:
                boxBackgroundImage.color = Constants.COLOR_SAGUME;
                speakerPortrait.sprite = sagumePortrait;
                speakerNameLabel.text = Constants.SAGUME_NAME;
                break;
            
            case Speakers.Utsuho:
                boxBackgroundImage.color = Constants.COLOR_UTSUHO;
                speakerPortrait.sprite = utsuhoPortrait;
                speakerNameLabel.text = Constants.UTSUHO_NAME;
                break;
            
            case Speakers.Aya:
                boxBackgroundImage.color = Constants.COLOR_AYA;
                speakerPortrait.sprite = ayaPortrait;
                speakerNameLabel.text = Constants.AYA_NAME;
                break;
            
            case Speakers.SagumeCry:
                boxBackgroundImage.color = Constants.COLOR_SAGUME;
                speakerPortrait.sprite = sagumeCryPortrait;
                speakerNameLabel.text = Constants.SAGUME_NAME;
                break;
            
            case Speakers.UtsuhoCry:
                boxBackgroundImage.color = Constants.COLOR_UTSUHO;
                speakerPortrait.sprite = utsuhoCryPortrait;
                speakerNameLabel.text = Constants.UTSUHO_NAME;
                break;
            
            case Speakers.AyaCry:
                boxBackgroundImage.color = Constants.COLOR_AYA;
                speakerPortrait.sprite = ayaCryPortrait;
                speakerNameLabel.text = Constants.AYA_NAME;
                break;
            
        }
        // speakerNameLabel.color = boxBackgroundImage.color;
        
        gameObject.SetActive ( true );
    }

    public void Hide ( ) {
        dialogueLabel.text = "";
        boxBackgroundImage.color = Constants.COLOR_DEFAULT;
        gameObject.SetActive ( false );
    }

    #endregion
}

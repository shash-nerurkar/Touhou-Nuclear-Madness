using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    #region Serializable Fields

    [ SerializeField ] private Image endImage;

    [ SerializeField ] private TextMeshProUGUI endWinnerHPLabel;

    [ SerializeField ] private Sprite [ ] endingBackgroundSprites;

    #endregion


    #region Methods
    
    public void ChangeBackgroundScene ( Ending ending, float winnerHP ) {
        if ( ending < 0 || ( int ) ending >= endingBackgroundSprites.Length )
            return;

        switch ( ending ) {
            case Ending.UtsuhoWin:
                endWinnerHPLabel.text = "OKUU BEAT THE LUNARI WITH " + winnerHP + "HP!";
                break;
            
            case Ending.SagumeWin:
                endWinnerHPLabel.text = "SHE GOT AWAY WITH " + winnerHP + "HP!";
                break;
            
            case Ending.AyaWin:
                endWinnerHPLabel.text = "AYA SAVED US WITH " + winnerHP + "HP!";
                break;

            case Ending.NarratorWin:
                endWinnerHPLabel.text = "WHO? HOW? SHE HAD " + winnerHP + "HP!";
                break;
        }

        endImage.sprite = endingBackgroundSprites [ ( int ) ending ];
    }

    #endregion
}

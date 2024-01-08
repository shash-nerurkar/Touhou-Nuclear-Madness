using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    #region Serializable Fields

    [ SerializeField ] private Image endImage;

    #endregion


    #region Fields

    #endregion


    #region Methods
    
    public void SetPanelValues ( Sprite endGameImageSprite ) {
        endImage.sprite = endGameImageSprite;
    }

    #endregion
}

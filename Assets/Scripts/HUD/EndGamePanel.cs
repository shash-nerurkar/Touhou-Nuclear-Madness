using System;
using UnityEngine;
using UnityEngine.UI;

public class EndGamePanel : MonoBehaviour
{
    #region Serializable Fields

    [ SerializeField ] private Image endImage;

    [ SerializeField ] private Sprite [ ] endingBackgroundSprites;

    #endregion

    
    #region Actions

    public static event Action<int> EndGame;

    #endregion


    #region Methods
    
    public void ChangeBackgroundScene ( int endIndex ) {
        if ( endIndex < 0 || endIndex >= endingBackgroundSprites.Length ) {
            Debug.Log ( "No scene background sprite found for index: " + endIndex );
            return;
        }

        endImage.sprite = endingBackgroundSprites [ endIndex ];

        EndGame?.Invoke ( endIndex );
    }

    #endregion
}

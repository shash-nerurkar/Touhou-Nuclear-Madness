using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : MonoBehaviour
{
    #region Serializable Fields

    [ SerializeField ] private GameObject playerHealthBarElementObject;

    [ SerializeField ] private GameObject playerHealthBar;

    [ SerializeField ] private Slider enemyHealthBarSlider;

    [ SerializeField ] private TextMeshProUGUI enemyHealthBarText;

    [ SerializeField ] private GameObject controlsTutorial;

    [ SerializeField ] private float controlsTutorialShowTime;

    #endregion


    #region Fields

    private List<GameObject> playerHealthBarElements;

    private Timer controlsTutorialShowTimer;

    #endregion


    #region Methods

    private void Awake ( ) {
        playerHealthBarElements = new List<GameObject> ( );
    }
    
    public void SetPanelValues ( float playerHealth, float enemyHealth ) {
        UpdatePlayerHealth ( playerHealth );

        enemyHealthBarSlider.maxValue = enemyHealth;
        UpdateEnemyHealth ( enemyHealthBarSlider.maxValue );
    }

    public void UpdatePlayerHealth ( float health ) {
        foreach ( GameObject playerHealthBarElement in playerHealthBarElements )
            Destroy ( playerHealthBarElement );
        playerHealthBarElements.Clear ( );

        for ( int i = 0; i < health; i++ ) {
            GameObject playerHealthBarElement = Instantiate ( playerHealthBarElementObject, Vector3.zero, Quaternion.identity, playerHealthBar.transform );
            playerHealthBarElements.Add ( playerHealthBarElement );
        }
    }

    public void UpdateEnemyHealth ( float health ) {
        enemyHealthBarSlider.value = health;
        enemyHealthBarText.text = enemyHealthBarSlider.value + "/" + enemyHealthBarSlider.maxValue;
    }

    public void ShowTutorial () {
        controlsTutorial.SetActive ( true );

        if ( controlsTutorialShowTimer == null )
            controlsTutorialShowTimer = gameObject.AddComponent<Timer> ( );
        
        controlsTutorialShowTimer.StartTimer ( maxTime: controlsTutorialShowTime, onTimerFinish: ( ) => {
            controlsTutorial.SetActive ( false );
        } );
    }

    #endregion
}

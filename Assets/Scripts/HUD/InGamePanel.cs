using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : MonoBehaviour
{
    #region Serializable Fields

    [ SerializeField ] private GameObject playerHealthBarElementObject;

    [ SerializeField ] private GameObject playerHealthBar;

    [ SerializeField ] private TextMeshProUGUI playerAbility1CountLabel;

    [ SerializeField ] private TextMeshProUGUI playerAbility2CountLabel;

    [ SerializeField ] private TextMeshProUGUI playerGrazeCountLabel;

    [ SerializeField ] private TextMeshProUGUI playerDamageMultiplierLabel;

    [ SerializeField ] private Slider enemyHealthBarSlider;

    [ SerializeField ] private TextMeshProUGUI enemyHealthBarText;

    [ SerializeField ] private GameObject controlsTutorial;

    [ SerializeField ] private float controlsTutorialShowTime;

    #endregion


    #region Fields

    private List<GameObject> playerHealthBarElements = new List<GameObject> ( );

    private Timer controlsTutorialShowTimer;

    #endregion


    #region Methods

    public void SetPanelValues ( float playerHealth, int ability1Count, int ability2Count, int grazeCount, float damageMultiplier, float enemyHealth ) {
        UpdatePlayerHealth ( playerHealth );
        UpdatePlayerAbility1 ( ability1Count );
        UpdatePlayerAbility2 ( ability2Count );
        UpdatePlayerGraze ( grazeCount );
        UpdatePlayerDamageMultiplier ( damageMultiplier );

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

    public void UpdatePlayerAbility1 ( int abilityCount ) {
        playerAbility1CountLabel.text = abilityCount.ToString ( );
    }

    public void UpdatePlayerAbility2 ( int abilityCount ) {
        playerAbility2CountLabel.text = abilityCount.ToString ( );
    }

    public void UpdatePlayerGraze ( int grazeCount ) {
        playerGrazeCountLabel.text = "Graze: " + grazeCount;
    }

    public void UpdatePlayerDamageMultiplier ( float damageMultiplier ) {
        playerDamageMultiplierLabel.text = "Damage: " + damageMultiplier + "x";

        float guessMultiplierCounts = Mathf.Log ( damageMultiplier, 2 );
        float tValue = Mathf.Clamp01 ( 0.2f * guessMultiplierCounts );
        playerDamageMultiplierLabel.color = Color.Lerp ( Constants.COLOR_PLAYER_UI, Color.blue, tValue );
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

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGamePanel : MonoBehaviour
{
    #region Serializable Fields


    #region Player Stats

    [ SerializeField ] private RectTransform playerStatsLayoutBuilder;

    [ SerializeField ] private GameObject playerHealthBarElementObject;

    [ SerializeField ] private GameObject playerHealthBar;

    [ SerializeField ] private Slider playerShootEnergyBarSlider;

    [ SerializeField ] private Image playerShootEnergyBarFillImage;

    [ SerializeField ] private TextMeshProUGUI playerAbility1UseCountLabel;

    [ SerializeField ] private TextMeshProUGUI playerAbility2UseCountLabel;

    [ SerializeField ] private TextMeshProUGUI playerGrazeCountLabel;

    [ SerializeField ] private TextMeshProUGUI playerDamageMultiplierLabel;

    #endregion


    #region Enemy Stats

    [ SerializeField ] private Slider enemyHealthBarSlider;

    [ SerializeField ] private TextMeshProUGUI enemyHealthBarText;

    #endregion


    #region Controls Tutorial

    [ SerializeField ] private GameObject controlsTutorial;

    [ SerializeField ] private float controlsTutorialShowTime;

    #endregion


    [ SerializeField ] private TextMeshProUGUI fightTimerLabel;

    #endregion


    #region Fields

    private List<Image> playerHealthBarElements = new List<Image> ( );

    private Timer controlsTutorialShowTimer;

    private float playerCurrentMaxHealth;

    #endregion


    #region Methods

    public void SetPanelValues (
        float playerHealth, float playerShootEnergy, int ability1Count, int ability2Count, int grazeCount, float damageMultiplier, 
        float enemyHealth
    ) {
        CreatePlayerHealthBarElements ( playerHealth );
        playerCurrentMaxHealth = playerHealth;

        playerShootEnergyBarSlider.maxValue = playerShootEnergy;
        UpdatePlayerShootEnergy ( playerShootEnergyBarSlider.maxValue );

        UpdatePlayerAbility1 ( ability1Count );
        UpdatePlayerAbility2 ( ability2Count );

        UpdatePlayerGraze ( grazeCount );

        UpdatePlayerDamageMultiplier ( damageMultiplier );


        enemyHealthBarSlider.maxValue = enemyHealth;
        UpdateEnemyHealth ( enemyHealthBarSlider.maxValue );


        UpdateFightTimer ( "00:00" );
    }

    
    #region Player Stats

    public void CreatePlayerHealthBarElements ( float maxHealth ) {
        foreach ( Image playerHealthBarElement in playerHealthBarElements )
            Destroy ( playerHealthBarElement.gameObject );
        playerHealthBarElements.Clear ( );

        for ( int i = 0; i < maxHealth; i++ ) {
            GameObject playerHealthBarElement = Instantiate ( playerHealthBarElementObject, Vector3.zero, Quaternion.identity, playerHealthBar.transform );
            playerHealthBarElements.Add ( playerHealthBarElement.GetComponent<Image> ( ) );
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate ( playerStatsLayoutBuilder );
    }

    public void UpdatePlayerHealth ( float health ) {
        for ( int i = 0; i < playerCurrentMaxHealth; i++ ) 
            playerHealthBarElements [ i ].enabled = ( i < health );
    }

    public void UpdatePlayerShootEnergy ( float shootEnergy ) {
        playerShootEnergyBarSlider.value = shootEnergy;
    }

    public void UpdatePlayerAbility1 ( int abilityCount ) {
        playerAbility1UseCountLabel.text = abilityCount.ToString ( );
    }

    public void UpdatePlayerAbility2 ( int abilityCount ) {
        playerAbility2UseCountLabel.text = abilityCount.ToString ( );
    }

    public void UpdatePlayerGraze ( int grazeCount ) {
        playerGrazeCountLabel.text = "Graze: " + grazeCount;
    }

    public void UpdatePlayerDamageMultiplier ( float damageMultiplier ) {
        Color currentPlayerUIColor = Constants.CalculateColorForDamageMultiplier ( Constants.COLOR_PLAYER_BASE, Constants.COLOR_PLAYER_FINAL, damageMultiplier );

        playerDamageMultiplierLabel.text = "Damage: " + damageMultiplier + "x";
        playerDamageMultiplierLabel.color = currentPlayerUIColor;

        playerShootEnergyBarFillImage.color = currentPlayerUIColor;
    }

    #endregion

    
    #region Enemy Stats

    public void UpdateEnemyHealth ( float health ) {
        enemyHealthBarSlider.value = health;
        enemyHealthBarText.text = enemyHealthBarSlider.value + "/" + enemyHealthBarSlider.maxValue;
    }

    #endregion


    public void ShowTutorial () {
        controlsTutorial.SetActive ( true );

        if ( controlsTutorialShowTimer == null )
            controlsTutorialShowTimer = gameObject.AddComponent<Timer> ( );
        
        controlsTutorialShowTimer.StartTimer ( maxTime: controlsTutorialShowTime, onTimerFinish: ( ) => {
            controlsTutorial.SetActive ( false );
        } );
    }

    public void UpdateFightTimer ( string fightTime ) {
        fightTimerLabel.text = fightTime;
    }

    #endregion
}

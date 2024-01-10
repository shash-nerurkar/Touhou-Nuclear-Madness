using UnityEngine;

public class ParticleSystemsManager : MonoBehaviour
{
    
    #region Serialized Fields

    [ SerializeField ] private ParticleSystem mainMenuParticleSystem;

    [ SerializeField ] private ParticleSystem scene01ParticleSystem;

    [ SerializeField ] private ParticleSystem scene02ParticleSystem;

    #endregion


    #region Fields

    #endregion


    #region Methods


    #region Event Subscriptions

    private void Awake ( ) {
        SceneManager.OnGameStateChanged += OnGameStateChanged;

        SceneManager.ChangeBackgroundScene += OnBackgroundSceneChanged;
    }

    private void OnDestroy ( ) {
        SceneManager.OnGameStateChanged -= OnGameStateChanged;

        SceneManager.ChangeBackgroundScene -= OnBackgroundSceneChanged;
    }

    #endregion
    

    private void OnGameStateChanged ( GameState gameState ) {
        if ( gameState == GameState.MainMenu ) {
            mainMenuParticleSystem.Play ( );
            scene01ParticleSystem.Stop ( );
            scene02ParticleSystem.Stop ( );
        }
    }

    private void OnBackgroundSceneChanged ( int sceneIndex ) {
        if ( sceneIndex == 0 ) {
            mainMenuParticleSystem.Stop ( );
            scene01ParticleSystem.Play ( );
            scene02ParticleSystem.Stop ( );
        }
        else if ( sceneIndex == 1 ) {
            mainMenuParticleSystem.Stop ( );
            scene01ParticleSystem.Stop ( );
            scene02ParticleSystem.Play ( );
        }
    }

    #endregion
}

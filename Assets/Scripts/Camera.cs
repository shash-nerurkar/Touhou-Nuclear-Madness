using UnityEngine;

public class Camera : MonoBehaviour
{
    #region Fields

    private float shakeIntensity;

    private Vector3 originalPosition;

    private bool isCameraShaking;
    public bool IsCameraShaking { get => isCameraShaking; }

    private Timer shakeTimer;

    #endregion


    #region Methods

    private void Awake ( ) {
        HUD.ShakeCamera += ShakeCamera;

        shakeTimer = gameObject.AddComponent<Timer> ( );
    }

    private void Start ( ) => originalPosition = transform.position;

    private void Update ( ) {
        if ( isCameraShaking )
            transform.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
        else
            transform.position = originalPosition; // + Random.insideUnitSphere * 0.05f;
    }

    public void ShakeCamera ( float shakeDuration = 0.5f, float shakeIntensity = 0.1f ) {
        this.shakeIntensity = shakeIntensity;

        isCameraShaking = true;
        shakeTimer.StartTimer ( maxTime: shakeDuration, onTimerFinish: ( ) => {
            isCameraShaking = false;
        } );
    }

    private void OnDestroy ( ) {
        HUD.ShakeCamera -= ShakeCamera;
    }

    #endregion
}

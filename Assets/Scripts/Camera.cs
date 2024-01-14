using System.Collections;
using UnityEngine;

public class Camera : MonoBehaviour
{
    #region Fields

    private float shakeDuration;
    
    private float shakeIntensity;

    private const float minShakeIntensity = 0;

    private Vector3 originalPosition;

    private bool isCameraShaking;
    public bool IsCameraShaking { get => isCameraShaking; }

    #endregion


    #region Methods


    #region Event Subscriptions
    
    private void Awake ( ) {
        SceneManager.ShakeCamera += ShakeCamera;   
    }
    
    private void OnDestroy ( ) {
        SceneManager.ShakeCamera -= ShakeCamera;
    }

    #endregion


    private void Start ( ) => originalPosition = transform.position;


    #region Shake Handlers

    public void ShakeCamera ( float shakeDuration, float shakeIntensity ) {
        this.shakeDuration = shakeDuration;
        this.shakeIntensity = shakeIntensity;

        isCameraShaking = true;
        StartCoroutine ( TweenShakeIntensity ( shakeIntensity, minShakeIntensity ) );
    }

    private void FixedUpdate ( ) {
        if ( isCameraShaking )
            transform.position = originalPosition + Random.insideUnitSphere * shakeIntensity;
        else
            transform.position = originalPosition;
    }

    #endregion


    public IEnumerator TweenShakeIntensity( float startValue, float endValue )
    {
        float elapsed = 0f;

        while ( elapsed < shakeDuration )
        {
            float t = elapsed / shakeDuration;
            shakeIntensity = Mathf.Lerp(startValue, endValue, t);

            elapsed += Time.deltaTime;
            
            yield return null;
        }

        shakeIntensity = endValue;

        isCameraShaking = false;
    }

    #endregion
}

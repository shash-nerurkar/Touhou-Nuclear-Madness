using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Serialized Fields
    
    [ SerializeField ] private Rigidbody2D rb;

    [ SerializeField ] private Collider2D cd;

    [ SerializeField ] private SpriteRenderer spriteRenderer;

    [ SerializeField ] private Animator animator;

    [ SerializeField ] private Material unhighlightedMaterial;
    [ SerializeField ] private Material highlightedMaterial;

    #endregion


    #region Fields

    private float speed;

    private float damage;

    private bool isFlying;

    private Vector3 shootDir;

    private BulletPathType pathType;

    private bool shouldDisappearOnTouchingScreenColliders;


    #region Lifetime related
    
    private float lifetime;
    
    private Timer lifetimeTimer;
    
    #endregion


    #region Damping related

    private bool isDamping;

    private float dampingValue;

    #endregion


    #region Straight-path related

    #endregion


    #region Sinusoidal-path related

    private float startTime;

    #endregion


    #region Curve-path related

    private float angle;

    private float curveDir;

    #endregion


    #region Scale related

    private IEnumerator scaleCoroutine;

    #endregion


    #endregion


    #region Methods

    public virtual void Init ( 
        BulletPathType pathType, Vector3 shootDir, float speed, float damage, 
        float scale = 1, bool scaleGradually = false, float scalingUpDuration = 0f,
        float curveDir = 1, float angle = 0, 
        bool isDamping = false, float dampingValue = 0.0f,
        bool shouldDisappearOnTouchingScreenColliders = true
    ) {
        isFlying = true;
        startTime = Time.time;
        transform.localScale = new Vector3 ( transform.localScale.x * shootDir.x, transform.localScale.y, transform.localScale.z );

        this.speed = speed;
        this.damage = damage;
        this.shootDir = shootDir;
        this.pathType = pathType;
        this.shouldDisappearOnTouchingScreenColliders = shouldDisappearOnTouchingScreenColliders;
        
        if ( scaleGradually ) {
            scaleCoroutine = ScaleCoroutine ( transform.localScale * scale, scalingUpDuration );
            StartCoroutine ( scaleCoroutine );
        }
        else {
            transform.localScale *= scale;
        }

        this.angle = angle * Mathf.Deg2Rad;
        this.curveDir = curveDir;

        this.isDamping = isDamping;
        this.dampingValue = dampingValue;
        
        lifetime = Random.Range ( 3.0f, 7.0f );
        lifetimeTimer = gameObject.AddComponent<Timer> ( );
        lifetimeTimer.StartTimer ( maxTime: lifetime, onTimerFinish: OnHit );
    }

    private void FixedUpdate ( ) {
        if ( isFlying )
            Fly ( );
    }

    private void Fly ( ) {
        if ( isDamping ) 
            speed = Mathf.Clamp ( speed - dampingValue, 0.1f, speed - dampingValue );
        
        Vector3 offset = new( );
        float time = Time.time - startTime;

        switch ( pathType ) {
            case BulletPathType.Straight:
                offset = shootDir * speed;
                break;
            
            case BulletPathType.Sinusoidal:
                offset.x = shootDir.x * speed;
                offset.y = 9.0f * Mathf.Sin ( Time.time * speed );
                break;
            
            case BulletPathType.Cosinusoidal:
                offset.x = shootDir.x * speed;
                offset.y = 9.0f * Mathf.Cos ( Time.time * speed );
                break;
              
            case BulletPathType.Curve:
                offset.x = shootDir.x * speed;
                offset.y = speed * Mathf.Sin(angle) * curveDir;
                angle += Time.deltaTime;
                break;
          
            case BulletPathType.Twirly:
                offset.x = ( Mathf.Cos ( angle ) * 2.0f ) + 1;
                offset.y = Mathf.Sin ( angle ) * 2.0f;
                angle += 5.0f * Time.deltaTime;
                break;
            
            case BulletPathType.Zigzag:
                offset.x = shootDir.x * Mathf.PingPong ( time * speed, 2 * 1.0f );
                offset.y = 1.0f * Mathf.Sin ( time * 1.0f * speed );
                break;
        }

        rb.MovePosition ( transform.position + offset * Time.deltaTime );
    }

    IEnumerator ScaleCoroutine ( Vector3 destScale, float duration ) {
        Vector3 originalScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.localScale = Vector3.Lerp(originalScale, destScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = destScale;
    }

    private void OnTriggerEnter2D ( Collider2D collided ) {
        int collidedLayer = collided.gameObject.layer;
        if (
            collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_PLAYER_GRAZE ) ||
            collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_SCREEN_BORDER ) && !shouldDisappearOnTouchingScreenColliders
        )
            return;
        
        cd.enabled = false;

        if ( collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_SCREEN_BORDER ) ) {
            if ( scaleCoroutine != null )
                StopCoroutine ( scaleCoroutine );
            
            OnHitAnimationComplete ( );
        }
        else if ( collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_PLAYER ) ) {        
            collided.GetComponent<Player> ( ).TakeDamage ( damage: damage );
            
            OnHit ( );
        }
        else if ( collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_ENEMY ) ) {        
            collided.GetComponent<Enemy> ( ).TakeDamage ( damage: damage );
            
            OnHit ( );
        }
    }

    public void OnHit ( ) {
        spriteRenderer.material = unhighlightedMaterial;
        
        isFlying = false;
        
        if ( scaleCoroutine != null )
            StopCoroutine ( scaleCoroutine );

        animator.SetBool ( "isHit", true );
    }

    public virtual void OnHitAnimationComplete ( ) {
        Destroy ( gameObject );
    }

    #endregion
}

using UnityEngine;

public class Bullet : MonoBehaviour
{
    #region Serialized Fields
    
    [ SerializeField ] private Rigidbody2D rb;

    [ SerializeField ] private Collider2D cd;

    [ SerializeField ] private SpriteRenderer spriteRenderer;

    [ SerializeField ] private Animator animator;

    [ SerializeField ] private float speed;

    [ SerializeField ] private float damage;

    [ SerializeField ] private Material unhighlightedMaterial;
    [ SerializeField ] private Material highlightedMaterial;

    #endregion


    #region Fields

    private bool isFlying;

    private bool isDamping;

    private float lifetime;

    private Timer dampingBulletTimer;

    private BulletPathType pathType;

    private float angle;

    private int curveDir;
    
    private float startTime;
    
    private float dampingValue;

    private Vector3 shootDir;

    #endregion


    #region Methods

    public void Init ( BulletPathType pathType, Vector3 shootDir, int curveDir = 1, float angle = 0, int scale = 1, bool isDamping = false, float speedScale = 1 ) {
        isFlying = true;

        dampingBulletTimer = gameObject.AddComponent<Timer> ( );

        transform.localScale = new Vector3 ( transform.localScale.x * shootDir.x, transform.localScale.y, transform.localScale.z );
        transform.localScale *= scale;

        this.pathType = pathType;
        this.shootDir = shootDir;
        speed *= speedScale;

        startTime = Time.time;
        this.angle = angle;
        this.curveDir = curveDir;

        this.isDamping = isDamping;
        dampingValue = Random.Range ( 0.05f, 0.3f );
        lifetime = Random.Range ( 3.0f, 7.0f );
        dampingBulletTimer.StartTimer ( maxTime: lifetime, onTimerFinish: OnHitAnimationComplete );
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
            
            case BulletPathType.Twirly:
                offset.x = ( Mathf.Cos ( angle ) * 2.0f ) + 1;
                offset.y = Mathf.Sin ( angle ) * 2.0f;
                angle += 5.0f * Time.deltaTime;
                break;
            
            case BulletPathType.Zigzag:
                offset.x = shootDir.x * Mathf.PingPong ( time * speed, 2 * 1.0f );
                offset.y = 1.0f * Mathf.Sin ( time * 1.0f * speed );
                break;
            
            case BulletPathType.Curve:
                offset.x = shootDir.x * speed;
                offset.y = speed * Mathf.Sin(angle) * curveDir;
                angle += Time.deltaTime;
                break;
        }

        rb.MovePosition ( transform.position + offset * Time.deltaTime );
    }

    private void OnTriggerEnter2D ( Collider2D collided ) {
        cd.enabled = false;

        int collidedLayer = collided.gameObject.layer;
        if ( collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_SCREEN_BORDER ) ) {
            OnHitAnimationComplete ( );
        }
        else if ( collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_PLAYER ) ) {        
            collided.GetComponent<Player> ( )?.TakeDamage ( damage: damage );
            
            SetHighlighted ( false );
        }
        else if ( collidedLayer == LayerMask.NameToLayer ( Constants.COLLISION_LAYER_ENEMY ) ) {        
            collided.GetComponent<Enemy> ( )?.TakeDamage ( damage: damage );
            
            SetHighlighted ( false );
        }
    }

    private void SetHighlighted ( bool isHighlighted ) {
        spriteRenderer.material = isHighlighted ? highlightedMaterial : unhighlightedMaterial;
        isFlying = isHighlighted;
        animator.SetBool ( "isHit", !isHighlighted );
    }

    public void OnHitAnimationComplete ( ) => Destroy ( gameObject );

    #endregion
}
using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] protected SpriteRenderer spriteRenderer;

    [ SerializeField ] protected Animator animator;

    [ SerializeField ] protected Collider2D cd;
    
    [ SerializeField ] protected Rigidbody2D rb;

    [ SerializeField ] protected Transform pivot;

    [ SerializeField ] protected Material unselectedMaterial;

    [ SerializeField ] protected Material selectedMaterial;
    
    #endregion


    #region Fields

    private Timer onHitTimer;
    
    protected float speed;

    protected float health;

    protected float onHitIDuration;

    protected bool isCurrent;

    #endregion


    #region Actions

    public event Action OnLose;

    public event Action<float> OnHit;

    #endregion


    #region Methods
    
    protected virtual void Awake ( ) { 
        onHitTimer = gameObject.AddComponent<Timer> ( );
    }
    
    public void TakeDamage ( float damage ) {
        health = Mathf.Clamp ( health - damage, 0, health - damage );

        if ( health == 0 ) {
            animator.SetBool ( "isLose", true );
            OnLoseFight ( );
        }
        else {
            animator.SetBool ( "isHit", true );
            OnGetHit ( );
        }
    }

    protected virtual void OnGetHit ( ) {
        onHitTimer.StartTimer ( maxTime: onHitIDuration, onTimerFinish: OnHitTimerFinish );

        OnHit?.Invoke ( health );
    }

    protected virtual void OnLoseFight ( ) {
        cd.enabled = false;

        OnLose?.Invoke ( );
    }

    private void OnHitTimerFinish ( ) {
        animator.SetBool ( "isHit", false );
    }

    public virtual void ToggleAsCurrent ( bool isCurrent ) {
        this.isCurrent = isCurrent;
        
        spriteRenderer.material = this.isCurrent ? selectedMaterial : unselectedMaterial;

        cd.enabled = this.isCurrent;
    }

    #endregion
}

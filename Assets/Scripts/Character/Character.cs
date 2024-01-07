using System;
using UnityEngine;

public abstract class Character : MonoBehaviour
{
    #region Serialized Fields

    [ SerializeField ] protected GameObject [ ] bulletObjects;
    
    [ SerializeField ] protected Rigidbody2D rb;

    [ SerializeField ] protected Collider2D cd;

    [ SerializeField ] protected SpriteRenderer spriteRenderer;

    [ SerializeField ] protected Animator animator;

    [ SerializeField ] protected Transform pivot;
    
    [ SerializeField ] protected int speed;

    [ SerializeField ] private float health;

    [ SerializeField ] protected float onHitIDuration;

    #endregion


    #region Fields

    private Timer onHitTimer;

    public float Health { get => health; }

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
        health = Mathf.Clamp ( Health - damage, 0, Health - damage );

        if ( Health == 0 ) {
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

    #endregion
}

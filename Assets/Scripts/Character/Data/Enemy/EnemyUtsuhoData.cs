using UnityEngine;


[ CreateAssetMenu ( fileName = "EnemySagumeData", menuName = "Character/Enemy/Enemy Utsuho Data" ) ]
public class EnemyUtsuhoData : ScriptableObject
{
    [ Header ("Attack type 1: Basic") ]
    
    [ SerializeField ] private float basicAttackBulletSpeed = 10f;
    
    [ SerializeField ] private float basicAttackBulletDamage = 1f;

    [ SerializeField ] private float basicAttackCooldownTime = 1f;

    [ SerializeField ] private int basicAttackBurstBulletCount = 3;

    [ SerializeField ] private float basicAttackBulletScale = 0.9f;

    [ SerializeField ] private float basicAttackBurstDelayInSeconds = 0.2f;


    [ Header ("Attack type 2: Sun") ]
    
    [ SerializeField ] private float sunAttackBulletSpeed = 10f;
    
    [ SerializeField ] private float sunAttackBulletDamage = 1f;

    [ SerializeField ] private float sunAttackCooldownTime = 5f;

    [ SerializeField ] private int sunAttackBulletCount = 15;

    [ SerializeField ] private float sunAttackBulletScale = 5;

    [ SerializeField ] private float sunAttackBulletScalingUpDuration = 0.5f;

    [ SerializeField ] private Vector2 sunAttackBulletDampingValueRange = new Vector2 ( 0.05f, 0.30f);

    [ SerializeField ] private float sunAttackBulletDelay = 1f;

    [ SerializeField ] private float sunAttackSpreadRange = 1f;


    public float BasicAttackBulletSpeed { get => basicAttackBulletSpeed; }
    public float BasicAttackBulletDamage { get => basicAttackBulletDamage; }
    public float BasicAttackCooldownTime { get => basicAttackCooldownTime; }
    public int BasicAttackBurstBulletCount { get => basicAttackBurstBulletCount; }
    public float BasicAttackBulletScale { get => basicAttackBulletScale; }
    public float BasicAttackBurstDelayInSeconds { get => basicAttackBurstDelayInSeconds; }
    public float SunAttackBulletSpeed { get => sunAttackBulletSpeed; }
    public float SunAttackBulletDamage { get => sunAttackBulletDamage; }
    public float SunAttackCooldownTime { get => sunAttackCooldownTime; }
    public int SunAttackBulletCount { get => sunAttackBulletCount; }
    public float SunAttackBulletScale { get => sunAttackBulletScale; }
    public float SunAttackBulletScalingUpDuration { get => sunAttackBulletScalingUpDuration; }
    public Vector2 SunAttackBulletDampingValueRange { get => sunAttackBulletDampingValueRange; }
    public float SunAttackBulletDelay { get => sunAttackBulletDelay; }
    public float SunAttackSpreadRange { get => sunAttackSpreadRange; }
}

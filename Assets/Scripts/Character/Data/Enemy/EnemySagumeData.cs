using UnityEngine;


[ CreateAssetMenu ( fileName = "EnemySagumeData", menuName = "Character/Enemy/Enemy Sagume Data" ) ]
public class EnemySagumeData : ScriptableObject
{
    [ Header ("Attack") ]

    [ SerializeField ] private float legendAttackCooldownTime = 2f;

    [ SerializeField ] private float legendAttackDelayTime = 1f;


    [ Header ("Attack type 1: Legend 1") ]

    [ SerializeField ] private float legend1BulletSpeed = 10f;

    [ SerializeField ] private float legend1BulletDamage = 1f;

    [ SerializeField ] private int legend1AttackBulletCount = 50;

    [ SerializeField ] private float legend1AttackBulletScale = 10;

    [ SerializeField ] private float legend1AttackBulletScalingUpDuration = 0.5f;

    [ SerializeField ] private float legend1AttackDelayInSeconds = 0.2f;

    [ SerializeField ] private string legend1DialogueText = "Kuchisake Onna...";
    
    [ SerializeField ] private Color legend1AttackBulletColor = new ( 0.620f, 0.251f, 0.251f );
    
    [ SerializeField ] private Color legend1AttackBulletBorderColor = new ( 1.000f, 0.000f, 0.000f );


    [ Header ("Attack type 2: Legend 2") ]
    
    [ SerializeField ] private float legend2BulletSpeed = 10f;

    [ SerializeField ] private float legend2BulletDamage = 1f;

    [ SerializeField ] private int legend2AttackWaveCount = 3;

    [ SerializeField ] private int legend2AttackBulletCountPerWave = 15;

    [ SerializeField ] private float legend2CurveAngle = 22.5f;

    [ SerializeField ] private float legend2AttackBulletScale = 1.5f;

    [ SerializeField ] private float legend2AttackDelayBetweenWavesInSeconds = 0.2f;

    [ SerializeField ] private string legend2DialogueText = "The Lochness...";
    
    [ SerializeField ] private Color legend2AttackBulletColor = new ( 0.620f, 0.251f, 0.251f );
    
    [ SerializeField ] private Color legend2AttackBulletBorderColor = new ( 1.000f, 0.000f, 0.000f );


    [ Header ("Attack type 3: Legend 3") ]
    
    [ SerializeField ] private float legend3BulletSpeed = 25f;
    
    [ SerializeField ] private float legend3BulletDamage = 1f;

    [ SerializeField ] private int legend3AttackBulletCount = 8;

    [ SerializeField ] private float legend3AttackDelayInSeconds = 0.2f;

    [ SerializeField ] private float legend3AttackSpread = 0.5f;

    [ SerializeField ] private float legend3AttackBulletScale = 1.25f;

    [ SerializeField ] private string legend3DialogueText = "Human Combustion...";
    
    [ SerializeField ] private Color legend3AttackBulletColor = new ( 0.620f, 0.251f, 0.251f );
    
    [ SerializeField ] private Color legend3AttackBulletBorderColor = new ( 1.000f, 0.000f, 0.000f );
    
    
    public float LegendAttackCooldownTime { get => legendAttackCooldownTime; }
    public float LegendAttackDelayTime { get => legendAttackDelayTime; }
    public float Legend1BulletSpeed { get => legend1BulletSpeed; }
    public float Legend1BulletDamage { get => legend1BulletDamage; }
    public int Legend1AttackBulletCount { get => legend1AttackBulletCount; }
    public float Legend1AttackBulletScale { get => legend1AttackBulletScale; }
    public float Legend1AttackBulletScalingUpDuration { get => legend1AttackBulletScalingUpDuration; }
    public float Legend1AttackDelayInSeconds { get => legend1AttackDelayInSeconds; }
    public string Legend1DialogueText { get => legend1DialogueText; }
    public Color Legend1AttackBulletColor { get => legend1AttackBulletColor; }
    public Color Legend1AttackBulletBorderColor { get => legend1AttackBulletBorderColor; }
    public float Legend2BulletSpeed { get => legend2BulletSpeed; }
    public float Legend2BulletDamage { get => legend2BulletDamage; }
    public int Legend2AttackWaveCount { get => legend2AttackWaveCount; }
    public int Legend2AttackBulletCountPerWave { get => legend2AttackBulletCountPerWave; }
    public float Legend2CurveAngle { get => legend2CurveAngle; }
    public float Legend2AttackBulletScale { get => legend2AttackBulletScale; }
    public float Legend2AttackDelayBetweenWavesInSeconds { get => legend2AttackDelayBetweenWavesInSeconds; }
    public string Legend2DialogueText { get => legend2DialogueText; }
    public Color Legend2AttackBulletColor { get => legend2AttackBulletColor; }
    public Color Legend2AttackBulletBorderColor { get => legend2AttackBulletBorderColor; }
    public float Legend3BulletSpeed { get => legend3BulletSpeed; }
    public float Legend3BulletDamage { get => legend3BulletDamage; }
    public int Legend3AttackBulletCount { get => legend3AttackBulletCount; }
    public float Legend3AttackDelayInSeconds { get => legend3AttackDelayInSeconds; }
    public float Legend3AttackSpread { get => legend3AttackSpread; }
    public float Legend3AttackBulletScale { get => legend3AttackBulletScale; }
    public string Legend3DialogueText { get => legend3DialogueText; }
    public Color Legend3AttackBulletColor { get => legend3AttackBulletColor; }
    public Color Legend3AttackBulletBorderColor { get => legend3AttackBulletBorderColor; }
}

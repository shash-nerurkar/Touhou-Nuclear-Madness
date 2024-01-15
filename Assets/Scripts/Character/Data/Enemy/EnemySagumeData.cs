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

    [ SerializeField ] private string legend1DialogueText = "Kuchisake Onna...";


    [ Header ("Attack type 2: Legend 2") ]
    
    [ SerializeField ] private float legend2BulletSpeed = 10f;

    [ SerializeField ] private float legend2BulletDamage = 1f;

    [ SerializeField ] private int legend2AttackBulletCount = 15;

    [ SerializeField ] private string legend2DialogueText = "The Lochness...";


    [ Header ("Attack type 3: Legend 3") ]
    
    [ SerializeField ] private float legend3BulletSpeed = 25f;
    
    [ SerializeField ] private float legend3BulletDamage = 1f;

    [ SerializeField ] private int legend3AttackBulletCount = 8;

    [ SerializeField ] private float legend3AttackDelayInSeconds = 0.2f;

    [ SerializeField ] private float legend3AttackSpread = 0.5f;

    [ SerializeField ] private string legend3DialogueText = "Human Combustion...";
    
    
    public float LegendAttackCooldownTime { get => legendAttackCooldownTime; }
    public float LegendAttackDelayTime { get => legendAttackDelayTime; }
    public float Legend1BulletSpeed { get => legend1BulletSpeed; }
    public float Legend1BulletDamage { get => legend1BulletDamage; }
    public int Legend1AttackBulletCount { get => legend1AttackBulletCount; }
    public string Legend1DialogueText { get => legend1DialogueText; }
    public float Legend2BulletSpeed { get => legend2BulletSpeed; }
    public float Legend2BulletDamage { get => legend2BulletDamage; }
    public int Legend2AttackBulletCount { get => legend2AttackBulletCount; }
    public string Legend2DialogueText { get => legend2DialogueText; }
    public float Legend3BulletSpeed { get => legend3BulletSpeed; }
    public float Legend3BulletDamage { get => legend3BulletDamage; }
    public int Legend3AttackBulletCount { get => legend3AttackBulletCount; }
    public float Legend3AttackDelayInSeconds { get => legend3AttackDelayInSeconds; }
    public float Legend3AttackSpread { get => legend3AttackSpread; }
    public string Legend3DialogueText { get => legend3DialogueText; }
}

using UnityEngine;


[ CreateAssetMenu ( fileName = "PlayerData", menuName = "Character/Player Data" ) ]
public class PlayerData : CharacterData
{
    [ Header ("Shoot") ]

    [ SerializeField ] private float shootCooldown = 0.1f;

    [ SerializeField ] [ Range ( 0.0f, 10.0f ) ] private float shootEnergy = 2.5f;

    [ SerializeField ] [ Range ( 0.0f, 2.0f ) ] private float shootEnergyPerBullet = 0.25f;

    [ SerializeField ] [ Range ( 0.0f, 1.0f ) ] private float shootEnergyRegenCycleSizeInSeconds = 0.1f;

    [ SerializeField ] [ Range ( 0.0f, 1.0f ) ] private float shootEnergyRegenPerCycle = 0.1f;


    [ Header ("Bullet") ]
    
    [ SerializeField ] private float bulletSpeed = 15f;

    [ SerializeField ] private float bulletDamage = 10f;
    
    [ SerializeField ] private Color bulletColorBaseDamage = new ( 0.300f, 0.443f, 0.566f );
    
    [ SerializeField ] private Color bulletColorMaxDamage = new ( 0.300f, 0.443f, 1.000f );
    
    [ SerializeField ] private Color bulletBorderColorBaseDamage = new ( 0.251f, 0.788f, 1.000f );
    
    [ SerializeField ] private Color bulletBorderColorMaxDamage = new ( 0.000f, 0.000f, 1.000f );
    

    [ Header ("Graze") ]

    [ SerializeField ] [ Range ( 1.25f, 3.0f ) ] private float grazeDamageMultiplier = 2;

    [ SerializeField ] private float grazeDamageMultiplierDuration = 2f;


    [ Header ("Ability 1: Bomb") ]

    [ SerializeField ] private float bombCooldown = 1f;

    [ SerializeField ] private int bombUseCount = 3;


    [ Header ("Ability 2") ]
    
    [ SerializeField ] private float ability2Cooldown = 1f;

    [ SerializeField ] private int ability2UseCount = 3;


    public float ShootCooldown { get => shootCooldown; }
    public float ShootEnergy { get => shootEnergy; }
    public float ShootEnergyRegenCycleSizeInSeconds { get => shootEnergyRegenCycleSizeInSeconds; set => shootEnergyRegenCycleSizeInSeconds = value; }
    public float ShootEnergyRegenPerCycle { get => shootEnergyRegenPerCycle; }
    public float ShootEnergyPerBullet { get => shootEnergyPerBullet; }
    public float BulletSpeed { get => bulletSpeed; }
    public float BulletDamage { get => bulletDamage; }
    public Color BulletColorBaseDamage { get => bulletColorBaseDamage; }
    public Color BulletColorMaxDamage { get => bulletColorMaxDamage; }
    public Color BulletBorderColorBaseDamage { get => bulletBorderColorBaseDamage; }
    public Color BulletBorderColorMaxDamage { get => bulletBorderColorMaxDamage; }
    public float GrazeDamageMultiplier { get => grazeDamageMultiplier; }
    public float GrazeDamageMultiplierDuration { get => grazeDamageMultiplierDuration; }
    public float BombCooldown { get => bombCooldown; }
    public int BombUseCount { get => bombUseCount; }
    public float Ability2Cooldown { get => ability2Cooldown; }
    public int Ability2UseCount { get => ability2UseCount; }
}

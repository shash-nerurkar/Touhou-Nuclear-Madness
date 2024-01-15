using UnityEngine;


[ CreateAssetMenu ( fileName = "PlayerData", menuName = "Character/Player Data" ) ]
public class PlayerData : CharacterData
{
    [ Header ("Shoot") ]

    [ SerializeField ] private float shootCooldown = 0.1f;
    public float ShootCooldown { get => shootCooldown; }
    
    [ SerializeField ] private float bulletSpeed = 15f;
    public float BulletSpeed { get => bulletSpeed; }
    
    [ SerializeField ] private float bulletDamage = 10f;
    public float BulletDamage { get => bulletDamage; }
    

    [ Header ("Graze") ]

    [ SerializeField ] [ Range ( 1.25f, 3.0f ) ] private float grazeDamageMultiplier = 2;
    public float GrazeDamageMultiplier { get => grazeDamageMultiplier; }

    [ SerializeField ] private float grazeDamageMultiplierDuration = 2f;
    public float GrazeDamageMultiplierDuration { get => grazeDamageMultiplierDuration; }


    [ Header ("Ability 1: Bomb") ]

    [ SerializeField ] private float bombCooldown = 1f;
    public float BombCooldown { get => bombCooldown; }

    [ SerializeField ] private int bombCount = 3;
    public int BombCount { get => bombCount; }


    [ Header ("Ability 2: ") ]
    
    [ SerializeField ] private float ability2Cooldown = 1f;
    public float Ability2Cooldown { get => ability2Cooldown; }

    [ SerializeField ] private int ability2Count = 3;
    public int Ability2Count { get => ability2Count; }
}

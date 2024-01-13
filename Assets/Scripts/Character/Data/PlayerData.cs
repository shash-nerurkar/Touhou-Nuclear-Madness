using UnityEngine;


[ CreateAssetMenu ( fileName = "PlayerData", menuName = "Character/Player Data" ) ]
public class PlayerData : CharacterData
{
    [ Header ("Player shoot") ]
    [ SerializeField ] private float shootCooldown;
    public float ShootCooldown { get => shootCooldown; }


    [ Header ("Player ability 1: Bomb") ]
    [ SerializeField ] private float bombCooldown;
    public float BombCooldown { get => bombCooldown; }

    [ SerializeField ] private int bombCount;
    public int BombCount { get => bombCount; }


    [ Header ("Player ability 2: ") ]
    [ SerializeField ] private float ability2Cooldown;
    public float Ability2Cooldown { get => ability2Cooldown; }

    [ SerializeField ] private int ability2Count;
    public int Ability2Count { get => ability2Count; }
}

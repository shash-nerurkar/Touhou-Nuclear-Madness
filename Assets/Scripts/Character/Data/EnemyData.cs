using UnityEngine;


[ CreateAssetMenu ( fileName = "EnemyData", menuName = "Character/Enemy Data" ) ]
public class EnemyData : CharacterData
{
    [ Header ("AI Movement") ]
    [ SerializeField ] private float moveCooldownTime;
    public float MoveCooldownTime { get => moveCooldownTime; }

    [ SerializeField ] [ Range ( 0.0f, 4.0f ) ] private float moveDistanceRange;
    public float MoveDistanceRange { get => moveDistanceRange; }
}

using UnityEngine;


public abstract class CharacterData : ScriptableObject
{
    [ Header ( "Info" ) ]
    
    [ SerializeField ] private new string name;
    public string Name { get => name; }

    [ SerializeField ] private Characters character;
    public Characters Character { get => character; }

    [ SerializeField ] private Color color;
    public Color Color { get => color; }

    [ SerializeField ] private Sprite sprite;
    public Sprite Sprite { get => sprite; }


    [ Header ( "Stats" ) ]

    [ SerializeField ] private float health = 5;
    public float Health { get => health; }
    
    [ SerializeField ] private float speed = 5;
    public float Speed { get => speed; }
    
    [ SerializeField ] private float acceleration = 1;
    public float Acceleration { get => acceleration; }

    [ SerializeField ] private float onHitIDuration = 1;
    public float OnHitIDuration { get => onHitIDuration; }

    [ SerializeField ] private GameObject [ ] bulletObjects;
    public GameObject [ ] BulletObjects { get => bulletObjects; }
}

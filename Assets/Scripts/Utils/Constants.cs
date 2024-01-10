using System;
using UnityEngine;

public class Constants {
    #region Object Tags

    public const string OBJECT_TAG_PLAYER = "Player";
    public const string OBJECT_TAG_HUD = "HUD";
    public const string OBJECT_TAG_MAIN_CAMERA = "MainCamera";
    
    #endregion


    #region Collision Layers
    
    public const string COLLISION_LAYER_GROUND = "Ground";
    public const string COLLISION_LAYER_SCREEN_BORDER = "Screen border";
    public const string COLLISION_LAYER_PLAYER = "Player";
    public const string COLLISION_LAYER_ENEMY  = "Enemy";
    
    #endregion


    #region Sounds
    
    public const string SOUND_WAX_CROSSBOW = "Wax Crossbow Sound";
    public const string MUSIC_THE_CHASE = "The Chase Music";
    
    #endregion


    #region Constant Strings
    
    public const string INFINITY_TEXT = "ꝏ";
    
    public const string NARRATOR_NAME = "Youmu";
    
    public const string UTSUHO_NAME = "Utsuho";
    
    public const string SAGUME_NAME = "Sagume";
    
    public const string AYA_NAME = "Aya";
    
    #endregion


    #region Character Base Positions
    
    public static Vector3 BASE_POSITION_ENEMY  = new Vector2 ( 7.5f, 0f );
    
    public static Vector3 BASE_POSITION_PLAYER = new Vector2 ( -7.5f, 0f );
    
    public static Vector3 BASE_POSITION_BYSTANDER_1 = new Vector2 ( -7.5f, 3f );

    #endregion


    #region Colors

    public static Color COLOR_DEFAULT = new ( 0.219f, 0.219f, 0.219f );

    public static Color COLOR_SAGUME = new ( 0.549f, 0.353f, 0.639f );

    public static Color COLOR_UTSUHO = new ( 0.137f, 0.518f, 0.090f );

    public static Color COLOR_AYA    = new ( 0.643f, 0.063f, 0.000f );

    #endregion


    #region Dialogue Sequences

    public static Dialogue [ ] DIALOGUE_SEQUENCE_MAIN_MENU = new Dialogue [ ] {
        new ( "Hello! This is a story about how Sagume, the Lunarian goddess, wreaked havoc in Utsuho Reiuji's nuclear power plant.", Speakers.Narrator ),
    };

    public static Dialogue [ ] DIALOGUE_SEQUENCE_1 = new Dialogue [ ] {
        new ( "Sagume is quietly soaring above the power plant. Aya spots her, and tails her. Utsuho, perched atop the plant, spots her too.", Speakers.Narrator ),
        new ( "... What a peculiar facility ... Looks quite secure ...", Speakers.Sagume ),
        new ( "... Oops", Speakers.Sagume ),
    };

    public static Dialogue [ ] DIALOGUE_SEQUENCE_2 = new Dialogue [ ] {
        new ( "!!!!", Speakers.Aya ),
        new ( "AAARRRRGGHHHH!!!!! WHO ?!? WHAT ?!? C'MERE YOU!!!", Speakers.Utsuho ),
        new ( "... ... ... ...", Speakers.Sagume ),
        new ( "ANY LAST WORDS, LUNARIAN ?", Speakers.Utsuho ),
        new ( "... ... ... ...", Speakers.Sagume ),
    };
    
    public static Dialogue [ ] DIALOGUE_SEQUENCE_3_1 = new Dialogue [ ] {
        new ( "Good riddance", Speakers.Utsuho ),
        new ( "(≽^╥⩊╥^≼)", Speakers.SagumeCry ),
        new ( "ufufu … This will make a nice story surely!", Speakers.Aya ),
    };

    public static Dialogue [ ] DIALOGUE_SEQUENCE_3_2 = new Dialogue [ ] {
        new ( "... weak", Speakers.Sagume ),
        new ( "(╬≖_≖) I WILL GET YOU FOR THIS. Run, Aya!", Speakers.UtsuhoCry ),
        new ( "... ... ... ...", Speakers.Sagume ),
        new ( "I have to report this!", Speakers.Aya ),
        new ( "... ... ... ...", Speakers.Sagume ),
    };

    public static Dialogue [ ] DIALOGUE_SEQUENCE_4_1 = new Dialogue [ ] {
        new ( "... ... ... ... weak", Speakers.Sagume ),
        new ( "(≽^╥⩊╥^≼)", Speakers.AyaCry ),
    };

    public static Dialogue [ ] DIALOGUE_SEQUENCE_4_2 = new Dialogue [ ] {
        new ( "ʱªʱªʱª(ᕑᗢूᓫ∗) This is a huge scoop!", Speakers.Aya ),
        new ( "(≽^╥⩊╥^≼)", Speakers.SagumeCry ),
    };

    public static Dialogue [ ] DIALOGUE_SEQUENCE_GAME_ENDED = new Dialogue [ ] {
        new ( "In the next day's Bunbunmaru..", Speakers.Narrator ),
    };

    #endregion


    #region Sounds

    public const string ON_DIALOGUE_POP_SOUND = "On Dialogue Pop Sound";

    public const string ON_PLAYER_HIT_SOUND = "On Player Hit Sound";

    public const string ON_PLAYER_SHOOT_SOUND = "On Player Shoot Sound";

    public const string CHATTING_MUSIC = "Chatting Music";

    public const string SCENE_01_MUSIC = "Scene-01 Combat Music";

    public const string SCENE_02_MUSIC = "Scene-02 Combat Music";

    public const string ON_EXPLOSION_SOUND = "On Explosion";

    public const string ON_WIND_SOUND = "On Wind";

    public const string ON_END_GAME_LOSS_SOUND = "On End Game Loss";

    public const string ON_END_GAME_WIN_SOUND = "On End Game Win";

    #endregion
}

public class Dialogue {
    public string Text { get => text; }
    private string text;

    public Speakers Speaker { get => speaker; }
    private Speakers speaker;

    public Dialogue ( string text, Speakers speaker ) {
        this.text = text;
        this.speaker = speaker;
    }
}

[ Serializable ] public enum BulletPathType {
    Straight,
    Sinusoidal,
    Cosinusoidal,
    Twirly,
    Zigzag,
    Curve,
}

[ Serializable ] public enum Speakers {
    Narrator,
    Sagume,
    Utsuho,
    Aya,
    SagumeCry,
    UtsuhoCry,
    AyaCry
}

[ Serializable ] public enum GameState {
    MainMenu,
    Playing,
    Chatting,
    Ended
}

[ Serializable ] public enum InGameState {
    PreExplosion,
    PostExplosion,
    PostFight1Branch1,
    PostFight1Branch2,
    PostFight2Branch1,
    PostFight2Branch2
}
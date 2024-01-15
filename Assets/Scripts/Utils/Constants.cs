using System;
using System.Collections.Generic;
using UnityEngine;

public class Constants {
    #region Collision Layers
    
    public const string COLLISION_LAYER_GROUND = "Ground";
    public const string COLLISION_LAYER_SCREEN_BORDER = "Screen border";
    public const string COLLISION_LAYER_PLAYER = "Player";
    public const string COLLISION_LAYER_ENEMY  = "Enemy";
    public const string COLLISION_LAYER_PLAYER_WEAPON  = "Player weapon";
    public const string COLLISION_LAYER_ENEMY_WEAPON  = "Enemy weapon";
    public const string COLLISION_LAYER_PLAYER_GRAZE  = "Player graze";
    
    #endregion


    #region Strings
    
    public const string CREATORS_NAMES = "Stash & Robin";
    
    public const string NARRATOR_NAME = "Youmu";
    
    public const string UTSUHO_NAME = "Utsuho";
    
    public const string SAGUME_NAME = "Sagume";
    
    public const string AYA_NAME = "Aya";
    
    public const string INFINITY_TEXT = "ꝏ";
    
    public const string ACHIEVEMENT_UNLOCKED_TEXT = "Achievement!";
    
    #endregion


    #region Colors

    // 383838
    public static Color COLOR_NARRATOR = new ( 0.219f, 0.219f, 0.219f );

    // 8c5aa3
    public static Color COLOR_SAGUME = new ( 0.549f, 0.353f, 0.639f );

    // 238417
    public static Color COLOR_UTSUHO = new ( 0.137f, 0.518f, 0.090f );

    // a41000
    public static Color COLOR_AYA    = new ( 0.643f, 0.063f, 0.000f );

    // 40C9FF
    public static Color COLOR_PLAYER_UI = new ( 0.251f, 0.788f, 1.000f );

    // FFFFFF
    public static Color COLOR_CREATORS = new ( 1f, 1f, 1f );

    #endregion


    #region Sounds

    public const string ON_DIALOGUE_POP_SOUND = "On Dialogue Pop Sound";

    public const string ON_PLAYER_HIT_SOUND = "On Player Hit Sound";

    public const string ON_PLAYER_SHOOT_SOUND = "On Player Shoot Sound";

    public const string ON_PLAYER_ABILITY1_SOUND = "On Player Bomb Sound";

    public const string ON_PLAYER_ABILITY2_SOUND = "On Player Ability2 Sound";

    public const string ON_PLAYER_DAMAGE_MULTIPLIER_INCREASED_SOUND = "On Player Damage Multiplier Increased Sound";

    public const string ON_PLAYER_DAMAGE_MULTIPLIER_DECREASED_SOUND = "On Player Damage Multiplier Decreased Sound";

    public const string ON_EXPLOSION_SOUND = "On Explosion";

    public const string ON_ACHIEVEMENT_UNLOCKED_SOUND = "On Achievement Unlocked";

    public const string ON_END_GAME_LOSS_MUSIC = "On End Game Loss Music";

    public const string ON_END_GAME_WIN_MUSIC = "On End Game Win Music";

    public const string MAIN_MENU_MUSIC = "Main Menu Music";

    public const string CHATTING_MUSIC = "Chatting Music";

    public const string SCENE_01_MUSIC = "Scene-01 Combat Music";

    public const string SCENE_02_MUSIC = "Scene-02 Combat Music";

    #endregion


    #region Class-specific constants


        #region SceneManager

        public const int SCENEMANAGER_CHAOS_MODE_RUN_COUNT_THRESHOLD = 7;

        public const int SCENEMANAGER_SKIP_DIALOGUES_MODE_RUN_COUNT_THRESHOLD = 10;

        #endregion


        #region Camera

        public const float CAMERA_MIN_SHAKE_INTENSITY = 0;

        #endregion


    #endregion


    #region Dialogue Sequences

    public static List<Dialogue> DIALOGUE_SEQUENCE_MAIN_MENU = new List<Dialogue> {
        new ( "Hello! This is a story about how Sagume, the Lunarian goddess, wreaked havoc in Utsuho Reiuji's nuclear power plant.", Characters.Narrator ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_1 = new List<Dialogue> {
        new ( "Sagume is quietly soaring above the power plant. Aya spots her, and tails her. Utsuho, perched atop the plant, spots her too.", Characters.Narrator ),
        new ( "... What a peculiar facility ... Looks quite secure ...", Characters.Sagume ),
        new ( "... Oops", Characters.Sagume ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_2 = new List<Dialogue> {
        new ( "!!!!", Characters.Aya ),
        new ( "AAARRRRGGHHHH!!!!! WHO ?!? WHAT ?!? C'MERE YOU!!!", Characters.Utsuho ),
        new ( "... ... ... ...", Characters.Sagume ),
        new ( "ANY LAST WORDS, LUNARIAN ?", Characters.Utsuho ),
        new ( "... ... ... ...", Characters.Sagume ),
    };
    
    public static List<Dialogue> DIALOGUE_SEQUENCE_3_1 = new List<Dialogue> {
        new ( "Good riddance", Characters.Utsuho ),
        new ( "(˚ ˃̣̣̥ - ˂̣̣̥ )", Characters.SagumeCry ),
        new ( "ufufu … This will make a nice story!", Characters.Aya ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_3_2 = new List<Dialogue> {
        new ( "... weak", Characters.Sagume ),
        new ( "(,,>_<,,) I WILL GET YOU FOR THIS. Run, Aya!", Characters.UtsuhoCry ),
        new ( "... ... ... ...", Characters.Sagume ),
        new ( "I have to report this!", Characters.Aya ),
        new ( "... ... ... ...", Characters.Sagume ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_4 = new List<Dialogue> {
        new ( "Aya tries to flee back to the office, but Sagume manages to corner her before she reaches.", Characters.Narrator ),
        new ( "... ready?", Characters.Sagume ),
        new ( "For the truth!", Characters.Aya ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_5_1 = new List<Dialogue> {
        new ( "... ... ... ... weak", Characters.Sagume ),
        new ( "(˚ ˃̣̣̥ - ˂̣̣̥ )", Characters.AyaCry ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_5_2 = new List<Dialogue> {
        new ( "o( ˶^-^˶ )o  This is a huge scoop!", Characters.Aya ),
        new ( "(╥ ω ╥)", Characters.SagumeCry ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_GAME_ENDED = new List<Dialogue> {
        new ( "In the next day's Bunbunmaru..", Characters.Narrator ),
    };

    public static List<List<Dialogue>> INGAME_DIALOGUE_SEQUENCES = new List<List<Dialogue>> {
        DIALOGUE_SEQUENCE_1,
        DIALOGUE_SEQUENCE_2,
        DIALOGUE_SEQUENCE_3_1,
        DIALOGUE_SEQUENCE_3_2,
        DIALOGUE_SEQUENCE_4,
        DIALOGUE_SEQUENCE_5_1,
        DIALOGUE_SEQUENCE_5_2,
    };

    public static Dictionary<Achievement, Dialogue> DIALOGUES_ACHIEVEMENTS = new Dictionary<Achievement, Dialogue> {
        { Achievement.Chaos_Difficulty, new Dialogue ( "CHAOS MODE UNLOCKED - Damage multipliers stack, but you have only 1 HP. Toggle from the Main Menu!", Characters.Creators ) },
        { Achievement.Skip_Dialogues_Mode, new Dialogue ( "TRYHARD MODE ENABLED - All dialogues are out. Hop right into battle!", Characters.Creators ) },
    };

    #endregion
}


public class Dialogue {
    public string Text { get => text; }
    private string text;

    public Characters Speaker { get => speaker; }
    private Characters speaker;

    public Dialogue ( string text, Characters speaker ) {
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


[ Serializable ] public enum Characters {
    Creators,
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
    Ended,
    BlockingInput
}


[ Serializable ] public enum InGameState {
    MainMenu,
    PreExplosion,
    PostExplosion,
    PostFight1Branch1,
    PostFight1Branch2,
    PreFight2,
    PostFight2Branch1,
    PostFight2Branch2,
    EndGame
}


[ Serializable ] public enum Ending {
    UtsuhoWin,
    SagumeWin,
    AyaWin,
    NarratorWin
}


[ Serializable ] public enum GameDifficulty {
    Default,
    Chaos,
}


[ Serializable ] public enum Achievement {
    Chaos_Difficulty,
    Skip_Dialogues_Mode,
}
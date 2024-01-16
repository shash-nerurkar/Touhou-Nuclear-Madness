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

        public const int SCENEMANAGER_SKIP_DIALOGUES_MODE_RUN_COUNT_THRESHOLD = 15;

        #endregion


        #region Camera

        public const float CAMERA_MIN_SHAKE_INTENSITY = 0;

        #endregion


    #endregion


    #region Dialogue Sequences

    public static List<Dialogue> DIALOGUE_SEQUENCE_MAIN_MENU = new List<Dialogue> {
        new ( "Hello! I'm Youmu. This game is about yet another Lunarian-Earthling tussle - the day Sagume Kishin, the Lunarian goddess, wreaked havoc in Utsuho Reiuji's nuclear power plant.", Characters.Narrator ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_1 = new List<Dialogue> {
        new ( "Sagume was quietly soaring above the power plant. Aya spotted her, but kept her distance. Utsuho, perched atop the plant, saw her too and, smelling trouble, approached her.", Characters.Narrator ),
        new ( "Sagume Kishin. Of all the Lunari. Whatever brings you to my humble plant?", Characters.Utsuho ),
        new ( "... What a peculiar facility ... I am simply visiting ...", Characters.Sagume ),
        new ( "It is of nuclear origin. Be wary of it's overwhelming power ...", Characters.Utsuho ),
        new ( "...", Characters.Sagume ),
        new ( "... lest it consume you.", Characters.Utsuho ),
        new ( "It looks quite secure to me ...", Characters.Sagume ),
        new ( "... Oops", Characters.Sagume ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_2 = new List<Dialogue> {
        new ( "!!!", Characters.Aya ),
        new ( "!!!!!!!!!!!!!!!!", Characters.Utsuho ),
        new ( "...", Characters.Sagume ),
        new ( "WHAT HAVE YOU DONE, LUNARI?", Characters.Utsuho ),
        new ( "...", Characters.Sagume ),
        new ( "ANY LAST WORDS?", Characters.Utsuho ),
        new ( "...", Characters.Sagume ),
    };
    
    public static List<Dialogue> DIALOGUE_SEQUENCE_3_1 = new List<Dialogue> {
        new ( "(˚ ˃̣̣̥ - ˂̣̣̥ )  ... I m-must retreat ... I will not f-forget this, Earthlings ...", Characters.SagumeCry ),
        new ( "Okuu! You beat her! What were the chances?", Characters.Aya ),
        new ( "As much as for Hell to be ablaze. I'd rather you misunderstood my strength, than miscalculated it, Tengu.", Characters.Utsuho ),
        new ( "Hmph. Nevertheless, this will make a nice story. Keep an eye out for tomorrow's copy, Raven.", Characters.Aya ),
        new ( "No. I spot the difference between the polite and the kind.", Characters.Utsuho ),
        new ( "I must go mend my plant now. Farewell.", Characters.Utsuho ),
        new ( "Bah, what a grump. Well, I must hurry back and push this out before dusk. Off I go!", Characters.Aya ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_3_2 = new List<Dialogue> {
        new ( "... weak ...", Characters.Sagume ),
        new ( "(,,>_<,,)  THERE WILL BE CONSEQUENCES, LUNARI. FLY, AYA!", Characters.UtsuhoCry ),
        new ( "!!!!", Characters.Sagume ),
        new ( "I have to report this. The Lunarians can't get away with it!", Characters.Aya ),
        new ( "... I cannot allow that to happen ... forgive me, Tengu, for it is your turn ...", Characters.Sagume ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_4 = new List<Dialogue> {
        new ( "Aya tries to flee back to the office, but Sagume manages to corner her before she reaches! A confrontation is imminent.", Characters.Narrator ),
        new ( "Prepare yourself ...", Characters.Sagume ),
        new ( "For the truth, I must be victorious. Come, Sagume Kishin!", Characters.Aya ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_5_1 = new List<Dialogue> {
        new ( "(˚ ˃̣̣̥ - ˂̣̣̥ )  This cannot be happening!", Characters.AyaCry ),
        new ( "It is the inevitable that has happened .... Remember this, crow ...", Characters.Sagume ),
        new ( "How dare you call me that!", Characters.AyaCry ),
        new ( "I shall allow you to publish about this ... Write about your shame, and fly ... perhaps, someday, it will not keep up ...", Characters.Sagume ),
        new ( "*glares*", Characters.Aya ),
        new ( "I must depart now ...", Characters.Sagume ),
        new ( "*sniffle*", Characters.AyaCry ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_5_2 = new List<Dialogue> {
        new ( "(╥ ω ╥)  How... how has a crow Tengu Earthling bested me, a Goddess of Lunarian descent?", Characters.SagumeCry ),
        new ( "It takes more than blood to be special, moon-dweller. But your kind will never understand that.", Characters.Aya ),
        new ( "Silence! I will not tolerate your chirping!", Characters.Sagume ),
        new ( "Careful now, Kishin. It is your own chirping that got you in this mess, remember? Although that is not significant to the story ...", Characters.Aya ),
        new ( "(,,>_<,,)  ARGH! I WILL LEAVE THIS TIME, BUT THIS IS NOT THE LAST YOU WILL SEE OF ME!", Characters.SagumeCry ),
        new ( "Bye-bye!", Characters.Aya ),
        new ( "o( ˶^-^˶ )o  This is a huge scoop!", Characters.Aya ),
    };

    public static List<Dialogue> DIALOGUE_SEQUENCE_GAME_ENDED = new List<Dialogue> {
        new ( "And so, in the next day's Bunbunmaru ...", Characters.Narrator ),
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
using System.Collections.Generic;
using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
  #region Serialized Fields
  
  [ SerializeField ] private Sound[] sounds;
  
  #endregion


  #region Fields

  private List<int> pausedSoundIndices = new List<int> ( );
  
  #endregion


  #region Methods


  #region Event Subscriptions

  private void Awake ( ) {
    SceneManager.PlaySound += Play;
    SceneManager.StopSound += Stop;
  }

  private void OnDestroy ( ) {
    SceneManager.PlaySound -= Play;
    SceneManager.StopSound -= Stop;
  }

  #endregion


  private void Start ( ) {
    foreach ( Sound sound in sounds ) {
      sound.source = gameObject.AddComponent<AudioSource> ( );
      sound.source.clip = sound.clip;

      sound.source.volume = sound.volume;
      sound.source.pitch = sound.pitch;
      sound.source.loop = sound.isLoop;
    }
  }
  
  private void OnGamePauseToggled( bool isPaused ) {
    if ( isPaused ) {
      pausedSoundIndices.Clear ( );

      for ( int index = 0; index < sounds.Length; index++ )
        if ( sounds[ index ].source.isPlaying ) {
          sounds[ index ].source.Pause ( );
          pausedSoundIndices.Add ( index );
        }
    }
    else {
      foreach ( int index in pausedSoundIndices )
        sounds[ index ].source.Play ( );
      
      pausedSoundIndices.Clear ( );
    }
  }


  #region Sound-state Managers

  private void Play( string name ) {
    Sound sound = Array.Find ( sounds, s => s.name == name );

    if ( sound == null )
      Debug.LogError ( "Sound " + name + " Not Found!" );
    else {
      sound.source.pitch = sound.pitch;
      sound.source.volume = sound.volume;
      sound.source.Play ( );
    }
  }

  private void Stop ( string name ) {
    Sound sound = Array.Find( sounds, s => s.name == name );

    if ( sound == null )
      Debug.LogError ( "Sound " + name + " Not Found!" );
    else
      sound.source.Stop ( );
  }

  #endregion


  #endregion
}

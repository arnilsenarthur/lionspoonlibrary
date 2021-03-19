/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    
                                                             
    Lion Spoon Dream Game Technology© - 2021

    Language library
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LionSpoon
{
    /// <summary>
    /// Sound library, used to play sounds in world or in HUD/UI
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        private static AudioSource[] audioSources;
        private static List<AudioSource> musicSources = new List<AudioSource>();
        private static List<AudioSource> fxSources = new List<AudioSource>();
        public StringAudioClip Clips = new StringAudioClip();
        private static SoundManager instance;

        /// <summary>
        /// __internal__
        /// </summary>
        public void Awake()
        {
            instance = this;
        }

        /// <summary>
        /// Get volume based on mode
        /// </summary>
        /// <returns></returns>
        public static float GetVolume(SoundMode mode)
        {
            if(mode == SoundMode.MUSIC)
                return GetMusicVolume();
            else if(mode == SoundMode.AUDIO)
                return GetFxVolume();

            return 1f;
        }

        /// <summary>
        /// Get fx volume
        /// </summary>
        /// <returns></returns>
        public static float GetFxVolume()
        {
            return SettingsProfile.StandardSettings.Get<float>("fxVolume",1f);
        }

        /// <summary>
        /// Get music volume
        /// </summary>
        /// <returns></returns>
        public static float GetMusicVolume()
        {
            return SettingsProfile.StandardSettings.Get<float>("musicVolume",1f);
        }

        /// <summary>
        /// Init sounds library
        /// </summary>
        public static void Init()
        {
            int AUDIO_SOURCES = LionSpoonLibraryManager.GetSettings().GetSoundSources();

            audioSources = new AudioSource[AUDIO_SOURCES];
            for(int i = 0; i < AUDIO_SOURCES; i ++)
            {
                GameObject ob = new GameObject("AudioSource_" + i);
                ob.transform.parent = null;
                audioSources[i] = ob.AddComponent<AudioSource>();
            }

            EventSystem.AddHandler("fxVolumeChanged",(volume) => 
            {
                foreach(AudioSource src in fxSources)
                    src.volume = (float) volume;
            });

            EventSystem.AddHandler("musicVolumeChanged",(volume) => 
            {
                foreach(AudioSource src in musicSources)
                    src.volume = (float) volume;
            });
        }

        /// <summary>
        /// Play an audio in world origin
        /// </summary>
        /// <param name="audio"></param>
        /// <returns></returns>
        public static int PlayAudio(string audio,SoundMode mode)
        {
            return PlayAudio(audio,GetVolume(mode),null,mode);
        }

        /// <summary>
        /// Play an audio in world origin with custom volume
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="volume"></param>
        /// <returns></returns>
        public static int PlayAudio(string audio,float volume,SoundMode mode)
        {
            return PlayAudio(audio,volume,null,Vector3.zero,mode);
        }

        /// <summary>
        /// Play an audio following object at pivot point with custom volume
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="volume"></param>
        /// <param name="parent"></param>
        /// <returns></returns>
        public static int PlayAudio(string audio,float volume,Transform parent,SoundMode mode)
        {
            return PlayAudio(audio,volume,parent,Vector3.zero,mode);
        }

        /// <summary>
        /// Play an audio in a custom world position with a volume of 100%
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int PlayAudio(string audio,Vector3 position,SoundMode mode)
        {
            return PlayAudio(audio,GetVolume(mode),null,Vector3.zero,mode);
        }

        /// <summary>
        /// Play an audio in a custom world position with a custom volume
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="volume"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int PlayAudio(string audio,float volume,Vector3 position,SoundMode mode)
        {
            return PlayAudio(audio,volume,null,Vector3.zero,mode);
        }

        /// <summary>
        /// Play an audion following object with custom offset and custom volume
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="volume"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int PlayAudio(string audio,float volume,Transform parent,Vector3 position,SoundMode mode)
        {
            return PlayAudio(audio,volume,parent,position,false,mode);
        }

        /// <summary>
        /// Play an audion following object with custom volume, looping or not
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="volume"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int PlayAudio(string audio,float volume,Transform parent,bool looping,SoundMode mode)
        {
            return PlayAudio(audio,volume,parent,Vector3.zero,looping,mode);
        }

        /// <summary>
        /// Play an audion following object with custom offset and custom volume, looping or not
        /// </summary>
        /// <param name="audio"></param>
        /// <param name="volume"></param>
        /// <param name="parent"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        public static int PlayAudio(string audio,float volume,Transform parent,Vector3 position,bool looping,SoundMode mode)
        {
            for(int i = 0; i < audioSources.Length; i ++)
            {
                AudioSource src = audioSources[i];
                if(src.isPlaying)
                    continue;

                src.volume = volume;
                src.transform.parent = parent;
                src.transform.localPosition = position;
                src.clip = instance.Clips[audio];
                src.loop = looping;

                src.Play();

                musicSources.Remove(src);
                fxSources.Remove(src);

                if(mode == SoundMode.AUDIO)
                    fxSources.Add(src);
                else if(mode == SoundMode.MUSIC)
                    musicSources.Add(src);

                return i;
            }

            return -1;
        }

        /// <summary>
        /// Stop audio from source
        /// </summary>
        /// <param name="source"></param>
        public static void Stop(int source)
        {
            audioSources[source].Stop();
        }

        /// <summary>
        /// Get audio source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static AudioSource GetSource(int source)
        {
            return audioSources[source];
        }
    }

    /// <summary>
    /// Enumeration to play audio
    /// </summary>
    public enum SoundMode
    {
        MUSIC,
        AUDIO,
        IGNORE
    }
}
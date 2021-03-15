/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    

    Lion Spoon Dream Game Technology© - 2021

    Main library
*/
//Configure libraries
#define LS_LANGUAGE
#define LS_SETTINGS
#define LS_SOUNDS

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Main LionSpoon proprietary library 
/// </summary>
namespace LionSpoon
{  
    /// <summary>
    /// Used to control initialization of all library modules
    /// </summary>
    public class LionSpoonLibraryManager
    {
        private static bool initilized = false;
        private static LionSpoonLibrarySettings settings;

        /// <summary>
        /// Init modules with custom settings
        /// </summary>
        /// <param name="settings"></param>
        public static void Init(LionSpoonLibrarySettings settings)
        {
            LionSpoonLibraryManager.settings = settings;
            if(initilized)
                return;

            initilized = true;

            //Language
            #if LS_LANGUAGE
            if((settings.GetModules() & LionSpoonLibraryModule.Language) == LionSpoonLibraryModule.Language)
                Language.ReloadLanguages();
            #endif

            #if LS_SOUNDS
            if((settings.GetModules() & LionSpoonLibraryModule.Sounds) == LionSpoonLibraryModule.Sounds)
                SoundManager.Init();
            #endif
        }

    
        /// <summary>
        /// Init libraries with default settings
        /// </summary>
        public static void Init()
        {
            Init(new LionSpoonLibrarySettings());
        }

        /// <summary>
        /// Check if libraries have been initiliazed
        /// </summary>
        /// <returns></returns>
        public static bool IsInitialized()
        {
            return initilized;
        }

        /// <summary>
        /// Get settings
        /// </summary>
        /// <returns></returns>
        public static LionSpoonLibrarySettings GetSettings()
        {
            return settings;
        }
    }
    
    [FlagsAttribute]

    /// <summary>
    /// Module Enumeration
    /// </summary>
    public enum LionSpoonLibraryModule
    {
        None = 0,
        Language = 1,
        Settings = 2,
        Sounds = 4,
        All = 255
    }

    /// <summary>
    /// Library settings holder
    /// </summary>
    public class LionSpoonLibrarySettings
    {
        private LionSpoonLibraryModule modules = LionSpoonLibraryModule.All;
        private bool debug = false;
        private int soundSources = 16;

        /// <summary>
        /// Default empty settings
        /// </summary>
        public LionSpoonLibrarySettings()
        {
            
        }

        /// <summary>
        /// Change "modules" setting
        /// </summary>
        /// <param name="modules"></param>
        /// <returns></returns>
        public LionSpoonLibrarySettings WithModules(LionSpoonLibraryModule modules)
        {
            this.modules = modules;
            return this;
        }  
        
        /// <summary>
        /// Change "debug" setting
        /// </summary>
        /// <param name="debug"></param>
        /// <returns></returns>
        public LionSpoonLibrarySettings WithDebug(bool debug)
        {
            this.debug = debug;
            return this;
        }

        /// <summary>
        /// Change "soundsources" setting
        /// </summary>
        /// <param name="debug"></param>
        /// <returns></returns>
        public LionSpoonLibrarySettings WithSoundSources(int sources)
        {
            this.soundSources = sources;
            return this;
        }

        /// <summary>
        /// Check if debug is enabled
        /// </summary>
        /// <returns></returns>
        public bool IsDebugEnabled()
        {
            return debug;
        }

        /// <summary>
        /// Get sound sources amount
        /// </summary>
        /// <returns></returns>
        public int GetSoundSources()
        {
            return soundSources;
        }


        /// <summary>
        /// Get loaded modules
        /// </summary>
        /// <returns></returns>
        public LionSpoonLibraryModule GetModules()
        {
            return this.modules;
        }
    }
}

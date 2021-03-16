/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    

    Lion Spoon Dream Game Technology© - 2021

    Level System
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace LionSpoon
{
    /// <summary>
    /// Holds a level profile
    /// </summary>
    public class Level
    {
        private Dictionary<string,object> options = new Dictionary<string, object>();
        private LevelSystem system;
        private int id = 0;

        /// <summary>
        /// Creates a new level
        /// </summary>
        /// <param name="id"></param>
        /// <param name="system"></param>
        public Level(int id,LevelSystem system)
        {
            this.id = id;
            this.system = system;
        }

        /// <summary>
        /// Get level id
        /// </summary>
        /// <returns></returns>
        public int GetId()
        {
            return id;
        }

        /// <summary>
        /// Get a attribute of a level
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key,T value)
        {
            if(options.ContainsKey(key))
                return (T)(object) options[key];

            return system.Get(key,value);
        }
        
        /// <summary>
        /// Set an attribute of a level
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public Level Set<T>(string key,T value)
        {
            options[key] = value;
            return this;
        }
    }
    
    /// <summary>
    /// Main base for level systems
    /// </summary>
    public class LevelSystem
    {
        private Dictionary<string,object> defaultOptions = new Dictionary<string, object>();
        private Level currentLevel = null;
        private List<Level> levels = new List<Level>();

        private List<Action<Level>> handlers = new List<Action<Level>>();
        
        /// <summary>
        /// Set a default attribute for levels of this system
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public LevelSystem WithDefault<T>(string key,T value)
        {
            defaultOptions[key] = value;
            return this;
        }

        /// <summary>
        /// Add a level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public Level WithNewLevel()
        {
            Level level;
            if(currentLevel == null)
                level = (currentLevel = new Level(levels.Count,this));
            else
                level = new Level(levels.Count,this);

            levels.Add(level);
            return level;
        }

        /// <summary>
        /// Get a level from index
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public Level GetLevel(int level)
        {
            return levels[level];
        }

        /// <summary>
        /// Get level count
        /// </summary>
        /// <returns></returns>
        public int GetLevelCount()
        {
            return levels.Count;
        }

        /// <summary>
        /// Get levels
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Level> GetLevels()
        {
            return levels;
        }

        /// <summary>
        /// Get default attribute
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(string key,T value)
        {
            if(defaultOptions.ContainsKey(key))
                return (T)(object) defaultOptions[key];

            return value;
        }

        /// <summary>
        /// Change level
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Level ChangeLevel(int index)
        {
            currentLevel = levels[index];
            foreach(Action<Level> lv in handlers)
                lv(currentLevel);

            return currentLevel;
        }

        /// <summary>
        /// Clear all handlers
        /// </summary>
        public void ClearHandlers()
        {
            handlers.Clear();
        }

        /// <summary>
        /// Add a handler to system
        /// </summary>
        /// <param name="lv"></param>
        /// <returns></returns>
        public LevelSystem WithHandler(Action<Level> lv)
        {
            handlers.Add(lv);
            return this;
        }

        /// <summary>
        /// Get the current selected level
        /// </summary>
        /// <returns></returns>
        public Level GetCurrentLevel()
        {
            return currentLevel;
        }
    }
}

/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    

    Lion Spoon Dream Game Technology© - 2021

    Settings library
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LionSpoon
{
    /// <summary>
    /// Used to hold a Map/Dict of settings/data
    /// </summary>
    public abstract class SettingsProfile
    {
        public static SettingsProfile StandardSettings = new HandledPlayerPrefsSettingsProfile("standard")
        .WithHandler("fxVolume","fxVolumeChanged")
        .WithHandler("musicVolume","musicVolumeChanged");

        /// <summary>
        /// Add a default value to setting profile (WITHOUT WRITE IN FILE)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract SettingsProfile WithDefault<T>(string key,T value);

        /// <summary>
        /// Get a setting value from setting profile (OR DEFAULT VALUE IF THERE'S NO VALUE PRESENT)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract T Get<T>(string key,T value);

        /// <summary>
        /// Set a value (IF THE VALUE IS EQUALS TO DEFAULT/STORED IT WILL NOT BE SAVED TO FILE)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract SettingsProfile Set<T>(string key,T value);

        /// <summary>
        /// Delete a value from setting profile (FROM DATA AND FROM FILE)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public abstract SettingsProfile Delete(string key);

        /// <summary>
        /// Delete all loaded data (FROM DATA AND FROM FILE)
        /// </summary>
        /// <returns></returns>
        public abstract SettingsProfile DeleteAllCurrent();      
    }

    /// <summary>
    /// Used to create a setting profile that calls events wehen value change  
    /// </summary>
    public class HandledPlayerPrefsSettingsProfile : PlayerPrefsSettingsProfile
    {
        /// <summary>
        /// Creates a new empty profile
        /// </summary>
        /// <param name="profile"></param>
        public HandledPlayerPrefsSettingsProfile(string profile) : base(profile)
        {
            
        }

        private Dictionary<string,string> handlers = new Dictionary<string, string>();

        public HandledPlayerPrefsSettingsProfile WithHandler(string field,string handler)
        {
            handlers[field] = handler;
            return this;
        }

        /// <summary>
        /// Set a value (IF THE VALUE IS EQUALS TO DEFAULT/STORED IT WILL NOT BE SAVED TO FILE)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override SettingsProfile Set<T>(string key,T value)
        {
            base.Set<T>(key,value);

            //Call event handler
            if(handlers.ContainsKey(key))
                EventSystem.CallEvent(handlers[key],value);

            return this;
        }
    }

    /// <summary>
    /// Used to hold a Map/Dict of settings/data based in unity PlayerPrefs
    /// </summary>
    public class PlayerPrefsSettingsProfile : SettingsProfile
    {
        private Dictionary<string,object> data = new Dictionary<string, object>();
        private string profile;

        /// <summary>
        /// Creates a new empty profile
        /// </summary>
        /// <param name="profile"></param>
        public PlayerPrefsSettingsProfile(string profile)
        {
            this.profile = profile;
        }

        /// <summary>
        /// Add a default value to setting profile (WITHOUT WRITE IN FILE)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override SettingsProfile WithDefault<T>(string key,T value)
        {
            if(!__IsInFile(key))
                data[key] = value;

            return this;
        }

        /// <summary>
        /// Get a setting value from setting profile (OR DEFAULT VALUE IF THERE'S NO VALUE PRESENT)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T Get<T>(string key,T value)
        {
            if(data.ContainsKey(key))
                return (T) data[key];
            else if(__IsInFile(key))
            {
                return (T)(data[key] =__ReadFromFile<T>(key));
            }
            return value;
        }

        /// <summary>
        /// Delete a value from setting profile (FROM DATA AND FROM FILE)
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override SettingsProfile Delete(string key)
        {
            __DeleteFromFile(key);

            if(data.ContainsKey(key))
                data.Remove(key);
            return this;
        }

        /// <summary>
        /// Delete all loaded data (FROM DATA AND FROM FILE)
        /// </summary>
        /// <returns></returns>
        public override SettingsProfile DeleteAllCurrent()
        {
            foreach(string k in data.Keys)
                __DeleteFromFile(k);
            data.Clear();
            return this;
        }

        /// <summary>
        /// Set a value (IF THE VALUE IS EQUALS TO DEFAULT/STORED IT WILL NOT BE SAVED TO FILE)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override SettingsProfile Set<T>(string key,T value)
        {
            if(!__IsInFile(key) || !data.ContainsKey(key) || data[key] != (object) value)
            {
                data[key] = value;

                //Save to file
                __SaveToFile<T>(key);
            }

            return this;
        }

        /// <summary>
        /// __internal__
        /// </summary>
        private bool __IsInFile(string key)
        {
            return PlayerPrefs.HasKey(__Key(key));
        }

        /// <summary>
        /// __internal__
        /// </summary>
        private void __SaveToFile<T>(string key)
        {
            if(typeof(T).Equals(typeof(int)))
                PlayerPrefs.SetInt(__Key(key),(int)data[key]);
            else if(typeof(T).Equals(typeof(bool)))
                PlayerPrefs.SetInt(__Key(key),((bool)data[key]) ? 1 : 0);
            else if(typeof(T).Equals(typeof(float)))
                PlayerPrefs.SetFloat(__Key(key),(float)data[key]);
            else if(typeof(T).Equals(typeof(string)))
                PlayerPrefs.SetString(__Key(key),(string)data[key]);

            PlayerPrefs.Save();
        }

        /// <summary>
        /// __internal__
        /// </summary>
        private T __ReadFromFile<T>(string key)
        {
            if(typeof(T).Equals(typeof(int)))
                return (T)(object) PlayerPrefs.GetInt(__Key(key));
            if(typeof(T).Equals(typeof(bool)))
                return (T)(object) (PlayerPrefs.GetInt(__Key(key)) == 1 ? true : false);
            else if(typeof(T).Equals(typeof(float)))
                return (T)(object) PlayerPrefs.GetFloat(__Key(key));
            else if(typeof(T).Equals(typeof(string)))
                return (T)(object) PlayerPrefs.GetString(__Key(key));

            return default(T);
        }

        /// <summary>
        /// __internal__
        /// </summary>
        private void __DeleteFromFile(string key)
        {
            PlayerPrefs.DeleteKey(__Key(key));
        }
        
        /// <summary>
        /// __internal__
        /// </summary>
        private string __Key(string key)
        {
            return profile + "_" + key;
        }

        
    }
}
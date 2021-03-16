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
using System.Collections.Generic;
using UnityEngine;

namespace LionSpoon
{
    /// <summary>
    /// Main language library class
    /// </summary>
    public class Language
    {   
        #region perLanguageData
        private string name;
        private string code;
        private bool locked = false;

        private Dictionary<string,string> messages = new Dictionary<string,string>();

        /// <summary>
        /// Creates a new language
        /// </summary>
        /// <param name="code"></param>
        private Language(string code)
        {
            this.code = code;
        }

        /// <summary>
        /// Lock language (FOR A FUTURE SECURITY/ANTI-HACK SOLUTION)
        /// </summary>
        /// <returns></returns>
        private Language Lock()
        {
            locked = true;
            return this;
        }

        /// <summary>
        /// Add a message to language
        /// </summary>
        /// <param name="key"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public Language With(string key,string message)
        {
            if(!locked)
                messages[key] = message;
                
            return this;
        }

        /// <summary>
        /// Remove a message from language
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Language Remove(string key)
        {
            if(!locked)
                messages.Remove(key);

            return this;
        }

        /// <summary>
        /// Check if language file is locked 
        /// </summary>
        /// <returns></returns>
        public bool IsLocked()
        {
            return locked;
        }

        /// <summary>
        /// Check if language contains a message
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsMessage(string key)
        {
            return messages.ContainsKey(key);
        }

        /// <summary>
        /// Get a translated message from language file replacing arguments
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public string GetLocalizedMessage(string key,string[] args)
        {
            if(messages.ContainsKey(key))
            {
                string s = messages[key];

                foreach(string n in args)
                {
                    int i = s.IndexOf("<>");
                    s = s.Substring(0,i) + n + s.Substring(i + 2);
                }

                return s;
            }

            return "MISSING_ENTRY";
        }

      
        /// <summary>
        /// Get a translated message from language file
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetLocalizedMessage(string key)
        {
            return GetLocalizedMessage(key,new string[0]);
        }

        /// <summary>
        /// Get localized language name
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return name;
        }
        #endregion
        
        #region staticLanguageData
        private static Dictionary<string,Language> languages = new Dictionary<string,Language>();
        private static SettingsProfile settings;

        /// <summary>
        /// Reload all language files
        /// </summary>
        public static void ReloadLanguages()
        {
            languages.Clear();

            object[] txt = Resources.LoadAll("Languages", typeof(TextAsset));

            if(LionSpoonLibraryManager.GetSettings().IsDebugEnabled())
                Debug.Log("Loading languages...");

            //Holds a "pointer" to actual loading language
            Language tmp; 

            foreach(object o in txt)
            {
                //Convert asset
                TextAsset text = (TextAsset) o;

                //Save language
                languages[text.name] = (tmp = new Language(text.name));

                if(LionSpoonLibraryManager.GetSettings().IsDebugEnabled())
                    Debug.Log("Language file found: '" + text.name + "'");

                string[] strings = text.text.Split('\n');

                //Load all lines of file
                foreach(string s in strings)
                {
                    if(s.Length < 3)
                        continue;

                    if(s.StartsWith("#"))
                        continue;
                
                    string[] st = s.Split('=');

                    if(st[0].Length == 0)
                        tmp.name = st[1];
                    else  
                        tmp.With(st[0],st[1]);
                }

                //For security, lock the language file
                tmp.Lock();
            }
        }

        /// <summary>
        /// Retrieve a language from key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Language GetLanguage(string key)
        {
            return languages[key];
        }

        /// <summary>
        /// Get all loaded languages
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Language> GetLanguages()
        {
            return languages.Values;
        }
        
        /// <summary>
        /// Set used settings for language settings
        /// </summary>
        /// <param name="settings"></param>
        public static void UseSettings(SettingsProfile settings)
        {
            Language.settings = settings;
        }

        /// <summary>
        /// Get current language (From settings module)
        /// </summary>
        /// <returns></returns>
        public static Language GetCurrentLanguage()
        {
            #if LS_SETTINGS
            #endif
            return languages[settings.Get<string>("language","EnUs")];
        }   

        /// <summary>
        /// Get a localized message from current language
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetMessage(string key)
        {
            return GetCurrentLanguage().GetLocalizedMessage(key);
        }  

        /// <summary>
        /// Get a localized message from current language replacing args
        /// </summary>
        /// <param name="key"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static string GetMessage(string key,string[] args)
        {
            return GetCurrentLanguage().GetLocalizedMessage(key,args);
        }

        #endregion
    }
}
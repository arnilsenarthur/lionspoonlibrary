/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    

    Lion Spoon Dream Game Technology© - 2021

    Google Play Services API
*/
using System;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

namespace LionSpoon
{
    public class GooglePlayServices
    {
        /// <summary>
        /// Init library
        /// </summary>
        public static void Init()
        {
            PlayGamesPlatform.DebugLogEnabled = LionSpoonLibraryManager.GetSettings().IsDebugEnabled();
            PlayGamesPlatform.Activate ();
        }

        /// <summary>
        /// Login to Google Play Games account
        /// </summary>
        public static void LogIn()
        {
            PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>{
                if(LionSpoonLibraryManager.GetSettings().IsDebugEnabled())
                    Debug.Log("Login: " + result);
            });

            Social.localUser.Authenticate ((bool success) =>
            {
                if(LionSpoonLibraryManager.GetSettings().IsDebugEnabled())
                    if (success)
                        Debug.Log ("Login Sucess");
                    else
                        Debug.Log ("Login failed");
                    
            });
        }

        /// <summary>
        /// Logout from Google Play Games account
        /// </summary>
        public static void LogOut()
        {
            ((PlayGamesPlatform)Social.Active).SignOut();
        }

        /// <summary>
        /// Add score to leaderboard
        /// </summary>
        /// <param name="scoreboardId"></param>
        /// <param name="score"></param>
        public static void AddScore(string scoreboardId,int score)
        {
            Social.ReportScore(score, scoreboardId, (bool success) => {
                if(LionSpoonLibraryManager.GetSettings().IsDebugEnabled())
                    Debug.Log("Scored added: " + success);
            });
        }

        /// <summary>
        /// Show achievements UI
        /// </summary>
        public static void ShowAchievementsUI()
        {
            Social.ShowAchievementsUI();
        }

        /// <summary>
        /// Show leaderboards UI
        /// </summary>
        public static void ShowLeaderboardUI()
        {
            Social.ShowLeaderboardUI();
        }

        /// <summary>
        /// Unlock a achievement for player
        /// </summary>
        /// <param name="achievementId"></param>
        /// <param name="progress"></param>
        public static void UnlockAchievement(string achievementId,float progress)
        {
            Social.ReportProgress(achievementId, progress, (bool success) => 
            {
                if(LionSpoonLibraryManager.GetSettings().IsDebugEnabled())
                    Debug.Log("Achievement Unlock: " + success);
            });
        }

        /// <summary>
        /// Check if player is authenticated
        /// </summary>
        /// <returns></returns>
        public static bool IsAuthenticated()
        {
            return Social.localUser.authenticated;
        }

    }
}
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

/// <summary>
/// Handle class for button events for GPG integration
/// </summary>
public class GPS : MonoBehaviour
{
    public void Login()
    {
        LionSpoon.GooglePlayServices.LogIn();
    }

    public void ShowAchievementsUI()
    {
         LionSpoon.GooglePlayServices.ShowAchievementsUI();
    }

    public void ShowLeaderboardUI()
    {
        LionSpoon.GooglePlayServices.ShowLeaderboardUI();
    }

    public void UnlockAchievement()
    {
        LionSpoon.GooglePlayServices.UnlockAchievement(GPGSIds.achievement_achievements1,100f);
    }

    public void OnLogOut ()
    {
        LionSpoon.GooglePlayServices.LogOut();
    }

    public void AddScore()
    {
        LionSpoon.GooglePlayServices.AddScore(GPGSIds.leaderboard_teste_placar,20000);
    }
}

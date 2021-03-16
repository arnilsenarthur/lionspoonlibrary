using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;


public class GPS : MonoBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        
    }

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

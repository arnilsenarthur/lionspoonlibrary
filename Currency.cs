/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    

    Lion Spoon Dream Game Technology© - 2021

    Currency Library
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LionSpoon
{
    /// <summary>
    /// Currency system
    /// </summary>
    public class Currency
    {
        private string id;
        private string symbol;

        /// <summary>
        /// Setup new currency
        /// </summary>
        /// <param name="id"></param>
        public Currency(string id,string symbol)
        {
            this.id = id;
            this.symbol = symbol;
        }

        /// <summary>
        /// Get currency name from current language
        /// </summary>
        /// <returns></returns>
        public string GetCurrencyName()
        {
            return Language.GetMessage("currency_" + id);
        }

        /// <summary>
        /// Get currency symbol from current language
        /// </summary>
        /// <returns></returns>
        public string GetCurrencySymbol()
        {
            return symbol;
        }

        /// <summary>
        /// Get the balance from player
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public float GetBalance(SettingsProfile profile)
        {
            return profile.Get<float>("balance_" + id,0);
        }

        /// <summary>
        /// Deposit balance to player
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="balance"></param>
        public void Deposit(SettingsProfile profile,float balance)
        {
            profile.Set<float>("balance_" + id,profile.Get<float>("balance_" + id,0) + balance);
        }

        /// <summary>
        /// Withdraw balance from player
        /// </summary>
        /// <param name="profile"></param>
        /// <param name="balance"></param>
        public void Withdraw(SettingsProfile profile,float balance)
        {
            profile.Set<float>("balance_" + id,profile.Get<float>("balance_" + id,0) - balance);
        }
    }
}
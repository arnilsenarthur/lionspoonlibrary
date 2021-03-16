/*
  _       _                   ____                                  
 | |     (_)   ___    _ __   / ___|   _ __     ___     ___    _ __  
 | |     | |  / _ \  | '_ \  \___ \  | '_ \   / _ \   / _ \  | '_ \ 
 | |___  | | | (_) | | | | |  ___) | | |_) | | (_) | | (_) | | | | |
 |_____| |_|  \___/  |_| |_| |____/  | .__/   \___/   \___/  |_| |_|
                                     |_|    

    Lion Spoon Dream Game Technology© - 2021

    Shop Library
*/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LionSpoon
{
    /// <summary>
    /// Used to create shop systems in game and in menus
    /// </summary>
    public class Shop
    {
        private Dictionary<string,ShopItem> items = new Dictionary<string, ShopItem>();
        private string id;
        private Currency currency;

        /// <summary>
        /// Creates a new shop
        /// </summary>
        /// <param name="id"></param>
        public Shop(string id)
        {
            this.id = id;
        }

        public Shop WithCurrency(Currency currency)
        {
            this.currency = currency;
            return this;
        }

        /// <summary>
        /// Get the id of shop
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            return id;
        }

        /// <summary>
        /// Register item to shop
        /// </summary>
        /// <param name="item"></param>
        public void WithItem(ShopItem item)
        {
            items[item.GetId()] = item;
        }

        /// <summary>
        /// Get enumerable of items of shop to display
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ShopItem> GetItems()
        {
            return items.Values;
        }

        /// <summary>
        /// Get a registered item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ShopItem GetItem(string id)
        {
            return this.items[id];
        }

        /// <summary>
        /// Get a player inventory
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public SimpleShopInventory GetSimpleInventory(SettingsProfile profile)
        {
            return new SimpleShopInventory(this,profile);
        }

        /// <summary>
        /// Get a player inventory
        /// </summary>
        /// <param name="profile"></param>
        /// <returns></returns>
        public CurrencedShopInventory GetInventory(SettingsProfile profile)
        {
            return new CurrencedShopInventory(this,profile);
        }

        /// <summary>
        /// Gets the current currency
        /// </summary>
        /// <returns></returns>
        public Currency GetUsedCurrency()
        {
            return currency;
        }
    }

    /// <summary>
    /// Holds a shop item info
    /// </summary>
    public class ShopItem
    {
        private bool unlimited = true;
        private int limit = 0;
        private float price = 0;
        private string id = "";

        /// <summary>
        /// Default ShopItem constructor. (Unlimited, free item, free price)
        /// </summary>
        /// <param name="id"></param>
        public ShopItem(string id)
        {
            this.id = id;
        }

        /// <summary>
        /// Set item unlimited
        /// </summary>
        /// <returns></returns>
        public ShopItem Unlimited()
        {
            this.unlimited = true;
            return this;
        }

        /// <summary>
        /// Set a limit to an item
        /// </summary>
        /// <param name="limit"></param>
        /// <returns></returns>
        public ShopItem WithLimit(int limit)
        {
            this.unlimited = false;
            this.limit = limit;
            return this;
        }

        /// <summary>
        /// Set item price
        /// </summary>
        /// <param name="price"></param>
        /// <returns></returns>
        public ShopItem WithPrice(float price)
        {
            this.price = price;
            return this;
        }

        /// <summary>
        /// Check if item is unlimited or not
        /// </summary>
        /// <returns></returns>
        public bool IsUnlimited()
        {
            return unlimited;
        }

        /// <summary>
        /// Get item limit
        /// </summary>
        /// <returns></returns>
        public int GetLimit()
        {
            return limit;
        }

        /// <summary>
        /// Get price of item
        /// </summary>
        /// <returns></returns>
        public float GetPrice()
        {
            return price;
        }

        /// <summary>
        /// Get internal id of item
        /// </summary>
        /// <returns></returns>
        public string GetId()
        {
            return id;
        }
    }

     /// <summary>
    /// Holds a player inventory info
    /// </summary>
    public abstract class ShopInventory
    {  
        /// <summary>
        /// Check if player has a minimum amount of an item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public abstract bool HasItem(string id,int amount);

        /// <summary>
        /// Check if player has at least 1 item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract bool HasItem(string id);
        /// <summary>
        /// Get player owned amount of an item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public abstract int GetItemAmount(string id);

        /// <summary>
        /// Remove item from player inventory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public abstract void RemoveItem(string id,int amount);

        /// <summary>
        /// Remove 1 item from player inventory
        /// </summary>
        /// <param name="id"></param>
        public abstract void RemoveItem(string id);
    }

    /// <summary>
    /// Holds a player inventory info
    /// </summary>
    public class CurrencedShopInventory : ShopInventory
    {
        private Shop shop;
        private SettingsProfile profile;

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="profile"></param>
        public CurrencedShopInventory(Shop shop,SettingsProfile profile)
        {
            this.shop = shop;
            this.profile = profile;
        }

        /// <summary>
        /// Check if player has a minimum amount of an item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public override bool HasItem(string id,int amount)
        {
            return profile.Get<int>("shop_" + shop.GetId() + "_" + id,0) >= amount;
        }

        /// <summary>
        /// Check if players has at least one item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool HasItem(string id)
        {
            return HasItem(id,1);
        }

        /// <summary>
        /// Get player owned amount of an item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override int GetItemAmount(string id)
        {
            return profile.Get<int>("shop_" + shop.GetId() + "_" + id,0);
        }

        /// <summary>
        /// Add an item to player inventory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void BuyItem(string id,int amount)
        {
            shop.GetUsedCurrency().Withdraw(profile,GetBuyPrice(id,amount));
            profile.Set<int>("shop_" + shop.GetId() + "_" + id,GetItemAmount(id) + amount);
        }

        /// <summary>
        /// Remove an item from player inventory
        /// </summary>
        /// <param name="id"></param>
        public void BuyItem(string id)
        {
            BuyItem(id,1);
        }

        /// <summary>
        /// Get buy price of an item 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public float GetBuyPrice(string id,int amount)
        {
            return shop.GetItem(id).GetPrice() * amount;
        }

        /// <summary>
        /// Check if player has enough money to buy an item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CanBuyItem(string id,int amount)
        {
            return shop.GetUsedCurrency().GetBalance(profile) >= GetBuyPrice(id,amount);
        }

        /// <summary>
        /// Check if player has enough money to buy an item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool CanBuyItem(string id)
        {
            return CanBuyItem(id,1);
        }

        /// <summary>
        /// Remove item from player inventory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public override void RemoveItem(string id,int amount)
        {
            profile.Set<int>("shop_" + shop.GetId() + "_" + id,GetItemAmount(id) - amount);
        }

        /// <summary>
        /// Remove 1 item from player inventory
        /// </summary>
        /// <param name="id"></param>
        public override void RemoveItem(string id)
        {
            RemoveItem(id,1);
        }
    }

    /// <summary>
    /// Holds a player inventory info and allows to add/remove items directly
    /// </summary>
    public class SimpleShopInventory : ShopInventory
    {
        private Shop shop;
        private SettingsProfile profile;

        /// <summary>
        /// Internal constructor
        /// </summary>
        /// <param name="shop"></param>
        /// <param name="profile"></param>
        public SimpleShopInventory(Shop shop,SettingsProfile profile)
        {
            this.shop = shop;
            this.profile = profile;
        }

        /// <summary>
        /// Check if player has a minimum amount of an item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public override bool HasItem(string id,int amount)
        {
            return profile.Get<int>("shop_" + shop.GetId() + "_" + id,0) >= amount;
        }

        /// <summary>
        /// Check if players has at least one item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool HasItem(string id)
        {
            return HasItem(id,1);
        }

        /// <summary>
        /// Get player owned amount of an item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override int GetItemAmount(string id)
        {
            return profile.Get<int>("shop_" + shop.GetId() + "_" + id,0);
        }

        /// <summary>
        /// Add an item to player inventory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public void AddItem(string id,int amount)
        {
            profile.Set<int>("shop_" + shop.GetId() + "_" + id,GetItemAmount(id) + amount);
        }

        /// <summary>
        /// Remove an item from player inventory
        /// </summary>
        /// <param name="id"></param>
        public void AddItem(string id)
        {
            AddItem(id,1);
        }

        /// <summary>
        /// Remove item from player inventory
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        public override void RemoveItem(string id,int amount)
        {
            profile.Set<int>("shop_" + shop.GetId() + "_" + id,GetItemAmount(id) - amount);
        }

        /// <summary>
        /// Remove 1 item from player inventory
        /// </summary>
        /// <param name="id"></param>
        public override void RemoveItem(string id)
        {
            RemoveItem(id,1);
        }
    }
}
# 🦁 Lion Spoon Library 🥄
This library adds multiples classes to help the development of games by Hangzhou Lionspoon Dream Game Technology .

## Modules:
- [X] Core System
- [X] Settings
- [X] Language
- [x] Sound
- [X] LevelSelection System
- [X] UI/HUD
- [X] PlayStore APIS
- [x] Coin & Shop System
- [ ] Upgrades (Timed or not) & Collectibles (Coins, etc...)

## Core:

Init all libraries with settings
```cs
//Init with all libraries
LionSpoonLibraryManager.Init(new LionSpoonLibrarySettings()
    .WithModules(LionSpoonLibraryModule.All)
    .WithDebug(false)
    .WithSoundSources(4)
);

//Init monobehaviour
UISystem.SetMonobehaviour(this);

//Init Settings
SettingsProfile st = new PlayerPrefsSettingsProfile("profileID");
st.Set<string>("testData","DataDataRandomData");
```

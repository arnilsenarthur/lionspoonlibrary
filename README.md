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
- [x] Gif & Png Capture System
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

## Gif Recording:
```cs
GifRecorder recorder;
recorder = new GifRecorder(20,20,400,400,new Rect(0,0,400,400));

//Start to record
recorder.Record(this);

//Create gif from recording
Gif g = recorder.CreateGif();

//Bind gif to image renderer
g.BindTo(image);

//Save gif and get results
g.Save(this,"filepath/capture.gif",(prog,end) => {
    Debug.Log("Prog: " + prog + " End: " + end);
});
```


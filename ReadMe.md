# ITR's Real Creative Mod

It's a mod for muck that allows you to spawn in any powerup, item, or mob at the press of a key:
[![I gave myself OVER 9000 powerups in MUCK - \[Itr's Real Creative Mod\]](http://img.youtube.com/vi/NGzuEf7aWhI/0.jpg)](http://www.youtube.com/watch?feature=player_embedded&v=NGzuEf7aWhI)

## How to use

### Pause Menu
If you press escape you might notice two extra buttons:
![Pause menu with added buttons "Generate ids" and "Creative"](https://raw.githubusercontent.com/ITR13/ITR-s-Real-Creative-Mod/EscapeMenu.png)

##### Generate ids 
Use this to see the ids and names of all the powerups, mobs, and items.
It will create a file in the UserData directory named "all_ids.txt", then attempt to open it.

##### Creative
This will open CreativeConfig.toml inside the UserData directory.
If you don't have an editor set to open .toml files, just choose any text editor.

### Config

##### Format
Mobs, powerups and items are configured in the form:
```toml
[[SomeType]]
id = SomeId
amount = SomeAmount
trigger = "SomeKey" 
hold = "SomeKey"
```
- SomeType is either mob, powerup, or item, depending on which you want to spawn
- SomeId is the id of what you want to spawn, as specified in all_ids.txt
- SomeAmount is the amount you want to spawn
 - For mobs this is the mobMultiplier and bossMultiplier you want to use (makes them stronger / more health?)
- SomeKey is the key you want to press to use the mod
 - *trigger* is the actual key you need to press, while *hold* is the key you need to hold while pressing the trigger key
 - If hold is None then only trigger is used

You can also spawn random powerups by using:
```toml
[[randomPowerup]]
whiteWeight = SomeNumber
blueWeight = SomeNumber
orangeWeight = SomeNumber
amount = SomeAmount
hold = "SomeKey"
trigger = "SomeKey"
```
The three weights are for the different rarities of powerups. 
0 means never getting the item, while higher numbers makes it more likely (weight) / (totalWeight)

##### Reloading
The file is automatically reloaded and reformatted while the game is open
When the file is reformatted, it will insert a "name" field for the powerups, items and mobs to make the file more readable (unless it's already there).

### In Game
Look at where you want something to spawn, then press the key combination.
If you want to start a new world you have to restart the entire game for the mod to work again (other than for mobs).
It should work in multiplayer, but I haven't tested. It will only work for the host.

## Installing
1. [Install the MelonLoader](https://github.com/LavaGang/MelonLoader#how-to-use-the-installer)
2. Start Muck once and wait until it's fully loaded, then close the game
3. [Download the latest release of `ITR's Real Creative Mod`] and put the dll in your "Mods" folder (same folder as the exe)
4. Start the game and follow "How to use"

## Other notes
If somebody knows why ItemManager.Instance isn't working, pls tell me. From what I can tell, other classes in the game use it completely fine, and I don't cache it or anything either.
Using FindObjectOfType<ItemManager>() finds one, but it just spawns items you can't interact with.  
I suspect one reason tons of active powerups causes so much lag, is due to how OnTriggerStay is set up for them. Might inject a more optimized version of it if I feel like it, but honestly not needed for the base game since you'll never have that many active powerup objects at once.


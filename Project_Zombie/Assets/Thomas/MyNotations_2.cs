//MY NOTATIONS 2 - DESIGN
//what can i learn from biding of isaac?
//each map should have its secrets.
//

//where to get luck?
//you get it by beating especial creatures.
//

//WHAT SHOULD WE HAVE IN THE CURRENT MAP
//you can mine resources? and resource have a chance to spawn something else or trigger something else.
//special enemies will spawn. once damaged they will flee and despawn in a short moment. killing them grant something. 
//there are boxes that grant something random for the run. like higher luck
//certain itens or quests can only be completed in certain round types, and those round types can be triggered in some way.

//possible secrets
//hitting some things that are hidden.
//there are vases that can be destroyed
//you can get random stuff as reward from the shrines.
//i want it to be faster. certain shrines just give stuff.
//there are certain terrain quests. like a place that you have to hold your ground, kill enemies in area. run around the map
//there should be a bit of plataformer?


//STORY CHARS:
//Stich: one handed doctor. gives you healing passives and trinkets
//Filla: a calculative bomber person who keeps bombing to not sleep. she grants you explosions stuff.
//Arko: 
//Tuknan: 






//VERSION 0.6 -==--==--=-=-=-=-==-=--==-=-=-

//NEW CONTENT (Start date: 09/08/24 - 19/08/24; End date: ;)
//you are able to alter the run at the start. choosing harder modes, or modes where you cannot heal or other stuff.
//vision fog. the player does not see things that visible for its character
//get 3d models.
//animatio  systems
//create sound system
//create save system, and save slots
//use dots with some parts of the game. like bullets and parts of enemies. (ONLY IF THERE ARE PERFOMANCE PROBLEMS)
//bosses and mini-bosses.
//create a new round type where there is boulder constantly falling down
//round type where 
//create a new map: there is an energe generator. there is a cart you can use to go to other places. 
//create a better fade ui for misc messages
//create enemy lancer, it has a shield and a lancer. it show an ability indcator for the lance attack, which is middle range.
//each ability has one out of three Coins. which are gained when obtaining an original ability. three of those can be traded in the 
//can talk with the merchant and use bless to reroll the abilities, or to show the especial abilities, which are bought with those especial coins.
//enemy spawned should be improved. because currently as long as you keep killing the enemy it will keep spawning the same especial. make a limit per round instead.
//Buy UI. need to be improved. also all the things should have a description.
//pressing escape when the settings in on the screen makes only the settings close
//power ups appear in the screen in some especial way

//RODRIGO REPORT
//highlight important words in dialogue.
//when you reload the stage it is showing the weapon you had. the tempo weapon.
//the teleport broke the game, and the enem,ies are no longer spawning.
//two resets. 
//not everywhere you should be able to fall
//map progression. 
//challenges would be a cool idea
//improve the aim. its no accurate enough. mostly the grpahical stuff, should show how unprecise the aim is.
//nerf the quantity of mage attacks. nerf the health.
//heavy assualt more ammo.
//the flying turret is not spending money
//the flying turret is shooting infnite.
//the game is breaking at 7



//GOAL
//the teleport broke the game, and the enem,ies are no longer spawning. <= REQUIRE FURTHER TESTING
//effects for teh teleporter, something cool
///when you reload the stage it is showing the weapon you had. the tempo weapon.
//not everywhere you should be able to fall
///improve the aim. its no accurate enough. mostly the grpahical stuff, should show how unprecise the aim is.
///nerf the quantity of mage attacks. nerf the health.


//what would make a teleporter cool?
///start rotating the teleporter and stop after the player leaves
//a sound start getting stronger
//if you are stunned during the teleport it should stop the teleporter.
//after 40% is charged then movement is locked. 
//shock start appearing from the machine
//the screen fades to blue, but it needs a cool whirl effect.

//if there is a wayout it will use it instead of teh telerpoter. always
//but it always inform the other teleporter that it can be used.
//if either of them has cooldown, the cooldown is activated.
//if you use a teleporter that does not have timer, nothing happens
//if you go to a teleporter that has tiomer, it starts the timer
//if you use a teleporter that has timer, it spots the timer.


//it starts with the sound, when the sounds starts getting really loud, the player gets locked
//smoke starts playing in the middle of teh sound
//once the player is locked, a blue effect triggers on his feet.
//the screen starts becoming black, mid screen transition the player is made invisible and thunder appears
//after transition we go to the next thing


//GOAL
//place the 3dmodels. enemy.
//3d models can use the gun_Perma, the hands will move to 
//animation for walking, dying, swapping guns, abilities.
//the player should be aiming at roughly the center and never shooting at further than the aim
//




//create animation
//walking
//walking back
//the player can hold different weapons
//reloading different weapons
//when the player gets hit
//when the player dies
//when the player falls
//when the player dashss
//when teh player interact with somethiung, and is doing nothing else
//then we start creating animations for the enemies.
//also animation for shooting. as it creats a backfire.


//now lets figure how to hold the weapon, and then the play animation right for holdign teh weapon
//then we are going overhaull the swap weapon system so that we can use animation
//then we need to check being hit and its animation
//reload animation
//then we are going to do dash
//then death
//then we are going to head to the player system


//it looks weird because of the animation.
//how to improve the animation?
//the gun_Perma is too big
//

//then the next step will be creating the enemy prefabs and animations
//certain non-especial enemies have a pool of prefabs they can spawn.
//i want a difference between running, and walking

//the only thing i will do for now is change teh speed.
//there is a cap to speed, a min and a max, and i will use to determine a value between 1 and 4


///now i will do the equip stuff.
///there will be stpo for each weapon.
///now there is a timer for swapping guns. it can be stopped by the same thing as reload
//then we will create a reload animation.
//then player getting hit
//animation i will do later 

//now i need to createa  system taht works like this
//you can use E and Q for swapping guns. 
//any temp gun_Perma chosen the perma gun_Perma will take its place.
//we will also overhaull the the ui for gun_Perma ui

//GOAL PATH
///need to fix the reload.
//need to test for getting a chest gun
//need to test for getting the upgraded

//GOAL ANIMATION
//create dash animation
//


//enemy has animations
//if the player has not being spotted
//WHAT THE FUCK!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
//there is a problem where enemy animation have numbers after them 

//now to do a blood effect 
//

//GOAL FOR TODAY
///the dash effect. <=can be improve but for now it will do.
///the enemy animation dealing damage.
///a red effect when enemies are hit
//blood particles from being damaged, both player and enemy
//blood also land in the ground and the walls.
///ranged animation


//GOAL FOR TODAY
///enemies have weakness and strenght against the different elements: Physical, Magical, Plasma, Corrupt and True
///only the player has armor, which is a value that blocks everything. but the player suffer something unique for each damage
//then we improve the damage popup.
//take this moment to improve the damage code for enemy and player.
//i want the different damages be ligned.
///how damage scaling works
//the damage per bullet should work with events
//spawn some blood 
//player death animation
//enemy death animation


//the fade needs to move and be visible from all angles.
//i need to place fade in the player´s thing, but before removing the enemy from the game the fade ui needs to return to the right container.
//


//think the player not rotating correctly. especially when you aim  atht ehe nemy
//its working when aiming at the enemy, but now need to fix it being a bit wrong when not aiming at anything
//



//instead of just damage, we are going to have a 
//guns do not have just damage, they have a list of the damage tehy can do, such as magical or physical
//pen work by adding to any value that was lowered, up to the original amount 
//the player has a damage stat. damage increases all weapon damage. by a percent amount. every gun has a scale. only the first damage 

//DAMAGE POP UP I WANT TO GO UP AND THEN FALL TO THE LEFT AS IT FADES AND ALSO A SIMBOL BY ITS SIDE?



//what i want here?
//i would like the picture of the guy?
//i would like to be able to swap to talk with the people
//upgrade the storage where?
//all the sale is done as a conversation?
//i would like the picture by the side. maybe we could set as a picture instead of the actual model.
//or even chaging the camera to zoom in the npc´s face.


//you start the dialogue but at teh side tehre are three options, as it always has, Dialogue,check itens, and upgrade store.

//GOAL FOR TODAY
///create the damage ui
//the damage per bullet should work with events. damage per bullet type of thing.
///spawn some blood 
///enemy death animation
///to remove the enemy corpse we will play an effect
//deal with the animation for the city.


//GOAL
///a proper death ui
///player death animation
///when the player dies we zoom in the player.

//when you first open a tab the itens will reveal in order.


//GOAL
///there should be no gun ui if there is just a perma gun
///now having two guns is causing problem

///then a portal appears below the player and it goes down.
///create the animation and effects for the gun chest
//then the other chests <= test them more and complete the resource and ability.
///improve getting the new abilities UI and maybe the effects
///more sound effects for picking the ability.
///give a sound for the buttons, just the buttons for meantime. they all will have the same sound for hover and click <= test it
///you can see your abilities, and you can scrap those abilities and you can reroll, rerolling it increases the price for the next time. <= test it
//create a sound effect for scrap and for bless rolling



//maybe we can add a cool effect when we choose the card. like they all disappear. and the card you choose increases a bit and decreases.

//GOAL TOMORROW
///test the things from yestrday
///the cool effect for the ability ui.
//the ability ui to just improve it. make space for the improved ability. if there are no abilities it should be turned off. but it should always start from left to right.
//return to finish the teleporter. <= I SHOULD FOCUS IN FINISHING THE FIRST MAP
///if you cannot use the bless button it should show somehow.
///improve the resource gain pop up.



//GOAL TOMORROW
//put some effects in the portal. they create a sound when they open, but its local.
//revisit shrine system
//create challenges.
//make the aim better. it needs an offset

//CHALLENGES
//



//CREATING THE MAP
//terrain stuff
//light stuff
//the vision now disables thing that are too far for perfoamnce, and hides enemies.



//I WILL BE HAVING PERFOMANCE PROBLEMS IN THIS BUILD. for now we continue till we find actual perfomance problems.
//replace foreach -> for loops. they apparently run better
//some behavior i might use dots.
//create multiple canvas so they are not all rendering all the time.
//meshes seem to be much heavier. there must be a way to reduce their cost.






//things for reset
//the rotation for when you are holding the gun is not quite right after reloading
//and also the enemy isnt attacking
//if you have only a perma weapon it should not show more stuff.
//the player keeps dying.

//why is the camrea moving like that?
//I KNOW WHY? its because of fall damage.

//perhaps i can get the hands a bit close to the center, the problem is that they are too much to the side.


//each gun needs to hold a value for damage 
//each ability will show stacks and damage or healing or whatever stats it has

//those stats will all be show through a description window.
//ALL THINGS WE MUST SHOW INFO ABOUT
//abilities found, each stack and how much you used them, and how much damage you delat with them
//same thing for temp gun.
//same thing for the drops
//same thing for the resources.
//generic info: like total damage, money spent and money gained.
//i need to show story quests.
//i need to show equipped gun
//equipped abilities
//equipped trinkets.
//shrine challenges


//for the temp:
//gun
//ability
//general info
//resource
//challenges



//


//THINGS TO CHANGE
///the enemy must rotate to always face the player
//the animation must dictate when the attack deals damage and when the zombie can continue walking
//the attack from the enemy must only be where the enemy is attacking, so cannot be based on range
//create hit animation
//also when the zombie is stunned it plays idle animation
//the enemies can ragdoll depending on the damage.

//blood particles, make enemy deaths fancy

//GOAL
///need a dash animation. i wont do a roll, the player will disappear then reapear, create some effect for appearing and disappearing
//the main menu animation
//improve the damage popup. difference for damage, show the crit, make it feel better
//the player getting hit
//the player death

//GOAL
//need to improve the pause menu
//need to improve the city scene, put graphic in teh cities 
//improve the buy stuff
//they should show the level and what building it is.
//there is a graphic level for these fellas.
//for building the thign





//MY GOAL FOR CITY BUILDING
///change the text for resource units
//change the graphic for building. a need a graphic for when its not done, and a new one for each different building
//create the training area.
//

//what can i do here?
//then i have to improve the ui of all shop stuff.


//VISION FOG
//the origin point is all wrong 
//its not related to the mouse position
//i dont want to just show waht the player can see, i want there to be a dark fog, and enemies that are not seen for long enough time 
//i will go back more to the end when i start using the light system


//GOAL
//the flying turret is not spending money
//the flying turret is shooting infnite.
//the game still breaks sometimes.
//pressing escape when the settings in on the screen should make only the settings close
//put the settings in the main menu

//GOAL
//every ability has one out of three coins. you can use that resource to buy stronger abilities.
//you can talk with the merchant. he will offer to roll the abiities once again or to show you the strong abilities.
//create 3 strong abilities
//Buy UI. need to be improved. also all the things should have a description.
//change the ui for description, abilities, equip window

//GOAL
//vision fog. the player does not see things that visible for its character
//bake light

//GOAL
//create minibosses that will appear every x turns.
//the bosses needs to be called
//create enemy lancer.

//GOAL
//create save system
//improve the main menu
//put the settings and improve the settings
//settings should also change graphics and should look better


//THINGS TO ADD IN THE MAIN MENU ANIMATION
//zombies walking in the background
//a mage levitating over the forest
//a comet passing
//a light from someone walking in the forest
//a dog coming over to play
//the player pull certain things from time to time, like especial weapons and bombs or rubber ducks
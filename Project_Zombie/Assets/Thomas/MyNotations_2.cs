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
//make the aim better. it needs an offset. the aim is all crazy for some reason.
///change the fountain to be a shrine as well. but its a shrine that has an effect 
///make the system spawn the fountain. if there is no health fountain it must spawn a fountain health, if tehre is there is two health fountain it must spawna buff one.
///remember the audio for the fountain health and buff 
///you can only get one curse at a time 
///you can only get three challenges at most.
///show those problems through fade ui.
//improve the quest interface
///when teh quest appear it informs what kind of challenge it is, the text fades and then the images moves away. maybe an animation that highlights the image as well


//TEST MORE THE QUEST SYSTEMS?
//i need place to hold those places. <= teste


//MY GOAL
//we need a system for closing all doors, they cannot be bought, and then opening only those that were already open.
//improve enemy spawn system
//create teh challenge that you must stay in the area. it might move and change places.
//damage someting enough in a duration, otherwise it explodes. or you fail.
//there are enemies that will run away from you. killing them will grant a unique resource, but in enough time the enemy disappears. <= teste
//we need to stop regular spawning 
//we need to create ui in tteh top that represents the amount to complete, also reprent a boss health. <= teste
//create little squares in the ground that are traps. if you press it you take damage.
//another trap summons a damage area.


//the area should be able to move, and change size, it change position.
//the damage object should do what? it has an attack pattern. create damage areas.


//GOAL
///can start a challenge of area. 
///the door closes.
///the spawn outside stops.
//the challenge begins with its own spawn system
///once done the door that was open, opens again
//the spawn system resumes
//create the damage object challenge
///create a trap. first its spikes in teh ground, play an animation and in the end of the animation deal damage and apply bleeding


//its spawning but for some reason its spawning somewhere else.

//GOAL
//the ranged doesnt have a death animation
//for some reason the enemies being spawned because of the challenge are being spawned away from the right position. but only in challenge.
//need to finish the knight



//i dont want the animation to curve the body that much.
//letss call the attack.







//who is spawning the traps?
//they spawn randomly. there is always a limit, and they can only spawn away from the player view.


//i will create lists that take round to spawn people.
//they will use their own list but use the round amount to contrl the frequency
//


//how am i going to do this?
//the bosses should have multiple attacks.

//door ui needs to be facing player
//cannot show price when the door is open
//when the door is open for chllanege should inform
//should have sound for money spent
//should have sound for door open

//

//MY GOAL - KNIGHT
///we get the knight moving and calling the attacks
///first the slash, and it deals damage
///then the thrust, it uses a boxcollider to deal damage.
///the attacks wait and then attack but without 
//then the knight will summon the blades. two that circle around and one htat moves randomly.
//then we create the skill 
//then we make it die.
///killing a boss grants 2000 points, 5 bless and a sigil. the sigil will be used to summon a boss.
//the sigil should show in the ui
//add sound effects.
//add especial effects.
//add a especial effect for teh flying blade. it has a sound as well, but only when you are close.
//
//now we are focusing on the blade.
///no idea whats happening to it. but it destroys itself when its created.
//the movement is better.
///now lets put everything together. and lets create teh chance thing.

//


//GOAL FOR TOMORROW
///get the blades working
///every ability has a chance to trigger. put that in the attackclass
//it can die and it has a death animation
//fix the animation using the rigging.
//add an effect 
//add sound

//effects
//slash
//charge thats works for all
//thrust, destroying everything in a line and raising smoking. 
//death needds a better looking effect.

//the effect will be a bunch of smoke and the cracked in the ground that slowly fade. thats it.
//i want the effect to persist there.
//what i can do is to trigger something 

//there will be no cracks in the ground because i found no assets for it.

//GOAL
///death animation for knight
///cool effect for the death, instead of just being pulled down
//add effect for slash. add sound
//add effect for thrust. i still want the smoke, that will be enough. add sound
//add effect for summoning the blades. add sound. a scream
//still need the rigging at least for the shield.
//shoud have sound for being damaged.
///should have sound for their steps, because they are heavy.
//maybe some breathing.
//i need some additional sound for the charging, just triggering the thing is no good
//also make the player footstep sound.


//now i need to ge thte idle animation working to show thats it not using the shield


//THEN KNIGHT IS COMPLETE



//so now we create the smoke effect, it needs to be something apart because it needs to last after the animation
//the blades are working. need tge random one?
//

//the collider is still not working when you are above. perphaps camera problem? lets use the boxcollider itself.
//



//the legs are not moving though, they stop for some reason.
//the rig is no longer working.
//lets make the slash work and feel nice.
//then we will make the thurst, no jumping steps.]
//

//so the slash is working, lets not move the the thrust
//now the overlapbox is not working
//its not hitting now.
///we have to make it rotate to face the player.
///but i also need to only call the attack if its facing the player.
//now lets check the ui.
//the box collider isnt exactly presicen, or maybe the ui isnt
//i wna thte ui to show it a bit more ready. when we are doing you are supposed o wait a bit more.
//now there is a ltitle bug the thing is not being called?

//sound for death
//check the death ps as well
//



//the problem is the 



//MY GOAL
//transform the fireball projectiol into a bulletscript


//MY GOAL - GHOST
//create the projectile for ghost. 
//create condition called "blind" which reduces the player´s vision
//the ghost create damage area where the player is. it deals damage and blinds
//first attack is without warning and fast.
//it passively creates areas of slow.

//first we will get the ghost moving towards the player
//not taking damage. the damage can still pop
//i need an animation for his movement
//then we will create the damage areas.

//we have a problem with stacking, because the speed is not stacking, and when they are completed they are bugging
///now lets take a look at teleport.
//it has the same condition of being walkable.

//LIST OF THINGS LKEFT TO DO
//area challenge
//test the enemy system spawn
//the knight is bugging
//the bd are not stacking correctly. found with ghost
//have to check the ghost death.


//TO DO
//the bd are not stacking correctly. found with ghost
//have to check the ghost death.
//mage shoot projectile. it triest  to follow the player.
//then barrage of projectiles. its random.
//then the meteor. its just when its too far for the mage.
//it creates a barrier at cooldown


//we are first going to do the simple attack. just a projectile straight at the player.
//and also check the meteor.



//i feel like just making an standard mage is not as interesting
//but if i give it buff abilities

//MY GOAL - MAGE 
//its first move and the most common is a projectil
//the second move is a barrage of projectiles
//it can passively create walls between it and the player.
//its third move it buffs all enemies, granting them additional damage and speed.

//twins should be an actual boss.


//i need to 

//GOAL
//i will actually do the merchant now.
//create a dialogue for the merchant
//so you can buy stuff and change the actual itens. they cannot change back
//the timer is still running so you can be pulled away from the dialogue because of it.
//lets also improve the teleporter.

//need to create triggers so triggers those things


//should be able to get quests from dialogue as well.
//

//change the ui when you are in dialogue.
//response units should not start with a hover.
//change the dialgoue. each npc has a camera position which when you talk with them the camera takes that position.
//what the merchant requires.

//actually the dialogue will work by zooming in
//the npc turns to the player
//teh response holder

//its not rotating to the characer.

//additioal things to put with the merchant
//a little casino machine that you can gain : points, bless,
//you can get additional musics to play here?
//you can find other npcs in this area.
//when the time is over the merchant says something as the player leaves.



//
//THE GOAL TOMORROW
///update the graphics for teleporter
///the teleporter effects
//imrpove the merchant room
//creat the animation for merchant
///sound effects (when start talking, when stop talking, when buy something from dialogue)
//background sound effect from the tavern
//the little cassino
//sound for dialogue.

//make the portal for the merchant. as long as you go through the portal you will be returned.
//this portal also triggers the merchant

//i will create a muischandler.
//acutlaly i wont use audio because then every npc will have the same audio and the player wojt have any.
//we need a sound for spending bless.

//dialogue not working?
//sound for typing.//

//when speeding dialogue there should be no sound or another sound
//another sound for clicking response
//i have no slotmachine asset /




//THE GOAL 
///need to be able to highlight dialogue. with custom colors for custom things.
///check for the dialogue. it had nothing.
//put the background sound for the merchant
///make sure the teleport timer is working.
///need to pull the player out through hte teleport timer.
//music system for backgroubnd music.
//teh response needs to hide when not being used.



//how it works?
//i want a music for boss
//i want a music for calm moments
//i want a music for tense moments
//i might want music for certain rooms.
//i want a music for mainmenu
//and a music default

//i need to allow for silence as well. 
//a boss can triggers its own music, the msuci doesnt loop.
//areas can trigger the music, and that music doesnt loop.
//then we have a default list for stage
//the default has two lists. low and high. 
//high always stop after a round is over.
//after high is over we have a moment of silence.
//after that moment we go to low
//we only return to high when there is a lot of happening
//the triggers are: a lot of enemies around, a mini-boss being spawned, fighting a boss, certain enemies



//
//THE GOAL TOMORROW
//do the sound handler and get bgm working
///fix the aim again. it seems to be a rotation problem rather than position
//fix the final parts about ghost
//return working on the mage


//we need to fix the aim. the rotation is all fucked
//then we need to fix the bd stacking not working.



//speed isnt stacking. what else isnt stacking?

//lets focus in the game then


//the animation isnt right
//get a blue meteor for the boss
//need a sound for the explosion. also for the other thing
//need an animation for shooting, but very importantly it keeps moving while it shoots.
//the black orb should have a sound as well
//


//MERCHANT




//need an idle animation for the merchant
//and an talking animation
//when you talk with it and when you stop talking. the sounds can change.
//i want sound when you choose something for the merchant.
//need sound for choosing the response.
//need a background music for the merchant.
//


//MY GOAL -  ARTILLERY
//it must always spawn in the farthest room possible. can only spawn if there are enough rooms opened.
//it shoot slow and huge damage areas.
//when it gets close it shoots barrages of 
//

//MY GOAL
//create a spawn system for the boss.
//
 


//MY GOAL - DEVIL
//its a melee character.
//attack 1 


//CREATE KNIGHT

//the damage works the same
//the animation works different in boss. each boss has its own animator. we dont use entityanimator here.
//


//knight has 5 attacks
//a swipe, that deals damage forward
//a thrust, deals damage from far from a line. 
//when the player is distance, the knighyt uses a shield, and gain velocity.
//summons blades that move around for a while. they work by moving randomly, till they meet a wall or a ledge then changing position


//the problem is that when the character has nothing to do it keeps following.

//CRATE GHOST
//ghost has only one attack. 
//ghost cannot be killed and just move towards the player.


//for the orb we need to check if there is ground in that region.
//we need something connected to the ghost that is all the way up so it can see.
//we can actually just shoot a spherecast smaller than the actual arae.




//we are going to use animation for both.
//then we are going to do the teleport.



//CREATEA ARTILLERY
//need to spawn in the farthest


//MY GOAL
//create Lancer 
//create Ghost
//create Artillery
//create Mage


//MAGE
//she walks slow
//if you are far from her she will shoot thunders are you
//when you are close enough, it shoots projectile
//spawn especial ranged enemies that cannot walk too far from the mage
//increase damage and speed of all enemies.
//create barries that block player projectiles.

//ARTILLERY
//it stands still.
//shoot at teh player from anywhere in the amap
//the player is supposed to find the enemy from teh sound.
//when the player gets close the artillery changes from single big shells to small ones.



//MY GOAL
//create the boss room.
//the boos room you can go and summon the boss
//if you summon a boss all enemies will be killed and the round stops
//create Devil
//create Tree
//create Corrupted tank


//minbosses always spawn at round semi-randomly. 
//but you should be able to do certain thinks for force a spawn. but it works a bit like a easteregg.



//MY GOAL
//create mini-bosses.
//one type must be summoned using bless 
//another summojns at a time and goes after the player
//another is a immortal beast that cannot be killed but after enough time they disappear.
//to summon a boss you must have certain keys. the boss must grant something. once a boss is summoned it closes all doors and has a challenge by itself.

//Mini - BOSS
//Lancer - he charges, he throws long ranged attacks, and has high defense
//Ghost - Cannot be killed. but fades.
//Mage - it buffs allother enemies, and throws some circle around. it also has shield that regenerate once down
//Trash - it infects areas where you cannot go for a duration. also the trail of it walking deals damage.
//Artillery - shoots from somewhere in the map and deals a lot of damage.



//BOSS
//Devil (Chains) : use a lot of chain to try and pull and do a lot of damage. force mellee.
//Tree:  it stands still and just throw a bunch of stuff. it summons a lot of enemies.
//Corrupt tank: a tank full of zombie that shoot a lot.

//MY GOAL
//create teh extremely detailed endgame.
//create the system for the curse to penalize


//CREATING THE MAP
//terrain stuff
//light stuff
//the vision now disables thing that are too far for perfoamnce, and hides enemies.
//create that fog effect for the door, because i cant without the new graphic


//I WILL BE HAVING PERFOMANCE PROBLEMS IN THIS BUILD. for now we continue till we find actual perfomance problems.
//replace foreach -> for loops. they apparently run better
//some behavior i might use dots.
//create multiple canvas so they are not all rendering all the time.
//meshes seem to be much heavier. there must be a way to reduce their cost.






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

//

//the problem is that i need to place it back


//currently -> cna i do it in 2 months?
//i am doing the mini-boss and boss -> 7 
//change the graphic of all enemies and create animations -> 7
//i still have to do the map, and the lgiht and graphics -> 7
//then after the light i need to do the system for shading everything that isnt visible by the player. -> 2
///then i need to creat eteh merchant dialogue, and the possiblity of the especial abilities. also highlight certain words -> 2
//improving the main menu -> 3
//create system for saving -> 3
//improve the settings ui -> 1
//improve the city. -> 7
//general bug fixes -> 7

//tahts about 63 days, a bit more than 2 months
//now its about 43 days -> i will say 50 days

//(21/09)
//Days passed: 8
//days left for target: 42
//Days left for project: 153

//RECORDING
//BOSS -> 13/09 - 


//I have Result: 163 days (day 13/09/2024)
//

//Version 0.7 ()
//we will be focusing on content here. we will create all passives, actives, perma and temp guns, story quests and challenges.
//create passives that stack on actions.
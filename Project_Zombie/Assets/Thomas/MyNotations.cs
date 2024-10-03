
//MY NOTATIONS


//does this game need a server? it would require for the mobile version. 
//or we can just check by time. but what would create many problems



//IT needs to be easy to change from mobile to pc. my plan is to do both, at least partially, leaving place for 



//GOAL
///move system. the player can move and can look around
//gun_Perma system. can equip and shoot the weapons.
//interaction system. can interact with items and doors.


//need to make it shoot now.

//GOAL
///there is the one perma and two temps
///create ammo system. perma always has infiite ammo
///can reload. 
///can change the behavior of bullets. we carry the bullet behavior in the player because this will be universal now.


//GOAL 
///create stat handler for entity.
///the player has stats
///the enemy has stats that can be scaled.
///first i needd to be able to change stats in a non-heavy manner
//i need to make every gun_Perma, enemy and etc assign to the right event. and remove said events if they are killed.
///can crit. and it is influenced by player critchance and critdamage. guns also have modifierse.
///handle penetration.
///stat reload speed affect reload speed.
///create a system for handling damage reduction.
///create popup for damage. it needs to always be facing the player and also show over everything else.

//GOAL
///can interact with a chest.
///when you receive resources the game is not paused. the icon appear and goes to the player and then it appears in the top what you gained and how much
///

//GOAL
///the effect of many guns appearing till one is hover.
///i need to randomly select 
//it shows the stats of that gun_Perma.
//we can reroll
//if we already have two guns then we need to select which one we will be replacing.

///cannot click till the thing is done spinning.
///now i want to do the reroll
///then i will do the equip with no other gun
///then we will make that your guns can appear by the left.
///you can hover them to see their stats in relation to the chosen. when you no longer hovering it instantly goes back to only the chosen
//if you have two guns you need to select one for replacement. then we need to actually replace them and simply throg away the replaced gun_Perma.
///the rotations of new guns seem to not be working.
///the chest gun is not swaping the right weapon


//when you receive a gun_Perma. it pauses the game and shows the the gun_Perma. you can roll again for the double of points.
//when you receive an ability. you gain three options which you must choose. you can roll for them again but you need tp spend a lot of points
//can interact with the door
//fix what happens when two interact are too close.

//GOAL
///gain point for shooting. create a nice effect for point gaining
///create a health bar.


//GOAL
///create bd system. create bd units that appear somewhere and that you can look at it.
///create a pause menu.






//GOAL
//create map 
//portal spawn
//the three chest spawns
//different chest behavior
//put the round or timer system
//enemies start spawning
//you are given quests to complete for rewards
//you can extract to win resources.
//the game can be started

//GOAL
//pause menu base


//problem! if you can extract that takes a bit of the sense of getting the temp guns
//actually extracting should be really hard.
//but what about the resources you got? you still get the resources. you can extract at certain places for a lower reward than continuining.


//GOAL
///can add and remove bd
///stats are correct.
///can stack passives.
///show all weapons you have and which one is currently being used. show the current ammo all weapons have.
///there is a weird problem that swapping guns are not working.
///fix the gun rotation
///fix the gun swaping to wrong currentBulletIndex.


//NEXT GOAL
///creat the chest ability ui
///three options for ability.
///you need to roll for these abiity and its based on their tier.
///you cannot get two rolls of the same ability.
//the rolls are influenced by your current abilities. there is a bit higher chance to get abilities that you already have.
//you cannot get an ability that you are already stacked.
//you can get abilities with different tiers. the same ability but better and they should be able to stack instead of clearing the previous progress.
//the ability cards should show tier, name, what it does per stack, the current stack and how much it would improve.


//GOAL
///create door 
///create pause
//create the possibility to change keys for whatever you want.

//NEXT GOAL
///create enemy behavior. 
///create pathfind
///take a look at destructable terrain
///create damage class
//create simple melee
//create simple ranged.
//create heavy
//create bomber.
//enemy can be shot and can be killed. show the damage pop
//enemy spawn from portals that can oepn in the world. but they also can be closed.


//GOAL
//create spawn system
///create ui indicators for teh zombie abilities.
///create three tipes of simple melee. they are just different streghts. they all move in straight lines to the player. 
///create a bomber enemy. the enemy explodes after getting close.
///create a tank enemy. its slow and a lot of health. it causes slow in area. its attack stun the player for a short duration
//create charger. it dashs at a straight line. only if it has direct line of sight with player. 
///create hound. it is fast and deals a lot of damage. but its easy to kill. 
//create mage. it creates damage areas that will attack on cooldown. need to have line of sight. 
//create shielded. can only be attacked from behind. the front require a certain level of penetration.

//NEXT GOAL
///create spawn system
///create system for gunboxes
///create system for resource box
//create system for ability box
///create stunned system


//GOAL
///fix the pathfind. they are not tarversing where it used to be a gate.
///the resource chest is not being destroyed.
///the resource chest is spawning at teh start. it shouldnt.
///we should go through the resource chest.
///the enemies are not dying.


//GOAL
///in the base we wont show any of the combat ui.
//we can go to the training area to see our stats.
//create ui to see all the resources.
//create Smith: can see the guns and whats required to get them and can upgrade.
//create ability: same thing but for ability.
//create armory: where you can equip all of these things.
//create hq: here you can launch missions
//training area: you can increase stats here.
//create the test area where you can try your abilities and attack the 
//there is a thing by the side that you can open to see all of your player stats and stuff


//GOAL
///create stunned system
///create immune system
///create dash system
///create bleeding system.
///now we create bdunits to show the player what he is suffering. the enemies can also have it but they disaply somewhere else.
///stun and bleeding apply to enemy
///the bd in top of enemy´s head is not working because i think grid doesnt work in this utation.
///you can see all your own bds and pause to check them better. can see the guns and can see the stats. we need a whole list by the right.
///change it so the description appears as a window instead of staying in place. then i place the passive holderMain there.
///create and update passive description
///create and update stat description
//create gun_Perma description

//GOAL
///create ability box. each enemy has a chance to carry a ability box that will be dropped once dead.
///shotgun spread is not working.
///fix the way damage pop up is working. the damage should stay after the nemey die. once the enemy die we remove the canvas, and then after there is no more damagepopup it is destroyed
///should not damage itself.
//BUG? after a while the fellas are just wondering around. 
///the chest is not being destroyed for some reason. just some times.
///fix the chest appearing stuck. we can use grtavity spawn the chest high and then let gravity adjust it.
///the gun is shooting twice sometimes.
///when swaping you block input till you release your hand
///reloading must update teh gununit


//GOAL
//can summon a sentry.
//enemies can attack turrets.
///can become invisible for a short duration. meaning that enemies will ignore the target. no visual stuff, just a bd and make it invisible
//the turret is losing aim of the enemy.
//turrets can be damaged and destroyed.
//turrets have a cooldown timer.
///can become immune for a short duration
///create system for a weapon to give the player passives.


//GOAL
///Pistol Speed: grants an amount of speed and less cooldown for dash
///Pistol Sharp: Increases damageback per {}. and a chance to dodge.
///fix the damage popup. its too far from head and doesnt follow the target. 
///SMG: has a chance of each shot to shoot one additioanl bullet.
///sawed off shotgun: deals more damage per additional pellet. 0.5 % per pellet.

//GOAL
///creates system for dodge. and when you dodge there is a popup
///creates system for damageback
///creates system for increasing the amount of shoots based in a _value.


//

//FOR TODAY
///shoot a fireaball at mouse
///create burning tick
///instantly reload the mag and deasl damage around based in the magsize.
///for a short duration all your weapons shoot twice as many bullets.


//FOR TODAY
///your weapon slot is linked to your gunclass. any changes are made directly.
///you can drag items.
///create window description
///if you release into a player slot and with the right type then we order the swap
///weapons you are using or abilities do not show in your options
///you cannot drag your weapon away because you must always have a weapon


//GOAL
///you can oepn the armory.
///you can buy stuff in the armory and it will appear in the tab.
///opening the tab always close citystore ui
///you can drag away and remove your abilities.
///you cannot move or do anything while the tab is open



//FOR TOMORROW
//passives stack properly when they are different levels. the basci will be influenced but the 3 stack is always the same.
//improve the passive algorithm :
//cannot give the player a passive that the player can no longer stack
//should prio a passive that the player has. increase it by 40% chance
///create lab
///create smith

//GOAL
///shoot a fireaball at mouse
///instantly reload the mag and deasl damage around based in the magsize.
///for a short duration all your weapons shoot twice as many bullets.

//GOAL
//pasive algorithm needs to take in mind the following:
//cannot give the player a passive that the player can no longer stack
//should prio a passive that the player has. increase it by 40% chance
//Passive Every hit applies bleeding
//Passive slow enemies by 1% per stack in shoot. at 4 has a chance to stun
//passive idle regen
//passive reload speed
//passive skillcooldown
//passive increase maxhealth. at 5 get a shield that is based off maxhealth.
//create shield mechanic.
//passive if the player has this item then he will revive once dead. but only once. cannot stack.
//passive deals 20% damage to bosses . at 3 deals 15% more damage to enemies with 80% health <= UNSURE
//passive enemies that have a debuff receive 3% more damage. at 5 they explode when killed.
//passive killing an enemy grants you 1 additional health. up to 100 per stack, can stack 5 times.
//heal one by every enemy you damage. the value_Level increase one by every stack. up to 5.
//Does nothing till stack 3 - enemies that you hit that are below 15% of health instantly die.
//passive enemies around you are randomly struck by lioghting. every stack grants an additional lighting and 1% to the damage. the damage is based off the player´s damage.
//passive dashing deal damage. every stack increases the damage. at stack 4 you get a an additioanl dash ammo.
//you are luckier with all chests.



//there should be more to do in the base 
//there should be quest and a story line.
//there is a bit of citry managing
//you can send people in missions. you can put people to work.
//but they require something. they require resources, and you can expand and build in new places build new stuff.


//the city data should tell what kind of ui it will open?
//we can put that ai apart from playercanvas because there is no need to carry that everywhere.
//also a singleton and its called baseCanvas.

//GOAL
//create base systems
//can visit lab where you can get active abilities
//can visit smith to get perma guns
//in the right side you can click and a window will appear where you can equip all that stuff
//can visit the launch pad where you start missions
//can visit the HQ where you can get the quests.
//
//


//NOW the goal is to create a playable enviroment. for this i require:
//a draft of base upgrade minigame: ability roll ; gun_Perma roll ; active abilities ; main guns ; improve stats(something different here)
//active ability: tier1 - 3; tier2 - 3)
//passive ability: tier1 - 6 ; tier 2 - 6
//main gun_Perma: tier 1 - 3; tier 2 - 2
//temp gun_Perma: tier 1 - 6; tier 2 - 3
//fix the gun_Perma popup. its not appearing really well and it should show really well.

//HOW BASE BUILDING WORK
//you can walk around. you can test abilities.
//you cant shoot guns in the base.



//ACTIVE
//summon a sentry where the mouse is. need to find the closest placable area for the sentry.
///become invisible
///become immune to damage for a short duration
//shoot a fireaball at mouse
//instantly reload the mag and deasl damage around based in the magsize.
//for a short duration all your weapons shoot twice as many bullets.




//PASSIVE (6/ 12)
//reduce skill cooldown. at 3 skills do more damage.
//increase regen when outside of combat (not takig damage for a while). at 3 greatly increase regen while standing still for long enough
//reduce incoming damage. at 3 it deals a portion of the damage back.
//increase reload speed. at 3 it refreshes the ammo by 10% per killed enemy.
//slows the enemy by %. at 3 chance to stun the enemies.
//any damage has a chance to cause bleeding.
//increase max health. at 3 gain a shield based in maxhealth that regens when havent taken damage for a duration.
//shoots faster. at 3 
//gain crit chance. at 3
//gain crit damage. at 3

//MAIN GUN: (0 / 5)
//Pistol: grants more base speed.
//Pistol: 
//Pistol Shotgun:
//Pistol:

//TEMP GUNS:
//Assault rifle 
//m60: has a lot of ammo
//Shotgun:
//air bazooka: it pushes everyone back.
//auto shotung:
//sniper
//



//the gunboxes will have a place they will stay and they immeditaly spawn in another place. which must be a certain distance. at least two in the distance.


//what if i dont have a room system
//limitations about camera?

//create the basic draft map.
//



//where to put the portals?


//THINGS TO NOTE
//the rooms should be big. 
//there should be no spawner by the side of a gunspot. there should be some distance.
//there must be a timer between box spawns.


//PASSIVE ABILITY (0/6)




//how will chest spawn work?
//the gun_Perma one has a spawn place. in whatever place of the map. once used it goes to another random spot.
//resource chest is found in certain spots. 
//the ability chest is spwaned by killing. 
//


//how will spawn work?
//the spawn should be influenced by how many rooms are open. 
//by what round it is




//GOAL
///create teh stage menu.
///can see the name of the store when interacting
///create loading screen.
///create the buttons for the pauseUI
///button for resume game
///button for settings
///button for load city (DEBUG)
///button for exit to deskptop.
///create the system for loading into teh stage.
///create the system for loading outn of stage. 
///remove all passives, all temp weapons, all bds
///fix the light. its too dark when you load the scene

///for now you will load into the stage and choose whenever you want to flee. 
///the ability unit is not working.


//GOAL
///create victoryUI
///create defeatUI
///need to show the resource you grabbed. and the resource you gained.
///i also want to show alll the passive abilities he got
///need to show all the stats. How many you killed, how long you took, how many chests openedd, how much points gained and spent
//

//BUG
//the enemies are getting stuck when they load

//GOAL
///the player can die
///you can use the defeat buttons
///those resources you get from the stage are carried to the city.


//GOAL
//create the round system. and create the periods of rest between rounds.
//add a ranged enemy
///add resources that take time to gather.
//add a special stage resource for doing something.



//GOAL FOR COMPLETION
//complete the passives.
//improve the spawn system.
//fix the enemies being stuck on spawn.
//create more temp guns


//ABOUT GUNS
//need to check if the tier is affecting how common the guns are.
//add the ability for guns to shoot through enemies.




//PASSIVE COMPLETION
///can stacks of different levels.
///improve passive random getter.
///the stack in an ability unit always show the bigger stack. 
///any damage you deal has a chance of causing bleed.
///reload passive
///skill cooldown
///slow the enemy with bullet. every 3 stacks increase the chance of stunning the target with a bullet.
///passive armor
///when stackalbe bd runs out of time ti removes 1 stack.
//being idle regens
///crit damage. after 5 stacks you gain vampirism based in your crit damage everytime you do crit.
///increase maxhealth. at 3 you gain a maxhealth shield. it fully regens after not taking damage for 5 seconds. 
///increase chance to dodge. at 3 dodging grants a portion of health back.
//increase your luck. at 3 stacks you get () from opening any chest. at 5 
///increase critchance. at stack 3 enemies with 80% healt of more received 15% more damage. 
//this item revives the player. cannot be stacked and cannot be found more than once. extremely rare as well.


//MY GOAL TODAY
///fix the bd stacking with additional stats each stack.
///shield system
///popup for health recover.
///clamp all values directly in the entity stat.

//MY GOAL TORDAY
///can add bullet behavior to the guns.
///certain debuffs cannot stack. so if the same appear the latter should be removed.
///max health passive - the health is not being calculated properly.
///shield mechanic based in maxhealth
///shield recovers after a period of not taking damage.
///dodge passive
///recovering health creats a health popup
///recover health from dodging
///crit damage and crit vamparism passive
///crit chance and damage for enemies over 80%
///popup for health recover

///clamp all values directly in the entity stat.


//MY GOAL
///the enemy for some reason is getting too close to attack. that is ahppening because he is too fast. need to force it to work either way.
///fix the enemy not moving from spawn
///the pause stat is weird. fix it
///add that you can see your gun at the pause
///you should not start with the shield on
///the health stacking passive didnt work; the problem is that its not calling for update.
//health bar in top of the enemy head is not working properly.
///the armor passive also not working?
///the portals are not working when you oepn teh area. they are not spawning enemies.
///the ability unit should start without selected_ForShow active.
///didnt see the gun box
///need the gun box to be rotated properly.
///need to show title of gun in the gunui.
///imrpveo gun algortinh. make sure the guns are found based in your roll chance.
//if the resource box has two of the same items tey should be delivered as one. <= 0Will ignore this for now
///the player is not touching the ground.
///give something to distinguish between the types of enemy.


//MY GOAL
//need to make enemies more interesting to fight.
//enemies can have a cap about how many there should be.
//ranged enemy shouldnt just follow the player. he should always try to keep a distance
//Hound just charge straight ahead. but asssassins try to flank.
//artillery will station somewhere and will shoot at the player. the shoot is always accompanied by a sound that should be in the map to help the player find it.
//Rooms might have traps at any moment. everytime the player gets inside we can check to see if there is a trap and close the door. then do something inside.
//


//MY GOAL
///create round system
//improve spawn system
//

//THINGS TO NOTE
//not showing the price for the things you can buy
//dash should go less far.
//the timer is going too slow at the start.
//the spawn is too slow.
//the map is too small. it wont work in maps like this
//the round is going faster per thing now. should be slower per turn.
//its spawning too much? how to fix it
//the giants are not working. they are getting stuck in the walls?
//The hound should be way faster.
//enemies are getting strong too fast.
//bullets are not being destroy by wall.
//the aim should be more free. it should only attack if you hover an enemy. or maybe we should have a false arm but the bullet should go to the object.


//maybe round should be slow. it always charge at the same time. the difference is that killing advances it further.

//MY GOAL
//the player is falling through the floor.
///show the price of itens when buying in the store
//fix the round. make it progress normally. 
//rework the spawner. something that feels more right.
///create a limit for enemies in the stage.
///bullets are not being destroy by wall.
///the aim should be more free. it should only attack if you hover an enemy. or maybe we should have a false arm but the bullet should go to the object.
//we will look at where the mouse is and shoot where the mouse is unless we hover at an enemy. 

//MY GOAL
//cgheck active abilities
//should be able to fall and thas insta death.
//


///Giant is doing nothing. he starts stunned.
///giant raidus effect is not working.
///the cap is not working


//ABOUT SPAWNER


//THINGS TO PUT IN THE ROOM
//there is a place to tune your temp weapon but its locked behind a requirement.
//there is a place that always hold the same resource depending on the stage.
//place for the stage quests like: survive in teh area for a timer
//random traps.
//boss arena where you can do some additional challenges. there is a cost behind as well.
//there should be secrets around the map. likes gates that can only be opened with things.
//the pause is triggered by the player.
//there are shrines that you need to do something to get them. they give you random possibilities.
//like destroy all current enemies, next turn will be a pause turn. the prayer it requires a test of honor, which would be the challenge.
//

//SPAWN BEHAVIOR
//at the start should go a bit faster. 
//it should spike way faster.
//need a way to force the player into leaving the first area.
//


//how to imrpove spawn system?
//


//ENEMIES
//simple - just walk forward.
//bomber - explode
//Hound - goes fast.
//Giant - slow walk and force the player away
//
//


//MYGOAl TOMORROW
//create console command system? need to be the type of system that i can easily comppletely removed from the game
//can spawn enemy.
//can kill all enemies
//can get a passive with _id
//can get gun_Perma with _id
//can buy all items in the store.



//NOTES FROM GAMEPLAY
//should deal more damage to enemies at the start. 
//the first area should definelty be bigger.
//there should be an introduction in the beggining to offset the nothing at the strat.
//more enemies but weaker enemies
//there should be something to force the player out of the inital room.
//i think the healthbar should not show;

//MY GOAL TOMORROW
//improve turret.


//MY GOAL TOMORROW
//finish the turret.
///fix the enemy not moving from spawn
//improve spawn system
//check ability and gun_Perma systems.



//SYSTEM
//every passsive has a type out of three. getting 3 of one type allows gives you a slot to get another buff.

//GOAL
//create save system
//create main menu. you can have multiple save slots.



//WHAT TO ADD IN THIS VERSION?
//shield enemy that only takes damage behind. or if you have enough penetration
//enemies that are ranged and they shoot at you, so the player can never just stand still. 
//enemies that sit still, but if you damage them they will go after them.



//how else the player evolves?
//increasing raw stats.
//increase ability slots from 1 - 3
//powerful new active new abilities - 
//powerful perma guns - 
//trinkets - which will grant additional passives.
//New enemies and challenges.




//GAMEPLAY IDEAS
//enemies that take damage from just a place. like only in the back.
//certain rules are applied at certain times. like all chesta have bombs for a periord; 
//quests - your mission and you gain a bonus for it.
//challenges - do something really hard to get a reward.
//things you can do in the stage.
//you can improve your perma weapon by going to the forge and using a resource.
//you can mine for extremely rare minerals that are used in the base.
//you can do a secondary quest for a stage currency.
//they are random: stay in the circle for long enough, kill a quantity in a short duration, do a simple puzzle, deliver a heavy object to another place. 

//WHAT I CAN DO NOW?
//there will be places to mine resources. Iron and copper.
//there is a computer that you can gain intel, which allows to build new builds or, when activated oyu must protect the place to gain the intel.
//gun_Perma boxes
//there are shrines randomly placed in the map. you must complete a random goal to get the bless from this thing. "kill 100 enemies". you gain a bless. 
//A - forces a random bless. you must complete it or you lose 15% of your health. it gives 10 bleses. low timer
//B - choose one goal out of three. you must complete it or you lose 50% of your health. it gives 10 blesses. high timer
//C - force random goal. does not need to be completed.
//bless is a stage coin that you can use for other stuff. but for what? rerolls, open doors, summon bosses,
//create the upgrade box?
//you can unluck passages and teleports.
//there are powers that will appear from dead enemies? what and where?
//secret doors that require conditions. - 



//in the end of the game you can trade blesses for other things.
//but you can also trade for other things. blesses grant you rerolls.


//MY GOAL FOR NEXT WEEK
//Shrines and stage goals. create the three types of goals. can fit three goals at once.
//can use bless to reroll.
//can only open the doors to the boss arena from teh center not the sides. once the center is open can open the sides.
//can fall. have to make the ground collider a bit more forgiving.
//can use a bridge that works for a short duration
//blueprint computer where you must activate them and protect the area. once you do that you gain intel. (dont need to worry about the resource for the meantime)
//can use the shrine for some kind of power. it will consume the shrine and cost bless.
//create the upgrade system for temp guns.
//create teh artilery enemy. it will spawn at a random position. it argets the player aywhere. can only spawn in opehnj area. create sound for the player to find it.
//create ranged. it will just shoot projectiles. slwo projectiles.
//create mage that will damage teh area. but it moves.
//probably i will have to revise the spawn system
//take a time to get things clean
//check active abilities.
//create 3 more passives

//THREE MORE PASSIVES
//increase luck. at 3 gain money after every chest is opened. 
//if you die you are revived. you can only get one of this. it should be extremely rare for this fella.
//gain 25% movespeed when below 25% health. cannot stack.


//MY GOAL - 1


//MY GOAL - 1.5
//also - when enemies are killed the damagepopup goes down.

//MY GOAL - 2
//create the blueprint computer.
//quests can also be seen in the pause ui




//GOAL OF TODAY
//the charge enemy
//make sure the shield is working
//apply effects to teh gun_Perma once the process is done
//create a bd system for the gun_Perma.
//connect to the gunstats. Damage, Magazine, firerate, Pen, Critchance, Critdamage, ReloadSpeed
//make a way to add tot he shield.

//GOAL OF TODAY
using static System.Collections.Specialized.BitVector32;

///move system. the player can move and can look around
///there is the one perma and two temps
///create ammo system. perma always has infiite ammo
///can reload. 
///can change the behavior of bullets. we carry the bullet behavior in the player because this will be universal now.
///create stat handler for entity.
///the player has stats
///the enemy has stats that can be scaled.
///first i needd to be able to change stats in a non-heavy manner
///can crit. and it is influenced by player critchance and critdamage. guns also have modifierse.
///handle penetration.
///stat reload speed affect reload speed.
///create a system for handling damage reduction.
///create popup for damage. it needs to always be facing the player and also show over everything else.
///can interact with a chest.
///when you receive resources the game is not paused. the icon appear and goes to the player and then it appears in the top what you gained and how much
///
///the effect of many guns appearing till one is hover.
///i need to randomly select 
///cannot click till the thing is done spinning.
///now i want to do the reroll
///then i will do the equip with no other gun
///then we will make that your guns can appear by the left.
///you can hover them to see their stats in relation to the chosen. when you no longer hovering it instantly goes back to only the chosen
///the rotations of new guns seem to not be working.
///the chest gun is not swaping the right weapon
///gain point for shooting. create a nice effect for point gaining
///create a health bar.
///create bd system. create bd units that appear somewhere and that you can look at it.
///create a pause menu.
///can add and remove bd
///stats are correct.
///can stack passives.
///show all weapons you have and which one is currently being used. show the current ammo all weapons have.
///there is a weird problem that swapping guns are not working.
///fix the gun rotation
///fix the gun swaping to wrong currentBulletIndex.
///creat the chest ability ui
///three options for ability.
///you need to roll for these abiity and its based on their tier.
///you cannot get two rolls of the same ability.
///create door 
///create pause
///create enemy behavior. 
///create pathfind
///take a look at destructable terrain
///create damage class
///create ui indicators for teh zombie abilities.
///create three tipes of simple melee. they are just different streghts. they all move in straight lines to the player. 
///create a bomber enemy. the enemy explodes after getting close.
///create a tank enemy. its slow and a lot of health. it causes slow in area. its attack stun the player for a short duration
///create hound. it is fast and deals a lot of damage. but its easy to kill. 
///create spawn system
///create system for gunboxes
///create system for resource box
///create stunned system
///fix the pathfind. they are not tarversing where it used to be a gate.
///the resource chest is not being destroyed.
///the resource chest is spawning at teh start. it shouldnt.
///we should go through the resource chest.
///the enemies are not dying.
///in the base we wont show any of the combat ui.
///create stunned system
///create immune system
///create dash system
///create bleeding system.
///now we create bdunits to show the player what he is suffering. the enemies can also have it but they disaply somewhere else.
///stun and bleeding apply to enemy
///the bd in top of enemy´s head is not working because i think grid doesnt work in this utation.
///you can see all your own bds and pause to check them better. can see the guns and can see the stats. we need a whole list by the right.
///change it so the description appears as a window instead of staying in place. then i place the passive holderMain there.
///create and update passive description
///create and update stat description
///create ability box. each enemy has a chance to carry a ability box that will be dropped once dead.
///shotgun spread is not working.
///fix the way damage pop up is working. the damage should stay after the nemey die. once the enemy die we remove the canvas, and then after there is no more damagepopup it is destroyed
///should not damage itself.
///the chest is not being destroyed for some reason. just some times.
///fix the chest appearing stuck. we can use grtavity spawn the chest high and then let gravity adjust it.
///the gun is shooting twice sometimes.
///when swaping you block input till you release your hand
///reloading must update teh gununit
///can become invisible for a short duration. meaning that enemies will ignore the target. no visual stuff, just a bd and make it invisible
///can become immune for a short duration
///create system for a weapon to give the player passives.
///Pistol Speed: grants an amount of speed and less cooldown for dash
///Pistol Sharp: Increases damageback per {}. and a chance to dodge.
///fix the damage popup. its too far from head and doesnt follow the target. 
///SMG: has a chance of each shot to shoot one additioanl bullet.
///sawed off shotgun: deals more damage per additional pellet. 0.5 % per pellet.
///creates system for dodge. and when you dodge there is a popup
///creates system for damageback
///creates system for increasing the amount of shoots based in a _value.
///shoot a fireaball at mouse
///create burning tick
///instantly reload the mag and deasl damage around based in the magsize.
///for a short duration all your weapons shoot twice as many bullets.
///your weapon slot is linked to your gunclass. any changes are made directly.
///you can drag items.
///create window description
///if you release into a player slot and with the right type then we order the swap
///weapons you are using or abilities do not show in your options
///you cannot drag your weapon away because you must always have a weapon
///you can oepn the armory.
///you can buy stuff in the armory and it will appear in the tab.
///opening the tab always close citystore ui
///you can drag away and remove your abilities.
///you cannot move or do anything while the tab is open
///create lab
///create smith
///shoot a fireaball at mouse
///instantly reload the mag and deasl damage around based in the magsize.
///for a short duration all your weapons shoot twice as many bullets.
///become invisible
///become immune to damage for a short duration
///create teh stage menu.
///can see the name of the store when interacting
///create loading screen.
///create the buttons for the pauseUI
///button for resume game
///button for settings
///button for load city (DEBUG)
///button for exit to deskptop.
///create the system for loading into teh stage.
///create the system for loading outn of stage. 
///remove all passives, all temp weapons, all bds
///fix the light. its too dark when you load the scene
///for now you will load into the stage and choose whenever you want to flee. 
///the ability unit is not working.
///create victoryUI
///create defeatUI
///need to show the resource you grabbed. and the resource you gained.
///i also want to show alll the passive abilities he got
///need to show all the stats. How many you killed, how long you took, how many chests openedd, how much points gained and spent
///the player can die
///you can use the defeat buttons
///those resources you get from the stage are carried to the city.
///add resources that take time to gather.
///can stacks of different levels.
///improve passive random getter.
///the stack in an ability unit always show the bigger stack. 
///any damage you deal has a chance of causing bleed.
///reload passive
///skill cooldown
///slow the enemy with bullet. every 3 stacks increase the chance of stunning the target with a bullet.
///passive armor
///when stackalbe bd runs out of time ti removes 1 stack.
///crit damage. after 5 stacks you gain vampirism based in your crit damage everytime you do crit.
///increase maxhealth. at 3 you gain a maxhealth shield. it fully regens after not taking damage for 5 seconds. 
///increase chance to dodge. at 3 dodging grants a portion of health back.
///increase critchance. at stack 3 enemies with 80% healt of more received 15% more damage. 
///fix the bd stacking with additional stats each stack.
///shield system
///popup for health recover.
///clamp all values directly in the entity stat.
///can add bullet behavior to the guns.
///certain debuffs cannot stack. so if the same appear the latter should be removed.
///max health passive - the health is not being calculated properly.
///shield mechanic based in maxhealth
///shield recovers after a period of not taking damage.
///dodge passive
///recovering health creats a health popup
///recover health from dodging
///crit damage and crit vamparism passive
///crit chance and damage for enemies over 80%
///popup for health recover
///clamp all values directly in the entity stat.
///the enemy for some reason is getting too close to attack. that is ahppening because he is too fast. need to force it to work either way.
///fix the enemy not moving from spawn
///the pause stat is weird. fix it
///add that you can see your gun at the pause
///you should not start with the shield on
///the health stacking passive didnt work; the problem is that its not calling for update.
///the armor passive also not working?
///the portals are not working when you oepn teh area. they are not spawning enemies.
///the ability unit should start without selected_ForShow active.
///didnt see the gun box
///need the gun box to be rotated properly.
///need to show title of gun in the gunui.
///imrpveo gun algortinh. make sure the guns are found based in your roll chance.
///the player is not touching the ground.
///give something to distinguish between the types of enemy.
///create round system
///show the price of itens when buying in the store
///create a limit for enemies in the stage.
///bullets are not being destroy by wall.
///the aim should be more free. it should only attack if you hover an enemy. or maybe we should have a false arm but the bullet should go to the object.
///Giant is doing nothing. he starts stunned.
///giant raidus effect is not working.
///the cap is not working
///fix the enemy not moving from spawn
///3 more passives 
///create luck
///luck needs to do something, it needs to increase the chance of getting items.
///luck actually increases all chance based stats. increase critchance and dodge chance.
///create revive
///create movespeed when low in health
///can fall and die. more ground collider more forgiving.
///can use bless to reroll. 
///be able to create conditions to be able to open a door.
///create ranged enemy
///also - certain passives should be available at all tiers.
///also - cannot dodge certain damages.
///also - for some reason the stats from the data are not being passed to the enemy.


//MY GOAL - 1.5
///i want to change the round system to actually be per round. it will make easier to control
///but make it so you can swtich between the two easily.
//also - when enemies are killed the damagepopup goes down.

//MY GOAL - 2
///create the shrine system
///they spawn in map
///create the different types of shrine
///create the system for the goal
///can fit three goal at the same time.
//create the blueprint computer.
//quests can also be seen in the pause ui


///now i need to get the quest from an algo
///higher chance of gettign bless, lower chance to get challenge and curse.
///also the shrine needs to spawn in teh world. there can bne only three srhines at any time.
///cannot open the shrine if you have 3 quests.
///the shrine costs points


//GOAL OF TODAY
///the upgrade weapon also costs money to start.
//the charge enemy
//make sure the shield is working
//apply effects to teh gun_Perma once the process is done
//create a bd system for the gun_Perma.
//connect to the gunstats. Damage, Magazine, firerate, Pen, Critchance, Critdamage, ReloadSpeed
//make a way to add tot he shield.

//GOAL OF TODAY
//criar upgrade datas.
///upgrade station aplica um stack de upgrade (10 % de todos os valores da arma) e uma habiliades aleatoria
///quando vc pega a arma da station tem um popup de q tipo de upgrade foi
///connetar os status alterados da arma para sua funcionalidades
///Damage,
///FireRate,
///Pen,
///ReloadSpeed,
///Magazine,
///CritDamage,
///CritChance,
//Vampirism

//GOAL
///shield should be working 
///when you damage the shield is should show a popup.
///shield should stop the wielder from receing damage explosion
///criar gun upgrade q permite a arma atrevessar uma pessoa.
///sniper can do it as well.
///shield should stop bullets going through.
///shield should check if there is enough pen, if there is enough it deals damage and is allowed through if i can.
///get explosion bullet working well. 
///make it so that the damage pop up stays same place in the x but changes in the y



//GOAL
///no menu de pause vc pode ver esses status alterados.
//also correct the bug where teh damage popup falls down.
///the popup from the upgrade station should be better.
///inform the player what was the power he got.
///create a way to replenish ammo. make it so enemies may drop
///put an image in the mouse. interaction makes the thing slowly rotate.
///if hover over an enemy is red.
///normally is white
///if over an interactable then its blue.

//MY GOAL - 3
///create temp upgrade system
///apply the changes once the machine is done
///the machine also cost stuff.
///create mage enemy
///create artillery enemy. create sound tips foar the player to find the artuillery.
//create a charger enemy. use the same behavior of the dash.
//create a bridge
//need to create a way to replenish ammo.
//have to check that the calculation of bullets is working
///a way to show what was the random upgrade the player got.
///put an image in the mouse, the mouse behaves differently based in what you are hovering.


//MY GOAL
///shooting
///swap
///reload
///dash
///enemies when hit
///enemies when they die


//MY GOAL - 4
//create sound for stuff:
//shooting
//spells
//portal spawning enemies.
//death
//buttons
//enemies being hit
//enemies being killed
//opening the gunchest

//MY GOAL - 5
//put grpahics and animation
//create muzzle flash
//create an effect gfor the bullets so they look better.


//NOW THE GOAL IS TO PLAY THE GAME AND FIX THE WORSE PROBLEMS.
//we should start from the main menu
//we should play in the builded version.




//MY GOAL
//mouse ui is not turning red when hovering enemy
///mouse ui is not showing when you open and pause and close
///mouse ui should have other shapes in main menu, city and pause
///spawning behind the portal. whatever rotation is not working.
///the shrine is not fetching quests
///the timer for shirne spawn should be bigger.
///the quest window is not opening when receiving a new quest
///the quest window is overlaying the pause ui.
///you can go through a gate with dash.
///the gun unit has a delay when getttgin description
///the round take too long to start
///the bullets should be faster
///the enemies need bigger colliders.
///there should be more enemies per wave
///the cooldown for the portal individual spawn should be lower.
//for some reason there are more people in the spawn list than are being spawned.


//MY GOAL
//i want enemy to shoot faster on sight but less faster in next shot
///the enemy projectile is hitting own enemies.
//i want more enemies in the third round.
//when going right there is an inivislb ecollider.
//remove the invisible wall by the right. otherwise player cannot shoot through it
//i want the enemies attack to be two hitter to 3 hitters. (ranged will be a 2 hitters and melee will be a 3 hitter
//cannot go through the right door. 
//too easy to fall in the top
//i need to put more places for shrines.
//


///check if shrine is spawning
///check if gunupgrade station is spawned.
///check if gunbox is spawning

///the gate is not opening
///the left last gate is not passable
///the ui for the gunbox is wrong. the container not working
///when you get a new weapon fromt the gunbox its not appearing in the gun slots.
///i think the bullets should not have shadow because the shadows seem like the bullets
///very common getting stuck in different grounds.
//the enemies are too slow perphaps or they getting easily stuck because they are taking too long to go after the player. perphaps the solution would be to respawn some of them if they are too far for too long.
//too many enemies in the second wave. or they are too tanky. 
///so in the builded version adding a gun is not updating the ui
///also i cannot see the description window for the gun unit
///the box didnt destroyed once the ability was chosen
//the slow bullet is not working and breaking the gun_Perma
///the ranged are shooting too fast.
///


//the problem is the end ui. when i create a new fella for the endui i should not set it for the class.
//FIXE GOALS
///certain abilities are just not being added and not show in the thing. what abilities are not being added?
///the slow bullet are breaking the bullet behavior. slow bullet should show as bd
///the new portals are not being used to spawn the fellas.
///the revive passive is not working
///when you restart the mission is keeps all the stuff in it.
//the ability where you gain crit chance is breaking. but only in builded version.
///ranged projectile is passing above the player.
///getting stuck in gate.
///revive is always appearing. it should be extremely rare.
///even if the player already have revive its still appearing.

//DESIGN GOALS
///bleed deals little damage. it should just deal more damage.
//


//now i just need to make sure the abilities are working. so all weapons will be available to get as you wish.

//MY GOAL
//when i leave the screen i lose the block


//PROBLEMS WITH BUILDED VERSION
///for some reason the camera is not rotating
//teh canvas is all fucked
///the resource ui is not working
///the stat in pause ui is not working
//the store ui
//the dash ui
//the gun_Perma owned ui
///the character is not interacting
//the add items appear in the main menu
///ui for the abilities are not appearing
///cannot dash
///cannot open the equi tab
///the guns and abilities available are not appearing in the equip tab.
//one of the ability store items is not working

//the city has no block
///the gun in equip window not updating
///the ability in equip window not updating


//how to get the thing to work? the unit is not working in teh continar


//WITH THIS THE VERSION 0.4 IS COMPLETED

//CRITICS FOR VERSION 0.4
//

//VERSION 0.5

//MECHANICS
//the gun_Perma box is changed to look more like cod
//create the drop system
//you can build new buildings for your city.
//there are npcs walking around. they do nothing but they have to pretend to work
//create pop resource for city.
//there is an area where you can train and test your guns and skills
//there is a building that you can perma increase your stats.

//CONTENT
//2 new enemies
//3 new guns
//3 new gun_Perma upgrades
//3 new quests
//5 new passive abilities
//the turret active ability.

//PERFOMANCE
//pooling for projectiles
//pooling for enemies

//IMPROVEMENTS
//create main menu that lookas better
//create settings menu
//create effeect for the bullet

//GENERAL FIXES
//when you increase the total_Damage health you should health by that same amount
//the giant is walking sideways towards the player. remember to fix that when you put the grqpahics
//the pause ui passive abiity number is not showing the number of stacks
//the ranged enemies are not moving, and other times they are stuck near a wall firing at the wall.
//the guns are all called shotugn

//MY GOAL FOR THE AFTER THIS
//work in the city
//work in the main menu. add the settings
//create the drop system.
//create the pooling system which will improve the perfoamnce for projectiles
//create pooling for enemies as well.
//need to work in the buy store item screen.
//fix that the equip window will show the last position of the drag item for a splitsecond.
//create more passive abilities
//create more active abilities
//more gun_Perma upgrades
//more enemies
//more guns
//more quests
//add effects to the bullets
//when you pick the resource box it should stack the different resource being granted if they are the same.
//i think the ranged units should try moving after every hit. especially if the player gets too close.
//the gun_Perma box shouldnt be a ui. it should be like cod, a weapon rising to the air as the game continues to play.


//now i have to do it for ability
//what else do i want to do?
//i need to make sure its working in the equip tab.
//and now we will improve the buy screen.
//i want an effect for buying, not instantly
//i need to add more stats to the gun_Perma stats description.

//HEREEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEE

//TODAY
///button that resets to default.
///i will just fixc the thing fading when its not supposed to fade.
//then i will do the thing where armory can see the guns that i can get from chest
//and lab can see the passives abilities as well.


//


//things i need to make sure. that the passive is working
//that the guns can be found.


//i want to spawn the npc
//and i want to receive the quest.
//i need more stuff for the equip tab. i need a place to check the quests. i should be able to check quests in the main building as well.
//i want to see quests
//i want to see drops
//and eventually i want to see trikents. additional things
//also i need to create a way to increase stats.
//i also need to find a way to add these fellas to the list.
//where would you find npcs: quests and secrets.
//

//TODAY
///i want to create teh additional holders for the equip tab. "Quest" and "Drop" and "trinket" but i will do nothing in them yet.
///now i want to spawn the especial npc <- DO THIS YOU FUCK.
//find a way to change the dialogue and progress it
///then we add a quest
///add those quests to the equip tab
///add those quests to the main building.


//GOAL
//able to progress dialogue
///fix for the especial npc number appearing when its not meant to
///all the gun units are show as hidden. only the temp ones should be at default.
//try to gain a weapon and see what happens. a perma and a temp.
//then i want to create the 5the cod box <= half created. have to test in game.
///then i need to check the ability side.

///now i have to test receiving a perma and a temp weapon
///then i go and do the same to abilities. 
///then i need to create the logic for passive ability spawning.


//now i need to create the thing for the drop system
//first we create the data to hold it all
//second we create the store where you can buy stuff
//then we create the equip tab

//now i will check if the perma stats are passing to the game
//i will create the perfoamcen stuff. pooling for projectiles and enemies.
//then we will fix the stuff


//i need to improve the movement. should move towards the mouse. take it from stone shard
//the abilities are black


//projectile pooling and enemy pooling
//that means that when i want to instantiate something i should use a reference instead. i need to get every place that ever shot a bullet




//most of it should be working

//should i just directly put it in the player instead of doing this?

//PROBLEMS TO FIX 
///i am need to update the units to show if they are maxlevel.
///also change speed so that its a bigger number.

//then i need to do the dialogue progress.
//then i head to the drop and the the stage part of the game
//also create the thing for increasing stat.
//whern i get here the goal is to first fix the bugs.
//then i will create stuff for perfoamnce. pooling


//the goal now is improving movement
//need to change teh cooldown for the dash. teh cooldown will go less far and will have a much higher cooldown.
//


//the dialogue i will skip for now. too stuck on it.
//lets just create the thing for improving stats
//then we will take a look at teh drop. at buying, getting, assign them onequip tab and increase 

//PROBLEMS
///the buttons from the holder are duplicating
///the units are not updating. thats because its only getting the actual list and once i change the currentBulletIndex list i am not updating the actual list.
///the problem the guns are not appearing in the equip tab
///for some reason the owned gun list is duplicating one item three times.


//i want to create the drop system.
//at least for now i will do the space in teh equip tab.
//then i will create the city data for it.
//i want to have a quest for expanding the drop system from 1 to 3.


//TOMORROW
//take a better look at movement and dash
///fix the ability getting dark
//fix the logerrors that were showing
//test some more the change to pooling
///fix the collider in the spawner
//stage quest not resetting <- have to test
//fix the dash passing through the door.
///equip tab is over the pause menu
//the suicide bomber deals damage to everyone. also the suicide bomber is immortal.

//MY REPORT
//what i need to do now to improve this game?
//i need to create teh respawning for the enemies.
//i need to make it challenging for the player. the player cant just peacefully stroll around the game
//i need to create ways to get the store contacts
//and i need to create story quests. so i need to create a screen for when you return from the city.
//i need more enemies. and i need them to be more vicious.
//i need the enemy projectiles be faster, but there is less of them
//i need an enemy that jumps at the player. need to keep player always guessing. i want the player to always keep moving.
//i need to create 
//need to make sure the abilities are fun. especially their interactions.
//create a melee system. perphaps incentivizing melee will do wonder for the game. to spare ammo and do some cool attacks
//i need to increase the speed of every enemy at later stages but to a cap.
//make a way so they cant dash through the gaps to areas where they havent opened it yet.




//perphaps the fireball should charge and leave the player stuck. unless they move which stops the charge.
//we can create enemies that keep damage the sourroudnings. if it gets close enough it will deal damage by ticks.


//THE GOAL IS TOO SIMPLY MAKE THE BASE GAME FUN. EVEN WITHOUT POWERS. THEN I WANT THE POWER TO IMPROVE TEH GAMEPLAY
//i want to achieve this through: movement, melee, enemies behavior.
//i want to be the kind of game that you are cobnstnatly in the edge.
//the way to achieve this:
//the player has to behave in ways to get what they want. melee for conversing ammo and more points. certain enemies have weak spots. 
//certains enemies spawn in different places so the player has to keep an eye.
//currently the base enemy offer no danger. too slow. increase speed, reduce attack. if they are near you they should deal damage.
//then the map will be another thing because the player will need to do things aruond the map. every map will have a gimmick.
//the game never lets the player just rest. 

//need a way core to the game how to restore ammo.
//ammo will be restored through random boxes. touching it grants you a portion of your ammo back.
//

//need a way to force the player out of one room. and that will be through the spawns. there will be way more spawns.
//ammo is gained by getting dropped ammo box. but if they are rare enough wahts the point of the melee?
//but when should we stagger?
//dodge also deals damage and stuns those hit. maybe this could be an ability.
//i will not be using the melee. because it doesnt fit with the style? but perphaps certain temp guns are melee.
//i need the mage to be more interesting. 
//you have weaker mage. is the same but it also fires the projectile.
//and the strong mage. is the same but the projectile is stronger and faster. and the artillery creates many 
//a shield enemy. it bounches everything you shoot slowly but its very slow to rotate.
//
//


//GOAL FOR IMPROVING GAMEPLAY
//reduce even more the dash.
//increase speed and attack speed for basic enemies.
//increase projectile speed.
//more enemies: Mage, Assassin, Shield and a charger.
//i need enemies to despawn and spawn close to the player. and to do we check the farthest enemies from the player and do that.
//created ammo box that recover a percent of the ammo of all weapons.
//create the different turn systems.
//create effect for the bullets.
//create a system where i can charge an ability
//create the drop system? think about this


//i will first get the system working in

//GOAL FOR NOW
//i will crewate teh drop system. the enemies shouls still drop ammo and abilities with the same chance <- TEST
//create ammo box. it replenishes a percent value. <- TEST
///create bullet effect
//fix the problem with the fellas spawning
//then i want to improving the spawning system, so they spawn only close. maybe some fellas only want to spawn behind the player.
//and also they should despawn to never be too far.
//i want the ability box, ammo box and drop box to be constantly rotating. <- TEST

//also need a way to war what the player has picked. and maybe a ui from the box could appear that stands still for a while.
//also let me start working in the spawn system.


//GOAL FOR TOMORROW
///test the ammobox
///first let create the city and the building that controls the drops
//teste teh drop spawn system <= check further later on other testing
///then i want to check if the especial conditions are working
///create an ui when you pickj a drop

//GOAL
//improve spawn
//each stage should have its own difficulty.
///there should be a list of closests gates and another in between. we spawn most in the closests.
///if the enemy is too far and out of sight then he will despawn and respawn closer to the player
///certain enemies are immune to this respawning. such as artillery - Giant is not.
//also the abilities shoukldnt apear if they are not being used. also should set the level for player about the ability.
//



//RULES FOR SPAWN SYSTEM
//we need a liskt of closests rooms. a gamehandler checks who is certain level for hr first and another for the seocnd. most are spawned in the first.
//we reform the list with period of 5. so it doesnt overrum too much.
//we check the enemies as well. if the enemies are ever too far away, and also not visible in the camera then we remove it and put it in the list to replaced again.
//but there should be exceptions, such as artillery. it doesnt care how far it is.
//create a way to tell in which room the player is.


//the drops i will create
//nuke all creatures low level
//Double points for a period
//next gunbox for free
//Health regen
//

//GOAL FOR TOMORROW
//


//turn types:
//Normal
//Madness: all doors close at random times and they stay closed for short durations. enemies outside just wait.
//Nightmare :enemies deal more damage and move faster.
// 

//DROP TYPES:
//Nuke all small enemies.
//



//Melee mechanic
//create a melee attack.
//it works as following: has ammo that can be restored, does damage but doesnt stagger, if the enemy already is staggered then it pushed them back and dealms more damage.
//you can deflect projectiles

//REPORTS AFTER PLAYING
//also the pop resource in the bar the text is wrong
//the stage canvas still has the button holder.
//the enemies are spawning regardless of the rules
// 


//GOALS _+_+_+_+_+_+_+_+_+_+_+_+_+_+_
///spawn system improved.
//create dev tool
//fix the bugs
//create the main menu with the new ui
///create the system for cooldown per round and cooldown per timer. other ability ui improvements
///create the system for you to hold the ability to charge the ability
//create new content (new passive, new active, new enemies, new gun_Perma abilities, new guns )
//put more things in the map. mechanics unique to this map.
//create the turret
//create an area where you can train in the city.
//

//FOUND BUGS
//bullet trails are bugging when rotating and holding
//you can dash through doors.
//you can dash through gaps.
///bomber is immortal and is dealing global damage
//quests  are not resetting
///the bomber keeps moving while exploding.
///the bomber is spawning with the explosion radius already visible
///also the bomber kept dying over and over again.

//GOAL STEPS
///nex thing i want to do is the ability system four cooldown
///and also change the ui description to show that it is different
///ability cooldowns that use timer automaticly the round ends.
///then i need to check teh gun algo for the gunchest
///then i need to check the ability algo for teh ability chest
//LUCK is a core stat and not gainable by passive,

//gained how? need to be by doing something. not tied to quests. temple quests should be secondary;
//


//GOAL STEPS
///i want to be able to call something on cooldown and on condition with a passive. like a storm in a closeby enemy.
///then i want the ability to be able to charge abilities.
///camera controls. i want to zoom and shake the camera.
///i want explosions nearby to causa the camera to shake. and the strenght is based on how close it was.
//perphaps i want movement to be weaker at the start. so that it feels more precise.


//what would be the new enemy?
//i want a shielded enemy
//i want a enemy that jumps
//i want a hive enemy. enemy that spawns others.
//i want a better mage.
//i want an enemy that causes areas to be not accessible for a short duration.
//

//currently we have:
//simple - which will be the masses.
//ranged - which throw simple projectiles
//bomber - explodes. deals a lot of damage but should be easy to deal with.
//Hound - fast enemy with low health.
//Giant - offers crowd control against the player. big and tanky.
//Mage - hard to deal with as they keep creating many speels to kill you. i want different mages.
//Artillery - Create a reason for the player to keep moving


//what i want
//Shielded: it doesnt take damagage from the front so it will charge at the player. charge simply means that it moves fast for a moment but also is that it doesnt turn very well.
//Jumper: it jumps at teh player. it jumps at the player cause damage but stun for a short duration
//trapper: it hides in a spot. if the player passes gets damaged and stunned
//

//the shielded enemy cannot be just tanking stuff.
//


//for some reason the shield is taking damage.
//what we can do is that the charge stops if the player is ahead, and then push the player backwards.
//problem is that i am not using physics. 
//so when i amshooting too close then it will simply stop the bulklet.
//i will use a raycast. it will check if its its wall. if its a shield and if its close enough then 

//i might change how the shield works to isntead be the enemybehavior. it will check the damage direction instead of the driection,.
//charge behavior does the push


//first thign i will change the enemyto actually check if it has a shield. this will prevent the problems. more as a safenet to be safe.
//second thing will be doing the charge.

//the little weird steps the charger takes.
//


//we will focus in the next step before charger just to ge4t me some time.

//GOAL STEPS
//get the shield enemy working well. <= it needs improvement but i will go to something else now.
///improve the mage: it will spam a bunch of attacks.
///a big eye that looks at the player. causing burning damage while the eye has line of sight.

//the charge is still doing that weird movement


//GOAL STEPS
//overhaul the control of spawn intensity throught the stage data.
//create a system where you can change the kinda of round. control spawn and stats and everything.
//when you reach 25 turns, the next rounds will use another system. instead they will be constantly spawning and consta increasing.
//need to create a cap of enemies so it doesnt spawn more. i should pu the cap at 150?



///first i will create the eye
///need to create a fakelist for the round
///need to pass the new graphic for the round ui
///need to create a list for spawns. and decided the chances for spawning from the especial list.
///Tthen we test the effect of bloodmoon
///i want to stress test the spawn cap. 

//we are having problem with with spawning. fix it. why is it spawning 6 and hwy is it not passing to the portals.
//

//PROBLEMS
//first the the value is not spawning correctly.
//second its not feeling good for some reason.
//why the thing is spawning.;
//sometimes the bullets are stuck in the air, waiting for - if the bullet is not moving then we destroy it.
//the cap for enemy spawn is not working?
//also cant spawn giant? problem with pooling


//Passive ideas:
//killing an enemy has a chance of converting that enemy as an ally
//Summon an ally after every ability use.
//damage bd can crit.
//can fly for certain durations.
//when enemies are killed they explode.
//increase damage percent.
//increase damage flat.

//create a stat called leadership that influences any allies damage and health.

//TEMP GUN IDEA
//Laser gun_Perma. once you shoot it charges and fire a beam that last a few seconds.
//Laser rifle. it charges and shoot a nomral bullet.
//Sword. swip in front
//


//GOAL STEPS
//then i will create two new passives (Thunder, )
//then i will create the turret
///then i will create a new enemy ()
//the reload ability was not working
//you could dash through walls
//decided what to do with the gaps
//and fix the player falling and dying.
//create an effect for the portal spawning enemy.

//foir now the perfomance seems alright. i need to focus in the other parts till i get in troulb ewith it

///fix reloadability. create a pop up when you use this ability.
///create an effect for the portal spawning.
//Laser gun_Perma and laser rifle
///can dash through walls and gates?
//can stand in the gaps outside of the map
///test the mage offset.
///should not be able to dash if you are already over teh wall.

//i will test the charge tomorrow.

//what am i going to do with these gaps? i want them for gameplay


//GOAL STEPS
///i want the ability to fly as well.
///also the button for return no longer working
///make falling a bit better. instead of idrect death, you see the character falling and the camera moves.
///can raise and lower barrier allowign for you to dash through it. those barrier will be opened by opening the right gates.
///check for abilities resetting on death
//check to reset quests on death
//check for stucks
///the dash ui shows the direction of the dash and if its facing a wall it shows a x


//GOAL STEPS
//create 2 new guns (laser, )
//create 2 new gun_Perma passives
///create another enemy
///create 2 more shrine quests.

//GOAL STEPS
///create the system for creating zombies through dead enemies.
//create the turret system. 
///create ui for allies. show their decay.
///creaste teh system where enemies are able to target allies
//make leadership stat influence them
///suddenly cannot reload weapons anymore
//there should be a limit to how many converted you can have?
///not only the player can trigger 

//GOAL STEPS


//also need more variable to choosing target. 
//certain enemies wont target anyone but the target.
//you cant keep changing targets.


//need to do a bit of a rewriting for the enemy targer dewtection
//make sure that it can deal with ally and players.

//so i want to change how they target

//GOAL
//add effect for explosions (mage, artillery, bomber, and fireball)
///also add shake effect from them as well.
///not show the flyholder if you dont have it.
//get different projectiles in the poolhandler. enemy and fireball
///need to test if the cap is working for spawning
///also reduce the amount of roll reducec per failed spawn
///make so that a spawn with 0 chance cannot be spawned.
///camera shake is erupting the camera.

//GOAL
///fix the giant
//finish the turret
//for fireball projectil: make it so you can see it growing while you charge it, and also put it in the pool, also creat teh eexplosion
///create teh vsx for burning in enemies.
///the bd are not cleared from dead enemies.
///the game reloads with the weapons reloaded.
///the ranged enemy is stuck? no projectile. 
///fix the giant attack. its not being called.


//ranged is taking too long to shoot
//and took long to adjust position.

//GOAL
///for some reason i swapned the main gun for the temp gun
//turn on the drop system


//GOAL
//create the Beserker, that shows its attack with indicator and rapidly charges. on arrival it does something.
//shield charges, but sloy, but doesnt show


//now thgat i am back what was i doing?
//creat ethe shield enemy and charge enemy.
//turn on the drop system
///now the enemies are not being cleared after reseting the thing. i dont know why. but it should be an easy fix.
///try to spread the enemies a bit, just a bit. the rule is that you cannot choose the same in a row.
//i need to get the right number of points.

//i want the player to be able to open the gate first level.
//but i want to give an option between rushing and playing it safe. what can i spend money earlier on?
//but in the early game you can buy sentries, allies, you can buy certain boxes. or they come out of enemies free.
//
//spend money to regen health
//you can unluck places you can buy stuff, for your weapons of for abilities with points


//so the next part toi realse this version is to create the mechanics for points


//GOAL
///Sentry (i still need this)
///a little bot that flies aruond the player. cannot bet target, but shoots the zombies.
///create random boxes, random bots that cost money to be used
///can spend money to regen health, it is consumed and takes some time to regen
///create a way to go to a different place, which you will return in time.
///in this area you have a seller that sells you a abilities or weapons upgrades. they cost points and are extremely expensives.
///they can also sell Curses, which are powerful passives that also have drawbacks.
///create the enemy that has a shield, and biggest functions is to try and annoy you by blocking your hits.
//create teh charge enemy, that i will jump at where the player is at and try to disrupt the player somehow.
///i need a system for spawning the random fountain, random guns, random sentries, random abilities
///the ability boxes need to be able to respawn, the sentries as well.
///fix the problem about the money not correctly updating.
//the weaponupgrade are also shown in the armory, and show what has been found and you can get more by upgrading armory. and from quests. 
///curses go in different lists, you cannot be offered the same curse. so we must be able to check for it.
///also the curses should go in a different box for the player to see.
//give some dialogue for the merchant
//activate drop system
///create pool system for turret flying





//THE LAST THING TO DO IT AFTER TESTING AND FIXES
//create testing tools
//can control spawn



//shield enemy will be just a regular enemy with a shield.
//the charge enemy might spawn with shield and might now.
//i will focus in the charge.
//this guy will be a knight. he has a shield and he shows his charge, then he quickly charge.
//and the basic enemies might gain a shield as well, but they wont charge.


//problems
//i dont weird rotation for the thing. maybe its not world canvas.

//


//actually i need to create teleporter buit i also need one that has timer.]

//now i need to create the timer for teleport once triggered it will forcefully teleport the player
//
//and then i will create the seller



//what you must do to unluck the teleporter?
//it must be an achievemtn which the player must progress towards.
//

//it will be a random seller every time
//it can sell abilities, gun_Perma upgrades or curses.
//there will always be three options right in front of the merchant
//you can talk with the merchant as well
//each of the options are alwayws random



//the problem with the spaw. i should put a cap list somewhere else.
//need a way to spreadn better the fellas in the portal.

//TESTING REPORT #1
///simple melee was spawned but couldnt move
//hound did a weird thing going around the ability chest.
///not spawning everyone. not able to chec
///when you restart the weapon doesnt start reloaded (ammo)
///for some reason i swapned the main gun for the temp gun
///fix the gun stats. shootspeed, magazine and reload speed are the worse.
///the wallgap are connected to wrong doors.
//turn on the drop system
///not show the fly holder
//only the same abilities? is there a reason for it?
///bullet not flying from ranged enemmy. it got stuck
///the bd are not cleared from dead enemies.
///hound need to be much faster
///giant is stuck for some reaosn. giants are spamming audio sounds as well.
//still the problem that the round text is invisible somtimes
///should not spawn enemies that have a 0 zero chance to spawn
///mages should have bigger cooldowns between attacks. otherwise they just keep spawning, and the random should be a bit more
//the character should be slow at the start. 
//the shield enemies are just breaking, they should have more time between spot and charging. 
//the game is creating weird situations where one portal is taking too long to spawn everything it was given
//

//DESIGN SUGGESTION REPORT 1#
//create stage {} that create random bolders around
//the goal is to create stage minigames - such as bus, carts,
//next version create mini-bosses that follow the player. mini-bosses have several moves.
//create events that lock rooms
//create traps. area that require to wait for the right moment that have traps in the floor and stuff like that.
//spawning things cost a lot. need to create pooling for other things as well.
//what if you dont have dash at the start?
//should i use dots for certain things like bullets? the perfomance is lowering too much


//TESTING REPORT #2
//for some reason when the ranger shoot that bullet gets thrown to the bullet pool. solution is to fix the bullet pool problem.
//

//DESIGN SUGGESTION REPORT 2#
//normal ranged bullet are a bit bigger and slower.
//


//TESTING REPORT #3
///Fix the player shooting enemy projectiles <= 
///find a way to warn the player has been shot <= 
//the fly turret is not targetting the simple enemy 1
///sometimes the roudn text is invisible. <= 
///respawning after too far from the player is causing problems. this is causing to break the rounds because the enemies are not respawning <=
//maybe shift the camera to see below better.  <= 
//the fly ui should be invisible if the player does not have fly <= 
//ability chests and turrets are destroying themselvs.
//fix the drop; dropping it too much
///chest ammo is not interact and it should be trigger. it should float above the ground and rotate <=
//do the same thing for the power. differ the two some way.
///assign the canvas for the chest ability <=
//the dog has too little rotation <= 
///the round text is insta. should not be
///shield not working
//the damage flash is getting stuck. <=
///the dog pathfind is weird. it shouod turn much faster.

//TESTING REPORT 4#
//i think the damage ui should be a bit higher
//the enemy projectile stopped? why?
///i want the shield attack to still throw the player
///the shield enemy is throwing the player but it should not yet
//the enemy mage stopped attacking? i think its because i destroyed an enemy for some reason. 
///the damage flash. maybe the mage caused it? nope. 
//the enemy projectile should haev shadow to help dodging it.
///the round text is turning invisible once again.
///the round is not passing? at least not showing that its passing.


///yellow chest disappear in the second buy. instant without giving the weapon. maybe it didnt reset right
///the chest crashed hte game. why are they exploding? the turret should shoot faster but last little.
//shift the camera better? the player being attacked from outside vision from basic ranged is not ideal.
///double points is not working. use the double point event.
///flying is being enabled at random points



//have to start checking the interactions between city and stage
//check if itens beinn found are appearing as found.
//

//the mage is not working and i suspect it has something to do with reusing the same mage.

//the chances to spawn the ability box are not good. too low. there should be more of them. 
//also there should be more random boxes around


///i need to do something about the shield rotation.
//

//i have to not use courotine for those two things
//

//Fix some things
///the drop launcher is duplicating its stuff
///it is not showing the pop in the resource tab
///not updating pop resource 
///remove the fucking dash sound
///make sure that the weapon you start with is added to your library.
///we are allowed to buy a gun that we already have. should show what guns you already have. 
//same things for the ability.
///change the name of the store ui.
//buy the ability but it doesnt appear in the inventory.





//DESIGN SUGGESTION REPORT 3#
//the player should win points by passing a round as well?
//


//PASSIVE IDEAS
//Allies last 10% more and have 10% more health. every 3 levels increase the quantity of allies you can have by 1.
//Spawn a flaming torch that keep moving around you. each increases damage of the fireball. every 3 increase the quantity of fireballs by 1.
//


//ACTIVE IDEAS
//Spawn an ally. he has no limit duration.
//spawn a ranged ally.
//Blocks against ranged attacks.






//first goal is to actually create an ally that can be targetted by enemies.

//more shrine quests: 
//open doors
//find resource boxes
//find ability boxes
//crit x times
//kill 3 giants
//mine x resources
//spend x points

//GOAL STEPS
//create training area.
//improve the main menu
//change the ui


//GOAL STEPS
//take a look at map mechanics
//start testing the game heavily.

//GOAL STEPS
//then i need to create story quests
//and a way to get luck that is interesting
//a way to get main characters.
//and new dialogues and quests.


//MY GOAL
//create npc behavior. they will spawn depending on your base state and they will work depending on what yuour base has avaiable.
///you can build bases. expanding the pop increases the number of houses. 
//create the economy for the city building
///the player should see all the avaialab eresources in the top.
///space for the npcs to spawn randomly in certain houses. the houses they spawn into should be consistent.

//MY GOAL
///Drop system created

//MY GOAL
//create an area where you can train in the city. when you leave the area you are locked but when you enter it you are allowed to use skills.
//create 3 new passives
//fix the turret

//MY GOAL
//improve the main menu
///create the settings menu

//MY GOAL
///change the gunbox to work like zombie cod
//create 2 new guns
//create 2 new gun_Perma upgrades

//MY GOAL
///you can improve your perma stats.
///it uses an extremely unique resources.
///create 2 new enemies
//create 3 new quests

//MY GOAL
///create system for pooling projetiles <- TEST MORE
///create system for pooling enemies <- TEST MORE
///create effect for the bullet

//GOAL
//fix the problems

//GOAL
///change how guns and abilities work <- FUrther testing

//GOAL
///create npc you can talk. they spawn in an empty house. empty house being a house that wasnt occupied by another especial npc. the houes occupied needs to be far enough from another occupied house
///you can talk with those npcs from teh main base as well.
//

//GOAL
//create dev tools for testers.
//can contorl what can spawn, force spawn resource, gunchest, ability, forcespawn enemies, stop and set the round. 

//GOAL
//change the ui


//CITY
//how to make the city interesting?
//we have character just walking around and working
//you have to improve the city.
//the city has a pop, which you gain by completing quests.
//you need to build houses to actually gain pop
//and you need other things to build houses.
//improving buildings require pop as resource
//create a minigame for a turret defense?
//i want minigames in the city as well
//pop resources: population you gain by completing missions but there is a cap limited by the main building. 
//Pop, Food, Iron, Eletrical, Bless Stone, BodyEnhancer, Blueprints, 
//Food is consumed by pop. not having enough food means you lose pop.
//Iron is required for everything.
//stell is required for more advanced stuff.
//Blueprints are used for abilities and guns but they are rarer.
//Eletrical components are used for abilities and certain guns and drops. unlucking things for the player to use in the stage.
//Bless stone is used for sacred buildings. extremely rare.
//Body enhnacer is used for for increasing stats
//


//how the temp upgrade system works?
//you have to choose one of the weapons. 
//so you hold the weapon you wish to give. then you interact and you lose that weapon
//it takes a time and when complete it delivers the weapons back.
//but how the upgrade works?
//it improves all gun_Perma stats.
//some of these fellas can stack.
//stats - damage, reloadspeed, magsize, 
//and it gives one additional value_Level :
//Headshot - first shot against target deals more damage.
//Double mag - 50% of the magazine
//explosive ammo - bullet deal damage in area.
//more barrles - 2 more bullets per shot
//thick bullet - each bullet goes through one additional target.
//blood consumer - this weapons has 15% vampirism.
//Light weight - gives movespeed when holding this weapon.
//Fragmentation Bullet - every bullet in contact with an enemy breaks off into three other pieces that deal 20% of the damage. they do apply status
//Possessed - start a quest that after completing it grants this gun_Perma higher damage.
//Sharp Projectile - grants this gun_Perma 25% pen.
//Reload Protector - when you reload this gun_Perma you recover 50 % of your shield. you get a shield of 50;
//




//MECHANICS - DROPS
//you will have abilities to set pre game
//you can choose cards, and those cards will spawn things in the game at every x turns.
//you start with the ammo one.
//

//upgrade system
//each gun_Perma must have ability passives.
//this wont be passive otherwise it will show in ui.
//so we create a "gunpassiveData" which it can hold and alter gunpassives.


//TO DO
//when you start the game all building are reset but for one, the main building
//in the top all resources will appear. the pop resource will appear differnetly showing how much it has and how much it is using
//when you interact with a building that hasnt been built it will show how much you require of pop.
//if you have enough you can built and the building will come from the ground
//you can upgrade the main building to increase the pop cap. you dont need to get the pop just the cap
//you can upgrade the buildings having enough resources.
//your base spawn npcs that walk around your base
//the npcs visit the places based in what buildings are working, they will sit in front and talking around.


//BUG FIND IN RODRIGO TEST
///tab equip window is above pause.
///reload instant not working ability
///the ability units are black for some reason. tehy cannot be seen.
//door in the left 
//when you respawn your current gun_Perma. what that means?
///suicide is moving and dealing global damage
//abilities 
///getting stuck in the wall. spawner has collider in show 
///going through wall. when dashing.
//quests are not reseting
//getting stuck
//should move towards where the mouse is.


//i need to change how weapons are found
//i probably dont want the same gun_Perma in different tiers.
//but i want guns to be improved. so perphaps i have two equal guns and the guy just replace the guns.
//the player should have a list that is defined by the tier list, so the same system, but we add a especial list of guns that the player get from quests.
//perpahps the tier system is not what i want. like what you should be able to do is instead have a quantity of guns and then you add the guns.
//abilitiies should also have a list for ownedabiilties that work in the same way as espeecial
//what about abilities?

//first i will check the mainbase. if its level 0 then i will reset the whole city. if not then i will update every fella based in their own level.
//


//where to put the especial npcs? 
//maybe they dont spawn in the map? they work as dialogue fellas that you can access in the mainbase
//but how to make the base more massive? i want the base to grow and become big.
//the way to do that is by adding a bunch of small houses to fit the pop isntead of just one. it will still be increased 
//where do i place the houses and the especific buildings? the main buildings are also quite close.
//the only reason you would explore the rest of the city is for minigames you can randomly find around the city.


//what do i want in teh dialogue? its supposed to be quite simple and easy the only things it will happen is dialogue then followed by accepting a quest.
//i want a quest system as well, but a quest system additional to the other quest system. this will be called story system
//
//


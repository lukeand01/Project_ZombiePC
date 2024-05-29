
//MY NOTATIONS


//does this game need a server? it would require for the mobile version. 
//or we can just check by time. but what would create many problems



//IT needs to be easy to change from mobile to pc. my plan is to do both, at least partially, leaving place for 



//GOAL
///move system. the player can move and can look around
//gun system. can equip and shoot the weapons.
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
//i need to make every gun, enemy and etc assign to the right event. and remove said events if they are killed.
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
//it shows the stats of that gun.
//we can reroll
//if we already have two guns then we need to select which one we will be replacing.

///cannot click till the thing is done spinning.
///now i want to do the reroll
///then i will do the equip with no other gun
///then we will make that your guns can appear by the left.
///you can hover them to see their stats in relation to the chosen. when you no longer hovering it instantly goes back to only the chosen
//if you have two guns you need to select one for replacement. then we need to actually replace them and simply throg away the replaced gun.
///the rotations of new guns seem to not be working.
///the chest gun is not swaping the right weapon


//when you receive a gun. it pauses the game and shows the the gun. you can roll again for the double of points.
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
///fix the gun swaping to wrong index.


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
//create gun description

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
///creates system for increasing the amount of shoots based in a modifier.


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
//heal one by every enemy you damage. the value increase one by every stack. up to 5.
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
//a draft of base upgrade minigame: ability roll ; gun roll ; active abilities ; main guns ; improve stats(something different here)
//active ability: tier1 - 3; tier2 - 3)
//passive ability: tier1 - 6 ; tier 2 - 6
//main gun: tier 1 - 3; tier 2 - 2
//temp gun: tier 1 - 6; tier 2 - 3
//fix the gun popup. its not appearing really well and it should show really well.

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
//the gun one has a spawn place. in whatever place of the map. once used it goes to another random spot.
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
//can get a passive with id
//can get gun with id
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
//check ability and gun systems.



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
//gun boxes
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
//create the shrine system
//they spawn in map
//create the different types of shrine
//create the system for the goal
//can fit three goal at the same time.
//create the blueprint computer.
//quests can also be seen in the pause ui

//MY GOAL - 3
///create temp upgrade system
//apply the changes once the machine is done
//the machine also cost stuff.
///create mage enemy
///create artillery enemy. create sound tips foar the player to find the artuillery.
//create a charger enemy. use the same behavior of the dash.
//create a bridge

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
//


//i will create the mage now.
//the mage has a similar behavior to ranged.
//but when he spots the player. 
//area of damage its already the artillery.
//buff the allies and do short and quicky area of damage attacks.


//the charger simply charges the player.
//he has armor in front so he cannot killed by shooting at his head unless you have enough pen.
//he stops at walls and he can fall


//WITH THIS THE VERSION 0.4 IS COMPLETED



//MY GOAL FOR THE AFTER THIS
//work in the city
//



//problem
//for a quest i may want certain things, but perphaps they should only care about how to achieve the thing
//kill 100 enemies - 
//or in the stage we can create classes that take a type and then we choose the values there.
//what about choosing the reward?
//would reward be this different?
//rewards can be: bless, bd stats, gun,  

//you should not be able to open gun if the gunupgradestation is working.
//
//or should?

//how the temp upgrade system works?
//you have to choose one of the weapons. 
//so you hold the weapon you wish to give. then you interact and you lose that weapon
//it takes a time and when complete it delivers the weapons back.
//but how the upgrade works?
//it improves all gun stats.
//stats - damage, reloadspeed, magsize, 
//and it gives one additional value :
//Headshot - first shot against target deals more damage.
//Double mag - 50% of the magazine
//explosive ammo - bullet deal damage in area.
//more barrles - 2 more bullets per shot
//thick bullet - each bullet goes through one additional target.
//blood consumer - this weapons has 15% vampirism.
//Light weight - gives movespeed when holding this weapon.


//upgrade system
//each gun must have ability passives.
//this wont be passive otherwise it will show in ui.
//so we create a "gunpassiveData" which it can hold and alter gunpassives.
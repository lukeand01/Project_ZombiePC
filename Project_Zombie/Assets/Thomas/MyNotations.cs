
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
///the effect of many guns appearing till one is selected.
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
//create a pause menu.

//GOAL



//GOAL
//create active ability system
//create passive ability system. the perma guns should also be capable of carrying abilities.


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
//create spawn system
//create system for gunboxes
//create system for resource box
//create system for ability box
//create stunned system


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
//create bleeding system.
//now we create bdunits to show the player what he is suffering. the enemies can also have it but they disaply somewhere else.
//you can see 

//GOAL
//can summon a sentry.
//can become invisible for a short duration
//can become immune for a short duration
//Pistol Speed: grants an amount of speed and less cooldown for dash
//Pistol Sharp: 
//SMG: has a chance of each shot to shoot one additioanl bullet.
//swaed off shotgun: each additional pellet deals 1% damage.



//and i need to show the stats.
//the ability just show the descrioption.
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
//become invisible
//become immune to damage for a short duration
//shoot a fireaball at mouse
//instantly reload the mag and deasl damage around based in the magsize.
//for a short duration all your weapons shoot twice as many bullets.
//

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
## UnitySpellGame
An old C# RPG that was enormously over-scoped. A short proof-of-concept is playable.

# Purpose
This project was mostly used as a method of learning programming, and a lot of the features exist simply because I thought it would be fun to make them.

# Functionality
The game is based on several main 'systems' (Found in Assets\CoreScripts):
- QuestManagement
- PlayerManagement
- Combat
- MapManagement
- SpellManagement
- GameDataManagement

These systems have sub-systems and potentially sub-subsystems that allow them to run. Here is a brief explanation of how they work:

- **QuestManagement**
  * This system runs all the quests in the game. Quests are series of tasks that the player can complete in order to get rewards.
  * Quests are built as a tree of "quest stages". They have an "initial stage" that they start on, and each stage has links (transitions) to other stages. When any event occurs in the game, any potential transitions for that event will run if their requirements are met.
  * Quests can give rewards when transitions are completed, and notify the player what stage they are on and whether they have started, completed, or failed their quest.
  * Quest stages also provide lists of dialogue that is printed to the player should they speak with entities in the game.

- **PlayerManagement**
  * This system runs the player's interaction with the game and different aspects of their game character, including items collected, health, and more.
  * The different parts of this system connect together many of the other systems of the game including quests, spells, maps, game data, and combat.

- **Combat**
  * This system runs the combat between the player and the other entities in the game. Each entity has a "CombatEntity" component that allows it to be affected by combat.
  * The combat system uses several effects that can be applied to CombatEntities: "damage", "damage per second" and "speed modifier".

- **MapManagement**
  * This system provides functionality for the in-game map that the player can view to see their location in the game world.
  * The map allows travelling between locations and interfaces with the quests system to display special markers to notify the user if any quests require them to travel to specific places.

- **SpellManagement**
  * This system manages the player's magic and spells.
  * In the game, the player can collect spell components that they construct their own magic out of. This system keeps track of collected components, as well as how they are combined in the player's current selection of spells.
  * The SpellManagement system also modifies each spell that the player creates according to the modifying components that they chose.
  * While I attempted to expand this system to run the spell-casting of *all* entities in the game, this was never completed and exists in some overcomplicated, unfinished state.

- **GameDataManagement**
  * This system combines all the other systems and more to save and load data to disk.
  * All the "managers" for the earlier systems are found in this folder of the project (Assets/CoreScripts/GameDataManagement).
  * It allows saving and loading almost all components of the game so the player can return to wherever they left off from at any point. Unfortunately, the system has a lot of bugs likely due to its over-complexity.
  * Additionally, it manages all the custom game-related assets in the game to ensure that they all have constant, unique identifiers.

# Running the Project
As of now, there is no way to run the project except to load it into Unity, and create a build from there. A compiled version may be added shortly.

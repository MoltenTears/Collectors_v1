Collectors_v1

Credit, Licences & Attribution
UI Asset Pack: www.kenney.nl
SceneHandler.cs: https://answers.unity.com/questions/242794/inspector-field-for-scene-asset.html
Singleton Pattern: https://gamedevtoday.com/singleton-unity-c-code/

Truck Accelerating: https://www.zapsplat.com/music/large-truck-drive-away/
in-game Background music: https://www.zapsplat.com/music/game-music-action-uplifting-house-anthem-fast-paced-retro-game-melody-leading-into-pumping-synth-chords/
Bell: https://www.zapsplat.com/music/small-handbell-single-ring/
Bins: https://www.zapsplat.com/music/small-metal-trash-can-fall-over-1/
Twang: https://www.zapsplat.com/music/cartoon-plucked-twang-boing-spring-jump-2/
cheer: https://www.zapsplat.com/music/light-applause-cheer/
menu button: https://www.zapsplat.com/music/organic-button-click-good-for-apps-games-ui-software-etc-7/
hub click: https://www.zapsplat.com/music/multimedia-plastic-style-button-click-2/
title background music: https://www.zapsplat.com/music/game-music-action-urban-groove-electro-breakbeat-with-a-funky-electronic-bass-and-record-scratching/

Project Management Repository
https://github.com/MoltenTears/Collectors_v1 (public repo)


Technical Requirements
PC Standalone executable capable of 60fps (really, no specs)


Operational Instructions
Mouse movement and left mouse button
Keyboard escape to trigger in-game pause screen


Overview of Collectors
While in-game, player has some control over a the mostly automated "Collectors"

If there is at least one(1) Collector at the Collector Depot (bottom left corner of city), then the player can press the 
green "Send Collector!" button on the HUD. This will result in a Collector being positioned at the front gate of the 
Collector Depot, ready for assignment. The player can now click the left mouse button on any of the intersections (marked 
by a blue dome, which turns red when it is being pointed at; indicating that this will be where the Collector is despatched to).
Road Hubs turn grey when a Collector is already in that collection Zone.

Once despatched, the player has no control over the Collectors' movements. It will first move to the selected destination, then
find the closest house with sufficient garbage to collect, move to that house, and repeat the process througout the zone until it
 is full (or there is insuficient garbage to collect).

Once full, the Collector will move to the Waste Centre (top right corner of the city), dump its waste, then return to the Collector
Depot for the player to despatch it again.

If the Collector does not find sufficient garbage to collect once it arrives at it's destination, it will return again to the 
Collector Depot.

If the Collector is partially full when it cannot find any more waste, it will stop collecting and take it to the Waste Centre.


ChangeLog (Descending order of implementation)

Pending Bugs:
- Pack Collection
- Depot/Node switching
- Lost Collector


Week 12:
---
FEATURE: added SFX for player feedback and background music for Title and first City.
---
BUG: Difficulty settings not working.
FIX: moved settings to GameManager and added a support singleton.
---
FEATURE: added pause screen
---
BUG: Despatch Collector button unclear when collector at gate but not yet assigned. 
FIX: Added text on button to inidcate that a Collector needed to be despatched before the button could be pressed again.
---
FEATURE: added "stink lines" depending on how much waste was awaiting collection at a house.
---
BUG: Collector would occasionally get "lost" and start spinning in circles.
FIX: method implemented to catch "lost Collector" and send them to the Waste Centre or Collector Depot depending on garbage in the 
Collector.
---
BUG: Collectors often stat collecting in packs due to the broad application of finding a house with garbage to collect.
FEATURE: split the city up into "zones" so that a Collector can be assigned to a Zone, only collect in that zone, and drop waste 
at the Waste Centre once all the houses in a zone have been visited once.
---
FEATURE: Implemented Starting Game Difficulties (Easy, Medium, Hard).
---

Week 11:
---
FEATURE:Implemented GameOver Overlay once player ran out of days left to play. Included GameOverOverlay.cs that called from 
GameManager.cs, WasteCentreManager.cs, and had to iterate through the outstanding Collectors to assess pending waste drop off.
Implemented satisfactionToWin as global variable that must be exceeded at the end ofthe game to have won.
---
BUG: after creating list of Active Collectors, nullReferenceException occured in CollectorInitialise.cs.
FIX: used correct iteration technique to check if gameObject was in list before actioning item on gameobject.
---
FEATURE: implemented time system displayed on HUD to show time of day (e.g. 2pm) and how many days left in the play session. Total 
number of days available set in GameManager.cs to be able to check a win/lose state.
---
BUG: When despatching multiple Collectors from the Depot, and there are still Collectors that have been despatched, but have not 
reached their destination, all pending Collectors will be re-assigned the destination for the last Collector Despatch. This results
in all pending Collectors going to the same destination before commencing collection.
FIX: re-design despatch functions to only assign a destination to the newly despatched Collector and only allow that Collector
to start Collecting when it arrives at its destination.
---
TUNING: adjusted the following values to optimise gameplay: speed Collector picks up garbage from house, speed Collector drops garbage
at waste centre, speed garbage is accumulated at houses, volume of garbage required at house before Collector will collect from it
---
FEATURE: UI to: Added to HUD Canvas: Collector Despatch section: a) show how many Collectors at the Depot, b) allow the player to despatch a Collector
IMPLEMENTATION: Added Canvas (in world space for convenience in design) for Heads-Up-Display. Created "Collector Despatch" object to
hold features. Used previously sourced UI buttons for "Collect" buton. Changed colour of button: a) if zero, red, b) otherwise, green.
Created CollectorDespatch.cs to handle button press functions. 9-sliced green and red button sprites for button scaling. Created static
image of Collector for display near button.
---
TUNING: minor fixes in SatisfactionManager.cs, adjusted all scripts attached to Collector Depot to clean up number Collectors displayed
inside Collector Depot and the HUD count
---
BUG: If multiple Collectors are collecting, they will occasionally start to collect as a pack. 
FIX: not yet identified
---
BUG: Occasionally, a Collector will stop collecting and start rotating at a specific house GarbageManager. Suspected the iterator finding the
closest house with garbage is not working correctly as the Collector will regularly miss houses nearby and go to a house further away to
collect garbage.
FIX: not yet identified
---
BUG: when Collector despatched from Collector Depot, to a RoadNode that is in a straight line from the Depot to the Waste Centre, when
the Collector is less than a city block from the Depot, it gets confused and attempts to return to the Depot. The result is the
Collector moves toward the selected RoadNode for 1 second, then turns around and moves toward the Depot for 1 second, then repeats. If
it gets to the Depot FrontGate, it is commences the return sequence. Otherwise, it's stuck in an infinite loop.
FIX: not yet identified.
---
FEATURE: when Collector is told it is now collecting, scan all houses and find the closest one with sufficient garbagwe to collect,
move to the house and start collecting, if full or no more garbage, go to the Waste Centre
---


Week 10:
---
BUG: Collector didn't stop collecting from a house with garbage (House garbage level went into negatives). 
FIX: added if statement into collection logic.

BUG: Collector not OntriggerExit
FIX: forgot that turned of Collector/Collector phyisics, turned back on.

BUG: Attempted to use Queue to manage Collectors at Waste Centre, not able to debug as Queues don't show in Unity Inspector. 
FIX: use two-step approach: first gather objects into an array, then sort these into a List. Array avoided as dynamic value prefered.
---
Updated GarbageManager.garbageDivisor to .garbageMultiplier. Adjusted calculation that depends on this value for the speed of garbage
accumulation in the city. garbageMultipler set as a value between zero and one. Variable used to allow global speed increase for 
garbage accumulation as time in the city gets closer to the end of the time limited play window.
---
FEATURE: setup Heads-up-Display (HUD) as Canvas (in world space for convenience in design).
---
Implemented player feedback mechanism: Waste Meter. Slider shows percentage of waste in the city as a percentage of the total 
possible volume of waste in the city. Slider updates live.
---
Implemented player feedback mechanism: Satisfaction Meter. Satisfacion at a house level is managed on two factors: 1) how much garbage
is inside the satisfaction trigger zone, and 2) how tollerant the household is to garbage inside 1. Both values are set based on which 
kind of house it is. Satisfaction at a house level is one of 4 values: High, Medium, Low, and None with each representing a value out
1 (e.g. Medium is 0.66, none is 0). Satisfaction at a city level is the current total value of satisfaction from all houses as a
percentage of the maximum total stisfaction in the city. Note: Satisfaction Meter and Waste Meter do no have a converse relationship.
The tollerance of each house to the amount of garbage inside their tollerance zone makes the Satisfaction Meter a more dynamic value.
---
TUNING: cleanup of the GameManager variables for easier use ini inspector.
---
FEATURE: Implemented a Title Screen scene and associated TitleScreen.cs to handle Instructions panel and scene loading from SceneHandler.cs.
Functions all delivered from button OnClick function calls.
---
FEATURE: New Industrial Block: Waste Centre. Changed return-when-full functionality to Waste Centre instead of Collector Depot. Several
scripts implemented to manage Waste Centre systems: WasteCentreManager.cs, WasteCentreLot.cs, WasteCentreFrontGate.cs, WasteDrop.cs,
WasteQueueSpot.cs. First Collector to arrive at Waste Centre commences drop off (single drop off position). If additional Collectors 
arrive while first drop still occuring, Collectors form a Queue visually in yard. When Drop off position is vacant, Collector drives
out of Waste Centre, the next Collector in the queue takes position and all other collectors jump forward in the queue.
---
FEATURE: Implementation of new Industrial Block: Collector Depot. Temporary testing: when Collector full, return to Depot. When Collector
arrives at depot, destroy object. Several scripts developed to manage Depot systems: DepotManager.cs, DepotFrontGate.cs, 
DepotLot.cs.
---
BUG: Repo not handling large volume of changes. Identified .gitignore at wrong level of project. Fixed repo; much faster :-)
---


WEEK 9:
---
BUG: small bin effectively always shown out the front of house. 
FIX: Modified SetOutBins to only show a bin if there is at least one small bin worth of garbage.
---
FEATURE: House block prefab created. when game commences, house type randomly selected from 3 types: Single, Family, Share, with each
having a variety of variables influenced by the type.
7 "bin" positions created on curb-side. GarbageManager.cs accumulated garbage at a set speed against house type based on garbageSpeed. 
Visual "bin" placed out on curb based on currentGarbageLevel and displayed as different size bins.
Garbage able to be collected from Collector when Collector isCollecting and hits the CollectionTrigger for the house. While collecting 
from the house, house stops accumulating garbage (to stop infinite loop).
---
BUG: child MeshRender on Collector de-coupling from parent NavMeshAgent resulting in MeshRenderer rolling away from NavMeshAgent (which
looks HILARIOUS!).
FIX: Turned on "Auto-sync transforms" in Project Settings >> Physics. All good.
---
FEATURE: locked camera to fixed position with an orthographic lens with an angle to produce an isometric persepective reminiscent of
90s city simulation games.
---
TESTING: amended RoadNodes to receive player interaction (left mouse button) to capture an "activated RoadNode". Implemented highlight
system to show player which RoadNode is about to be selected.
---
FEATURE: Created city block prefabs with NavMeshObstacle attached to limit the navigable area the Collector can move on. Result was
that the negative space between city blocks became the "streets" that the Collectors could move around. Solution very positive.
---
TESTING: changed movement model from splines to NavMesh. 
---
=======

TUNING: adjusted the following values to optimise gameplay: speed Collector picks up garbage from house, speed Collector drops garbage
at waste centre, speed garbage is accumulated at houses, volume of garbage required at house before Collector will collect from it

FEATURE: UI to: a) show how many Collectors at the Depot, b) allow the player to despatch a Collector
IMPLEMENTATION: Added Canvas (in world space for convenience in design) for Heads-Up-Display. Created "Collector Despatch" object to
hold features. Used previously sourced UI buttons for "Collect" buton. Changed colour of button: a) if zero, red, b) otherwise, green.
Created CollectorDespatch.cs to handle button press functions. 9-sliced green and red button sprites for button scaling.
>>>>>>> master


WEEK 8:
---
Creation of RoadNode prefab that gets information from other Nodes at the ends of it's arms.
---
TESTING: researched splines for use as roads in city. Method applicable, but not understood enough for implementation with current
programmer skillset.
---
repo reset for new game
---
Game concept adjusted and approved by Lecturer
---

WEEK 7:
Game concept drafted and approved by Lecturer
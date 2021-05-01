Collectors_v1

Credit, Licences & Attribution
UI Asset Pack: www.kenney.nl


Project Management Repository
https://github.com/MoltenTears/Collectors_v1 (public repo)


Technical Requirements
PC Standalone executable capable of 60fps (really, no specs)


Operational Instructions
Mouse movement and left mouse button
Keyboard escape to trigger in-game menu and mian menu exit


Overview of Collectors
While in-game, player has some control over a the mostly automated "Collectors"

If there is at least one(1) Collector at the Collector Depot (bottom left corner of city), then the player can press the 
green "Send Collector!" button on the HUD. This will result in a Collector being positioned at the front gate of the 
Collector Depot, ready for assignment. The player can now click the left mouse button on any of the intersections (marked 
by a blue dome, which turns red when it is being pointed at; indicating that this will be where the Collector is despatched to).

Once despatched, the player has no control over the Collectors' movements. It will first move to the selected destination, then
find the closest house with sufficient garbage to collect, move to that house, and repeat the process until it is full (or there
is insuficient garbage to collect).

Once full, the Collector will move to the Waste Centre (top right corner of the city), dump its waste, then return to the Collector
Depot for the player to despatch it again.

If the Collector does not find sufficient garbage to collect once it arrives at it's destination, it will return again to the 
Collector Depot.

If the Collector is partially full when it cannot find any more waste, it will stop collecting and take it to the Waste Centre.


ChangeLog (Descending order of implementation)

Week 11:
BUG: When despatching multiple Collectors from the Depot, and there are still Collectors that have been despatched, but have not 
reached their destination, all pending Collectors will be re-assigned the destination for the last Collector Despatch. This results
in all pending Collectors going to the same destination before commencing collection.
FIX: re-design despatch functions to only assign a destination to the newly despatched Collector and only allow that Collector
to start Collecting when it arrives at its destination.

TUNING: adjusted the following values to optimise gameplay: speed Collector picks up garbage from house, speed Collector drops garbage
at waste centre, speed garbage is accumulated at houses, volume of garbage required at house before Collector will collect from it

FEATURE: UI to: a) show how many Collectors at the Depot, b) allow the player to despatch a Collector
IMPLEMENTATION: Added Canvas (in world space for convenience in design) for Heads-Up-Display. Created "Collector Despatch" object to
hold features. Used previously sourced UI buttons for "Collect" buton. Changed colour of button: a) if zero, red, b) otherwise, green.
Created CollectorDespatch.cs to handle button press functions. 9-sliced green and red button sprites for button scaling.


Week 10
- BUG: Collector didn't stop collecting from a house with garbage (House garbage level went into negatives). FIX: added if statement into collection logic.
- Waste Collection Centre created
	- Design:
		- When Collector garbage collected reaches maximum, Collector stops collecting and moves to Waste Centre
		- When at Waste Centre, add self to List of Collectors at Waste Centre
		- First Collector in List commences drop-off of Waste to Waste Centre
		- Once first Collector is empty, it leaves the Waste Centre and returns to the Collector Depot
		- Next Collector in List becomes first and commences drop-off of waste
	- BUG: Collector not OntriggerExit, FIX: forgot that turned of Collector/Collector phyisics, turned back on.
	- BUG: Attempted to use Queue to manage Collectors at Waste Centre, not able to debug as Queues don't show in Unity Inspector. FIX: changed to List
	- BUG:  FIX:

- Collector Depot
	- Design:
	- BUG: FIX:
	
Collectors_v1

Credit, Licences & Attribution
UI Asset Pack: www.kenney.nl


Project Management Repository



Technical Requirements



Operational Instructions



ChangeLog

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
	
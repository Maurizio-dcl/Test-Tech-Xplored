## Cloud Saves

Use a small serializable `SaveData` model containing a save version, date, round, profile ID, and each actor's definition ID, stats, 
action cooldowns, and map position.
Would probably serialize this as JSON rather than `PlayerPrefs`.

Before `GameLoop` starts, a load state could ask for a `ISaveStore` for the local save and, 
if enabled, an `ICloudSaveService` for the backend copy. 
Each human input device (local or network) provides its current player profile.

The host requests the local and cloud save for every participating profile and waits for all of them before continuing. 
For each profile it compares date and content hash: if only one is newer it uses that copy, 
if both changed, it asks that profile's user to choose Local or Cloud.

I'm assuming each device/profile combination saves the whole state of the game, so they must match or
the most recent is going to be used.

The resolved `SaveData` entry is passed into the game bootstrap. 
For a new game, actors are created from `BossSpawnsDefinition` as today, for a loaded game, 
the bootstrap resolves saved definition ID, creates and restores the actors, 
and places them at their saved positions instead. 

Saving can snapshot the same runtime state at safe points such as the end of a round or be manual-only.

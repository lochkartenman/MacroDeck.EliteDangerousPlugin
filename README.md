# MacroDeck.EliteDangerousPlugin
A simple plugin for [https://macrodeck.org](|MacroDeck) that pulls information from the game Elite Dangerous as MacroDeck variables. These variables can then be used for example to switch the button state of MacroDeck buttons or vary the key presses send to the game accoring to some conditions.

## How it works
The plugin watches for changes to the [https://elite-journal.readthedocs.io/en/latest/Status%20File/](Journal File) located in `%UserProfile%\Saved Games\Frontier Developments\Elite Dangerous\status.json` and updates the MacroDeck variables accordingly.

The following variables are currently provided:
| Variable                      | Type | Meaning |
| ----------------------------- | ---- | ------------- |
| `elite_timestamp`             | Bool | Time and Date of last update.  |
| `elite_is_landed`             | Bool | True if landed on a planet. |
| `elite_is_docked`             | Bool | True if docked at a station. |
| `elite_is_in_danger`          | Bool | True if in combat, i.e. the logout-timer is active.|
| `elite_is_interdicted`        | Bool | True if player is being interdicted |
| `elite_is_overheating`        | Bool | True if heat is at or above 100% |
| `elite_shields_down`          | Bool | True if shields are down |
| `elite_landing_gear_deployed` | Bool | True if landing gear is deployed |
| `elite_hardpoints_deployed`   | Bool | True if hardpoints are deployed |
| `elite_cargo_scoop_deployed`  | Bool | True if cargo scoop is deployed |
| `elite_flight_assist_enabled` | Bool | True if flight assist is enabled |
| `elite_silent_running`        | Bool | True if silent running is active |
| `elite_fsd_supercruise`       | Bool | True if in supercruise |
| `elite_fsd_mass_locked`       | Bool | True if FSD can not be activated due to mass lock |
| `elite_fsd_charging`          | Bool | True if FSD is charging |
| `elite_fsd_cooldown`          | Bool | True if FSD can not be activated due to cool down |
| `elite_fsd_jump`              | Bool | True if in hyperspace jump |
| `elite_fsd_available`         | Bool | True if FSD can be activated. This takes into account deployed cargo scoop, hardpoints etc. |
| `elite_analysis_mode`         | Bool | True if cockpit analysis mode is selected |
| `elite_combat_mode`           | Bool | True if cockpit combat mode is selected |
| `elite_lights_enabled`        | Bool | True if head lights are enabled |
| `elite_night_vision_enabled`  | Bool | True if night vision is enabled |
| `elite_pips_weapons`          | Int  | Number of half-pips in weapons, i.e. 8 = full pips |
| `elite_pips_systems`          | Int  | Number of half-pips in systems, i.e. 8 = full pips |
| `elite_pips_engines`          | Int  | Number of half-pips in engines, i.e. 8 = full pips |
| `elite_focus_internal`        | Bool | True if internal panel is selected/focused |
| `elite_focus_comms`           | Bool | True if comms panel is selected/focused |
| `elite_focus_role`            | Bool | True if role panel is selected/focused |
| `elite_focus_external`        | Bool | True if external panel is selected/focused |
| `elite_focus_galaxy_map`      | Bool | True if galaxy map UI is selected |
| `elite_focus_system_map`      | Bool | True if system map UI is selected |
| `elite_focus_fss`             | Bool | True if Full Spectrum Scanner (FSS) UI is selected |
| `elite_focus_dss`             | Bool | True if Detailed Surface Scanner (DSS) UI is selected |
| `elite_focus_codex`           | Bool | True if codex UI is selected |
| `elite_fire_group`            | Int  | 0-based index of the selected fire group |

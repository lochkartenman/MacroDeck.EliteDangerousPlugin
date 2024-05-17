using System;
using SuchByte.MacroDeck.Plugins;
using SuchByte.MacroDeck.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace Lochkartenman.EliteDangerous
{
    public class Plugin : MacroDeckPlugin
    {
        private FileSystemWatcher journalStatusFileWatcher;
        private JsonSerializerOptions jsonOptions = new() {
            MaxDepth = 2,
            NumberHandling = JsonNumberHandling.Strict,
            PropertyNameCaseInsensitive = true,
            IncludeFields = true,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
        };

        public override bool CanConfigure => false;

        private static string journalStatusFileName { get => "Status.json"; }
        private static string GetJournalDirectory() {
            string userProfileDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            return Path.Combine(userProfileDirectory, "Saved Games", "Frontier Developments", "Elite Dangerous");
        }
        private static string GetJournalStatusFilePath() { 
            return Path.Combine(GetJournalDirectory(), journalStatusFileName); 
        }

        public override void Enable() {
            string journalDirectory = GetJournalDirectory();

            if(!Path.Exists(journalDirectory)) {
                MacroDeckLogger.Error(this, "Elite Dangerous Journal Directory '" + journalDirectory + "' not found.");
                return;
            }

            UpdateVariables(DeserializeJournalStatusFileOrDefault());

            try {
                journalStatusFileWatcher = new(journalDirectory, journalStatusFileName) {
                    IncludeSubdirectories = false,
                    NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime
                };
                journalStatusFileWatcher.Created += OnJournalStatusFileEvent;
                journalStatusFileWatcher.Changed += OnJournalStatusFileEvent;
                //journalStatusFileWatcher.Deleted += OnStatusFileEvent;
                journalStatusFileWatcher.Error += OnJournalStatusFileWatcherError;
                journalStatusFileWatcher.EnableRaisingEvents = true;
            }
            catch (Exception ex) {
                MacroDeckLogger.Error(this, "Failed to enable Status File monitoring: " + ex.Message);
                journalStatusFileWatcher?.Dispose();
            }
        }
        private void OnJournalStatusFileWatcherError(object sender, ErrorEventArgs e) {
            MacroDeckLogger.Error(this, "Error watching for changes to Journal Status File: " + e.GetException().Message);
        }
        private void OnJournalStatusFileEvent(object sender, FileSystemEventArgs args) {
            UpdateVariables(DeserializeJournalStatusFileOrDefault());
        }

        private static string ReadJournalStatusFileText() {
            for (int remainingRetries = 3; remainingRetries >= 0; remainingRetries--) {
                try {
                    return File.ReadAllText(GetJournalStatusFilePath());
                }
                catch (IOException) {
                    if(remainingRetries == 0) {
                        throw;
                    }
                }
            }
            return string.Empty;
        }

        private EliteStatus DeserializeJournalStatusFileOrDefault() {
            try {
                EliteStatusJson jsonObject = JsonSerializer.Deserialize<EliteStatusJson>(ReadJournalStatusFileText(), jsonOptions);
                return new EliteStatus(jsonObject);
            }
            catch(IOException ex) {
                MacroDeckLogger.Error(this, "Error reading Journal Status File: " + ex.Message);
            }
            catch(JsonException ex) {
                MacroDeckLogger.Error(this, "Error parsing Journal Status File: " + ex.Message);
            }
            catch(Exception ex) {
                MacroDeckLogger.Error(this, "Unknown error wile reading and/or parsing Journal Status File: " + ex.Message);
            }

            return new EliteStatus();
        }

        private void UpdateVariables(EliteStatus status) {
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_timestamp", status.Timestamp.ToString("o"), SuchByte.MacroDeck.Variables.VariableType.String, this, ["Timestamp of last update."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_is_landed", status.IsLanded, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if landed on a planet."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_is_docked", status.IsDocked, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if docked at a station."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_is_in_danger", status.IsInDanger, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if timer for logout is active."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_is_interdicted", status.IsInterdicted, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if player is being interdicted."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_is_overheating", status.IsOverheating, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if heat is at or above 100%."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_shields_down", status.ShieldsDown, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if shields are down."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_landing_gear_deployed", status.LandingGearDeployed, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if landing gear is deployed."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_hardpoints_deployed", status.HardpointsDeployed, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if hardpoints are deployed."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_flight_assist_enabled", status.FlightAssistEnabled, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if flight assist is enabled."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_cargo_scoop_deployed", status.CargoScoopDeployed, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if cargo scoop is deployed."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_silent_running", status.SilentRunning, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if silent running is active."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_fsd_supercruise", status.FsdSupercruise, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if ship is in supercruise."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_fsd_mass_locked", status.FsdMassLocked, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if FSD is mass-locked."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_fsd_charging", status.FsdCharging, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if FSD is charging."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_fsd_cooldown", status.FsdCooldown, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if FSD is in cooldown."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_fsd_jump", status.FsdJump, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if ship is in hyperspace jump."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_fsd_available", status.FsdAvailable, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if FSD is ready for charging. Inhibited by deployed landing gear, hardpoints, cooldown, etc."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_analysis_mode", status.AnalysisMode, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if cockpit is in analysis mode."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_combat_mode", status.CombatMode, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if cockpit is in combat mode."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_lights_enabled", status.LightsEnabled, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if external lights are enabled."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_night_vision_enabled", status.NightVisionEnabled, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if night vision is enabled."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_pips_weapons", status.PipsWeapons, SuchByte.MacroDeck.Variables.VariableType.Integer, this, ["Number of half-pips in weapons, i.e. 8 equals 4 pips."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_pips_systems", status.PipsSystems, SuchByte.MacroDeck.Variables.VariableType.Integer, this, ["Number of half-pips in engines, i.e. 8 equals 4 pips."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_pips_engines", status.PipsEngines, SuchByte.MacroDeck.Variables.VariableType.Integer, this, ["Number of half-pips in systems, i.e. 8 equals 4 pips."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_internal", status.FocusInternal, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if internal panel is focused or selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_comms", status.FocusComms, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if comms panel (chat) is focused or selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_role", status.FocusRolePanel, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if role panel is focused or selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_external", status.FocusExternal, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if external panel is focused or selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_galaxy_map", status.FocusGalaxyMap, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if galaxy map is focused or selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_system_map", status.FocusSystemMap, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if system map is focused or selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_fss", status.FocusFullSpectrumScanner, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True of Full Spectrum Scanner is selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_dss", status.FocusDetailedSurfaceScanner, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if Detailed Surface Scanner is selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_focus_codex", status.FocusCodex, SuchByte.MacroDeck.Variables.VariableType.Bool, this, ["True if codex is selected."]);
            SuchByte.MacroDeck.Variables.VariableManager.SetValue("elite_fire_group", status.FireGroup, SuchByte.MacroDeck.Variables.VariableType.Integer, this, ["Index number of selected fire group."]);
        }
    }
}

using System;

namespace Lochkartenman.EliteDangerous
{
    internal class EliteStatus
    {
        public EliteStatus() {}
        public EliteStatus(EliteStatusJson eliteStatusJson) {
            Timestamp = eliteStatusJson.Timestamp;
            IsDocked = (eliteStatusJson.Flags & 0x0000_0001) != 0;
            IsLanded = (eliteStatusJson.Flags & 0x0000_0002) != 0;
            IsInDanger = (eliteStatusJson.Flags & 0x0040_0000) != 0;
            IsInterdicted = (eliteStatusJson.Flags & 0x0080_0000) != 0;
            IsOverheating = (eliteStatusJson.Flags & 0x0010_0000) != 0;
            ShieldsDown = (eliteStatusJson.Flags & 0x0000_0008) == 0;
            LandingGearDeployed = (eliteStatusJson.Flags & 0x0000_0004) != 0;
            FlightAssistEnabled = (eliteStatusJson.Flags & 0x0000_0020) == 0;
            HardpointsDeployed = (eliteStatusJson.Flags & 0x0000_0040) != 0;
            LightsEnabled = (eliteStatusJson.Flags & 0x0000_0100) != 0;
            CargoScoopDeployed = (eliteStatusJson.Flags & 0x0000_0200) != 0;
            SilentRunning = (eliteStatusJson.Flags & 0x0000_0400) != 0;
            FsdMassLocked = (eliteStatusJson.Flags & 0x0001_0000) != 0;
            FsdCharging = (eliteStatusJson.Flags & 0x0002_0000) != 0 || (eliteStatusJson.Flags2 & 0x0008_0000) != 0;
            FsdCooldown = (eliteStatusJson.Flags & 0x0004_0000) != 0;
            FsdJump = (eliteStatusJson.Flags & 0x4000_0000) != 0;
            FsdSupercruise = (eliteStatusJson.Flags & 0x0000_0010) != 0;
            AnalysisMode = (eliteStatusJson.Flags & 0x0800_0000 ) != 0;
            NightVisionEnabled = (eliteStatusJson.Flags & 0x1000_0000) != 0;
            if(eliteStatusJson.Pips.Length == 3) {
                PipsSystems = eliteStatusJson.Pips[0];
                PipsEngines = eliteStatusJson.Pips[1];
                PipsWeapons = eliteStatusJson.Pips[2];
            }
            FocusInternal = eliteStatusJson.GuiFocus == 1;
            FocusExternal = eliteStatusJson.GuiFocus == 2;
            FocusComms = eliteStatusJson.GuiFocus == 3;
            FocusRolePanel = eliteStatusJson.GuiFocus == 4;
            FocusGalaxyMap = eliteStatusJson.GuiFocus == 6;
            FocusSystemMap = eliteStatusJson.GuiFocus == 7 || eliteStatusJson.GuiFocus == 8;
            FocusFullSpectrumScanner = eliteStatusJson.GuiFocus == 9;
            FocusDetailedSurfaceScanner = eliteStatusJson.GuiFocus == 10;
            FocusCodex = eliteStatusJson.GuiFocus == 11;
            FireGroup = eliteStatusJson.FireGroup;
        }

        public DateTimeOffset Timestamp { get; private set; } = DateTimeOffset.MinValue;
        public bool IsLanded { get; private set; } = false;
        public bool IsDocked { get; private set; } = false;
        public bool IsInDanger { get; private set; } = false;
        public bool IsInterdicted { get; private set; } = false;
        public bool IsOverheating { get; private set; } = false;
        public bool ShieldsDown { get; private set; } = false;
        public bool LandingGearDeployed { get; private set; } = false;
        public bool HardpointsDeployed { get; private set; } = false;
        public bool FlightAssistEnabled { get; private set; } = true;
        public bool CargoScoopDeployed { get; private set; } = false;
        public bool SilentRunning { get; private set; } = false;
        public bool FsdSupercruise { get; private set; } = false;
        public bool FsdMassLocked { get; private set; } = false;
        public bool FsdCharging { get; private set; } = false;
        public bool FsdCooldown { get; private set; } = false;
        public bool FsdJump { get; private set; } = false;
        public bool FsdAvailable { get => !IsLanded && !IsDocked && !FsdMassLocked && !FsdCharging && !LandingGearDeployed && !CargoScoopDeployed && (!HardpointsDeployed || FsdSupercruise); }
        public bool AnalysisMode { get; private set; } = false;
        public bool CombatMode { get => !AnalysisMode; }
        public bool LightsEnabled { get; private set; } = false;
        public bool NightVisionEnabled { get; private set; } = false;
        public int PipsWeapons { get; private set; } = 0;
        public int PipsSystems { get; private set; } = 0;
        public int PipsEngines { get; private set; } = 0;
        public bool FocusInternal { get; private set; } = false;
        public bool FocusComms { get; private set; } = false;
        public bool FocusRolePanel { get; private set; } = false;
        public bool FocusExternal { get; private set; } = false;
        public bool FocusGalaxyMap { get; private set; } = false;
        public bool FocusSystemMap { get; private set;} = false;
        public bool FocusFullSpectrumScanner { get; private set; } = false;
        public bool FocusDetailedSurfaceScanner { get; private set; } = false;
        public bool FocusCodex { get; private set; } = false;
        public int FireGroup { get; private set; } = 0;
    }
}
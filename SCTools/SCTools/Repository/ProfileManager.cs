using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using NLog;
using NSW.StarCitizen.Tools.Lib.Helpers;

namespace NSW.StarCitizen.Tools.Repository
{
    public sealed class ProfileManager
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly string _profilesPath = Path.Combine(Program.ExecutableDir, "profiles");
        private readonly IDictionary<string, CfgData> _profiles = new Dictionary<string, CfgData>(StringComparer.InvariantCultureIgnoreCase);
        private const string ProfileExtension = ".cfg";
        public IReadOnlyDictionary<string, CfgData> Profiles => new ReadOnlyDictionary<string, CfgData>(_profiles);

        public ProfileManager() {}

        public void Load()
        {
            _profiles.Clear();
            if (Directory.Exists(_profilesPath))
            {
                _logger.Info($"Load profiles at path: {_profilesPath}");
                var profileFiles = Directory.EnumerateFiles(_profilesPath, $"*{ProfileExtension}");
                foreach (var profilePath in profileFiles)
                {
                    _logger.Info($"Load profile: {profilePath}");
                    var profileFile = new CfgFile(profilePath);
                    var profileData = profileFile.Read();
                    if (profileData != null)
                    {
                        _profiles.Add(GetProfileName(profilePath), profileData);
                    }
                }
            }
        }

        public static bool IsValidProfileName(string profileName) =>
            !string.IsNullOrWhiteSpace(profileName) && profileName.IndexOfAny(Path.GetInvalidFileNameChars()) < 0;

        public bool CreateProfile(string profileName, CfgData profileData)
        {
            _logger.Info($"Create profile: {profileName}");
            if (_profiles.ContainsKey(profileName))
            {
                _logger.Warn($"New profile name: {profileName} already exist");
                return false;
            }
            if (CreateProfilesDirectory())
            {
                var profileFile = new CfgFile(GetProfilePath(profileName));
                if (profileFile.Save(profileData))
                {
                    _profiles.Add(profileName, profileData);
                    return true;
                }
            }
            return false;
        }

        public bool SaveProfile(string profileName, CfgData profileData)
        {
            _logger.Info($"Save profile: {profileName}");
            if (!_profiles.ContainsKey(profileName))
            {
                _logger.Error($"Save profile: {profileName} not found");
                return false;
            }
            if (CreateProfilesDirectory())
            {
                var profileFile = new CfgFile(GetProfilePath(profileName));
                if (profileFile.Save(profileData))
                {
                    _profiles[profileName] = profileData;
                    return true;
                }
            }
            return false;
        }

        public bool RenameProfile(string oldProfileName, string newProfileName)
        {
            _logger.Info($"Rename profile: {oldProfileName} to {newProfileName}");
            if (_profiles.ContainsKey(newProfileName))
            {
                _logger.Warn($"New profile name: {newProfileName} already exist");
                return false;
            }
            if (!_profiles.TryGetValue(oldProfileName, out var profileData))
            {
                _logger.Error($"Rename profile: {oldProfileName} not found");
                return false;
            }
            try
            {
                var oldProfilePath = GetProfilePath(oldProfileName);
                var newProfilePath = GetProfilePath(newProfileName);
                if (File.Exists(newProfilePath))
                {
                    File.Delete(newProfilePath);
                }
                File.Move(oldProfilePath, newProfilePath);
                _profiles.Remove(oldProfileName);
                _profiles.Add(newProfileName, profileData);
            }
            catch (Exception e)
            {
                _logger.Error(e, $"Failed rename profile: {oldProfileName} to {newProfileName}");
                return false;
            }
            return true;
        }

        public void DeleteProfile(string profileName)
        {
            _logger.Info($"Delete profile: {profileName}");
            _profiles.Remove(profileName);
            if (!FileUtils.DeleteFileNoThrow(GetProfilePath(profileName)))
            {
                _logger.Error($"Delete profile failed: {profileName}");
            }
        }

        private bool CreateProfilesDirectory()
        {
            if (!Directory.Exists(_profilesPath))
            {
                _logger.Info($"Create profiles directory: {_profilesPath}");
                try
                {
                    Directory.CreateDirectory(_profilesPath);
                }
                catch (Exception e)
                {
                    _logger.Error(e, $"Failed create profiles directory: {_profilesPath}");
                    return false;
                }
            }
            return true;
        }

        private string GetProfilePath(string profileName) => Path.Combine(_profilesPath, profileName + ProfileExtension);

        private static string GetProfileName(string profilePath) => Path.GetFileNameWithoutExtension(profilePath);
    }
}

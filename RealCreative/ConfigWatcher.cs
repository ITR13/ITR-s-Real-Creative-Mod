using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MelonLoader;
using Tomlet;
using UnityEngine;

namespace RealCreative
{
    static class ConfigWatcher
    {
        private const string FileName = "CreativeConfig.toml";

        private static readonly string FileDirectory = Path.Combine(
            Environment.CurrentDirectory,
            "UserData"
        );

        public static readonly string FullPath = Path.Combine(
            FileDirectory,
            FileName
        );

        public static CreativeConfig CreativeConfig = new CreativeConfig();

        private static readonly FileSystemWatcher FileSystemWatcher;
        private static bool _dirty = false;

        static ConfigWatcher()
        {
            FileSystemWatcher = new FileSystemWatcher(FileDirectory, FileName)
            {
                NotifyFilter = (NotifyFilters)((1 << 9) - 1),
                EnableRaisingEvents = true
            };
            FileSystemWatcher.Changed += (_, __) => _dirty = true;
            FileSystemWatcher.Created += (_, __) => _dirty = true;
            FileSystemWatcher.Renamed += (_, __) => _dirty = true;
            FileSystemWatcher.Deleted += (_, __) => _dirty = true;
            _dirty = true;
        }

        public static void Unload()
        {
            FileSystemWatcher.EnableRaisingEvents = false;
            _dirty = false;
        }

        public static bool UpdateIfDirty()
        {
            if (!_dirty) return false;
            _dirty = false;

            if (!File.Exists(FullPath))
            {
                MelonLogger.Msg(
                    $"Creating default config file at \"{FullPath}\""
                );

                var toml = TomletMain.TomlStringFrom(CreativeConfig.Default());
                File.WriteAllText(FullPath, toml);

                CreativeConfig = new CreativeConfig();
                return true;
            }

            MelonLogger.Msg("Updating Tracer configs");

            try
            {
                var toml = File.ReadAllText(FullPath);
                CreativeConfig = TomletMain.To<CreativeConfig>(toml);
            }
            catch (Exception e)
            {
                MelonLogger.Error(e.ToString());
                MelonLogger.Msg(
                    "Something went wrong when deserializing toml"
                );
                return false;
            }

            CreativeConfig = CreativeConfig ?? new CreativeConfig();

            CreativeConfig.powerup = FixNames(CreativeConfig.powerup, DataExtractor.GetPowerups());
            CreativeConfig.item = FixNames(CreativeConfig.item , DataExtractor.GetItems());
            CreativeConfig.mob = FixNames(CreativeConfig.mob, DataExtractor.GetMobs());


            try
            {
                var toml = TomletMain.TomlStringFrom(CreativeConfig.Default());
                File.WriteAllText(FullPath, toml);
            }
            catch (Exception e)
            {
                MelonLogger.Error(e.ToString());
                MelonLogger.Msg(
                    "Something went wrong when reserializing toml"
                );
            }


            RemoveNones(CreativeConfig.powerup);
            RemoveNones(CreativeConfig.item);
            RemoveNones(CreativeConfig.mob);

            return true;

            // Local Methods
            List<SpawnData> FixNames(List<SpawnData> list, List<Tuple<int, string>> idsAndNames)
            {
                if (list == null) return new List<SpawnData>();

                // For some stuff this goes from dict to list to dict, but whatever
                var names = idsAndNames
                    .Where(pair => !string.IsNullOrWhiteSpace(pair.Item2))
                    .ToDictionary(pair => pair.Item1, pair => pair.Item2);

                foreach (var entry in list)
                {
                    if (!names.TryGetValue(entry.id, out var name)) continue;
                    entry.name = name;
                }

                return list;
            }
            void RemoveNones(List<SpawnData> datas) =>
                datas.RemoveAll(data => data.hold == KeyCode.None && data.trigger == KeyCode.None);
        }
    }
}

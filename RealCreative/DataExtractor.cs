using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;

namespace RealCreative
{
    public static class DataExtractor
    {
        private const string FileName = "all_ids.txt";

        private static readonly string FileDirectory = Path.Combine(
            Environment.CurrentDirectory,
            "UserData"
        );
        private static readonly string FullPath = Path.Combine(
            FileDirectory,
            FileName
        );

        public static void GenerateAndSaveAndOpen()
        {
            var sb = GenerateString();
            SaveToFileAndShow(FullPath, sb);
        }

        public static List<Tuple<int, string>> GetMobs() =>
            MobSpawner.Instance.allMobs.Select((mob, id) => Tuple.Create(id, mob.name)).ToList();

        public static List<Tuple<int, string>> GetItems() =>
            ItemManager.Instance.allItems.Select(pair => Tuple.Create(pair.Key, pair.Value.name)).ToList();

        public static List<Tuple<int, string>> GetPowerups() => 
            ItemManager.Instance.allPowerups.Select(pair => Tuple.Create(pair.Key, pair.Value.name)).ToList();

        public static StringBuilder GenerateString()
        {
            var sb = new StringBuilder();

            void AddAll(List<Tuple<int, string>> list)
            {
                foreach (var (id, name) in list)
                {
                    sb.Append(id);
                    sb.Append("\t");
                    sb.AppendLine(name);
                }
                sb.AppendLine();
            }

            sb.AppendLine("# Powerups");
            AddAll(GetPowerups());
            sb.AppendLine("# Items");
            AddAll(GetItems());
            sb.AppendLine("# Mobs");
            AddAll(GetMobs());

            return sb;
        }

        public static void SaveToFileAndShow(string path, StringBuilder sb)
        {
            File.WriteAllText(path, sb.ToString());
            MelonLogger.Msg($"Saved info to '{path}'");
            Process.Start(path);
        }
    }
}

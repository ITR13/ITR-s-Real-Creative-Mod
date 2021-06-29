using System;
using System.Collections.Generic;
using UnityEngine;

namespace RealCreative
{
    [Serializable]
    public class CreativeConfig
    {
        public static CreativeConfig Default()
        {
            var config = new CreativeConfig();

            void AddAll(List<Tuple<int, string>> src, List<SpawnData> dst)
            {
                foreach (var (id, name) in src)
                {
                    dst.Add(
                        new SpawnData
                        {
                            id = id,
                            name = name,
                            amount = 1,
                            hold = KeyCode.None,
                            trigger = KeyCode.None,
                        }
                    );
                }
            }

            AddAll(DataExtractor.GetPowerups(), config.powerup);
            AddAll(DataExtractor.GetItems(), config.item);
            AddAll(DataExtractor.GetMobs(), config.mob);

            config.powerup[0].trigger = KeyCode.Keypad1;
            config.powerup[0].amount = 10;

            config.item[87].trigger = KeyCode.Keypad2;

            config.mob[10].hold = KeyCode.LeftControl;
            config.mob[10].trigger = KeyCode.Keypad3;
            
            return config;
        }

        public List<SpawnData>
            powerup = new List<SpawnData>(),
            item = new List<SpawnData>(), 
            mob = new List<SpawnData>();
    }

    [Serializable]
    public class SpawnData
    {
        public string name;
        public int id, amount;
        public KeyCode hold, trigger;
    }
}

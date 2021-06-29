using System;
using System.Collections.Generic;
using MelonLoader;
using UnityEngine;

namespace RealCreative
{
    public static class InputHandler
    {
        public static void DoInput()
        {
            var updatePos = true;

            var config = ConfigWatcher.CreativeConfig;

            HandleList(config.powerup, Spawner.SpawnPowerup);
            HandleList(config.item, Spawner.SpawnItem);
            HandleList(config.mob, Spawner.SpawnMob);

            foreach (var data in config.randomPowerup)
            {
                if (data.trigger == KeyCode.None) continue;
                if (data.hold != KeyCode.None && !Input.GetKey(data.hold)) continue;
                if (!Input.GetKeyDown(data.trigger)) continue;

                if (updatePos)
                {
                    updatePos = false;
                    Spawner.UpdateSpawnPosition();
                }

                if (!Spawner.SpawnRandomPowerup(data)) break;
            }

            return;
            void HandleList(List<SpawnData> spawnDatas, Func<int, int, bool> onTrigger)
            {
                foreach (var data in spawnDatas)
                {
                    if (!HandleData(data, onTrigger))
                    {
                        break;
                    }
                }
            }

            bool HandleData(SpawnData spawnData, Func<int, int, bool> onTrigger)
            {
                if (spawnData.trigger == KeyCode.None) return true;
                if (spawnData.hold != KeyCode.None && !Input.GetKey(spawnData.hold)) return true;
                if (!Input.GetKeyDown(spawnData.trigger)) return true;

                MelonLogger.Msg($"Spawning {spawnData.amount} * '{spawnData.name}' at {updatePos}");

                if (updatePos)
                {
                    updatePos = false;
                    Spawner.UpdateSpawnPosition();
                }

                return onTrigger?.Invoke(spawnData.id, spawnData.amount) ?? true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelonLoader;
using UnityEngine;

namespace RealCreative
{
    public static class InputHandler
    {
        public static void DoInput()
        {
            var updatePos = true;

            HandleList(ConfigWatcher.CreativeConfig.powerup, Spawner.SpawnPowerup);
            HandleList(ConfigWatcher.CreativeConfig.item, Spawner.SpawnItem);
            HandleList(ConfigWatcher.CreativeConfig.mob, Spawner.SpawnMob);

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

﻿using System;
using MelonLoader;
using UnityEngine;

namespace RealCreative
{
    public static class Spawner
    {
        public static Vector3 SpawnPosition { get; private set; }

        public static void UpdateSpawnPosition()
        {
            var mainCamera = Camera.main;
            if (mainCamera == null)
            {
                MelonLogger.Warning("Failed to find main camera");
                return;
            }

            var screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f);
            if (!Physics.Raycast(mainCamera.ScreenPointToRay(screenCenter), out var hitInfo)) return;

            SpawnPosition = hitInfo.point + hitInfo.normal * 0.3f;
        }

        public static bool SpawnMob(int id, int quantity)
        {
            var mobSpawner = MobSpawner.Instance;
            if (mobSpawner == null)
            {
                MelonLogger.Error("Failed to find MobSpawner");
                return false;
            }

            var mobManager = MobManager.Instance;
            if (mobManager == null)
            {
                MelonLogger.Error("Failed to find MobManager");
                return false;
            }

            mobSpawner.ServerSpawnNewMob(
                mobManager.GetNextId(),
                id,
                SpawnPosition, 
                quantity, quantity
            );
            return true;
        }

        public static bool SpawnPowerup(int id, int quantity)
        {
            var itemManager = ItemManager.Instance;
            if (itemManager == null)
            {
                MelonLogger.Error("Failed to find ItemManager");
                return false;
            }
            for (var i = 0; i < quantity; i++)
            {
                var nextId = itemManager.GetNextId();
                itemManager.DropPowerupAtPosition(id, SpawnPosition, nextId);
                ServerSend.DropPowerupAtPosition(id, nextId, SpawnPosition);
            }

            return true;
        }

        public static bool SpawnItem(int id, int quantity)
        {
            var itemManager = ItemManager.Instance;
            if (itemManager == null)
            {
                MelonLogger.Error("Failed to find ItemManager");
                return false;
            }
            var nextId = itemManager.GetNextId();
            itemManager.DropItemAtPosition(id, quantity, SpawnPosition, nextId);
            ServerSend.DropItemAtPosition(id, quantity, nextId, SpawnPosition);
            return true;
        }

        public static bool SpawnRandomPowerup(SpawnRandomPowerupData data)
        {
            var itemManager = ItemManager.Instance;
            if (itemManager == null)
            {
                MelonLogger.Error("Failed to find ItemManager");
                return false;
            }

            MelonLogger.Msg($"Spawning {data.amount} random powerups at {SpawnPosition}");

            for (var i = 0; i < data.amount; i++)
            {
                var randomPowerup = itemManager.GetRandomPowerup(data.whiteWeight, data.blueWeight, data.orangeWeight);
                var nextId = itemManager.GetNextId();
                itemManager.DropPowerupAtPosition(randomPowerup.id, SpawnPosition, nextId);
                ServerSend.DropPowerupAtPosition(randomPowerup.id, nextId, SpawnPosition);
            }

            return true;
        }
    }
}

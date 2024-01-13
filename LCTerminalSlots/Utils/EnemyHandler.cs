using LCTerminalSlots.Patches;
using System;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LCTerminalSlots.Utils
{
    internal class EnemyHandler
    {
        internal static (bool, string) AttemptSpawnEnemy(string enemyName, int amount)
        {
            var currentLevel = RoundAPI.CurrentLevel;
            if (currentLevel is null) return (false, "Null");

            bool selectedEnemy = false;
            string outEnemyName = "";

            foreach (var enemy in currentLevel.Enemies)
            {
                if (enemy.enemyType.enemyName.ToLower().Contains(enemyName.ToLower()))
                {
                    try
                    {
                        CreateEnemy(enemy, amount, true);
                        selectedEnemy = true;
                        outEnemyName = enemy.enemyType.enemyName;
                    }
                    catch
                    {
                        return (false, outEnemyName);
                    }
                }
            }

            if (!selectedEnemy)
            {
                foreach (var outEnemy in currentLevel.OutsideEnemies)
                {
                    if (outEnemy.enemyType.enemyName.ToLower().Contains(enemyName.ToLower()))
                    {
                        try
                        {
                            CreateEnemy(outEnemy, amount, false);
                            selectedEnemy = true;
                            outEnemyName = outEnemy.enemyType.enemyName;
                        }
                        catch
                        {
                            return (false, outEnemyName);
                        }
                    }
                }
            }

            return (true, outEnemyName);
        }

        private static void CreateEnemy(SpawnableEnemyWithRarity enemy, int amount, bool inDungeon)
        {
            var roundInstance = RoundManager.Instance;
            var levelInstance = RoundAPI.CurrentLevel;
            if (levelInstance is null) return;

            if (inDungeon)
            {
                for (int i = 0; i < amount; i++)
                {
                    roundInstance.SpawnEnemyOnServer(
                        roundInstance.allEnemyVents[Random.Range(0, roundInstance.allEnemyVents.Length)].floorNode.position,
                        roundInstance.allEnemyVents[i].floorNode.eulerAngles.y,
                        levelInstance.Enemies.IndexOf(enemy));
                }
            }
            else
            {
                for (int i = 0; i < amount; i++)
                {
                    var enemyObj = UnityEngine.Object.Instantiate(levelInstance
                        .OutsideEnemies[levelInstance.OutsideEnemies.IndexOf(enemy)]
                        .enemyType.enemyPrefab, GameObject.FindGameObjectsWithTag("OutsideAINode")[Random.Range(0, GameObject.FindGameObjectsWithTag("OutsideAINode").Length - 1)].transform.position, Quaternion.Euler(Vector3.zero));
                    enemyObj.gameObject.GetComponentInChildren<NetworkObject>().Spawn(destroyWithScene: true);
                }
            }
        }
    }
}

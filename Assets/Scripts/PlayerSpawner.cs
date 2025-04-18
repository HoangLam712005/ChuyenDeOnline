﻿using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    public GameObject PlayerPrefab;
    public float spawnRange = 10f; // Phạm vi ngẫu nhiên cho vị trí xuất hiện

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnRange, spawnRange),
                1,
                Random.Range(-spawnRange, spawnRange)
            );

            Runner.Spawn(PlayerPrefab, spawnPosition, Quaternion.identity,
                Runner.LocalPlayer, (runner, obj) =>
                {
                    var _player = obj.GetComponent<PlayerSetup>();
                    _player.SetupCamera();
                }
            );

            Debug.Log("add player with synchronized random color");
        }
    }
}

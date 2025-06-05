



using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
   
    public GameObject[] gameplayCharacterPrefabs;
    public Transform spawnPoint;
    public GameObject playerPrefab;

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            if (gameplayCharacterPrefabs == null || gameplayCharacterPrefabs.Length == 0)
            {
                Debug.LogError("gameplayCharacterPrefabs is empty or null.");
                return;
            }

            // Đọc index prefab từ PlayerPrefs
            int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);

            if (selectedIndex < 0 || selectedIndex >= gameplayCharacterPrefabs.Length)
            {
                Debug.LogError("Selected index out of range.");
                return;
            }

            GameObject selectedPrefab = gameplayCharacterPrefabs[selectedIndex];
            Debug.Log("Spawning player prefab: " + selectedPrefab.name);

            Vector3 randomOffset = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            Vector3 finalPosition = spawnPoint != null ? spawnPoint.position + randomOffset : randomOffset;

            NetworkRunner.OnBeforeSpawned onBeforePlayerSpawned = (runner, obj) =>
            {
                var setup = obj.GetComponent<PlayerSetup>();
                if (setup != null)
                    setup.SetupCamera();

                // Gán tên người chơi nếu có    
                string playerName = PlayerPrefs.GetString("PlayerName", "Unknown");
                if (obj.TryGetComponent<PlayerProperties>(out var props))
                {
                    props.SetPlayerName(playerName);
                }
            };

            Runner.Spawn(selectedPrefab, finalPosition, Quaternion.identity, Runner.LocalPlayer, onBeforePlayerSpawned);
        }
    }

}




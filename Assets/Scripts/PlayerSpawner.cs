//using UnityEngine;
//using Fusion;

//public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
//{
//    public GameObject PlayerPrefab;
//    public float spawnRange = 10f; // Phạm vi ngẫu nhiên cho vị trí xuất hiện

//    public void PlayerJoined(PlayerRef player)
//    {
//        // Gán nickname
//        string nickname;
//        if (player == Runner.LocalPlayer)
//        {
//            nickname = PlayerPrefs.GetString("nickname", $"Player {player.PlayerId}");
//        }
//        else
//        {
//            nickname = $"Player {player.PlayerId}";
//        }
//        PlayerNicknameManager.SetNickname(player, "TuanAnh"); // Gọi class xử lý lưu tên

//        // Spawn nhân vật
//        if (player == Runner.LocalPlayer)
//        {
//            Vector3 spawnPosition = new Vector3(
//                Random.Range(-spawnRange, spawnRange),
//                1,
//                Random.Range(-spawnRange, spawnRange)
//            );

//            Runner.Spawn(PlayerPrefab, spawnPosition, Quaternion.identity,
//                Runner.LocalPlayer, (runner, obj) =>
//                {
//                    var _player = obj.GetComponent<PlayerSetup>();
//                    _player.SetupCamera();
//                }
//            );

//            Debug.Log("add player with synchronized random color");
//        }
//    }
//}


//using System.Collections.Generic;
//using UnityEngine;
//using Fusion;
//using System.Runtime.Serialization;

//public class PlayerSpawner : NetworkBehaviour
//{
//    public List<GameObject> gameplayCharacterPrefabs;
//    public Transform spawnPoint; // Kéo một Empty GameObject làm điểm spawn

//    void Start()
//    {
//        int index = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
//        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

//        NetworkObject player = Runner.Spawn(gameplayCharacterPrefabs[index], spawnPoint.position, Quaternion.identity);
//        PlayerProperties props = player.GetComponent<PlayerProperties>();
//        if (props != null)
//        {
//            props.SetPlayerName(playerName);
//        }
//    }
//}

using UnityEngine;
using Fusion;

public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
{
    //public NetworkRunner Runner; // Đối tượng NetworkRunner
    public GameObject[] gameplayCharacterPrefabs; // Mảng prefab nhân vật
    public Transform spawnPoint; // Điểm spawn
    public int index; // Chỉ số prefab cần spawn
    public GameObject playerPrefab; // Prefab nhân vật

    //void Start()
    //{
    //    // Log các giá trị để kiểm tra
    //    //Debug.Log($"Runner: {Runner}");
    //    Debug.Log($"gameplayCharacterPrefabs: {gameplayCharacterPrefabs}");
    //    Debug.Log($"Prefab at index {index}: {(index >= 0 && index < gameplayCharacterPrefabs.Length ? gameplayCharacterPrefabs[index] : "Invalid index")}");
    //    Debug.Log($"SpawnPoint: {spawnPoint}");
    //    Debug.Log($"SpawnPoint Position: {spawnPoint?.position}");

    //    // Kiểm tra các tham số
    //    //if (Runner == null || !Runner.IsRunning)
    //    //{
    //    //    Debug.LogError("Runner is null or not running. Please assign a valid NetworkRunner.");
    //    //    return;
    //    //}
    //    if (gameplayCharacterPrefabs == null || gameplayCharacterPrefabs.Length == 0)
    //    {
    //        Debug.LogError("GameplayCharacterPrefabs is null or empty. Please assign prefabs.");
    //        return;
    //    }
    //    if (index < 0 || index >= gameplayCharacterPrefabs.Length)
    //    {
    //        Debug.LogError($"Index {index} is out of bounds. Ensure it is within the range of gameplayCharacterPrefabs.");
    //        return;
    //    }
    //    if (spawnPoint == null)
    //    {
    //        Debug.LogError("SpawnPoint is null. Please assign a valid Transform.");
    //        return;
    //    }

    //    // Kiểm tra xem prefab có phải là NetworkObject
    //    var prefab = gameplayCharacterPrefabs[index];
    //    if (prefab.GetComponent<NetworkObject>() == null)
    //    {
    //        Debug.LogError($"Prefab at index {index} is not a NetworkObject. Please assign a valid NetworkObject prefab.");
    //        return;
    //    }

    //    // Spawn player
    //    try
    //    {
    //        NetworkObject player = Runner.Spawn(
    //           playerPrefab, // Thay thế bằng prefab nhân vật
    //                         //gameplayCharacterPrefabs[index],
    //            spawnPoint.position,
    //            Quaternion.identity
    //        );
    //        Debug.Log("Player spawned successfully!");
    //    }
    //    catch (System.Exception ex)
    //    {
    //        Debug.LogError($"Failed to spawn player: {ex.Message}");
    //    }
    //}

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Debug.Log("PlayerPrefab:" + playerPrefab);

            Vector3 randomOffset = new Vector3(Random.Range(-5, 5), 0, Random.Range(-5, 5));
            Vector3 finalPosition = spawnPoint != null ? spawnPoint.position + randomOffset : randomOffset;

            NetworkRunner.OnBeforeSpawned onBeforePlayerSpawned = (runner, obj) =>
            {
                var playerSetup = obj.GetComponent<PlayerSetup>();
                if (playerSetup != null)
                {
                    playerSetup.SetupCamera();
                }
            };
            Runner.Spawn(gameplayCharacterPrefabs[index], finalPosition, Quaternion.identity, Runner.LocalPlayer, onBeforePlayerSpawned);


        }
    }
}


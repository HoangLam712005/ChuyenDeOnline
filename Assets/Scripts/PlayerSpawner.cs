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
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public List<GameObject> gameplayCharacterPrefabs;
    public Transform spawnPoint; // Kéo một Empty GameObject làm điểm spawn

    void Start()
    {
        int index = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

        GameObject player = Instantiate(gameplayCharacterPrefabs[index], spawnPoint.position, Quaternion.identity);

        PlayerProperties props = player.GetComponent<PlayerProperties>();
        if (props != null)
        {
            props.SetPlayerName(playerName);
        }
    }
}
using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using System.Collections;

public class LoadGame : MonoBehaviourPunCallbacks
{
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private GameObject CastlePrefab;

    void Start()
    {
        if (SettingManager.Instance.getOnline())
        {
            Debug.Log($"LoadGame active on {PhotonNetwork.NickName} | ViewID: {photonView?.ViewID}");
            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("👑 Master phân vị trí spawn cho toàn bộ player...");
                StartCoroutine(StartSpawn());
            }
        }
        else
        {
            Instantiate(CastlePrefab, spawnPoints[0].position, Quaternion.identity);
            Debug.Log("Đã Tọa thành chính Thành Công");
        }
    }

    private IEnumerator StartSpawn()
    {
        yield return new WaitForSeconds(3f);
        AssignSpawnPositions();
    }

    /// <summary>
    /// Master gửi index spawn cho từng player
    /// </summary>
    private void AssignSpawnPositions()
    {
        int index = 0;

        foreach (Player p in PhotonNetwork.PlayerList)
        {
            int assignedIndex = index % spawnPoints.Count;
            Debug.Log($"📦 Gửi vị trí spawn {assignedIndex} cho {p.NickName}");
            photonView.RPC(nameof(ReceiveSpawnPosition), p, assignedIndex);
            index++;
        }
    }

    /// <summary>
    /// Mỗi client tự nhận vị trí spawn và tự tạo object của mình
    /// </summary>
    [PunRPC]
    private void ReceiveSpawnPosition(int index)
    {
        Vector3 pos = spawnPoints[index].position;

        Debug.Log($"🚀 {PhotonNetwork.NickName} nhận vị trí spawn {index} tại {pos}");

        // Mỗi client chỉ tạo object cho chính mình
        PhotonNetwork.Instantiate("A_MultiplayerSytem/Prefab/Player/House/Castle", pos, Quaternion.identity);
    }
}

using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class RoomKickManager : MonoBehaviourPunCallbacks
{
    public void clickPlayer2()
    {
        KickPlayerByOrder(2);
    }

    public void clickPlayer3()
    {
        KickPlayerByOrder(3);
    }

    public void clickPlayer4()
    {
        KickPlayerByOrder(4);
    }

    // Kick theo thứ tự vào phòng (order: 1 = first join, 2 = second...)
    public void KickPlayerByOrder(int order)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogWarning("❌ Chỉ MasterClient mới có thể kick.");
            return;
        }

        var sorted = PhotonNetwork.PlayerList.OrderBy(p => p.ActorNumber).ToList();
        if (order < 1 || order > sorted.Count) { Debug.LogWarning("Thứ tự không hợp lệ"); return; }

        Player target = sorted[order - 1];
        if (target == PhotonNetwork.LocalPlayer) { Debug.LogWarning("Không thể kick chính mình."); return; }

        // 1) Xoá object của họ trước để tránh 'ghost'
        PhotonNetwork.DestroyPlayerObjects(target);

        // 2) Thêm vào banlist (tùy chọn)
        AddToBanList(target.ActorNumber);

        // 3) Gửi RPC để yêu cầu họ rời
        photonView.RPC("RPC_DoKick", target);

        Debug.Log($"Đã gửi yêu cầu kick {target.NickName} (Actor {target.ActorNumber})");
    }

    void AddToBanList(int actorNumber)
    {
        var room = PhotonNetwork.CurrentRoom;
        string ban = room.CustomProperties.ContainsKey("ban") ? room.CustomProperties["ban"].ToString() : "";
        if (!ban.Contains(actorNumber + ","))
        {
            ban += actorNumber + ",";
            room.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { "ban", ban } });
        }
    }

    [PunRPC]
    void RPC_DoKick()
    {
        Debug.Log("Bạn bị kick khỏi phòng bởi chủ phòng.");
        PhotonNetwork.LeaveRoom();
        // Có thể Disconnect nếu cần
    }
}

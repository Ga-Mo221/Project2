using UnityEngine;
using Photon.Pun;

public class CastleManager : MonoBehaviourPun
{
    public static CastleManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Castle Castle = null;
    private int ID = 0;

    public void Import(Castle c, int id)
    {
        bool _isOnline = SettingManager.Instance.getOnline();
        if (_isOnline)
            if (!c.photonView.IsMine) return; // chỉ chạy trên chủ sở hữu
            
        if (Castle != null)
        {
            Debug.Log($"[{PhotonNetwork.NickName}] ⚠️ Castle đã tồn tại!");
            return;
        }

        Castle = c;
        ID = id;

        if (_isOnline)
            Debug.Log($"[{PhotonNetwork.NickName}] ✅ Gán Castle {c.name} (ID={ID}) thành công!");
        }

    public int getID() => ID;
}

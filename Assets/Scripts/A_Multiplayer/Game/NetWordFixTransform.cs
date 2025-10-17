using UnityEngine;
using Photon.Pun;

public class NetWordFixTransform : MonoBehaviourPun, IPunObservable
{
    [SerializeField] private bool position = true;
    [SerializeField] private bool scale = false;

    [SerializeField] private Transform _targetTransform; // object chứa position & scale thật
     // Sync variables
    private Vector3 _networkPosition;
    private Vector3 _networkScale;
    private float _smoothTime = 10f;


    void Start()
    {
        if (_targetTransform == null)
            _targetTransform = transform;
    }

    void Update()
    {
        // Cập nhật vị trí mượt mà nếu không phải của local player
        if (SettingManager.Instance.getOnline() && !photonView.IsMine)
        {
            if (position) _targetTransform.position = Vector3.Lerp(_targetTransform.position, _networkPosition, Time.deltaTime * _smoothTime);
            if (scale) _targetTransform.localScale = Vector3.Lerp(_targetTransform.localScale, _networkScale, Time.deltaTime * _smoothTime);
        }
    }

    //=====================//
    // 🌐 POSITION + SCALE //
    //=====================//
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (!SettingManager.Instance.getOnline()) return;
        if (!position && !scale) return;
        if (stream.IsWriting)
        {
            // Gửi position & scale
            if (position) stream.SendNext(_targetTransform.position);
            if (scale) stream.SendNext(_targetTransform.localScale);
        }
        else
        {
            // Nhận dữ liệu
            if (position) _networkPosition = (Vector3)stream.ReceiveNext();
            if (scale) _networkScale = (Vector3)stream.ReceiveNext();
        }
    }
}

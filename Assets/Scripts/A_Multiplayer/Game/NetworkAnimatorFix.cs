using UnityEngine;
using Photon.Pun;
using System.Collections;

public class NetworkAnimatorFix : MonoBehaviourPun
{
    [Header("References")]
    [SerializeField] private Animator _anim;

    // Local cache để tránh gửi liên tục
    [SerializeField] private bool _lastMove;
    [SerializeField] private bool _lastDetectEnemy;
    [SerializeField] private int _lastTypeFarm;
    [SerializeField] private int _lastTypeCastle;

    void Awake()
    {
        if (_anim == null)
            _anim = GetComponent<Animator>();

        if (_anim == null)
            Debug.LogWarning("⚠️ Không tìm thấy Animator trong " + name);
    }

    //=====================//
    //  🔥 CASTLE SECTION  //
    //=====================//
    public void ChangeTypeCastle(int value)
    {
        if (_lastTypeCastle == value) return; // không gửi nếu không thay đổi
        _lastTypeCastle = value;
        _anim.SetInteger("Type", value);

        if (photonView.IsMine)
            photonView.RPC(nameof(RPC_ChangeTypeCastle), RpcTarget.Others, value);
    }

    [PunRPC]
    private void RPC_ChangeTypeCastle(int value)
    {
        StartCoroutine(setTypeCastle(value));
    }
    private IEnumerator setTypeCastle(int value)
    {
        yield return new WaitForSeconds(0.3f);
        _anim.SetInteger("Type", value);
    }


    //=====================//
    //  ⚔️ PLAYER SECTION  //
    //=====================//
    public void Move(bool moving)
    {
        if (_lastMove == moving) return;
        _lastMove = moving;
        _anim.SetBool("Moving", moving);

        if (SettingManager.Instance.getOnline() && photonView.IsMine)
            photonView.RPC(nameof(RPC_Moving), RpcTarget.Others, moving);
    }

    [PunRPC]
    private void RPC_Moving(bool moving)
    {
        _anim.SetBool("Moving", moving);
    }


    public void Attack_or_Farm()
    {
        _anim.SetBool("attack", true);

        if (SettingManager.Instance.getOnline() && photonView.IsMine)
            photonView.RPC(nameof(RPC_PlayAttack), RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_PlayAttack()
    {
        _anim.SetBool("attack", true);
    }

    public void ReturnAttack()
    {
        _anim.SetBool("attack", false);
    }


    public void FarmType(int value)
    {
        if (_lastTypeFarm == value) return;
        _lastTypeFarm = value;

        _anim.SetInteger("TypeFarm", value);
        if (SettingManager.Instance.getOnline() && photonView.IsMine)
            photonView.RPC(nameof(RPC_FarmType), RpcTarget.Others, value);
    }

    [PunRPC]
    private void RPC_FarmType(int value)
    {
        _anim.SetInteger("TypeFarm", value);
    }


    public void DetecEnemy(bool state)
    {
        if (_lastDetectEnemy == state) return;
        _lastDetectEnemy = state;

        _anim.SetBool("Detectenemy", state);
        if (SettingManager.Instance.getOnline() && photonView.IsMine)
            photonView.RPC(nameof(RPC_DetecEnemy), RpcTarget.Others, state);
    }

    [PunRPC]
    private void RPC_DetecEnemy(bool state)
    {
        _anim.SetBool("Detectenemy", state);
    }


    public void Die()
    {
        _anim.SetTrigger("Die");
        if (SettingManager.Instance.getOnline() && photonView.IsMine)
            photonView.RPC(nameof(RPC_Die), RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_Die()
    {
        _anim.SetTrigger("Die");
    }


    public void Heal()
    {
        _anim.SetTrigger("Heal");
        if (SettingManager.Instance.getOnline() && photonView.IsMine)
            photonView.RPC(nameof(RPC_Heal), RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_Heal()
    {
        _anim.SetTrigger("Heal");
    }


    public void Direction(int dir)
    {
        _anim.SetInteger("Direction", dir);
        if (SettingManager.Instance.getOnline() && photonView.IsMine)
            photonView.RPC(nameof(RPC_Direction), RpcTarget.Others, dir);
    }

    [PunRPC]
    private void RPC_Direction(int dir)
    {
        _anim.SetInteger("Direction", dir);
    }
}

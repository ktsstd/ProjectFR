using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class CollaborationSkill : MonoBehaviour
{
    public PhotonView pv;
    public int[] elementalSet = new int[] {10, 10};

    float elementalResetTime = 5;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
    }

    private void Update()
    {
        if (elementalSet[0] != 10 && elementalSet[1] != 10)
        {
            elementalResetTime -= Time.deltaTime;
            if (elementalResetTime <= 0)
            {
                elementalResetTime = 5;
                elementalSet[0] = 10;
                elementalSet[1] = 10;
            }
        }
    }

    [PunRPC]
    public void ElementalSettingMaster(int _code)
    {
        pv.RPC("ElementalSetting", RpcTarget.All, _code);
    }

    [PunRPC]
    public void ElementalSetting(int _code)
    {
        for (int slot = 0; slot < elementalSet.Length; slot++)
        {
            if (elementalSet[slot] == _code)
                return;
            if (elementalSet[slot] == 10)
            {
                elementalSet[slot] = _code;
                return;
            }
        }
    }
}

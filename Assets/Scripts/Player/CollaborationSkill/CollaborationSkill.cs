using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class CollaborationSkill : MonoBehaviour
{
    public GameObject playerUiObj;
    PlayerUi playerUi;

    public PhotonView pv;
    public int[] elementalSet = new int[] {10, 10};

    float elementalResetTime = 5;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        playerUi = playerUiObj.GetComponent<PlayerUi>();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (elementalSet[0] != 10 && elementalSet[1] != 10)
            {
                elementalResetTime -= Time.deltaTime;
                if (elementalResetTime <= 0)
                {
                    elementalResetTime = 5;
                    elementalSet[0] = 10;
                    elementalSet[1] = 10;
                    pv.RPC("ElementalSetting", RpcTarget.All, elementalSet);
                }
            }
        }

        playerUi.elementalData(elementalSet[0], elementalSet[1]);
    }

    [PunRPC]
    public void ElementalSettingMaster(int _code)
    {
        for (int slot = 0; slot < elementalSet.Length; slot++)
        {
            if (elementalSet[0] == _code || elementalSet[1] == _code)
                return;
            if (elementalSet[slot] == 10)
            {
                elementalSet[slot] = _code;
                pv.RPC("ElementalSetting", RpcTarget.All, elementalSet);
                return;
            }
        }
    }

    [PunRPC]
    public void ElementalSetting(int[] _setting)
    {
        elementalSet = _setting;
    }
}

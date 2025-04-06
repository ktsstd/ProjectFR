using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class FusionSkill : MonoBehaviour
{
    public List<GameObject> playerList = new List<GameObject>();
    public GameObject playerUiObj;
    PlayerUi playerUi;

    public PhotonView pv;
    public int[] elementalSet = new int[] {10, 10};
    public bool isFusionSkillReady;
    public GameObject skillPosObj;

    float elementalResetTime = 10;

    public GameObject FireAndLightningEF;

    public float FusionSkillCoolTime;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        playerUi = playerUiObj.GetComponent<PlayerUi>();
        Invoke("PlayerListSetting", 1f);
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (elementalSet[0] != 10 && elementalSet[1] != 10)
            {
                // elementalResetTime -= Time.deltaTime;
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

    public void PlayerListSetting()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        playerList = new List<GameObject>(players);
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

    public int RetrunFusionSkillReady(int _code)
    {
        if (elementalSet[0] == _code && elementalSet[1] != 10)
            return elementalSet[1];
        else if (elementalSet[0] != 10 && elementalSet[1] == _code)
            return elementalSet[0];
        else
            return 10;
    }

    public void FusionSkillSetting(Vector3 _skillPos)
    {
        if ((elementalSet[0] == 3 && elementalSet[1] == 1) || (elementalSet[0] == 1 && elementalSet[1] == 3)) // F&L
        {
            skillPosObj.SetActive(true);
            skillPosObj.transform.position = _skillPos;
        }
        else if ((elementalSet[0] == 3 && elementalSet[1] == 1) || (elementalSet[0] == 1 && elementalSet[1] == 3)) // L&W
        {
            // ??
        }
        else
            return;
    }

    public void FusionSkillRangeOff()
    {
        skillPosObj.SetActive(false);
    }

    public void UseFusionSkill(Vector3 _skillPos)
    {
        if ((elementalSet[0] == 3 && elementalSet[1] == 1) || (elementalSet[0] == 1 && elementalSet[1] == 3)) // F&L
        {
            skillPosObj.SetActive(false);
            pv.RPC("UseFireAndLightning", RpcTarget.All, _skillPos);
        }
        else if ((elementalSet[0] == 3 && elementalSet[1] == 1) || (elementalSet[0] == 1 && elementalSet[1] == 3)) // L&W
        {
            // ??
        }
        else
        {
            // misfire
        }

        elementalSet[0] = 10;
        elementalSet[1] = 10;
        pv.RPC("ElementalSetting", RpcTarget.All, elementalSet);
    }

    [PunRPC]
    public IEnumerator UseFireAndLightning(Vector3 _skillPos) // F&L
    {
        foreach (GameObject player in playerList)
        {
            if (player.name == "Fire(Clone)" || player.name == "Lightning(Clone)")
            {
                player.GetComponent<PlayerController>().UsingFusionSkill(true);
            }
        }
        foreach (GameObject player in playerList)
        {
            if (player.name == "Fire(Clone)")
            {
                player.GetComponent<Fire>().FireAndLightning(_skillPos);
                yield return new WaitForSeconds(1f);
            }
        }
        foreach (GameObject player in playerList)
        {
            if (player.name == "Lightning(Clone)")
            {
                player.GetComponent<Lightning>().LightningAndFire(_skillPos);
                yield return new WaitForSeconds(1f);
            }
        }
        GameObject skill = Instantiate(FireAndLightningEF, _skillPos, FireAndLightningEF.transform.rotation);
        foreach (GameObject player in playerList)
        {
            if (player.name == "Fire(Clone)" || player.name == "Lightning(Clone)")
            {
                player.GetComponent<PlayerController>().pv.RPC("LookAtTarget", RpcTarget.All, skill.name, 2f);
            }
        }
        yield return new WaitForSeconds(2);
        foreach (GameObject player in playerList)
        {
            if (player.name == "Fire(Clone)" || player.name == "Lightning(Clone)")
            {
                player.GetComponent<PlayerController>().UsingFusionSkill(false);
            }
        }
    }
}
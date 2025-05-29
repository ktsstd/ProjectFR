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
    public GameObject WaterAndLightningEF;
    public GameObject CloudEF;
    public GameObject EarthAndFireEF;
    public GameObject EarthAndWaterEF;

    float fusionSkillCoolTime;
    float fusionSkillMaxCoolTime;

    public Image[] cutScene;
    public Sprite[] playerCutIn;
    public RectTransform[] movePos;
    public RectTransform[] returnPos;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        playerUi = playerUiObj.GetComponent<PlayerUi>();
        Invoke("PlayerListSetting", 1f);
    }

    private void Update()
    {
        if (fusionSkillCoolTime >= 0)
            fusionSkillCoolTime -= Time.deltaTime;
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
        playerUi.InputFusionSkillData(fusionSkillCoolTime, fusionSkillMaxCoolTime);
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

    public bool FusionSkillCoolTimeCheck()
    {
        if (fusionSkillCoolTime >= 0)
            return false;
        else
            return true;
    }

    public void FusionSkillSetting(Vector3 _skillPos)
    {
        if ((elementalSet[0] == 3 && elementalSet[1] == 1) || (elementalSet[0] == 1 && elementalSet[1] == 3)) // F&L
        {
            skillPosObj.SetActive(true);
            skillPosObj.transform.position = _skillPos;
        }
        else if ((elementalSet[0] == 0 && elementalSet[1] == 1) || (elementalSet[0] == 1 && elementalSet[1] == 0)) // W&L
        {

        }
        else if ((elementalSet[0] == 3 && elementalSet[1] == 2) || (elementalSet[0] == 2 && elementalSet[1] == 3)) // E&F
        {
            skillPosObj.SetActive(true);
            skillPosObj.transform.position = _skillPos;
        }
        else if ((elementalSet[0] == 0 && elementalSet[1] == 2) || (elementalSet[0] == 2 && elementalSet[1] == 0)) // W&E
        {

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
            pv.RPC("FusionSkillCoolTime", RpcTarget.All, 30f);
        }
        else if ((elementalSet[0] == 0 && elementalSet[1] == 1) || (elementalSet[0] == 1 && elementalSet[1] == 0)) // L&W
        {
            pv.RPC("UseWaterAndLightning", RpcTarget.All, null);
            pv.RPC("FusionSkillCoolTime", RpcTarget.All, 30f);
        }
        else if ((elementalSet[0] == 2 && elementalSet[1] == 3) || (elementalSet[0] == 3 && elementalSet[1] == 2)) // E&F
        {
            pv.RPC("UseEarthAndFire", RpcTarget.All, _skillPos);
            pv.RPC("FusionSkillCoolTime", RpcTarget.All, 30f);
        }
        else if ((elementalSet[0] == 0 && elementalSet[1] == 2) || (elementalSet[0] == 2 && elementalSet[1] == 0)) // W&E
        {
            pv.RPC("UseWaterAndEarth", RpcTarget.All, null);
            pv.RPC("FusionSkillCoolTime", RpcTarget.All, 20f);
        }
        else
        {
            pv.RPC("FusionSkillCoolTime", RpcTarget.All, 5f);
            pv.RPC("GlassImageCrack", RpcTarget.All, null);
        }

        StartCoroutine("FusionSkillCutIn");
        elementalSet[0] = 10;
        elementalSet[1] = 10;
        pv.RPC("ElementalSetting", RpcTarget.All, elementalSet);
    }

    public IEnumerator FusionSkillCutIn()
    {
        cutScene[0].gameObject.SetActive(true);
        cutScene[1].gameObject.SetActive(true);
        if (elementalSet[0] != 10 && elementalSet[1] != 10)
        {
            cutScene[0].sprite = playerCutIn[elementalSet[0]];
            cutScene[1].sprite = playerCutIn[elementalSet[1]];
        }
        else
        {
            cutScene[0].sprite = playerCutIn[1];
            cutScene[1].sprite = playerCutIn[1];
        }

        while (Vector2.Distance(cutScene[0].rectTransform.anchoredPosition, movePos[0].anchoredPosition) > 0.1f)
        {
            cutScene[0].rectTransform.anchoredPosition = Vector2.MoveTowards(cutScene[0].rectTransform.anchoredPosition, movePos[0].anchoredPosition, 15);
            cutScene[1].rectTransform.anchoredPosition = Vector2.MoveTowards(cutScene[1].rectTransform.anchoredPosition, movePos[1].anchoredPosition, 15);
            yield return null;
        }

        yield return new WaitForSeconds(1.5f);

        while (Vector2.Distance(cutScene[0].rectTransform.anchoredPosition, returnPos[0].anchoredPosition) > 0.1f)
        {
            cutScene[0].rectTransform.anchoredPosition = Vector2.MoveTowards(cutScene[0].rectTransform.anchoredPosition, returnPos[0].anchoredPosition, 15);
            cutScene[1].rectTransform.anchoredPosition = Vector2.MoveTowards(cutScene[1].rectTransform.anchoredPosition, returnPos[1].anchoredPosition, 15);
            yield return null;
        }

        cutScene[0].gameObject.SetActive(false);
        cutScene[1].gameObject.SetActive(false);
    }


    [PunRPC]
    public void FusionSkillCoolTime(float _time)
    {
        fusionSkillCoolTime = _time;
        fusionSkillMaxCoolTime = _time;
    }

    [PunRPC]
    public IEnumerator GlassImageCrack()
    {
        playerUi.GlassImageCrack(true);
        yield return new WaitForSeconds(5);
        playerUi.GlassImageCrack(false);
    }

    [PunRPC]
    public IEnumerator UseFireAndLightning(Vector3 _skillPos) // F&L
    {
        StartCoroutine("FusionSkillCutIn");
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

    [PunRPC]
    public IEnumerator UseWaterAndLightning() // W&L
    {
        StartCoroutine("FusionSkillCutIn");
        GameManager.Instance.StartCoroutine(GameManager.Instance.FusionSkybox());
        GameObject cloud = null;
        foreach (GameObject player in playerList)
        {
            if (player.name == "Water(Clone)" || player.name == "Lightning(Clone)")
            {
                player.GetComponent<PlayerController>().UsingFusionSkill(true);
            }
        }
        foreach (GameObject player in playerList)
        {
            if (player.name == "Water(Clone)")
            {
                player.GetComponent<Water>().WaterAndLightning();
            }
        }
        foreach (GameObject player in playerList)
        {
            if (player.name == "Lightning(Clone)")
            {
                player.GetComponent<Lightning>().LightningAndWater();
            }
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine("WaterAndLightningRecovery");
        foreach (GameObject player in playerList)
        {
            if (player.name == "Lightning(Clone)")
            {
                cloud = Instantiate(CloudEF, player.transform.position + Vector3.up * 25, FireAndLightningEF.transform.rotation);
            }
        }
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate("Blessing & Thunder", Vector3.zero, WaterAndLightningEF.transform.rotation);
        }
        foreach (GameObject player in playerList)
        {
            if (player.name == "Water(Clone)" || player.name == "Lightning(Clone)")
            {
                player.GetComponent<PlayerController>().pv.RPC("LookAtTarget", RpcTarget.All, cloud.name, 2f);
            }
        }
        yield return new WaitForSeconds(2f);
        Destroy(cloud);
        foreach (GameObject player in playerList)
        {
            if (player.name == "Water(Clone)" || player.name == "Lightning(Clone)")
            {
                player.GetComponent<PlayerController>().UsingFusionSkill(false);
            }
        }
    }

    public IEnumerator WaterAndLightningRecovery()
    {
        int count = 1;
        while (count <= 10)
        {
            foreach (GameObject player in playerList)
            {
                player.GetComponent <PlayerController>().pv.RPC("OnPlayerRecovery", RpcTarget.All, 50f);
            }
            yield return new WaitForSeconds(1);
            count += 1;
        }
    }

    [PunRPC]
    public IEnumerator UseEarthAndFire(Vector3 _skillPos)
    {
        foreach (GameObject player in playerList)
        {
            if (player.name == "Earth(Clone)" || player.name == "Fire(Clone)")
            {
                player.GetComponent<PlayerController>().UsingFusionSkill(true);
            }
        }
        foreach (GameObject player in playerList)
        {
            if (player.name == "Earth(Clone)")
            {
                player.GetComponent<Earth>().EarthAndFire(_skillPos);
            }
        }
        yield return new WaitForSeconds(0.5f);
        foreach (GameObject player in playerList)
        {
            if (player.name == "Fire(Clone)")
            {
                player.GetComponent<Fire>().FireAndEarth(_skillPos);
            }
        }
        yield return new WaitForSeconds(2f);
        GameObject skill = Instantiate(EarthAndFireEF, _skillPos, EarthAndFireEF.transform.rotation);
        foreach (GameObject player in playerList)
        {
            if (player.name == "Earth(Clone)" || player.name == "Fire(Clone)")
            {
                player.GetComponent<PlayerController>().pv.RPC("LookAtTarget", RpcTarget.All, skill.name, 2f);
            }
        }
        yield return new WaitForSeconds(2);
        foreach (GameObject player in playerList)
        {
            if (player.name == "Earth(Clone)" || player.name == "Fire(Clone)")
            {
                player.GetComponent<PlayerController>().UsingFusionSkill(false);
            }
        }
    }

    [PunRPC]
    public IEnumerator UseWaterAndEarth()
    {
        foreach (GameObject player in playerList)
        {
            if (player.name == "Earth(Clone)" || player.name == "Water(Clone)")
            {
                player.GetComponent<PlayerController>().UsingFusionSkill(true);
            }
        }
        foreach (GameObject player in playerList)
        {
            if (player.name == "Earth(Clone)")
            {
                player.GetComponent<Earth>().EarthAndWater();
            }
        }
        foreach (GameObject player in playerList)
        {
            if (player.name == "Water(Clone)")
            {
                player.GetComponent<Water>().WaterAndEarth();
            }
        }
        yield return new WaitForSeconds(1f);
        GameObject skill = Instantiate(EarthAndWaterEF, Vector3.zero, EarthAndFireEF.transform.rotation);
        foreach (GameObject player in playerList)
        {
            if (player.name == "Earth(Clone)" || player.name == "Water(Clone)")
            {
                player.GetComponent<PlayerController>().pv.RPC("LookAtTarget", RpcTarget.All, skill.name, 2f);
            }
        }
        yield return new WaitForSeconds(2);
        foreach (GameObject player in playerList)
        {
            if (player.name == "Earth(Clone)" || player.name == "Water(Clone)")
            {
                player.GetComponent<PlayerController>().UsingFusionSkill(false);
            }
        }
    }
}
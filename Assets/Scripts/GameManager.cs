using Cinemachine;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView pv;
    private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private Transform SpawnPos;
    [SerializeField] private Transform WaveEastFirstPos;
    [SerializeField] private Transform WaveWestFirstPos;
    [SerializeField] private Transform WaveSouthFirstPos;
    [SerializeField] private Transform WaveNorthFirstPos;
    [SerializeField] private Transform WaveSecondPos;
    [SerializeField] private Transform WaveThirdPos1;
    [SerializeField] private Transform WaveThirdPos2;
    [SerializeField] private Transform WaveThirdPos3;
    [SerializeField] private Transform WaveThirdPos4;
    [SerializeField] private GameObject Quit;
    [SerializeField] private Material SkyBoxMat;
    [SerializeField] private Material WLSkyBoxMat;
    private Material FSkyBoxMat;
    private Light FLight;
    [SerializeField] private Light DLight;

    [SerializeField] private TextMeshProUGUI WaveText;
    [SerializeField] private TextMeshProUGUI WaveAllMonsterCountText;
    [SerializeField] private TextMeshProUGUI LastTime;

    [SerializeField] private PlayerUi playerUi;
    [SerializeField] GameObject WaveBar;
    [SerializeField] GameObject BossHpBar;
    [SerializeField] GameObject Timer;
    [SerializeField] Object objectS;

    [SerializeField] CartMove cartS;

    private int WaveAllMonster;
    private int WaveCount = 0;
    public int WaveLoopCount = 0;
    public int selectedMode = 0;
    public float LastWaveTime = 0;
    public bool isSpawn = false;
    public bool Gaming = false;
    public GameObject localPlayerCharacter;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("GameManager").GetComponent<GameManager>();
            }
            return _instance;
        }
    }

    void Awake()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedMode", out object selectedMode);
        this.selectedMode = (int)selectedMode;
        pv = GetComponent<PhotonView>();
        virtualCamera = FindAnyObjectByType<CinemachineVirtualCamera>();
        FLight.color = DLight.color;
        FSkyBoxMat = RenderSettings.skybox;

        string prefabName = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedCharacter", out object character);
        if ((int)character == 0)
            prefabName = "Water";
        else if ((int)character == 1)
            prefabName = "Lightning";
        else if ((int)character == 2)
            prefabName = "Earth";
        else if ((int)character == 3)
            prefabName = "Fire";

        localPlayerCharacter = PhotonNetwork.Instantiate(prefabName, SpawnPos.position, Quaternion.identity);

        if (SceneManagerHelper.ActiveSceneName == "Stage1")
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (this.selectedMode == 0)
                {
                    pv.RPC("SetWaveTimer", RpcTarget.All, 5f);
                    StartCoroutine(WaveStart());
                }
                else if (this.selectedMode == 1)
                {
                    pv.RPC("SetWaveTimer", RpcTarget.All, 15f);
                    StartCoroutine(InfiniteWaveStart());
                }
            }
            StartCoroutine(CheckMonsterC());
            WaveCount += 1;
            Gaming = false;
        }
        else if (SceneManagerHelper.ActiveSceneName == "Stage2")
        {
            cartS.currentState = CartMove.States.Move; // 나중에 연출로 바꾸기
        }
    }
    [PunRPC]
    public void SetWaveTimer(float TimerF)
    {
        LastWaveTime = TimerF;
        Gaming = false;
    }

    void Update()
    {
        if (SceneManagerHelper.ActiveSceneName == "Stage1")
        {
            WaveAllMonsterCountText.text = (GameObject.FindGameObjectsWithTag("Enemy").Length + "/" + WaveAllMonster);

            if (WaveCount <= 3 && Gaming)
            {
                WaveBar.SetActive(true);
                BossHpBar.SetActive(false);
                Timer.SetActive(false);
                WaveText.text = ("Wave " + WaveCount + "/4");
            }
            else if (WaveCount == 4 && Gaming)
            {
                WaveBar.SetActive(false);
                BossHpBar.SetActive(true);
                Timer.SetActive(false);
            }

            if (!Gaming)
            {
                WaveBar.SetActive(false);
                BossHpBar.SetActive(false);
                Timer.SetActive(true);
                LastTime.text = "00:" + Mathf.Ceil(LastWaveTime);
            }
            if (WaveCount == 1)
                WaveAllMonster = 10;
            else if (WaveCount == 2)
                WaveAllMonster = 25;
            else if (WaveCount == 3)
                WaveAllMonster = 24;
            else if (WaveCount == 4 && !isSpawn)
            {
                RenderSettings.skybox = SkyBoxMat;
                DLight.color = new Color(1f, 0.3216f, 0f, 1f);
            }
            if (WaveCount !=4)
            {
                RenderSettings.skybox = FSkyBoxMat;
                DLight.color = FLight.color;
            }
        }
        else if (SceneManagerHelper.ActiveSceneName == "Stage2")
        {

        }
        if (LastWaveTime > 0)
        {
            LastWaveTime -= Time.deltaTime;
        }
    }

    public IEnumerator CheckMonsterC()
    {
        while (!isSpawn)
        {
            yield return new WaitForSeconds(5f);
            CheckMonster();
        }
    }

    public IEnumerator CheckPlayerC()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            CheckPlayer();
        }
    }

    public void CheckMonster()
    {
        if (!PhotonNetwork.IsMasterClient) return;
        if (SceneManagerHelper.ActiveSceneName == "Stage1")
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !isSpawn && selectedMode == 0)
            {
                pv.RPC("SetWaveTimer", RpcTarget.All, 5f);
                StartCoroutine(WaveStart());
                isSpawn = true;
                WaveCount += 1;
                HealPlayer();
            }
            else if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !isSpawn && selectedMode == 1)
            {
                pv.RPC("SetWaveTimer", RpcTarget.All, 15f);
                StartCoroutine(InfiniteWaveStart());
                isSpawn = true;
                WaveCount += 1;
                HealPlayer();
            }
        }
    }

    public void CheckPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        bool AllPlayerAlive = false;
        foreach (GameObject player in players)
        {
            PlayerController playerS = player.GetComponent<PlayerController>();
            if (playerS.playerHp > 0)
            {
                AllPlayerAlive = true;
                break;
            }
        }
        if (!AllPlayerAlive)
        {
            objectS.Damaged(9999999999999f);
        }
    }
    float HpHeal;
    public void HealPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            PlayerController playerS = player.GetComponent<PlayerController>();
            if (playerS.playerHp > 0)
            {
                if (PhotonNetwork.PlayerList.Length == 1)
                {
                    HpHeal = playerS.playerInfo.hp * 0.3f;
                }
                else if (PhotonNetwork.PlayerList.Length == 2)
                {
                    HpHeal = playerS.playerInfo.hp * 0.15f;
                }
                else if (PhotonNetwork.PlayerList.Length >= 3)
                {
                    HpHeal = playerS.playerInfo.hp * 0.1f;
                }
                playerS.playerHp += HpHeal;
                if (playerS.playerHp > playerS.playerInfo.hp)
                {
                    playerS.playerHp = playerS.playerInfo.hp;
                }
            }
        }
        pv.RPC("AllHeal", RpcTarget.All);
    }
    [PunRPC]
    public void AllHeal()
    {
        PlayerController playerS = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        playerUi.InputHpData(playerS.playerHp, playerS.playerInfo.hp);
    }

    public void GoToMain()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", null },
            { "selectedCharacter", -1 },
            { "selectedMode", 0 }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);
        //SaveManager saveS = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        //saveS.OnClickResetSetting();

        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main");
    }
    [PunRPC]
    public void Gmingtrue()
    {
        Gaming = true;
    }

    IEnumerator WaveStart()
    {
        isSpawn = true;
        if (PhotonNetwork.IsMasterClient)
        { 
            yield return new WaitForSeconds(LastWaveTime);
            pv.RPC("Gmingtrue", RpcTarget.All);
            //string Firemonster = "TEMPMONSTER/Spirit of Fire";
            string Sleebam = "Monster/Stage1/Sleebam";
            string Mugolin = "Monster/Stage1/Mugolin";
            //string SwmapSlime = "Monster/Stage1/SwampSlime";
            string Grave = "Monster/Stage1/Grave";
            string Boss = "Boss/Stage1/Boss";

            int a = Random.Range(1, 4);
            int b = Random.Range(1, 5);
            Transform[] spawnPositions = GetSpawnPositions(a, b);

            if (WaveCount == 1)
            {
                StartCoroutine(InstantiateMonsters(Mugolin, 10));
            }
            else if (WaveCount == 2)
            {
                StartCoroutine(InstantiateMonsters(Mugolin, 10));
                StartCoroutine(InstantiateMonsters(Sleebam, 15));
            }
            else if (WaveCount == 3)
            {
                StartCoroutine(InstantiateMonsters(Mugolin, 10));
                StartCoroutine(InstantiateMonsters(Sleebam, 10));
                StartCoroutine(InstantiateMonsters(Grave, 4));
            }
            else if (WaveCount == 4)
            {
                Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
                PhotonNetwork.Instantiate(Boss, randomSpawnPos.position, Quaternion.identity);
                SoundManager.Instance.PlayBgm(Boss);
                isSpawn = false;
            }
        }
    }

    IEnumerator InfiniteWaveStart()
    {
        isSpawn = true;
        if (PhotonNetwork.IsMasterClient)
        {
            // 상점 활성화
            yield return new WaitForSeconds(LastWaveTime);
            pv.RPC("Gmingtrue", RpcTarget.All);
            // 상점 비활성화
            string Sleebam = "Monster/Stage1/Sleebam";
            string Mugolin = "Monster/Stage1/Mugolin";
            string Grave = "Monster/Stage1/Grave";
            string Boss = "Boss/Stage1/Boss";

            int a = Random.Range(1, 4);
            int b = Random.Range(1, 5);
            Transform[] spawnPositions = GetSpawnPositions(a, b);

            if (WaveCount == 1)
            {
                StartCoroutine(InstantiateMonsters(Mugolin, 10));
            }
            else if (WaveCount == 2)
            {
                StartCoroutine(InstantiateMonsters(Mugolin, 10));
                StartCoroutine(InstantiateMonsters(Sleebam, 15));
            }
            else if (WaveCount == 3)
            {
                StartCoroutine(InstantiateMonsters(Mugolin, 10));
                StartCoroutine(InstantiateMonsters(Sleebam, 10));
                StartCoroutine(InstantiateMonsters(Grave, 4));
            }
            else if (WaveCount == 4)
            {
                Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
                PhotonNetwork.Instantiate(Boss, randomSpawnPos.position, Quaternion.identity);
                SoundManager.Instance.PlayBgm(Boss);
                isSpawn = false;
            }
            else if (WaveCount == 5)
            {
                WaveCount = 1;
                WaveLoopCount += 1;
                photonView.RPC("GiveAllPlayerGold", RpcTarget.All, 200);
                StartCoroutine(InfiniteWaveStart());
                isSpawn = true;
                HealPlayer();
            }
        }
    }

    [PunRPC]
    public void GiveAllPlayerGold(int gold)
    {
        PlayerController playerCtrl = GameManager.Instance.localPlayerCharacter.GetComponent<PlayerController>();
        playerCtrl.money += gold;
    }

    IEnumerator InstantiateMonsters(string monsterType, int count)
    {
        int Instantiatetime;
        if (count > 15)
        {
            Instantiatetime = Mathf.CeilToInt(count / 15); 
        }
        else
        {
            Instantiatetime = 0;
        }
        
        for (int inst = 0; inst <= Instantiatetime; inst++)  
        {
            int a = Random.Range(1, 4);
            int b = Random.Range(1, 5);
            Transform[] spawnPositions = GetSpawnPositions(a, b);
            int realcount = Mathf.FloorToInt(count / 15); 
            int realinstcount = count - (realcount * 15);
            if (realinstcount <= 0 && Instantiatetime <= 0) 
            {
                realinstcount = count;
            }
            if (inst >= Instantiatetime && realinstcount > 0)
            {
                if (a != 2)
                {
                    int sppos = 0;
                    for (int sp = 0; sp < realinstcount; sp++)
                    {
                        sppos += 1;
                        if (sppos > spawnPositions.Length - 1)
                        {
                            sppos = 0;
                        }
                        PhotonNetwork.Instantiate(monsterType, spawnPositions[sppos].position, Quaternion.identity);
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                else
                {
                    for (int i = 0; i < realinstcount; i++)
                    {
                        Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
                        PhotonNetwork.Instantiate(monsterType, randomSpawnPos.position, Quaternion.identity);
                        if (a != 1)
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.1f);
                        }
                    }
                }
            }
            else if (inst < Instantiatetime)
            {
                if (a == 2)
                {
                    int sppos = 0;
                    for (int sp = 0; sp < 15; sp++)
                    {
                        sppos += 1;
                        if (sppos > spawnPositions.Length - 1)
                        {
                            sppos = 0;
                        }
                        PhotonNetwork.Instantiate(monsterType, spawnPositions[sppos].position, Quaternion.identity);
                        yield return new WaitForSeconds(0.5f);
                    }
                }
                else
                {
                    for (int i = 0; i < 15; i++)
                    {
                        Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
                        PhotonNetwork.Instantiate(monsterType, randomSpawnPos.position, Quaternion.identity);
                        if (a != 1)
                        {
                            yield return new WaitForSeconds(0.5f);
                        }
                        else
                        {
                            yield return new WaitForSeconds(0.1f);
                        }
                    }
                }
            }
        }
        isSpawn = false;
        GameObject[] MonsterCount = GameObject.FindGameObjectsWithTag("Enemy");
        Debug.Log(MonsterCount.Length);
    }
    Transform[] GetSpawnPositions(int a, int b)
    {
        Transform[] spawnPositions = null;

        if (a == 1)
        {
            switch (b)
            {
                case 1: 
                    spawnPositions = GetChildTransforms(WaveEastFirstPos); 
                    break;
                case 2: 
                    spawnPositions = GetChildTransforms(WaveWestFirstPos); 
                    break;
                case 3: 
                    spawnPositions = GetChildTransforms(WaveSouthFirstPos); 
                    break;
                case 4: 
                    spawnPositions = GetChildTransforms(WaveNorthFirstPos); 
                    break;
            }
        }
        else if (a == 2)
        {
            spawnPositions = GetChildTransforms(WaveSecondPos);
        }
        else if (a == 3)
        {
            switch (b)
            {
                case 1: spawnPositions = GetChildTransforms(WaveThirdPos1); break;
                case 2: spawnPositions = GetChildTransforms(WaveThirdPos2); break;
                case 3: spawnPositions = GetChildTransforms(WaveThirdPos3); break;
                case 4: spawnPositions = GetChildTransforms(WaveThirdPos4); break;
            }
        }

        return spawnPositions;
    }

    private Transform[] GetChildTransforms(Transform parent)
    {
        List<Transform> childTransforms = new List<Transform>();
        foreach (Transform child in parent)
        {
            childTransforms.Add(child);
        }
        return childTransforms.ToArray();
    }

    public void OnClickQuit()
    {
        Application.Quit();
    }

    Coroutine shakeCoroutine;
    float[] shaketime = new float[2] { 0, 0 };
    [PunRPC]
    public void OnCameraShake(int _time)
    {
        if (shakeCoroutine != null)
        {
            if ((Time.time - shaketime[0]) - shaketime[1] < _time)
            {
                StopCoroutine(shakeCoroutine);
            }
        }
        shaketime[0] = Time.time;
        shaketime[1] = (float)_time;
        shakeCoroutine = StartCoroutine("CameraShake", _time);
    }

    IEnumerator CameraShake(int _time)
    {
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 3;
        yield return new WaitForSeconds(_time);
        virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
    }

    public virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isSpawn);
            stream.SendNext(WaveCount);
            stream.SendNext(LastWaveTime);
        }
        else
        {
            isSpawn = (bool)stream.ReceiveNext();
            WaveCount = (int)stream.ReceiveNext();
            LastWaveTime = (float)stream.ReceiveNext();
        }
    }
    public void OnClickUISfx()
    {
        SoundManager.Instance.PlayUISfxShot(0);
    }

    public IEnumerator FusionSkybox()
    {
        Material material = RenderSettings.skybox;
        RenderSettings.skybox = WLSkyBoxMat;
        yield return new WaitForSeconds(10);
        RenderSettings.skybox = material;
    }
}

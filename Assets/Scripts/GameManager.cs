using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
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
    private int WaveCount = 5;
    private bool isSpawn = false;
    //private bool QuitOn = false;

    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }
            return _instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        string prefabName = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedCharacter", out object character);
        if ((int)character == 0)
            prefabName = "Water";
        else if ((int)character == 3)
            prefabName = "Fire";
        PhotonNetwork.Instantiate(prefabName, SpawnPos.position, Quaternion.identity);
        Debug.Log("Wave Start at 5sec / TestMod");
        StartCoroutine(WaveStart());
        WaveCount += 1;
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (!QuitOn)
        //    {
        //        Quit.SetActive(true);
        //        QuitOn = true;
        //    }
        //    else
        //    {
        //        Quit.SetActive(false);
        //        QuitOn = false;
        //    }
        //}

    }

    public void CheckMonster()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && !isSpawn)
        {
            StartCoroutine(WaveStart());
            WaveCount += 1;
            Debug.Log("Wave Start at 5sec / TestMod");
        }
    }

    IEnumerator WaveStart()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            yield return new WaitForSeconds(5);
            isSpawn = true;

            string Firemonster = "TEMPMONSTER/Spirit of Fire";
            string Sleebam = "Monster/Sleebam";
            string Mugolin = "Monster/Mugolin";
            string Solborn = "Monster/Solborn";
            string Boss = "Boss";

            int a = Random.Range(1, 4);
            int b = Random.Range(1, 5);

            Transform[] spawnPositions = GetSpawnPositions(a, b);

            if (spawnPositions != null)
            {
                // 몬스터 인스턴스화 함수 호출
                if (WaveCount == 1)
                {
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Firemonster, 15));
                }
                else if (WaveCount == 2)
                {
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Firemonster, 15));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Sleebam, 10));
                }
                else if (WaveCount == 3)
                {
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Firemonster, 20));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Sleebam, 15));
                    //StartCoroutine(InstantiateMonsters(spawnPositions, a, Mugolin, 10));
                }
                else if (WaveCount == 4)
                {
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Firemonster, 25));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Sleebam, 15));
                    //StartCoroutine(InstantiateMonsters(spawnPositions, a, Mugolin, 10));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Solborn, 5));
                }
                else if (WaveCount == 5)
                {
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Firemonster, 30));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Sleebam, 25));
                    //StartCoroutine(InstantiateMonsters(spawnPositions, a, Mugolin, 15));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Solborn, 10));
                }
                else if (WaveCount == 6)
                {
                    Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
                    PhotonNetwork.Instantiate(Boss, randomSpawnPos.position, Quaternion.identity);
                }
            }
            isSpawn = false;
        }
        else
        {
            Debug.Log("you are not a master client");
        }
    }

    // Spawn 위치 결정
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
                case 1:
                    spawnPositions = GetChildTransforms(WaveThirdPos1);
                    break;
                case 2:
                    spawnPositions = GetChildTransforms(WaveThirdPos2);
                    break;
                case 3:
                    spawnPositions = GetChildTransforms(WaveThirdPos3);
                    break;
                case 4:
                    spawnPositions = GetChildTransforms(WaveThirdPos4);
                    break;
            }
        }

        return spawnPositions;
    }

    IEnumerator InstantiateMonsters(Transform[] spawnPositions, int a, string monsterType, int count)
    {
        Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];

        for (int i = 0; i < count; i++)
        {
            PhotonNetwork.Instantiate(monsterType, randomSpawnPos.position, Quaternion.identity);

            if (a != 1)
            {
                yield return new WaitForSeconds(0.5f);
            }
        }
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
}

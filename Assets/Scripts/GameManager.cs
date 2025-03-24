using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class GameManager : MonoBehaviourPun
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
    [SerializeField] private GameObject[] MonsterPrefab;
    [SerializeField] private int poolsize = 60;

    private Dictionary<string, List<GameObject>> monsterPools = new Dictionary<string, List<GameObject>>();
    private int WaveCount = 0;
    private bool isSpawn = false;

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
        foreach (GameObject prefab in MonsterPrefab)
        {
            string key = prefab.name;
            monsterPools[key] = new List<GameObject>();
            for (int i = 0; i < poolsize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                monsterPools[key].Add(obj);
            }
        }

        string prefabName = "";
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedCharacter", out object character);
        if ((int)character == 0)
            prefabName = "Water";
        else if ((int)character == 3)
            prefabName = "Fire";
        PhotonNetwork.Instantiate(prefabName, SpawnPos.position, Quaternion.identity);

        Debug.Log("Wave Start at 5sec / TestMod");
        WaveCount += 1;
        StartCoroutine(WaveStart());
    }

    void Update()
    {
    
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

            string Firemonster = "Spirit of Fire";
            string Sleebam = "Sleebam";
            string Mugolin = "Mugolin";
            string Solborn = "Solborn";
            string Boss = "Boss";

            int a = Random.Range(1, 4);
            int b = Random.Range(1, 5);

            Transform[] spawnPositions = GetSpawnPositions(a, b);

            if (spawnPositions != null)
            {
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
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Mugolin, 10));
                }
                else if (WaveCount == 4)
                {
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Firemonster, 25));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Sleebam, 15));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Mugolin, 10));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Solborn, 5));
                }
                else if (WaveCount == 5)
                {
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Firemonster, 30));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Sleebam, 25));
                    StartCoroutine(InstantiateMonsters(spawnPositions, a, Mugolin, 15));
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
            Debug.Log("You are not a master client");
        }
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

    IEnumerator InstantiateMonsters(Transform[] spawnPositions, int a, string monsterPrefabName, int count)
    {
        for (int i = 0; i < count; i++)
        {
            Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
            GameObject monster = GetPooledObject(monsterPrefabName);
            if (monster != null)
            {
                monster.transform.position = randomSpawnPos.position;
                monster.transform.rotation = Quaternion.identity;
                monster.SetActive(true);
            }
            if (a != 1)
            {
                yield return new WaitForSeconds(0.5f);
            }
            
        }
    }

    GameObject GetPooledObject(string prefabName)
    {
        if (monsterPools.ContainsKey(prefabName))
        {
            foreach (GameObject obj in monsterPools[prefabName])
            {
                if (!obj.activeInHierarchy)
                {
                    return obj;
                }
            }
            GameObject prefab = MonsterPrefab.FirstOrDefault(p => p.name == prefabName);
            if (prefab != null)
            {
                GameObject newObj = Instantiate(prefab);
                newObj.SetActive(false);
                monsterPools[prefabName].Add(newObj);
                return newObj;
            }
        }
        return null;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Security.Cryptography;

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
    private int WaveCount = 2;
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

            if (WaveCount == 1)
            {
                StartCoroutine(InstantiateMonsters(Firemonster, 120));
            }
            else if (WaveCount == 2)
            {
                //StartCoroutine(InstantiateMonsters(Solborn, 5));
                StartCoroutine(InstantiateMonsters(Firemonster, 150));
                StartCoroutine(InstantiateMonsters(Sleebam, 100));
            }
            else if (WaveCount == 3)
            {
                Transform randomSpawnPos = spawnPositions[Random.Range(0, spawnPositions.Length)];
                PhotonNetwork.Instantiate(Boss, randomSpawnPos.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.Log("you are not a master client");
        }
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
                    for (int sp = 0; sp < realinstcount; sp++)
                    {
                        PhotonNetwork.Instantiate(monsterType, spawnPositions[sp].position, Quaternion.identity);
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

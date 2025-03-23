using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

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
    private int WaveCount = 0;
    private bool isSpawn = false;
    private bool QuitOn = false;

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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!QuitOn)
            {
                Quit.SetActive(true);
                QuitOn = true;
            }
            else
            {
                Quit.SetActive(false);
                QuitOn = false;
            }
        }
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
            string TestBoss = "TestBoss";

            int a = Random.Range(1, 4);
            int b = Random.Range(1, 5);

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

            if (a == 3)
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

            if (spawnPositions != null)
            {
                foreach (var position in spawnPositions)
                {
                    PhotonNetwork.Instantiate(Firemonster, position.position, Quaternion.identity);
                    yield return new WaitForSeconds(1);
                }
            }

            isSpawn = false;
        }
        else
        {
            Debug.Log("you are not a matser cl");
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

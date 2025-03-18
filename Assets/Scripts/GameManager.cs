using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform SpawnPos;
    [SerializeField]
    private Transform SpawnPos2;
    [SerializeField]
    private GameObject Quit;
    private bool QuitOn = false;
    // Start is called before the first frame update
    void Start()
    {
        string prefabName = "Water";
        PhotonNetwork.Instantiate(prefabName, SpawnPos.position, Quaternion.identity);
        string prefabName2 = "TEMPMONSTER/Spirit of Fire";
        PhotonNetwork.Instantiate(prefabName2, SpawnPos2.position, Quaternion.identity);
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

    public void OnClickQuit()
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManagement : MonoBehaviourPunCallbacks
{
    [SerializeField] TextMeshProUGUI TutorialText;
    [SerializeField] GameObject LightningCharacter;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [SerializeField] GameObject herepoint;
    [SerializeField] GameObject FirstPoint;
    [SerializeField] GameObject SecondPoint;
    [SerializeField] GameObject ThirdPoint;
    [SerializeField] Transform SpawnPos;
    [SerializeField] FusionSkill fusion;
    public int curTutorialProcess;
    bool canTouch = false;
    PlayerController playerS;
    SaveManager SM;

    private static TutorialManagement _instance;
    public static TutorialManagement Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("TutorialManager").GetComponent<TutorialManagement>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        //PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("selectedCharacter", out object character);
        SM = GameObject.Find("SaveManager").GetComponent<SaveManager>();
        PhotonNetwork.Instantiate("Fire", SpawnPos.position, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        curTutorialProcess = 1;
        TutorialProcess(curTutorialProcess);
        playerS = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        // LookAtTarget
    }
    private void Update()
    {
        if (canTouch && Input.GetMouseButtonDown(0))
        {
            canTouch = false;
            NextProcess();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 대충 스킵 할거냐는 메시지
        }
        if (curTutorialProcess == 20 && Input.GetKeyDown(KeyCode.R))
        {
            NextProcess();
        }
        if (curTutorialProcess == 21 && fusion.fusionSkillCoolTime >= 0)
        {
            NextProcess();
        }
    }
    private void TutorialProcess(int TutorialProcessCode)
    {
        //if (isFadeOut) return;
        curTutorialProcess = TutorialProcessCode;
        Color color = TutorialText.color;
        color.a = 1f;
        TutorialText.color = color;
        if (PlayerPrefs.GetInt("Language") == 0)
        {
            if (TutorialProcessCode == 1)
            {
                TutorialText.text = "기본적인 튜토리얼입니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 2)
            {
                TutorialText.text = "튜토리얼에선 게임의 조작법과\n맵의 목표에 대해서 설명하겠습니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 3)
            {
                TutorialText.text = "간단한 이동부터 시작하겠습니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 4)
            {
                TutorialText.text = "마우스 오른쪽 클릭으로 이동할 수 있습니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 5)
            {
                TutorialText.text = "스페이스바로 대쉬 할 수 있습니다..";
                canTouch = true;
            }
            else if (TutorialProcessCode == 6)
            {
                TutorialText.text = "지정된 곳까지 이동해보세요";
                herepoint.SetActive(true);
                herepoint.transform.position = FirstPoint.transform.position;
                playerS.photonView.RPC("LookAtTarget", RpcTarget.All, FirstPoint.name, 3f);
            }
            else if (TutorialProcessCode == 7)
            {
                TutorialText.text = "잘하셨습니다!";
                canTouch = true;
            }
            else if (TutorialProcessCode == 8)
            {
                TutorialText.text = "이제 공격을 배워보겠습니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 9)
            {
                TutorialText.text = "QWE를 사용하여 기본 스킬을 사용할 수 있습니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 10)
            {
                TutorialText.text = "Q는 기본 공격입니다.\n적에게 사용하여 처치해보세요!";
                PhotonNetwork.Instantiate("Monster/Stage1/Solborn", SecondPoint.transform.position, Quaternion.identity);
                playerS.photonView.RPC("LookAtTarget", RpcTarget.All, SecondPoint.name, 3f);
            }
            else if (TutorialProcessCode == 11)
            {
                TutorialText.text = "잘하셨습니다!";
                canTouch = true;
            }
            else if (TutorialProcessCode == 12)
            {
                TutorialText.text = "W와 E는 스킬입니다.\n해골이 계속 나오는 무덤을 파괴해보세요!!";
                PhotonNetwork.Instantiate("Monster/Stage1/Grave", SecondPoint.transform.position, Quaternion.identity);
                playerS.photonView.RPC("LookAtTarget", RpcTarget.All, SecondPoint.name, 3f);
                // 적 소환
            }
            else if (TutorialProcessCode == 13)
            {
                TutorialText.text = "잘하셨습니다!";
                canTouch = true;
            }
            else if (TutorialProcessCode == 14)
            {
                TutorialText.text = "각 맵마다 수행해야하는 목표가 다릅니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 15)
            {
                TutorialText.text = "해당 맵에선 가운데의 수정을 보호하는 기믹입니다.\n수정은 맵의 정가운데에 있습니다.";
                GameObject Crystal = GameObject.Find("Crystal");
                playerS.photonView.RPC("LookAtTarget", RpcTarget.All, Crystal.name, 3f);
                canTouch = true;
            }
            else if (TutorialProcessCode == 16)
            {
                //StopCoroutine(LookAtTarget(Pltransform, 0f));
                //virtualCamera.LookAt = Pltransform.transform;
                TutorialText.text = "수정의 체력은 미니맵의 하단에 있습니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 17)
            {
                TutorialText.text = "이제 게임의 메인 기믹인 합동기에 대해서 배워보겠습니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 18)
            {
                TutorialText.text = "캐릭터마다 기본적인 원소가 존재하며\n적절한 원소를 배합하여 합동기를 사용할 수 있습니다.";
                Instantiate(LightningCharacter, ThirdPoint.transform.position, Quaternion.identity);
                canTouch = true;
            }
            else if (TutorialProcessCode == 19)
            {
                TutorialText.text = "합동기는 캐릭터의 스킬을 연계하여\n강력한 공격을 할 수 있는 기술입니다.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 20)
            {
                TutorialText.text = "R을 사용 하여 원소를 등록할 수 있습니다.\n불 원소를 등록해보세요!";
                fusion.pv.RPC("ElementalSettingMaster", RpcTarget.MasterClient, 1);
            }
            else if (TutorialProcessCode == 21)
            {
                TutorialText.text = "R키를 키다운 하여 합동기를 사용할 수 있습니다.\n이제 합동기를 사용해보세요!";
            }
            else if (TutorialProcessCode == 22)
            {
                TutorialText.text = "축하합니다! 튜토리얼을 완료하셨습니다!\n이제 본격적으로 게임을 즐겨보세요!";
                canTouch = true;
            }
            else if (TutorialProcessCode == 23)
            {
                GoToMain();
            }
            else
            {
                TutorialText.text = "알 수 없는 오류입니다.";
                TutorialProcess(1);

            }
        }
        else if (PlayerPrefs.GetInt("Language") == 1)
        {
            if (TutorialProcessCode == 1)
            {
                TutorialText.text = "This is the basic tutorial.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 2)
            {
                TutorialText.text = "In this tutorial, we will explain the game controls\nand the objectives of the map.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 3)
            {
                TutorialText.text = "Let's start with simple movement.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 4)
            {
                TutorialText.text = "You can move by right-clicking the mouse.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 5)
            {
                TutorialText.text = "You can dash using the spacebar.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 6)
            {
                TutorialText.text = "Try moving to the designated spot.";
                herepoint.SetActive(true);
                herepoint.transform.position = FirstPoint.transform.position;
                playerS.photonView.RPC("LookAtTarget", RpcTarget.All, FirstPoint.name, 3f);
            }
            else if (TutorialProcessCode == 7)
            {
                TutorialText.text = "Well done!";
                canTouch = true;
            }
            else if (TutorialProcessCode == 8)
            {
                TutorialText.text = "Now, let's learn how to attack.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 9)
            {
                TutorialText.text = "You can use basic skills with Q, W, and E.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 10)
            {
                TutorialText.text = "Q is the basic attack.\nUse it to defeat enemies!";
                PhotonNetwork.Instantiate("Monster/Stage1/Solborn", SecondPoint.transform.position, Quaternion.identity);
                playerS.photonView.RPC("LookAtTarget", RpcTarget.All, SecondPoint.name, 3f);
            }
            else if (TutorialProcessCode == 11)
            {
                TutorialText.text = "Well done!";
                canTouch = true;
            }
            else if (TutorialProcessCode == 12)
            {
                TutorialText.text = "W and E are skills.\nTry destroying the tombs that continuously spawn skeletons!";
                PhotonNetwork.Instantiate("Monster/Stage1/Grave", SecondPoint.transform.position, Quaternion.identity);
                playerS.photonView.RPC("LookAtTarget", RpcTarget.All, SecondPoint.name, 3f);
            }
            else if (TutorialProcessCode == 13)
            {
                TutorialText.text = "Well done!";
                canTouch = true;
            }
            else if (TutorialProcessCode == 14)
            {
                TutorialText.text = "Each map has different objectives to complete.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 15)
            {
                TutorialText.text = "In this map, you must protect the crystal in the center.\nThe crystal is located at the very center of the map.";
                GameObject Crystal = GameObject.Find("Crystal");
                playerS.photonView.RPC("LookAtTarget", RpcTarget.All, Crystal.name, 3f);
                canTouch = true;
            }
            else if (TutorialProcessCode == 16)
            {
                TutorialText.text = "The crystal's health is shown at the bottom of the minimap.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 17)
            {
                TutorialText.text = "Now, let's learn about the main game mechanic: fusion skills.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 18)
            {
                TutorialText.text = "Each character has a basic element.\nYou can combine elements to use fusion skills.";
                Instantiate(LightningCharacter, ThirdPoint.transform.position, Quaternion.identity);
                canTouch = true;
            }
            else if (TutorialProcessCode == 19)
            {
                TutorialText.text = "Fusion skills are powerful attacks created by linking character skills.";
                canTouch = true;
            }
            else if (TutorialProcessCode == 20)
            {
                TutorialText.text = "Press R to register an element.\nTry registering the fire element!";
                fusion.pv.RPC("ElementalSettingMaster", RpcTarget.MasterClient, 1);
            }
            else if (TutorialProcessCode == 21)
            {
                TutorialText.text = "Hold down the R key to use fusion skills.\nNow, try using a fusion skill!";
            }
            else if (TutorialProcessCode == 22)
            {
                TutorialText.text = "Congratulations! You have completed the tutorial!\nEnjoy the game!";
                canTouch = true;
            }
            else if (TutorialProcessCode == 23)
            {
                GoToMain();
            }
            else
            {
                TutorialText.text = "An unknown error occurred.";
                TutorialProcess(1);
            }
        }


        //StartCoroutine(FadeOutText());
    }
    public void GoToMain()
    {
        ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", null },
            { "selectedCharacter", -1 }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(properties);

        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main"); 
    }
    public void CheckMonster()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            NextProcess();
        }
    }
    public void NextProcess()
    {
        TutorialProcess(curTutorialProcess += 1);
    }
}

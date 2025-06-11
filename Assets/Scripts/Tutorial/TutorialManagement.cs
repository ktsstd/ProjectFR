using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cinemachine;

public class TutorialManagement : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TutorialText;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    int curTutorialProcess;
    bool canTouch = false;
    GameObject Pltransform;
    // Start is called before the first frame update
    void Start()
    {
        curTutorialProcess = 10;
        TutorialProcess(curTutorialProcess);
        // LookAtTarget
    }
    private void Update()
    {
        if (canTouch && Input.GetMouseButtonDown(0))
        {
            NextProcess();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // 대충 스킵 할거냐는 메시지
        }
    }
    private void TutorialProcess(int TutorialProcessCode)
    {
        //if (isFadeOut) return;
        curTutorialProcess = TutorialProcessCode;
        Color color = TutorialText.color;
        color.a = 1f;
        TutorialText.color = color;

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
            TutorialText.text = "지정된 곳까지 이동해보세요";
            Pltransform = virtualCamera.LookAt.gameObject;
            // 위치 만들기
            //LookAtTarget(오브젝트, 3f);
        }
        else if (TutorialProcessCode == 6)
        {
            TutorialText.text = "잘하셨습니다!";
            canTouch = true;
        }
        else if (TutorialProcessCode == 7)
        {             
            TutorialText.text = "이제 공격을 배워보겠습니다.";
            canTouch = true;
        }
        else if (TutorialProcessCode == 8)
        {
            TutorialText.text = "QWE를 사용하여 기본 스킬을 사용할 수 있습니다.";
            canTouch = true;
        }
        else if (TutorialProcessCode == 9)
        {
            TutorialText.text = "Q는 기본 공격입니다.\n적에게 사용하여 처치해보세요!";
            // 적 소환
            //LookAtTarget(적, 3f);
        }
        else if (TutorialProcessCode == 10)
        {
            TutorialText.text = "잘하셨습니다!";
            canTouch = true;
        }
        else if (TutorialProcessCode == 11)
        {
            TutorialText.text = "W와 E는 스킬입니다.\n적에게 사용하여 처치해보세요!";
            // 적 소환
        }
        else if (TutorialProcessCode == 12)
        {
            TutorialText.text = "잘하셨습니다!";
            canTouch = true;
        }
        else if (TutorialProcessCode == 13)
        {
            TutorialText.text = "각 맵마다 수행해야하는 목표가 다릅니다.";
            canTouch = true;
        }
        else if (TutorialProcessCode == 14)
        {
            TutorialText.text = "해당 맵에선 가운데의 수정을 보호하는 기믹입니다.\n수정은 맵의 정가운데에 있습니다.";
            GameObject Crystal = GameObject.Find("Crystal");
            LookAtTarget(Crystal, 99999f);
            canTouch = true;
        }
        else if (TutorialProcessCode == 15)
        {
            StopCoroutine(LookAtTarget(Pltransform, 0f));
            virtualCamera.LookAt = Pltransform.transform;
            TutorialText.text = "수정의 체력은 미니맵의 하단에 있습니다.";
            canTouch = true;
        }
        else if (TutorialProcessCode == 16)
        {
            TutorialText.text = "몬스터들로 부터 수정을 지키세요!";
            // 적 소환
            // LookAtTarget(적, 3f);
        }
        else if (TutorialProcessCode == 17)
        {
            TutorialText.text = "잘하셨습니다!";
        }
        else if (TutorialProcessCode == 18)
        {
            TutorialText.text = "이제 게임의 메인 기믹인 합동기에 대해서 배워보겠습니다.";
            canTouch = true;
        }
        else if (TutorialProcessCode == 19)
        {
            TutorialText.text = "캐릭터마다 기본적인 원소가 존재하며\n적절한 원소를 배합하여 합동기를 사용할 수 있습니다.";
            // 캐릭터 보여주기
        }
        else if (TutorialProcessCode == 20)
        {
            TutorialText.text = "합동기는 캐릭터의 스킬을 연계하여\n강력한 공격을 할 수 있는 기술입니다.";
            canTouch = true;
        }
        else if (TutorialProcessCode == 21)
        {
            TutorialText.text = "R키를 키다운 하여 원소를 등록할 수 있습니다.\n불 원소를 등록해보세요!";
        }
        else
        {
            TutorialText.text = "알 수 없는 오류입니다.";
            TutorialProcess(1);
        }
        //StartCoroutine(FadeOutText());
    }

    //bool isFadeOut = false;
    //private IEnumerator FadeOutText()
    //{
    //    isFadeOut = true;
    //    Color color = TutorialText.color;
    //    while (color.a > 0f)
    //    {
    //        color.a -= Time.deltaTime / 2f;
    //        TutorialText.color = color;
    //        yield return null;
    //    }
    //    isFadeOut = false;
    //}
    private IEnumerator LookAtTarget(GameObject obj, float _time)
    { 
        virtualCamera.LookAt = obj.transform;
        yield return new WaitForSeconds(_time);
        virtualCamera.LookAt = Pltransform.transform;
    }
    private void NextProcess()
    {
        TutorialProcess(curTutorialProcess += 1);
        canTouch = false;
    }
}

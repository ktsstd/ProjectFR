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
    // Start is called before the first frame update
    void Start()
    {
        TutorialProcess(1);
        // LookAtTarget
    }
    private void Update()
    {
        if (canTouch && Input.)
        {

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
            TutorialText.text = "";
        else if (TutorialProcessCode == 3)
            TutorialText.text = "";
        else
            TutorialText.text = "알 수 없는 오류입니다.";

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
        GameObject Pltransform = virtualCamera.LookAt.gameObject;
        virtualCamera.LookAt = obj.transform;
        yield return new WaitForSeconds(_time);
        virtualCamera.LookAt = Pltransform.transform;
    }
    private void NextProcess()
    {
        TutorialProcess(curTutorialProcess += 1);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Photon.Pun;

public class BossSkill1 : MonoBehaviour
{
    private PlayableDirector playableDirector;
    public GameObject Skill1Obj;
    private Animator animator;

    void Start()
    {
        Skill1Obj = transform.parent.gameObject;
        playableDirector.Play();
    }
    void OnEnable()
    {
        playableDirector = GetComponent<PlayableDirector>();
        playableDirector.stopped += OnTimelineStopped;
    }

    void OnDisable()
    {
        playableDirector.stopped -= OnTimelineStopped;
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        Destroy(Skill1Obj);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimeLineStart : MonoBehaviour
{
    public PlayableDirector playableDirector;
    void Start()
    {
        playableDirector.Play();
        Invoke("SelfDestroy", 2f);
    }

    void SelfDestroy()
    {
        Destroy(gameObject);
    }
}

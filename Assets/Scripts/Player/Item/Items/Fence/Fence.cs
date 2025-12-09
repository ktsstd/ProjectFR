using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fence : Obstacle
{
    public override void Start()
    {
        SoundManager.Instance.PlayItemSfx(1, transform.position);

        StartSetting(3000f, 60f);
        base.Start();
    }
}

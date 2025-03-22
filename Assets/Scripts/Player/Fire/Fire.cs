using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : PlayerController
{
    bool isFlameSpray = false;

    public override void RunAnimation()
    {
        if (!isFlameSpray)
            animator.SetBool("isRun", isMoving);
        else
            animator.SetBool("isRun_2", isMoving);

    }
}

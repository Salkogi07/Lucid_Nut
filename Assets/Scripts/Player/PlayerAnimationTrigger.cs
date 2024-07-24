using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    private Player player => GetComponentInChildren<Player>();

    private void AnimationTrigger()
    {
        player.AnimationTrigger();
    }
}

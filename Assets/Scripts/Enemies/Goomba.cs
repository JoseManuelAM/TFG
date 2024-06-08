using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : Enemy
{

    protected override void Awake()
    {
        base.Awake();
    }
    //Sobreescritura del método cuando el Goomba es pisado por el jugador
    public override void Stomped(Transform player)
    {
        AudioManager.Instance.PlayStomp();
        animator.SetTrigger("Hit");
        gameObject.layer = LayerMask.NameToLayer("OnlyGround");
        Destroy(gameObject, 1f);
        autoMovement.PauseMovement();
        Dead();
    }
}

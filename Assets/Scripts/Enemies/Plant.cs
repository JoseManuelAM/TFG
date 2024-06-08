using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : Enemy
{
    //La planta muere al ser golpeada por un proyectil del jugador, si tiene equipado la estrella o si la golpea un caparazón
    public override void HitFireball()
    {
        Dead();
        Destroy(transform.parent.gameObject);
    }
    public override void HitStarman()
    {
        Dead();
        Destroy(transform.parent.gameObject);
    }
    public override void HitRollingShell()
    {
        Dead();
        Destroy(transform.parent.gameObject);
    }
}

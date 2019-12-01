using System.Collections;
using System.Collections.Generic;
using AI;
using UnityEngine;

public class BigEnemy : BasicEnemy
{
    public override void InitEnemy()
    {
        EnemyType = EnemyType.Big;
    }
}

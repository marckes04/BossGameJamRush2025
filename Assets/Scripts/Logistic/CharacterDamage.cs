using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamage : MonoBehaviour
{
    public LayerMask collisionLayer;
    public float radius = 1f;
    public float damage = 2f;

    public bool isPlayer, isEnemy, isBoss;

    public GameObject hitFx;


    // Update is called once per frame
    void Update()
    {
        DetectionCollision();
    }

    void DetectionCollision()
    {

    }
}

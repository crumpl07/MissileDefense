using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public GameObject ExplosionEffect;
    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.name == "Missile(Clone)")
        {
            Destroy(collision.gameObject);
            Instantiate(ExplosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}

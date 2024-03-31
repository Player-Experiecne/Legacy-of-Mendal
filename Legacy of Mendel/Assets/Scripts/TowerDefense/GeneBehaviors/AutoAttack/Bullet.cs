using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject attackTarget;
    public float speed = 20f;

    public float instantDamage;


    void FixedUpdate()
    {
        if (attackTarget == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.LookAt(attackTarget.transform);
        transform.position += transform.forward * (speed * Time.deltaTime);
    }

    public void HitTarget()
    {
        DealInstantDamage(attackTarget);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject == attackTarget)
        {
            HitTarget();
            Destroy(gameObject);
        }
    }

        private void DealInstantDamage(GameObject target)
    {
        HP targetHP = target.GetComponent<HP>();
        if (targetHP != null)
        {
            targetHP.TakeDamage(instantDamage);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters;
using UnityEngine;

public class TrunkAttack : EnemyBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePoint;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float playerdistance;
     private float timer;
    [SerializeField] private float cooldownTime;
    private bool canattack = false;

    private bool isplayerdetected = false;

    protected override void Awake()
    {
       base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        isplayerdetected = Physics2D.Raycast(this.transform.position,
            Vector2.right * facingDirection, playerdistance
            , playerLayer);

        if (isplayerdetected)
            Attack();
        

    }


    private void Attack()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            anim.SetTrigger("TrunkAttack"); // fires once, resets automatically
            timer = cooldownTime;
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = isplayerdetected ? Color.green : Color.red;
        Gizmos.DrawLine(this.transform.position, new Vector2(this.transform.position.x + (playerdistance * facingDirection), this.transform.position.y));
    }

     public void Shoot()
    {
        Debug.Log("Shoot");
       GameObject Bullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
        Bullet.GetComponent<BulletBehaviour>().SetDirection(facingDirection);

    }

}

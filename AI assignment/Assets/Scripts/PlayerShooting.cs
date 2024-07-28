//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class PlayerShooting : MonoBehaviour
//{
//    public GameObject bulletPrefab;
//    public GameObject bulletPoint;
//    public GameObject ourCamera;

//    public AttributeManager PlayerATM;

//    public EnemyFSM enemy; // Reference to the enemy script

//    //public Transform bulletSpawnPoint; // Where the bullet will spawn
//    public float bulletSpeed;

//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            ShootBullet();
//        }
//    }

//    void ShootBullet()
//    {
//        Debug.Log("shooting");


//        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, transform.rotation);
//        bullet.GetComponent<Rigidbody>().AddForce(ourCamera.transform.forward * bulletSpeed);
//        BulletShooting bulletScript = bullet.GetComponent<BulletShooting>();

//        if (enemy != null)
//        {
//            enemy.SetBulletInitialPosition(bulletScript.initialPosition);
//        }

//        Destroy(bullet, 2);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject bulletPoint;
    public GameObject ourCamera;
    public AttributeManager PlayerATM;
    public EnemyFSM enemy; // Reference to the enemy script
    public float bulletSpeed;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        Debug.Log("player shooting");

        GameObject bullet = Instantiate(bulletPrefab, bulletPoint.transform.position, transform.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(ourCamera.transform.forward * bulletSpeed);

        if (enemy == null)
        {
            Debug.LogWarning("Enemy reference is not set!");
        }

        if (enemy != null)
        {
            enemy.SetBulletInitialPosition(bullet.transform.position);
            enemy.hasDetectedBullet = false;
        }

        Destroy(bullet, 2);
    }
}



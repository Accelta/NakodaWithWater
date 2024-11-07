using UnityEngine;

public class EnemyCannon : MonoBehaviour
{
    public Transform[] cannonMainBodies;
    public Transform[] cannonBarrels;
    public Transform[] firePoints;
    public GameObject bulletPrefab;

    private float fireRate;
    private float bulletForce;
    private float nextFireTime;

    public void SetCannonStats(float fireRate, float bulletForce)
    {
        this.fireRate = fireRate;
        this.bulletForce = bulletForce;
    }

    public void RotateToward(Transform target)
    {
        for (int i = 0; i < cannonMainBodies.Length; i++)
        {
            Vector3 directionToPlayer = (target.position - cannonMainBodies[i].position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            cannonMainBodies[i].rotation = Quaternion.Slerp(cannonMainBodies[i].rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    public void TryFire()
    {
        if (Time.time >= nextFireTime)
        {
            foreach (var firePoint in firePoints)
                FireBullet(firePoint);
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    private void FireBullet(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class Tir : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform fireStartingPoint;
    public Camera cam;
    public float speed = 60f;

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Quaternion direction = Quaternion.Euler(cam.transform.eulerAngles.x, transform.eulerAngles.y, 0);
            GameObject bullet = Instantiate(bulletPrefab, fireStartingPoint.position, direction * bulletPrefab.transform.rotation);
            bullet.GetComponentInChildren<Rigidbody>().AddForce(direction * Vector3.forward * speed, ForceMode.VelocityChange);
            Destroy(bullet, 3f);
        }
    }
}

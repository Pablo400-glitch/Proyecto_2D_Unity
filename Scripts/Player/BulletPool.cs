using UnityEngine;

public class BulletPool : ObjectPool
{
    public int maxBullets = 20; // Max Number of bullets
    private int currentBullets; // Available bullets

    public delegate void UpdateAmmo(int amount);
    public event UpdateAmmo onUpdateAmmo;
    private PlayerController player;

    protected override void Start()
    {
        base.Start();
        currentBullets = maxBullets; // Initialize bullets
    }

    public override GameObject GetObject()
    {
        // If there are no bullets available return null
        if (currentBullets <= 0)
        {
            Debug.Log("Sin balas disponibles.");
            return null; 
        }

        GameObject bullet = base.GetObject(); // Get an available bullet
        currentBullets--; // Reduce the number of available bullets

        InvokeAmmoEvent(); 

        ResetBullet(bullet);

        return bullet;
    }

    public void Reload(int amount)
    {
        currentBullets = Mathf.Clamp(currentBullets + amount, 0, maxBullets);
        InvokeAmmoEvent(); 
    }

    public int GetAvailableBullets()
    {
        return currentBullets;
    }

    // Reset the bullet position and velocity
    private void ResetBullet(GameObject bullet)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f; // Reset Angular velocity
        }

        bullet.transform.position = Vector3.zero;
        bullet.transform.rotation = Quaternion.identity;
    }

    // Here I invoke the event to update the ammo on the UI
    public void InvokeAmmoEvent()
    {
        if (onUpdateAmmo != null)
        {
            onUpdateAmmo(currentBullets);
        }
    }
}

using UnityEngine;

public class BulletPool : ObjectPool
{
    public int maxBullets = 20; // Máxima cantidad de balas en el inventario
    private int currentBullets; // Balas disponibles

    public delegate void UpdateAmmo(int amount);
    public event UpdateAmmo onUpdateAmmo;
    private PlayerController player;

    protected override void Start()
    {
        base.Start();
        currentBullets = maxBullets; // Inicializamos las balas disponibles
    }

    public override GameObject GetObject()
    {
        if (currentBullets <= 0)
        {
            Debug.Log("Sin balas disponibles.");
            return null; // No permitir disparar si no hay balas
        }

        GameObject bullet = base.GetObject(); // Obtener una bala del pool
        currentBullets--; // Reducir del inventario
        
        InvokeAmmoEvent(); // Invocar evento de actualización de balas

        ResetBullet(bullet); // Reiniciar propiedades de la bala

        return bullet;
    }

    public void Reload(int amount)
    {
        currentBullets = Mathf.Clamp(currentBullets + amount, 0, maxBullets); // Recargar balas
        InvokeAmmoEvent(); // Invocar evento de actualización de balas
    }

    public int GetAvailableBullets()
    {
        return currentBullets;
    }

    private void ResetBullet(GameObject bullet)
    {
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = Vector2.zero; // Reiniciar velocidad
            rb.angularVelocity = 0f; // Reiniciar rotación angular
        }

        bullet.transform.position = Vector3.zero; // Reiniciar posición
        bullet.transform.rotation = Quaternion.identity; // Reiniciar rotación
    }

    public void InvokeAmmoEvent()
    {
        if (onUpdateAmmo != null)
        {
            onUpdateAmmo(currentBullets);
        }
    }
}

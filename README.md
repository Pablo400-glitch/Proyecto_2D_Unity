# 2D Project - FDV

## Introduction

This project is a 2D game developed in Unity for the subject `Fundamentos del Desarrollo de Videojuegos`. 

The game is a 2D platformer similar to the classic Metroid games, where the player has to explore the map and defeat enemies to progress. The player has an armor similar to Samus Aran, and can shoot projectiles to defeat enemies. The player can also jump to avoid enemies and obstacles.

## Entities

For the entities of the game, I have the following:

### Player

The player is the main character of the game, and has the following properties:

- Can move left and right
- Can jump
- Can duck
- Can shoot projectiles in all directions
- Has a health bar
- Has a ammo bar

You can find the controllers for the player in the `/Assets/Scripts/Player` folder. The player has the following scripts:

- `PlayerController`: This script controls the movement of the player, and the player's actions like jumping, ducking and shooting.

- `PlayerHealth`: This script controls the health of the player, and the player's death.

![Player Sprite](/images/Player.png)

*Figure 1: Player Sprite*

### Pickups

For the game I created the pickups that the player can collect to restore health and ammo. The pickups are the following:

#### Health Pickup

The health pickup is an orb that the player can collect to restore health. This orb has the tag `Heal`, and the player can collect it by walking over it. The logic of the health pickup is in the `PlayerHealth` script, where the player's health is restored when the player collides with the orb.

```csharp
## PlayerHealth.cs
private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Heal"))
    {
        if (currentHealth == playerHealth)
        {
            player.InvokeMessage("Ya tienes la vida al máximo");
            return;
        }
        player.InvokeMessage("Te has curado");
        Heal();
        Destroy(other.gameObject);
    }
}

private void Heal()
{
    ModifyHealth(1);
}

private void ModifyHealth(int amount)
{
    currentHealth += amount;
    currentHealth = Mathf.Clamp(currentHealth, 0, playerHealth);

    if (onUpdateHealth != null)
    {
        onUpdateHealth(currentHealth);
    }
    onUpdateHealth.Invoke(currentHealth);
}
```

![Health Pickup Sprite](/images/Heal_Orb.png)

*Figure 2: Health Pickup Sprite*

#### Ammo Pickup

The ammo pickup is an orb that the player can collect to restore ammo. This orb has the tag `Orb`, and the player can collect it by walking over it. The logic of the ammo pickup is in the `PlayerController` script, where the player's ammo is restored when the player collides with the orb.

```csharp
## PlayerController.cs

private void OnTriggerEnter2D(Collider2D other)
{
    BulletPool pool = FindObjectOfType<BulletPool>();
    if (other.gameObject.CompareTag("Orb"))
    {
        if (pool.GetAvailableBullets() == pool.maxBullets)
        {
            InvokeMessage("Cargador Completo");
            return;
        }
        pool.Reload(5);
        Destroy(other.gameObject);
        InvokeMessage("Balas Recargadas");
    }
}
```

![Ammo Pickup Sprite](/images/Ammo_Orb.png)

*Figure 3: Ammo Pickup Sprite*

### Enemies

For the game I created the enemies that the player has to defeat to progress through the level. The enemies have the following properties:

#### Crab

The crab is the first enemy of the game, and has the following properties:

- Can move left and right
- Can damage the player on contact

You can find the controllers for the crab in the `/Assets/Scripts/Enemies` folder. The crab has the following scripts:

- `CrabMovement`: This script controls the movement of the crab, and the crab's actions like moving left and right.

- `EnemyHealth`: This script controls the health of the enemy, and the enemies death.

![Crab Sprite](/images/Crab.png)

*Figure 4: Crab Sprite*

#### Octopus

The octopus is the second enemy of the game, and has the following properties:

- Can Jump every `2.5 seconds`
- Can damage the player on contact

You can find the controllers for the octopus in the `/Assets/Scripts/Enemies` folder. The octopus has the following scripts:

- `OctopusMovement`: This script has the jump logic for the octopus

- `EnemyHealth`: This script controls the health of the enemy, and the enemies death

![Octopus Sprite](/images/Octopus.png)

*Figure 5: Octopus Sprite*

#### Jumper

The jumper is the third enemy of the game, and has the following properties:

- Can check if the player is in range of `2.5 units`
- Can move left or right, following the player if the player is in range
- Can jump every `2.5 seconds` if the player is in range
- Can damage the player on contact

You can find the controllers for the jumper in the `/Assets/Scripts/Enemies` folder. The jumper has the following scripts:

- `JumperMovement`: This script controls the movement of the jumper, and the jumper's actions like jumping and following the player.

- `EnemyHealth`: This script controls the health of the enemy, and the enemies death.

![Jumper Sprite](/images/Jumper.png)

*Figure 6: Jumper Sprite*

## Controls

The player can control the character with the following keys:

- `A`: Move left
- `D`: Move right
- `Space`: Jump
- `S`: Duck
- `W`: Look up
- `V`: Activate Shooting Mode
- `E`: Shoot While in Shooting Mode

The main functions for the player controller are the following:

```csharp
## PlayerController.cs

private void Movement()
{
    float moveH = Input.GetAxis("Horizontal");

    if (Input.GetButton("Jump") && !isJumping)
    {
        rb2D.AddForce(transform.up * thrust, ForceMode2D.Impulse);
        isJumping = true;
        animator.SetBool("Jump", true);
    }

    if (Input.GetKey(KeyCode.S))
    {
        animator.SetBool("IsDucking", true);
        StopPlayer();
        ChangeSpriteOrientation(moveH);
        return;
    }
    else
    {
        animator.SetBool("IsDucking", false);
    }

    if (Input.GetKey(KeyCode.W) && !isJumping)
    {
        animator.SetBool("ShootUp", true);
        StopPlayer();
        ChangeSpriteOrientation(moveH);
        return;
    }
    else
    {
        animator.SetBool("ShootUp", false);
    }

    if (Mathf.Abs(moveH) > 0.01f)
    {
        Vector2 velocity = rb2D.velocity;
        velocity.x = moveH * speed;
        rb2D.velocity = velocity;

        animator.SetBool("Walk", true);
    }
    else
    {
        StopPlayer();
        animator.SetBool("Walk", false);
    }

    ChangeSpriteOrientation(moveH);
}

private void FireBullet()
{
    if (Input.GetKey(KeyCode.V)) {
        animator.SetBool("isShooting", true);
        if (Input.GetKeyDown(KeyCode.E) && Time.time >= nextFireTime) {
            GameObject bullet = BulletSpawner();

            if (bullet == null)
            {
                Debug.LogError("El objeto no tiene un componente Rigidbody2D.");
                return;
            }
            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            if (!spriteRenderer.flipX)
                rb.velocity = transform.right * bulletSpeed; 
            else
                rb.velocity = -transform.right * bulletSpeed; 

            if (Input.GetKey(KeyCode.W) && !isJumping)
                rb.velocity = transform.up * bulletSpeed; 

            nextFireTime = Time.time + fireRate;   
        }
    } else {
        animator.SetBool("Stand", false);
        animator.SetBool("isShooting", false);
    }         
}

```

## Map

The map is a 2D platformer map, where the player has to progress through the level to get to the end. The map has the following properties:

- Is divided in two main sections
- Has a start point
- Has a end point
- Has enemies
- Has obstacles
- Has health and ammo pickups

The map is divided in two main sections, the first section is the start of the level, where the player has to defeat some of the enemies and get to the door to the next section. The second section is the end of the level, where the player has to defeat some other enemies and get to the end of the level.

![Map First Part](/images/Map_First_Part.png)

*Figure 7: Map First Part*

![Map Second Part](/images/Map_Second_Part.png)

*Figure 8: Map Second Part*

### Elements

The map has the following elements:

- `Health Pickup`: The health pickup is an orb with animation that the player can pick up to restore health.

- `Ammo Pickup`: The ammo pickup is an orb that the player can pick up to restore ammo.

- `Enemies`: The enemies are the main obstacles of the game, and the player has to defeat them to progress through the level.

- `Obstacles`: The obstacles like platforms and walls are the main elements of the map, and the player has to avoid them to progress through the level.

## Physics

The player and enemies have a `Rigidbody2D` component set to **dynamic**, allowing them to interact with the game's physics. Additionally, the player has a `BoxCollider2D` to detect collisions with enemies and obstacles. Similarly, enemies have a `BoxCollider2D` to collide with the player and obstacles.  

On the other hand, bullets have a `Rigidbody2D` set to **kinematic**, meaning they are not affected by the game's physics but can still detect and trigger collisions.

I also added a **trap** to briefly stop the player upon entering the second part of the level. This is done using a `Distance Joint` (Blue Block), which activates when the player collides with a specific collider (Orange Block). The joint remains active until the player breaks the trap, allowing them to continue.

![Trap](/images/Distance_Joint.png)

*Figure 9: Stop Player Trap*

## UI

The game has a UI that displays the player's health and ammo, the objetives and events of the game. The UI has the following elements:

- `Health Bar`: The health bar displays the player's health, and is updated when the player collects health pickups or takes damage.

- `Ammo Bar`: The ammo bar displays the player's ammo, and is updated when the player collects ammo pickups or shoots projectiles.

- `Objectives`: The objectives display the current objectives of the game, and are updated when the player completes an objective.

- `Events`: The events display the current events of the game, and are updated when the player completes an event. For example, when the player collects a health pickup, the event is updated to display the message "You have healed".

![UI](/images/UI.png)

*Figure 10: UI*

## Various

### Pooling

To add bullets to the game, I used a `Bullet Pool` to manage the bullets. The bullet pool is a list of bullets that the player can shoot, and the bullets are reused when the player shoots. This allows the player to shoot multiple bullets without creating new bullets every time. 

The pool has a maximum number of bullets that the player can shoot, and the bullets are reused when the player shoots. The bullets are created when the player shoots, and return to the pool when the bullet collides with an enemy, obstacle or gets to certain distance. When the limit is reached the player can't shoot anymore.

Before creating the class `BulletPool`, I created a class called `ObjectPool` to manage the objects in the pool. Is a generic class that can be used to create pools of any object. 

### Events

For the events I used the delegate pattern to create events that the player can subscribe to. The events are used to update the UI when the player collects health or ammo pickups, and to display messages when the player completes an objective or event.

In the class `EventManager` is where I process the events and the messages that the player can receive. For example, when the player collects a health pickup, the event is updated to change the health bar. 

First I declare the delegate and the event in the `PlayerHealth` class:

```csharp
## PlayerHealth.cs

public int playerHealth = 3;
public delegate void UpdateHealth(int amount);
public event UpdateHealth onUpdateHealth;
private int currentHealth;

private void ModifyHealth(int amount)
{
    currentHealth += amount;
    currentHealth = Mathf.Clamp(currentHealth, 0, playerHealth);

    if (onUpdateHealth != null)
    {
        onUpdateHealth(currentHealth);
    }
    onUpdateHealth.Invoke(currentHealth);
}

....

private void Heal()
{
    ModifyHealth(1);
}

....

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.gameObject.CompareTag("Heal"))
    {
        if (currentHealth == playerHealth)
        {
            player.InvokeMessage("Ya tienes la vida al máximo");
            return;
        }
        player.InvokeMessage("Te has curado");
        Heal();
        Destroy(other.gameObject);
    }
}
```

Later I subscribe to the event in the `EventManager` class:

```csharp
## EventManager.cs
public GameObject player;
public Slider healthSlider;

private PlayerHealth playerHealth;

void Start()
{
    playerHealth = player.GetComponent<PlayerHealth>();
    playerHealth.onUpdateHealth += UpdateHealthCounter;
}

void UpdateHealthCounter(int Health)
{
    healthSlider.value = Health;
}

....

void OnDestroy()
{
    playerHealth.onUpdateHealth -= UpdateHealthCounter;
}
```

### Cinemachine Effects

To add effects to the game, I used the `Cinemachine` package to create a camera that follows the player, to make the game more dynamic. The camera follows the player when the player moves, zoom out when I want to let the player see more of the map and move the camera to some parts of the map to show the player what has changed in the level.

In my game I used the `Cinemachine Virtual Camera` to create the camera that follows the player, and the `Cinemachine Confiner` to limit the camera to the map. 

When the player defeats all the enemies from the first part of the level, the camera moves to the door to the second part of the level, to show the player where to go next. 

> This is shown in the `GameManager` script.

Later on when the player reaches the second part of the level, the camera zooms out to show the player the new obstacles and enemies that the player has to defeat to get to the end of the level. 

> This is shown in the `CameraNewPart` script.

(add gif showing this effects)

### Background

For the background I used the parallax effect to create the illusion of depth in the game. The background has two layers, the first layer is the background and the second layer is the foreground. The background moves slower than the foreground, creating the illusion of depth.

With the script `ParallaxEffect` I can control the speed of the background and the foreground, and the distance between the layers. The background and foreground move at different speeds, creating the parallax effect and they are constantly moving to create the illusion of depth. 

The speed of the background and foreground can be controlled with the `baseSpeed` and `speedOffset` variables.

```csharp
public class ParalaxEffect : MonoBehaviour
{
    public Vector2 baseSpeed = new Vector2(0.5f, 0.0f);
    private Material[] Layers;
    public float speedOffset = 0.1f;

    void Start()
    {
        Layers = GetComponent<Renderer>().materials;
    }

    void Update()
    {
        for (int i = 0; i < Layers.Length; i++)
        {
            Material m = Layers[i];
            Vector2 newOffset = m.GetTextureOffset("_MainTex") + baseSpeed * (speedOffset / (i + 1.0f)) * Time.deltaTime;
            m.SetTextureOffset("_MainTex", newOffset);
        }
    }
}
```

(add gif showing this effect)

## Scripts

### Player

[PlayerController.cs](Scripts/Player/PlayerController.cs)

### Enemies

### Others
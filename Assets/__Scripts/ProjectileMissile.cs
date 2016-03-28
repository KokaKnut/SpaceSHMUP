using UnityEngine;
using System.Collections;

public class ProjectileMissile : Projectile {

    public float speed = 1f;
    public Vector3 targetOffset;
    public Vector3 targetRotationSpeed;
    public float targetingFactor;

    [SerializeField]
    private Enemy enemyTarget;

    void Awake()
    {
        // Test to see whether this has passed off screen every 2 seconds
        InvokeRepeating("CheckOffscreen", 2f, 2f);
        speed = Main.GetWeaponDefinition(type).velocity;
        gameObject.GetComponentInChildren<SpriteRenderer>().transform.Rotate(new Vector3(0,0,Random.value * 360));
    }

    void Update()
    {
        if (enemyTarget != null)
        {
            SpriteRenderer sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
            sprite.enabled = true;
            sprite.transform.position = enemyTarget.transform.position + targetOffset;
            sprite.transform.Rotate(targetRotationSpeed);
        }
        else
        {
            gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }

    void FixedUpdate()
    {
        if (enemyTarget != null)
        {
            transform.rotation.SetFromToRotation(Vector3.zero, transform.position - enemyTarget.transform.position);
            GetComponent<Rigidbody>().AddForce((enemyTarget.transform.position - transform.position).normalized * speed);
        }
        else
        {
            FindTarget();
        }
    }

    void FindTarget()
    {
        GameObject[] enemies =  GameObject.FindGameObjectsWithTag("Enemy");
        Vector3 closest = new Vector3(0, 500, 0);
        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy thisEnemy = enemies[i].GetComponent<Enemy>();
            thisEnemy.bounds.center = thisEnemy.transform.position + thisEnemy.boundsCenterOffset;
            Vector3 modifiedVector = transform.position - thisEnemy.bounds.center;

            modifiedVector.y *= targetingFactor;
            if (modifiedVector.magnitude < closest.magnitude)
            {
                if (!(thisEnemy.bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(thisEnemy.bounds, BoundsTest.offScreen) != Vector3.zero))
                {
                    closest = enemies[i].transform.position;
                    enemyTarget = thisEnemy;
                }
            }
        }
    }

    public override void SetType(WeaponType eType)
    {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(type);
        GetComponentInChildren<ParticleSystemRenderer>().material.color = def.projectileColor;
        Renderer[] rednerers = GetComponentsInChildren<Renderer>();
        foreach (var item in rednerers)
        {
            item.material.color = def.projectileColor;
        }
    }

    void CheckOffscreen()
    {
        if (Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class ProjectileMissile : Projectile {

    public float speed = 1f;
    
    [SerializeField]
    private Enemy target;

    void Awake()
    {
        // Test to see whether this has passed off screen every 2 seconds
        InvokeRepeating("CheckOffscreen", 2f, 2f);
        FindTarget();
        speed = Main.GetWeaponDefinition(type).velocity;
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            transform.rotation.SetLookRotation(transform.position - target.transform.position);
            GetComponent<Rigidbody>().AddForce((target.transform.position - transform.position).normalized * speed);
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
            if ((transform.position - thisEnemy.bounds.center).magnitude < closest.magnitude)
            {
                if (!(thisEnemy.bounds.extents == Vector3.zero || Utils.ScreenBoundsCheck(thisEnemy.bounds, BoundsTest.offScreen) != Vector3.zero))
                {
                    closest = enemies[i].transform.position;
                    target = thisEnemy;
                }
            }
        }
    }

    public override void SetType(WeaponType eType)
    {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(type);
        GetComponent<Renderer>().material.color = def.projectileColor;
        GetComponentInChildren<ParticleSystemRenderer>().material.color = def.projectileColor;
        GetComponentInChildren<Renderer>().material.color = def.projectileColor;
    }

    void CheckOffscreen()
    {
        if (Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}

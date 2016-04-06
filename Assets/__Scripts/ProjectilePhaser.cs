using UnityEngine;
using System.Collections;

public class ProjectilePhaser : Projectile {

    public float wiggleWidth = 0f;
    public float wiggleFreq = 0f;

    private float offset = 0f;

    void Awake()
    {
        // Test to see whether this has passed off screen every 2 seconds
        InvokeRepeating("CheckOffscreen", 2f, 2f);
        offset = Random.value; //* 2 * Mathf.PI;
    }

    void FixedUpdate()
    {
        transform.position += new Vector3(Mathf.Sin(Time.fixedTime * (wiggleFreq + offset)) / wiggleWidth,0f,0f);
    }

    public override void SetType(WeaponType eType)
    {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(type);
        GetComponent<Renderer>().material.color = def.projectileColor;
        GetComponent<TrailRenderer>().material.color = def.projectileColor;
    }

    void CheckOffscreen()
    {
        if (Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}

using UnityEngine;
using System.Collections;

public class ProjectileMissile : MonoBehaviour {

    public float speed = 1f;

    [SerializeField]
    private WeaponType _type;
    private Enemy target;

    // This public property masks the field _type & takes action when it is set
    public WeaponType type
    {
        get
        {
            return (_type);
        }
        set
        {
            SetType(value);
        }
    }

    void Awake()
    {
        // Test to see whether this has passed off screen every 2 seconds
        InvokeRepeating("CheckOffscreen", 2f, 2f);
        FindTarget();
    }

    void Update()
    {
        if (target != null)
        {
            transform.rotation.SetLookRotation(transform.position - target.transform.position);
            transform.position = (transform.position - target.transform.position).normalized * Time.deltaTime * speed;
        }
    }

    void FindTarget()
    {

    }

    public void SetType(WeaponType eType)
    {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        GetComponent<Renderer>().material.color = def.projectileColor;
    }

    void CheckOffscreen()
    {
        if (Utils.ScreenBoundsCheck(GetComponent<Collider>().bounds, BoundsTest.offScreen) != Vector3.zero)
        {
            Destroy(this.gameObject);
        }
    }
}

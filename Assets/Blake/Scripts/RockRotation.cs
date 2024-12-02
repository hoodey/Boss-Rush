using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRotation : MonoBehaviour
{
    Rigidbody rb;
    float xRotate;
    float zRotate;
    Vector3 rotation;
    float lifeTimer = 0.0f;
    float lifeTime;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        lifeTime = Random.Range(3.0f, 5.0f);
        xRotate = Random.Range(-50, 50);
        zRotate = Random.Range(-50, 50);
        rotation = new Vector3(xRotate, 0, zRotate);
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer > lifeTime)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            rotation = new Vector3(0, 0, 0);
        }
    }

    private void FixedUpdate()
    {
        rb.AddTorque(rotation);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitLogic : MonoBehaviour
{

    [SerializeField] GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        KnockBackOnly();
    }

    public void KnockBackOnly()
    {
        Damage damage = new Damage();
        damage.amount = 2;
        damage.direction = new Vector3(Random.Range(-10f,10f), 10f, Random.Range(-10f,10f));
        damage.knockbackForce = 5f;

        player.GetComponent<Damageable>().Hit(damage);
    }
}

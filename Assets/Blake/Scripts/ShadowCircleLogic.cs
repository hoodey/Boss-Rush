using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowCircleLogic : MonoBehaviour
{
    float lifeTime = 2.5f;
    float lifeTimer = 0.0f;
    bool changeMat = false;
    [SerializeField] Material mat;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale *= 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer < lifeTime)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, (transform.localScale / 2f), 0.005f);
        }
        else if (lifeTimer >= lifeTime && !changeMat)
        {
            gameObject.GetComponent<MeshRenderer>().material = mat;
            changeMat = true;
        }
        else if (lifeTimer >= lifeTime + 2f)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}

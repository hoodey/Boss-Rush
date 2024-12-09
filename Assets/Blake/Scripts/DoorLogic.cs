using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] float finalHeight;
    Vector3 finalPos;
    bool doorOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        finalPos = new Vector3(door.transform.position.x, finalHeight, door.transform.position.z);

    }

    // Update is called once per frame
    void Update()
    {
        if (doorOpen && door.transform.position != finalPos)
        {
            OpenDoor();
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Player")
        {
            doorOpen = true;
        }
    }

    void OpenDoor()
    {

        door.transform.position = Vector3.Lerp(door.transform.position, finalPos, 0.01f);
    }
}

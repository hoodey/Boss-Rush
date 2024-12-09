using ElToro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class DoorLogic : MonoBehaviour
    {
        [SerializeField] GameObject door;
        [SerializeField] float finalHeight;
        [SerializeField] int doorNum;
        Vector3 finalPos;
        bool doorOpen = false;
        [SerializeField] BossLogic BL;

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
            if (doorNum == 2)
            {
                BL.myStateMachine.ChangeState(new IdleState(BL.myStateMachine, BL));
            }
        }

        void OpenDoor()
        {

            door.transform.position = Vector3.Lerp(door.transform.position, finalPos, 0.01f);
        }
    }
}


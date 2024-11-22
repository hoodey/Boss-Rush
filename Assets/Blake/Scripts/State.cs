using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class State
    {
        public StateMachine machine;
        public float elapsedTime;

        public State(StateMachine machine)
        {
            this.machine = machine;
            elapsedTime = 0;
        }

        public virtual void OnEnter()
        {

        }

        public virtual void OnUpdate()
        {
            elapsedTime += Time.deltaTime;
        }

        public virtual void OnExit()
        {

        }
    }

}

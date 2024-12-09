using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class StateMachine
    {
        public State currentState;

        public StateMachine()
        {

        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.OnUpdate();
            }
        }

        public void ChangeState(State newState)
        {
            if (currentState != null)
                currentState.OnExit();

            currentState = newState;

            newState.OnEnter();

        }
    }
}
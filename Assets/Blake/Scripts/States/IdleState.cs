using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace ElToro
{
    public class IdleState : State
    {
        public BossLogic BL;

        public IdleState(StateMachine m, BossLogic BL) : base(m) 
        {
            machine = m;
            this.BL = BL;
        }

        public override void OnEnter()
        {
            Debug.Log("Entered Idle");
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            if (elapsedTime >= 2.0f)
            {
                machine.ChangeState(new PatrolState(machine, BL));
            }
        }

        public override void OnExit()
        {
            Debug.Log("Exited Idle");
            base.OnExit();
        }
    }
}
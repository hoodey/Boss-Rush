using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class DeathState : State
    {
        public BossLogic BL;


        public DeathState(StateMachine m, BossLogic BL) : base(m)
        {
            machine = m;
            this.BL = BL;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            BL.agent.ResetPath();
            BL.agent.velocity = new Vector3(0f, 0f, 0f);
            BL.anim.SetTrigger("died");
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void OnExit()
        {
            base.OnExit();
        }

    }
}

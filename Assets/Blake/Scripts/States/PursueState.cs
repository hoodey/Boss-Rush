using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class PursueState : State
    {
        public BossLogic BL;
        public PursueState(StateMachine m, BossLogic BL) : base(m)
        {
            this.BL = BL;
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();
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

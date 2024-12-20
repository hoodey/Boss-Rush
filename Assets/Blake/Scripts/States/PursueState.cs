using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ElToro
{
    public class PursueState : State
    {
        public BossLogic BL;
        public float deAggroTimer;
        public PursueState(StateMachine m, BossLogic BL) : base(m)
        {
            this.BL = BL;
            machine = m;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            
            BL.agent.SetDestination(BL.player.position);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            if (BL.agent.destination != BL.player.position)
            {
                BL.agent.SetDestination(BL.player.position);
            }
            if (BL.PlayerInSight)
            {
                deAggroTimer = 0;
            }

            if (!BL.PlayerInSight)
            {
                deAggroTimer += Time.deltaTime;
            }

            if (deAggroTimer > 3f)
            {
                machine.ChangeState(new IdleState(machine, BL));
            }
        }

        public override void OnExit()
        {
            base.OnExit();
            
        }
    }
}

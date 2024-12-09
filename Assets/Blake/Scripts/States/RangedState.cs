using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ElToro
{
    public class RangedState : State
    {
        public BossLogic BL;
        public float deAggroTimer = 0.0f;
        public float attackCD;
        public float attackTimer = 0.0f;

        public RangedState(StateMachine m, BossLogic BL) : base(m)
        {
            machine = m;
            this.BL = BL;
            attackCD = BL.rangedAttackCD;
        }

        public override void OnEnter()
        {
            PerformRangedAttack();
            base.OnEnter();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //Increase our attack timer each frame
            attackTimer += Time.deltaTime;

            //Evaluate if player is close enough to pursue, in view long enough to do another ranged attack, or out of view
            //If close enough to pursue range, switch to pursue
            if (BL.PlayerInSight && Vector3.Distance(BL.player.position, BL.transform.position) <= BL.meleePursueRange)
            {
                machine.ChangeState(new PursueState(machine, BL));
            }
            //if in view until CD is over
            else if (BL.PlayerInSight && attackTimer >= attackCD)
            {
                PerformRangedAttack();
                attackTimer = 0.0f;
            }
            else if (BL.PlayerInSight)
            {
                deAggroTimer = 0f;
            }
            //out of view / deaggro
            else if (!BL.PlayerInSight)
            {
                deAggroTimer += Time.deltaTime;
                if (deAggroTimer >= attackCD)
                {
                    machine.ChangeState(new PatrolState(machine, BL));
                }
            }
            else
            {
                //Debug.Log("Chillin'");
            }
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        public void PerformRangedAttack()
        {
            //Stop patrol movement
            BL.agent.ResetPath();
            //Turn to player
            var dirToPlayer = (BL.player.transform.position - BL.transform.position).normalized;
            dirToPlayer.y = 0;
            BL.transform.forward = dirToPlayer;
            //Cue audio file of a grunt
            SoundEffectsManager.instance.PlayAudioClip(BL.rangedRoar, true);
            SoundEffectsManager.instance.PlayAudioClip(BL.rocks, true);
            //ranged attack animation (with function trigger)
            BL.anim.SetTrigger("rangedAttack");
            BL.transform.forward = dirToPlayer;
        }
    }
}

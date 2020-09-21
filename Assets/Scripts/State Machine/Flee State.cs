using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace State_Machine
{
    public class FleeState : State
    {
        private Vector3 _destination;
        public FleeState(StateController stateController) : base(stateController)
        {

        }
        public override void CheckTransitions()
        {
            if (!StateController.CheckIfInRange())
            {
                StateController.ResetPatrolPoints();
                StateController.SetState(new PatrolState(StateController));
            }

            if (!(Vector3.Distance(StateController.transform.position, StateController.enemyToChase.transform.position) > StateController.tagRadius))
            {
                StateController.SetState(new ChaseState(StateController));
            }
        }
        public override void Act()
        {
            _destination = StateController.getFlee();
            StateController.ai.SetTarget(_destination);
        }

        public override void OnStateEnter()
        {
            StateController.flee = true;
            
            _destination = StateController.getChased();
            if(StateController.ai.agent != null)
            {
                StateController.ai.agent.speed = 1.5f;
            }
            StateController.ai.SetTarget(_destination);
            StateController.ChangeColor(Color.red);
        }

        public override void OnStateExit()
        {
            StateController.flee = false;
        }
    }
}

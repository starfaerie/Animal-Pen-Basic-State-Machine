using UnityEngine;

namespace State_Machine
{
    public class PatrolState : State
    {
        private Transform _destination;

        public PatrolState(StateController stateController) : base(stateController)
        {

        }

        public override void CheckTransitions()
        {
            if (StateController.CheckIfInRange())
            {
                StateController.SetState(new ChaseState(StateController));
            }
        }
        public override void Act()
        {
            _destination = StateController.GetNextNavPoint();
            StateController.ai.SetTarget(_destination);
        }

        public override void OnStateEnter()
        {
            _destination = StateController.GetNextNavPoint();
            if(StateController.ai.agent != null)
            {
                StateController.ai.agent.speed = 1f;
            }
            StateController.ai.SetTarget(_destination);
            StateController.ChangeColor(Color.blue);
        }
    }
}

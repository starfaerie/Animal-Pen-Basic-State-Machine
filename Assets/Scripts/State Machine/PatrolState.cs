using UnityEngine;

namespace State_Machine
{
    public class PatrolState : State
    {
        private Vector3 _destination;

        public PatrolState(StateController stateController) : base(stateController)
        {

        }

        public override void CheckTransitions()
        {
            if (StateController.CheckIfInRange())
            {
                StateController.SetState(new FleeState(StateController));
            }
        }
        public override void Act()
        {
            if (Vector3.Distance(StateController.transform.position, _destination) > StateController.tagRadius) return;
            _destination = StateController.GetNextNavPoint();
            StateController.ai.SetTarget(_destination);
        }

        public override void OnStateEnter()
        {
            _destination = StateController.GetNextNavPoint();
            if(StateController.ai.agent != null)
            {
                StateController.ai.agent.speed = 0.7f;
            }
            StateController.ai.SetTarget(_destination);
            StateController.ChangeColor(Color.blue);
        }
    }
}

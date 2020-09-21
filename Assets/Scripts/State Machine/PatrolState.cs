using UnityEngine;

namespace State_Machine
{
    public class PatrolState : State
    {
        private Vector3 _destination;
        private float _range = 5f;

        public PatrolState(StateController stateController) : base(stateController)
        {

        }

        public override void CheckTransitions()
        {
            //if the player is in range
            if (StateController.CheckIfInRange(_range))
            {
                //move to the flee state
                StateController.SetState(new FleeState(StateController));
            }
        }
        public override void Act()
        {
            //if the current _destination is within 2 meters
            if (!(Vector3.Distance(StateController.gameObject.transform.position, _destination) < 2f)) return;
            Debug.Log(("moving to next"));
            _destination = StateController.GetNextNavPoint();
            StateController.ai.SetTarget(_destination);
            
            Debug.Log(_destination);
        }

        private void GoToNextPoint()
        {
            _destination = StateController.GetNextNavPoint();
            if(StateController.ai.agent != null)
            {
                StateController.ai.agent.speed = 1f;
            }
            StateController.ai.SetTarget(_destination);
            StateController.ChangeColor(Color.blue);
        }

        public override void OnStateEnter()
        {
            GoToNextPoint();
        }
    }
}

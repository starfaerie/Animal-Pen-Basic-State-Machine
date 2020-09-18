using UnityEngine;

namespace State_Machine
{
    
    [RequireComponent(typeof(UnityStandardAssets.Characters.ThirdPerson.AICharacterControl))]
    public class StateController : MonoBehaviour
    {
        //public GameObject[] randPoints; //random points the AI travels to when fleeing
        public GameObject[] patrolPoints; //patrol points of this AI
        public GameObject[] enemies; //enemies of this AI
        public float detectionRadius = 5f; //detection radius of this AI

        [HideInInspector]
        public UnityStandardAssets.Characters.ThirdPerson.AICharacterControl ai;
        [HideInInspector]
        public Renderer[] childrenRend;
        [HideInInspector]
        public GameObject enemyToChase; //gameObject that this AI is currently set to "chase"
        [HideInInspector]
        public State currentState;
        private int _patrolPointNum;
        //private int _randPoint;

        public Transform GetNextNavPoint()
        {
            _patrolPointNum = (_patrolPointNum + 1) % patrolPoints.Length; //iterates through all patrol points, moving to the next point each time this method is called, automagically loops back to the first point when it runs out of points
            return patrolPoints[_patrolPointNum].transform; //returns the position of the next patrol point
        }
        public Transform getChased()
        {
            _patrolPointNum = (_patrolPointNum + 1) % patrolPoints.Length; //iterates through all patrol points, moving to the next point each time this method is called, automagically loops back to the first point when it runs out of points
            return patrolPoints[_patrolPointNum].transform; //returns the position of the next patrol point
        }

        /*public Transform GetNextRandPoint()
        {
            _randPoint = transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * speed);
            return randPoints[_randPoint].transform;
        */

        public void ChangeColor(Color color)
        {
            foreach(Renderer r in childrenRend)
            {
                foreach(Material m in r.materials)
                {
                    m.color = color;
                }
            }
        }

        public bool CheckIfInRange()
        {
            if (enemies == null) return false;
        
            foreach (GameObject go in enemies)
            {
                if (!(Vector3.Distance(go.transform.position, transform.position) < detectionRadius)) continue;
            
                enemyToChase = go;
                return true;
            }
            return false;
        }

        private void AssignDefaults()
        {
            ai = GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>();
            childrenRend = GetComponentsInChildren<Renderer>();

            SetState(new PatrolState(this)); //sets the first state to the PatrolState
        }

        private void Start()
        {
            AssignDefaults();
        }

        private void Update()
        {
            currentState.CheckTransitions();
            currentState.Act();
        }
        public void SetState(State state)
        {
            currentState?.OnStateExit();
            currentState = state;
            gameObject.name = "AI agent in state" + state.GetType().Name;
            currentState?.OnStateEnter();
        }
    }
}

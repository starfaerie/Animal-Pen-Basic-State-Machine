using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace State_Machine
{
    
    [RequireComponent(typeof(UnityStandardAssets.Characters.ThirdPerson.AICharacterControl))]
    public class StateController : MonoBehaviour
    {
        [HideInInspector]
        public Vector3[] patrolPoints; //patrol points of this AI
        public GameObject[] enemies; //enemies of this AI
        public float detectionRadius = 8f; //detection radius of this AI
        public float tagRadius = 2f;

        [HideInInspector]
        public UnityStandardAssets.Characters.ThirdPerson.AICharacterControl ai;
        [HideInInspector]
        public Renderer[] childrenRend;
        [HideInInspector]
        public GameObject enemyToChase; //gameObject that this AI is currently set to "chase"
        [HideInInspector]
        public State currentState;
        private int _patrolPointNum;
        [HideInInspector]
        public bool flee;

        public Vector3 GetNextNavPoint()
        {
            _patrolPointNum = _patrolPointNum = (_patrolPointNum + 1) % patrolPoints.Length; //iterates through all patrol points, moving to the next point each time this method is called, automagically loops back to the first point when it runs out of points
            return patrolPoints[_patrolPointNum]; //returns the position of the next patrol point
        }
        public Vector3 getChased()
        {
            _patrolPointNum = (_patrolPointNum + 1) % patrolPoints.Length; //iterates through all patrol points, moving to the next point each time this method is called, automagically loops back to the first point when it runs out of points
            return patrolPoints[_patrolPointNum]; //returns the position of the next patrol point
        }

        public Vector3 getFlee()
        {
            Vector3 currentPos = gameObject.transform.position;
            Vector3 fleeDirection = (currentPos - enemyToChase.transform.position).normalized;
            return currentPos + fleeDirection * 3f;
        }

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

            if (flee == false)
            {
                foreach (GameObject go in enemies)
                {
                    if (!(Vector3.Distance(go.transform.position, transform.position) < detectionRadius)) continue;

                    enemyToChase = go;
                    return true;
                }
                return false;
            }
            
            foreach (GameObject go in enemies)
            {
                if (!(Vector3.Distance(go.transform.position, transform.position) < detectionRadius * 1.5f)) continue;

                enemyToChase = go;
                return true;
            }
            return false;
        }

        private void AssignDefaults()
        {
            ai = GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>();
            childrenRend = GetComponentsInChildren<Renderer>();
            enemyToChase = enemies[0].gameObject;

            ResetPatrolPoints();

            SetState(new PatrolState(this)); //sets the first state to the PatrolState
        }

        public void ResetPatrolPoints()
        {
            //get 4 random numbers in a given range
            float rand0 = Random.Range(-10f, 10f);
            float rand1 = Random.Range(-10f, 0f) + Mathf.Abs(rand0);
            float rand2 = Random.Range(0f, 10f) + Mathf.Abs(rand1);
            float rand3 = Random.Range(1f, 4f);
            
            //get the AI and enemy's position
            Vector3 currentPos = transform.position;
            Vector3 enemyPos = enemyToChase.transform.position;
            Vector3 enemyDir = (enemyPos - currentPos).normalized * 3f;

            //assign 9 new patrol points
            patrolPoints = new[]
            {
                new Vector3(rand0, 0, rand0) - enemyDir * rand3,
                new Vector3(rand0, 0, rand1) - enemyDir * rand3,
                new Vector3(rand0, 0, rand2) - enemyDir * rand3,
                new Vector3(rand1, 0, rand0) - enemyDir * rand3,
                new Vector3(rand1, 0, rand1) - enemyDir * rand3,
                new Vector3(rand1, 0, rand2) - enemyDir * rand3,
                new Vector3(rand2, 0, rand0) - enemyDir * rand3,
                new Vector3(rand2, 0, rand1) - enemyDir * rand3,
                new Vector3(rand2, 0, rand2) - enemyDir * rand3
            };
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

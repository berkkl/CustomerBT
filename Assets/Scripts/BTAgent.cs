using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BTAgent : MonoBehaviour
{
	public BehaviorTree tree;
	public NavMeshAgent agent;

	public enum ActionState { IDLE, WORKING }
	public ActionState _state = ActionState.IDLE;

	WaitForSeconds waitForSecdonds;

	public Node.Status treeStatus = Node.Status.RUNNING;
	public void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		tree = new BehaviorTree();
		waitForSecdonds = new WaitForSeconds(UnityEngine.Random.Range(0.1f, 0.8f));
		StartCoroutine(Behave());
	}

	IEnumerator Behave()
	{
		while (true)
		{
			treeStatus = tree.Process();
			yield return waitForSecdonds;
		}
	}

	public Node.Status GoToDestination(Vector3 destination, float distance = 3)
	{
		float distanceToTarget = Vector3.Distance(destination, this.transform.position);
		if (_state == ActionState.IDLE)
		{
			Debug.Log($"{this.name} has started moving to {destination}");
			_state = ActionState.WORKING;
			agent.SetDestination(destination);
			return Node.Status.RUNNING;
		}
		else if (Vector3.Distance(agent.pathEndPosition, destination) >= distance)
		{
			Debug.Log($"Status Failed. NPC Can't reach the destination, difference is: {Vector3.Distance(agent.pathEndPosition, destination)}");
			_state = ActionState.IDLE;
			return Node.Status.FAILURE;
		}
		else if (distanceToTarget < distance)
		{
			Debug.Log($"Status Success. NPC Reached to target. Now IDLE.");
			_state = ActionState.IDLE;
			return Node.Status.SUCCESS;
		}

		return Node.Status.RUNNING;
	}

	public BehaviorTree GetBehaviorTree()
	{
		return tree;
	}
}
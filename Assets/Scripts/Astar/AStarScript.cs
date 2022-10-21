using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AStarScript : MonoBehaviour {

	public FollowAStarScript followScript; // new ref
	public int princessNo; // new var

	public bool check = true;

	public GridScript gridScript;
	public HueristicScript hueristic;

	protected int gridWidth;
	protected int gridHeight;

	GameObject[,] pos;

	protected Vector3 start;
	protected Vector3 goal;

	public Path path;

	protected PriorityQueue<Vector3> frontier;
	protected Dictionary<Vector3, Vector3> cameFrom = new Dictionary<Vector3, Vector3>();
	protected Dictionary<Vector3, float> costSoFar = new Dictionary<Vector3, float>();
	protected Vector3 current;


	//Start() broken into these functions to be called from the GameManager at different times
	public virtual void InitAstar(){
		cameFrom = new Dictionary<Vector3, Vector3>();
		costSoFar = new Dictionary<Vector3, float>();
		CalculatePath(new Path(hueristic.gameObject.name, gridScript));
		followScript.InitPath();
	}
	public void StartRace() {
		followScript.StartMove();
	}


	protected virtual void CalculatePath(Path path){
		this.path = path;

		start = gridScript.start[princessNo - 1]; // start vec now pulled from array indexed from princess number
		goal = gridScript.goal;
		
		gridWidth = gridScript.gridWidth;
		gridHeight = gridScript.gridHeight;

		pos = gridScript.GetGrid();

		frontier = new PriorityQueue<Vector3>();
		frontier.Enqueue(start, 0);

		cameFrom.Add(start, start);
		costSoFar.Add(start, 0);

		int exploredNodes = 0;

		while(frontier.Count != 0){
			exploredNodes++;
			current = frontier.Dequeue();			

			if(current.Equals(goal))
				break;		
			
			for(int x = -1; x < 2; x+=2)
				AddNodesToFrontier((int)current.x + x, (int)current.y);			
			for(int y = -1; y < 2; y+=2)
				AddNodesToFrontier((int)current.x, (int)current.y + y);			
		}

		current = goal;

		while(!current.Equals(start)){
			GameObject go = pos[(int)current.x, (int)current.y];
			path.Insert(0, go, new Vector3((int)current.x, (int)current.y));

			current = cameFrom[current];
		}

		path.Insert(0, pos[(int)current.x, (int)current.y]);
		path.nodeInspected = exploredNodes;
	}

	void AddNodesToFrontier(int x, int y){
		if(x >=0 && x < gridWidth && 
		   y >=0 && y < gridHeight)
		{
			Vector3 next = new Vector3(x, y);
			float new_cost = costSoFar[current] + gridScript.GetMovementCost(pos[x, y]);
			if(!costSoFar.ContainsKey(next) || new_cost < costSoFar[next]) { 
				//rearranged assignment to reference dictionaries in heuristic
				costSoFar[next] = new_cost;
				cameFrom[next] = current;

				float priority = new_cost + Heuristic(x, y); // heuristic now local to AStarScript
				frontier.Enqueue(next, priority);

			}
		}
	}

	// idk, i'm not feeling the heuristic, most optimal route makes sense for this game and the calculation expense is negligible
	// I gave it another shot now that I could write it in here, but I decided against it after this point
	float Heuristic(int x, int y) {
		float penalty = 0;
		Vector3 coord = new Vector3(x, y);

		// add to penalty if coord is further from goal than previous coord
		if (Mathf.Abs(goal.x - x) > Mathf.Abs(goal.x - cameFrom[coord].x)) 
			penalty += gridScript.GetMovementCost(pos[x, y]) / 2;
		if (Mathf.Abs(goal.y - y) > Mathf.Abs(goal.y - cameFrom[coord].y)) 
			penalty += gridScript.GetMovementCost(pos[x, y]) / 2;
		
		//return penalty;

		// return 0 for the win
		return 0;
	}
}

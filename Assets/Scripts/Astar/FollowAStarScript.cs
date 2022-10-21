using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowAStarScript : MonoBehaviour {

	public GameManager gameManager; // new ref

	protected bool move = false;

	protected Path path;
	public AStarScript astar;
	public Step startPos;
	public Step destPos;

	protected int currentStep = 1;

	protected float lerpPer = 0;

	public Color lineColor; // new var
	int lineIndex; // new var
	LineRenderer line; // new type

	//Start() broken into these functions to be called from the GameManager at different times
	public void InitPath() {
		path = astar.path;
		startPos = path.Get(0);
		destPos = path.Get(currentStep);
		lineIndex = 0;
		transform.position = startPos.gameObject.transform.position;

		//fixed the LineRenderer from drawing default pink
		line = GetComponent<LineRenderer>();
		line.startColor = lineColor; line.endColor = lineColor;

		line.positionCount = 1;

		// fixed line not drawing first node
		Vector3 vec = transform.position;
		vec.z = -1;
		line.SetPosition(lineIndex, vec);
		lineIndex++;
	}
	public virtual void StartMove(){
		move = true;
	}


	protected virtual void Update () {
		if (move){
			lerpPer += Time.deltaTime/destPos.moveCost;

			transform.position = Vector3.Lerp(startPos.gameObject.transform.position, 
			                                  destPos.gameObject.transform.position, 
			                                  lerpPer);


			if (lerpPer >= 1){
				lerpPer = 0;

				// moved from AStarScript to draw line as princess travels
				line.positionCount++; 
				Vector3 vec = transform.position;
				vec.z = -1;
				line.SetPosition(lineIndex, vec);

				currentStep++;

				if(currentStep >= path.steps){
					currentStep = 0;
					move = false;
					gameManager.LogFinishTime(astar); // new call to GameManager when finished					
				} 

				startPos = destPos;
				destPos = path.Get(currentStep);
				lineIndex++;
			}
		}
	}


}


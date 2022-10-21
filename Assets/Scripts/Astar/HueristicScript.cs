using UnityEngine;
using System.Collections;

public class HueristicScript : MonoBehaviour {
		
	public virtual float Hueristic(int x, int y, Vector3 start, Vector3 goal, GridScript gridScript){

        //creates a new vector3 that tracks the current grid position
        Vector3 current = new Vector3(x, y);
        //creates a distance value based on the distance from the current node to goal. 
        float distance = Vector3.Distance(current, goal);

        return (distance);
    }
}

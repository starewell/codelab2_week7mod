using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class FourthGridScript : GridScript
{
      string[] gridString = new string[]{
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
		"----------------------",
	};

    public char[] allChars;

	// Use this for initialization
	void Start () {
		gridWidth = gridString[0].Length;
		gridHeight = gridString.Length;

	}

	public override GameObject[,] GetGrid(){

		if (gridArray == null){
			RandomGrid();
			gridArray = new GameObject[gridWidth, gridHeight];
			
			float offsetX = (gridWidth  * -spacing)/2f;
			float offsetY = (gridHeight * spacing)/2f;

			for(int x = 0; x < gridWidth; x++){
				for(int y = 0; y < gridHeight; y++){
					GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
					quad.transform.localScale = new Vector3(spacing, spacing, spacing);
					quad.transform.position = new Vector3(offsetX + x * spacing, 
					                                      offsetY - y * spacing, 0);

					quad.transform.parent = transform;

					gridArray[x, y] = quad;
					
					quad.GetComponent<MeshRenderer>().sharedMaterial = GetMaterial(x, y);

					if(goal.x == x && goal.y == y){
						goalSprite.transform.position = quad.transform.position;
					}
				}
			}
		}

		return gridArray;
	}

	public override float GetMovementCost(GameObject go){
		return base.GetMovementCost(go);
	}
	
	protected override Material GetMaterial(int x, int y){

		char c = gridString[y].ToCharArray()[x];

		Material mat;

		switch(c){
		case 'r': 
			mat = mats[1];
			break;
		case 'w': 
			mat = mats[2];
			break;
		case 'l': 
			mat = mats[3];
			break;
		case 'p': 
			mat = mats[4];
			break;
		case 'f': 
			mat = mats[5];
			break;
		case 'e':
			mat = mats[6];
			break;
		default: 
			mat = mats[0];
			break;
		}
	
		return mat;
	}

    private void RandomGrid(){
        for(int y = 0; y < gridHeight; y++){
            for(int x = 0; x < gridWidth; x++){
                int randomChar = Random.Range(0, allChars.Length);
                StringBuilder sb = new StringBuilder(gridString[y]);
                sb[x] = allChars[randomChar];
                gridString[y] = sb.ToString();
            }
        }
    }
    
}

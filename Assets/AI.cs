using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AI : MonoBehaviour {

	public Transform target;

	private List<TileManager.Tile> tiles = new List<TileManager.Tile>();
	private TileManager manager;
	public GhostMove blinky;

	public TileManager.Tile nextTile = null;

	void Awake()
	{
		manager = GameObject.Find("Game Manager").GetComponent<TileManager>();
		tiles = manager.tiles;

		if(blinky == null)	Debug.Log ("game object blinky not found");
		if(manager == null)	Debug.Log ("game object Game Manager not found");
	}

	void FixedUpdate()
	{ 
		if(blinky.AI){

			// get the target tile position (round it down to int so we can reach with Index() function)
			Vector3 targetPos = new Vector3 (target.position.x+0.499f, target.position.y+0.499f);
			TileManager.Tile targetTile = tiles[manager.Index((int)targetPos.x, (int)targetPos.y)];

			// get current tile
			Vector3 currentPos = new Vector3(transform.position.x + 0.499f, transform.position.y + 0.499f);
			TileManager.Tile currentTile = tiles[manager.Index ((int)currentPos.x, (int)currentPos.y)];


			// get the next tile according to direction
			if(blinky.direction.x > 0)	nextTile = tiles[manager.Index ((int)(currentPos.x+1), (int)currentPos.y)];
			if(blinky.direction.x < 0)	nextTile = tiles[manager.Index ((int)(currentPos.x-1), (int)currentPos.y)];
			if(blinky.direction.y > 0)	nextTile = tiles[manager.Index ((int)currentPos.x, (int)(currentPos.y+1))];
			if(blinky.direction.y < 0)	nextTile = tiles[manager.Index ((int)currentPos.x, (int)(currentPos.y-1))];
				
			if(nextTile.occupied || currentTile.isIntersection)
			{
				//---------------------
				// IF WE BUMP INTO WALL
				if(nextTile.occupied && !currentTile.isIntersection)
				{
					// if blinky moves to right or left and there is wall next tile
					if(blinky.direction.x != 0)
					{
						if(currentTile.down == null)	blinky.direction = new Vector3(0, 1, 0);
						else 							blinky.direction = new Vector3(0, -1, 0);
					
					}

					// if blinky moves to up or down and there is wall next tile
					else if(blinky.direction.y != 0)
					{
						if(currentTile.left == null)	blinky.direction = new Vector3(1, 0, 0); 
						else 							blinky.direction = new Vector3(-1, 0, 0);

					}

				}

				//---------------------------------------------------------------------------------------
				// IF WE ARE AT INTERSECTION
				// calculate the distance to target from each available tile and choose the shortest one
				if(currentTile.isIntersection)
				{

					float dist1, dist2, dist3, dist4;
					dist1 = dist2 = dist3 = dist4 = 999999f;
					if(currentTile.up != null && !currentTile.up.occupied && !(blinky.direction.y < 0)) dist1 = manager.distance(currentTile.up, targetTile);
					if(currentTile.down != null && !currentTile.down.occupied &&  !(blinky.direction.y > 0)) dist2 = manager.distance(currentTile.down, targetTile);
					if(currentTile.left != null && !currentTile.left.occupied && !(blinky.direction.x > 0)) dist3 = manager.distance(currentTile.left, targetTile);
					if(currentTile.right != null && !currentTile.right.occupied && !(blinky.direction.x < 0)) dist4 = manager.distance(currentTile.right, targetTile);

					if(Mathf.Min(dist1, dist2, dist3, dist4) == dist1) blinky.direction = new Vector3(0, 1, 0);
					if(Mathf.Min(dist1, dist2, dist3, dist4) == dist2) blinky.direction = new Vector3(0, -1, 0);
					if(Mathf.Min(dist1, dist2, dist3, dist4) == dist3) blinky.direction = new Vector3(-1, 0, 0);
					if(Mathf.Min(dist1, dist2, dist3, dist4) == dist4) blinky.direction = new Vector3(1, 0, 0);

				}

			}

			// if there is no decision to be made, designate next waypoint for the ghost
			else
			{
				blinky.direction = blinky.direction;	// setter updates waypoint
			}

		}
	}
}
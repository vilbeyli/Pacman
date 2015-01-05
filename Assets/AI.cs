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
		if(blinky.ready){

			// get the target tile position (round it down to int so we can reach with Index() function)
			Vector3 targetPos = new Vector3 (target.position.x+0.499f, target.position.y+0.499f);
			TileManager.Tile targetTile = tiles[manager.Index((int)targetPos.x, (int)targetPos.y)];

			// get current tile
			Vector3 currentPos = new Vector3(transform.position.x + 0.499f, transform.position.y + 0.499f);
			TileManager.Tile currentTile = tiles[manager.Index ((int)currentPos.x, (int)currentPos.y)];

			Debug.Log ("dir: " + blinky.direction.x + ", " + blinky.direction.y);

			// get the next tile according to direction
			if(blinky.direction.x > 0)	nextTile = tiles[manager.Index ((int)(currentPos.x+1), (int)currentPos.y)];
			if(blinky.direction.x < 0)	nextTile = tiles[manager.Index ((int)(currentPos.x-1), (int)currentPos.y)];
			if(blinky.direction.y > 0)	nextTile = tiles[manager.Index ((int)currentPos.x, (int)(currentPos.y+1))];
			if(blinky.direction.y < 0)	nextTile = tiles[manager.Index ((int)currentPos.x, (int)(currentPos.y-1))];

			//Debug.Log ("Current Tile: (" + currentTile.x + ", " + currentTile.y + ") ");
			//Debug.Log ("Next Tile: (" + nextTile.x + ", " + nextTile.y + ")");
				
			if(nextTile.occupied || currentTile.isIntersection)
			{
				//---------------------
				// IF WE BUMP INTO WALL
				if(nextTile.occupied)
				{
					// if blinky moves to right or left and there is wall next tile
					if(blinky.direction.x > 0 || blinky.direction.x < 0)
					{

						// if either of the tiles are available, calculate distance and choose
						if(nextTile.down != null && nextTile.up != null)
						{
							float dist1 = manager.distance(nextTile.up, targetTile);
							float dist2 = manager.distance(nextTile.down, targetTile);

							// if upper neighbor tile is closer to the target, go up
							if( dist1 < dist2 )	blinky.direction = new Vector3(0,1,0);
							else 				blinky.direction = new Vector3(0,-1,0);
						}

						// if only one tile is available
						else
						{
							Debug.Log ("One tile available when going left or right");
							if(nextTile.down == null)	blinky.direction = new Vector3(0, 1, 0);
							if(nextTile.up == null)	blinky.direction = new Vector3(0, -1, 0);
						}
					}

					// if blinky moves to up or down and there is wall next tile
					else if(blinky.direction.y > 0 || blinky.direction.y < 0)
					{
						
						// if either of the tiles are available, calculate distance and choose
						if(nextTile.right != null && nextTile.left != null)
						{
							float dist1 = manager.distance(nextTile.right, targetTile);
							float dist2 = manager.distance(nextTile.left, targetTile);
							
							// if right neighbor tile is closer to the target, go right
							if( dist1 < dist2 )	blinky.direction = new Vector3(1,0,0);
							else 				blinky.direction = new Vector3(-1,0,0);
						}
						
						// if only one tile is available
						else
						{
							Debug.Log ("One tile available when going up or down");
							if(nextTile.left == null)	blinky.direction = new Vector3(1, 0, 0);
							if(nextTile.right == null)	blinky.direction = new Vector3(-1, 0, 0);
						}
					}

				}

				//--------------------------
				// IF WE ARE AT INTERSECTION
				if(currentTile.isIntersection)
				{
					float dist1, dist2, dist3, dist4;
					dist1 = dist2 = dist3 = dist4 = 999999f;
					if(currentTile.up != null && !currentTile.up.occupied && !(blinky.direction.y < 0)) dist1 = manager.distance(currentTile.up, targetTile);
					if(currentTile.down != null && !currentTile.down.occupied &&  !(blinky.direction.y > 0)) dist2 = manager.distance(currentTile.down, targetTile);
					if(currentTile.left != null && !currentTile.left.occupied && !(blinky.direction.x > 0)) dist3 = manager.distance(currentTile.left, targetTile);
					if(currentTile.right != null && !currentTile.right.occupied && !(blinky.direction.x > 0)) dist4 = manager.distance(currentTile.right, targetTile);
				
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
			//Debug.Log ("Next Tile: " + nextTile.x + ", "  + nextTile.y + " Dir: (" + blinky.direction.x + ", " + blinky.direction.y + ")");
		}
	}
}
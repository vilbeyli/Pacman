using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TileManager : MonoBehaviour {

	public class Tile
	{
		public int x { get; set; }
		public int y { get; set; }
		public bool occupied {get; set;}
		public int adjacentCount {get; set;}
		public bool isIntersection {get; set;}
		
		public Tile left,right,up,down;
		
		public Tile(int x_in, int y_in)
		{
			x = x_in; y = y_in;
			occupied = false;
			left = right = up = down = null;
		}


	};
	
	public List<Tile> tiles = new List<Tile>();
	
	// Use this for initialization
	void Start () 
	{
		ReadTiles();

	}
	
	// Update is called once per frame
	void Update () 
	{
		//DrawNeighbors();

	}
	
	//-----------------------------------------------------------------------
	// Read the tile infor from file: 1 = free tile, 0 = wall
	void ReadTiles()
	{
		string path = Application.dataPath + "/Data/tiles.txt";
		StreamReader stream = new StreamReader(path);
		
		// reading tiles from top to bottom. X:2 Y:30 (X goes up, Y goes down)
		// so that x and y of tile will be the same as world coordinates.
		int X = 2, Y = 30;
		while(!stream.EndOfStream)
		{
			string line = stream.ReadLine();
			
			X = 2;		// for every line
			for(int i = 0; i < line.Length ; ++i)
			{
				Tile newTile = new Tile(X,Y);
				
				// if the tile we read is a valid tile (movable)
				if(line[i] == '1')
				{
					// check for left-right neighbor
					if(i!=0 && line[i-1] == '1')
					{
						// assign each tile to the corresponding side of other tile
						newTile.left = tiles[tiles.Count-1];
						tiles[tiles.Count-1].right = newTile;

						// adjust adjcent tile counts of each tile
						newTile.adjacentCount++;
						tiles[tiles.Count-1].adjacentCount++;
					}
				}
				
				// if the current tile is not movable
				else   newTile.occupied = true;
				
				// check for up-down neighbor
				int upNeighbor = tiles.Count - 26;  // up neighbor index
				if(Y < 30 && !newTile.occupied && !tiles[upNeighbor].occupied)
				{
					tiles[upNeighbor].down = newTile;
					newTile.up = tiles[upNeighbor];

					// adjust adjcent tile counts of each tile
					newTile.adjacentCount++;
					tiles[upNeighbor].adjacentCount++;
				}
				
				tiles.Add(newTile);
				X++;
			}
			
			Y--;
			//Debug.Log ("----------");
		}

		// after reading all tiles, determine the intersection tiles
		foreach(Tile tile in tiles)
		{
			if(tile.adjacentCount > 2)
				tile.isIntersection = true;
		}
		stream.Close();
	}
	
	
	//-----------------------------------------------------------------------
	// Draw lines between neighbor tiles
	void DrawNeighbors()
	{
		foreach(Tile tile in tiles)
		{
			Vector3 pos = new Vector3(tile.x, tile.y, 0);
			Vector3 up = new Vector3(tile.x+0.1f, tile.y+1, 0);
			Vector3 down = new Vector3(tile.x-0.1f, tile.y-1, 0);
			Vector3 left = new Vector3(tile.x-1, tile.y+0.1f, 0);
			Vector3 right = new Vector3(tile.x+1, tile.y-0.1f, 0);
			
			if(tile.up != null)		Debug.DrawLine(pos, up);
			if(tile.down != null)	Debug.DrawLine(pos, down);
			if(tile.left != null)	Debug.DrawLine(pos, left);
			if(tile.right != null)	Debug.DrawLine(pos, right);
		}
		
	}
	
	//----------------------------------------------------------------------
	// returns the index in the tiles list of a given tile's coordinates
	public int Index(int X, int Y)
	{
		return (30-Y)*26 + X-2;
	}
	
	public int Index(Tile tile)
	{
		return (30-tile.y)*26 + tile.x-2;
	}

	//----------------------------------------------------------------------
	// returns the distance between two tiles
	public float distance(Tile tile1, Tile tile2)
	{
		return Mathf.Sqrt( Mathf.Pow(tile1.x - tile2.x, 2) + Mathf.Pow(tile1.y - tile2.y, 2));
	}
}

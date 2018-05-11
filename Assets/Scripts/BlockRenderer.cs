using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;
using Random = UnityEngine.Random;



public class BlockRenderer : MonoBehaviour {
	
	//2D array	
	//Map size should be odd number
	private static int mapSize = 11;
	private static int centerPos;
	private int initBlockRange = 1;
	private int minInit;
	private int maxInit;
	
	private int quizBlockRange = 1;
	
	private int[,] map2D = new int[mapSize,mapSize];
	public GameObject nullBlock;
	public Sprite nullBlockSprite;
	public Sprite[] BlockSprites = new Sprite[6];
	private GameObject[,] map2dRendered = new GameObject[mapSize,mapSize];
	private int[,] problemSets = new int[4, 3];


	private int[,] stackDir = new int[4,2] {{-1,0},{0,-1},{1,0},{0,1}}; 
	
	// Use this for initialization
	void Start ()
	{
		centerPos = (int) (mapSize / 2);
		minInit = centerPos - initBlockRange;
		maxInit = centerPos + initBlockRange;
		ArrayInitializer();
		ProblemMaker();
		MapRenderer();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			MatrixRoataion();
			MapRenderer();
		}
		
		// #TODO Blocks should be gathered to the center

		if (Input.GetKeyDown(KeyCode.LeftArrow))
		{
			StackProblem(0);
			ProblemMaker();
			MapRenderer();
			
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			StackProblem(1);
			ProblemMaker();
			MapRenderer();
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			StackProblem(2);
			ProblemMaker();
			MapRenderer();
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			StackProblem(3);
			ProblemMaker();
			MapRenderer();
		}
	}

	void StackProblem(int dir)
	{
		
		//Problem Axis
		int pAxis = 0;
		//Problem orthnogonal Axis
		int poAxis = 0;

		// i : problem colum j  : other
		switch (dir)
		{
			case 0: 	
				for (int i = minInit; i <=maxInit; i++)
				{
					if (problemSets[dir, i-minInit] != -1)
					{
						for (int j = 1; j < mapSize - 1; j++)
						{
							if (map2D[j,i] != -1)
							{
								pAxis = i;
								poAxis = j;
								break;
							}
						}
						poAxis = poAxis + stackDir[dir, 0];
						pAxis = pAxis + stackDir[dir, 1];
						map2D[poAxis, pAxis] = problemSets[dir, i-minInit];
					}
					
				}
				break;
			case 1:
				for (int i = minInit; i <=maxInit; i++)
				{
					if (problemSets[dir, i-minInit] != -1)
					{
						for (int j = 1; j < mapSize - 1; j++)
						{
							if (map2D[i,j] != -1)
							{
								pAxis = i;
								poAxis = j;
								break;
							}
						}
						pAxis = pAxis + stackDir[dir, 0];
						poAxis = poAxis + stackDir[dir, 1];
						map2D[pAxis, poAxis] = problemSets[dir, i-minInit];
					}
					
				}
				break;
			case 2:
				for (int i = minInit; i <=maxInit; i++)
				{
					if (problemSets[dir, i-minInit] != -1)
					{
						for (int j = mapSize-2; j >=1; j--)
						{
							if (map2D[j,i] != -1)
							{
								pAxis = i;
								poAxis = j;
								break;
							}
						}
						poAxis = poAxis + stackDir[dir, 0];
						pAxis = pAxis + stackDir[dir, 1];
						map2D[poAxis, pAxis] = problemSets[dir, i-minInit];
					}
					
				}

				break;
			case 3:
				for (int i = minInit; i <=maxInit; i++)
				{
					if (problemSets[dir, i-minInit] != -1)
					{
						for (int j = mapSize-2; j >=1; j--)
						{
							if (map2D[i,j] != -1)
							{
								pAxis = i;
								poAxis = j;
								break;
							}
						}
						pAxis = pAxis + stackDir[dir, 0];
						poAxis = poAxis + stackDir[dir, 1];
						map2D[pAxis, poAxis] = problemSets[dir, i-minInit];
					}
					
				}
				break;
		}
			
	}

	// problem set : min Init to max init ==> It can be changed
	void ProblemMaker()
	{
		for(int i = minInit; i<=maxInit;i++)
		{
			int num = RandomBlockNumber();
			map2D[0, i] = num;
			problemSets[0, i - minInit] = num;

			num = RandomBlockNumber();
			map2D[i, 0] = num;
			problemSets[1, i - minInit] = num;
			
			num = RandomBlockNumber();
			map2D[mapSize - 1, i] = num;
			problemSets[2, i - minInit] = num;
			
			num = RandomBlockNumber();
			map2D[i, mapSize - 1] = num;
			problemSets[3, i - minInit] = num;

		}
		
	}

//If I seperate the quadrant ==> it might be east to render the blocks...
	void MatrixRoataion()
	{
		int[,] dummyMap2D = new int[mapSize,mapSize];
		for (int i = 1; i < mapSize-1; i++)
		{
			for (int j = 1; j < mapSize-1; j++)
			{
				dummyMap2D[mapSize-j-1, i] = map2D[i, j];
			}
		}
		for (int i = 1; i < mapSize-1; i++)
		{
			for (int j = 1; j < mapSize-1; j++)
			{
				map2D[i, j] = dummyMap2D[i, j];
			}
		}
		
	}
	
	int RandomBlockNumber()
	{
		return ((Random.Range(0f, 1f) > 0.1) ? (int) Random.Range(1.0f, 6.0f) : -1);
	}
	
	void MapRenderer()
	{
		for (int i = 0; i < mapSize; i++)
		{
			for (int j = 0; j < mapSize; j++)
			{
				if (map2D[i, j] == -1)
					map2dRendered[i, j].GetComponent<SpriteRenderer>().sprite = nullBlockSprite;
				else
					map2dRendered[i, j].GetComponent<SpriteRenderer>().sprite = BlockSprites[map2D[i, j]];
			}
		}
	}

	
	
	//Nothing : -1 else : Color(1 to 5)
	void ArrayInitializer()
	{
		//prefab Initializer
		for (int i = 0; i < 11; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				map2D[i, j] = -1;
				Vector3 pos =  new Vector3(i-centerPos,j-centerPos,0f);
				map2dRendered[i,j] = Instantiate(nullBlock, pos, Quaternion.identity);
			}
		}
		//Map initializer
		//Random으로 방향성마다 더 확장할지 않할지를 해주는 함수가 있으면 좋을듯 ==> 정사각형 말고 다른 모양 시작함수
		for (int i = minInit; i <= maxInit; i++)
		{
			for (int j = minInit; j <= maxInit; j++)
			{
				if (i == centerPos && j == centerPos)
					map2D[i, j] = 0;
				else
					map2D[i, j] = (int) Random.Range(1.0f, 6.0f);
				
				if ((i == minInit || j == minInit || i == maxInit || j == maxInit) && Random.Range(0f, 1f) > 0.9)
					map2D[i, j] = -1;
			}
		}
		
	}

}

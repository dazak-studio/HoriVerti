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

	private int[,] checkerDir = new int[4, 2] {{-1, 0}, {0, -1}, {1, 0}, {0, 1}};
	private float blockNumber = 4;
	
	// Use this for initialization
	void Start ()
	{
		centerPos = (int) (mapSize / 2);
		minInit = centerPos - initBlockRange;
		maxInit = centerPos + initBlockRange;
		ArrayInitializer();
		ProblemMaker(0);
		ProblemMaker(1);
		ProblemMaker(2);
		ProblemMaker(3);
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
			GroupInputFeedback(0);
			
		}
		if (Input.GetKeyDown(KeyCode.DownArrow))
		{
			StackProblem(1);
			GroupInputFeedback(1);
		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
		{
			StackProblem(2);
			GroupInputFeedback(2);
		}
		if (Input.GetKeyDown(KeyCode.UpArrow))
		{
			StackProblem(3);
			GroupInputFeedback(3);
		}
	}

	void GroupInputFeedback(int index)
	{
		Checker();
		MapRenderer();
		ProblemMaker(index);
		MapRenderer();
	}

	//Making Vector
	
	//DFS for each vector
	
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
	void ProblemMaker(int index)
	{
		for(int i = minInit; i<=maxInit;i++)
		{
			switch (index)
			{
				case 0:
					int num = RandomBlockNumber();
					map2D[0, i] = num;
					problemSets[0, i - minInit] = num;
					break;
				case 1:
					num = RandomBlockNumber();
					map2D[i, 0] = num;
					problemSets[1, i - minInit] = num;
					break;
				case 2:
					num = RandomBlockNumber();
					map2D[mapSize - 1, i] = num;
					problemSets[2, i - minInit] = num;
					break;
				case 3:
					num = RandomBlockNumber();
					map2D[i, mapSize - 1] = num;
					problemSets[3, i - minInit] = num;
					break;
			}
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
		return ((Random.Range(0f, 1f) > 0.1) ? (int) Random.Range(1.0f, blockNumber) : -1);
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

	private List<int[]> sameblockList = new List<int[]>();
	private List<int[]> checkerList = new List<int[]>();
	


	private int[,] checkDummyArray = new int[mapSize,mapSize];

	void Checker()
	{
		sameblockList.Clear();

		//Dummy Array Initializer
		for (int i = 1; i < mapSize - 1; i++)
		{
			for (int j = 1; j < mapSize - 1; j++)
			{
				checkDummyArray[i, j] = map2D[i, j];
			}
		}

		for (int i = 0; i < mapSize; i++)
		{
			checkDummyArray[0, i] = -1;
			checkDummyArray[i, 0] = -1;
			checkDummyArray[mapSize - 1, i] = -1;
			checkDummyArray[i, mapSize - 1] = -1;
		}

		//Check one block by one block
		for (int i = 1; i < mapSize - 1; i++)
		{
			for (int j = 1; j < mapSize - 1; j++)
			{
				int color = checkDummyArray[i, j];
				if (color >= 1)
				{
					Debug.Log("=============start=============="+" (" + i + ","+ j+")");
					checkerList.Add(new int[] {i, j});
					sameblockList.Add(new int[] {i, j});
					checkDummyArray[i, j] = -1;
					stackChecker(color);


					for (int k = 0; k < sameblockList.Count; k++)
					{
						Debug.Log("[Total List] :" + sameblockList.Count + "= " + "(" + sameblockList[k][0] + "," +
						          sameblockList[k][1] + ")" + color);
					}

					if (sameblockList.Count >= 3)
					{
						Debug.Log("Delete");
						for (int k = 0; k < sameblockList.Count; k++)
						{
							map2D[sameblockList[k][0], sameblockList[k][1]] = -1;
						}
					}

					sameblockList.Clear();

				}

			}
		}

	}



	void stackChecker(int color)
	{
		do
		{
			Debug.Log("Start sameblock count " + sameblockList.Count);
			int i = checkerList[0][0];
			int j = checkerList[0][1];
			checkerList.RemoveAt(0);

			
			Debug.Log("[Funtion Start]color : "+ color + " (" + i + ","+ j+")");
			for (int k = 0; k < 4; k++)
			{
				if (checkDummyArray[i + checkerDir[k, 0], j + checkerDir[k, 1]]==color)
				{
					checkDummyArray[i, j] = -1;
					checkerList.Add(new int[]{i + checkerDir[k, 0], j + checkerDir[k, 1]});
					sameblockList.Add(new int[]{i + checkerDir[k, 0], j + checkerDir[k, 1]});
				}
			}

			for (int k = 0; k < checkerList.Count; k++)
			{
				Debug.Log("==> :" + checkerList.Count + "= " + "(" + checkerList[k][0] + "," +
				          checkerList[k][1] + ")" + color);
			}
		} while (checkerList.Count != 0);

		Debug.Log("Done==============" + color );

	}
	/*
	void Checker()
	{
		sameblockList.Clear();
		
		//Dummy Array Initializer
		for (int i = 1; i < mapSize-1; i++)
		{
			for (int j = 1; j < mapSize-1; j++)
			{
				checkDummyArray[i, j] = map2D[i, j];
			}
		}
		for (int i = 0; i < mapSize; i++)
		{
			checkDummyArray[0, i] = -1;
			checkDummyArray[i, 0] = -1;
			checkDummyArray[mapSize - 1, i] = -1;
			checkDummyArray[i, mapSize - 1] = -1;
		}

		//Check one block by one block
		for (int i = 1; i < mapSize - 1; i++)
		{
			for (int j = 1; j < mapSize - 1; j++)
			{
				int color = checkDummyArray[i, j];
				if (color >= 1)
				{
					Debug.Log("=================start==============");
					recurChecker(i, j, color);
					
					
					for (int k = 0; k < sameblockList.Count; k++)
					{
						Debug.Log("Count :" + (sameblockList.Count) + "= " + "("+sameblockList[k][0] + "," +sameblockList[k][1] + ")" +color);
					}
					
					if (sameblockList.Count >= 3)
					{
						Debug.Log("Delete");
						for (int k = 0; k < sameblockList.Count; k++)
						{
							map2D[sameblockList[k][0], sameblockList[k][1]] = -1;
						}
					}
					sameblockList.Clear();
				}

			}
		}

		if (sameblockList.Count >= 3)
		{
			foreach (var test in sameblockList)
			{
				//Debug.Log("_" + test[0]+"_"+test[1]);
			}
		}
	}



	void recurChecker(int i, int j,int color)
	{
			Debug.Log("[Funtion Start]color : "+ color + " (" + i + ","+ j+")");
		
			sameblockList.Add(new int[] {i,j});
			checkDummyArray[i, j] = -1; 
			for (int k = 0; k < 4; k++)
			{
				if (checkDummyArray[i + checkerDir[k, 0], j + checkerDir[k, 1]]==color)
				{
					recurChecker(i + checkerDir[k, 0],j + checkerDir[k, 1],color);
				}
			}						
	}
	*/
	
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
					map2D[i, j] = (int) Random.Range(1.0f, blockNumber);
				
				if ((i == minInit || j == minInit || i == maxInit || j == maxInit) && Random.Range(0f, 1f) > 0.9)
					map2D[i, j] = -1;
			}
		}
		
	}

}

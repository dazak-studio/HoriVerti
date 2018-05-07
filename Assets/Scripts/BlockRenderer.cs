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
	
	private int[,] map2D = new int[mapSize,mapSize];
	public GameObject nullBlock;
	public Sprite nullBlockSprite;
	public Sprite[] BlockSprites = new Sprite[6];
	private GameObject[,] map2dRendered = new GameObject[mapSize,mapSize];


	// Use this for initialization
	void Start ()
	{
		centerPos = (int) (mapSize / 2);
		minInit = centerPos - initBlockRange;
		maxInit = centerPos + initBlockRange;
		ArrayInitializer();
		MapRenderer();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			MatrixRoataion();
			MapRenderer();
		}
	}

//If I seperate the quadrant ==> it might be east to render the blocks...
	void MatrixRoataion()
	{
		int[,] dummyMap2D = new int[mapSize,mapSize];
		for (int i = 0; i < 11; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				dummyMap2D[mapSize-j-1, i] = map2D[i, j];
			}
		}
		for (int i = 0; i < 11; i++)
		{
			for (int j = 0; j < 11; j++)
			{
				map2D[i, j] = dummyMap2D[i, j];
			}
		}
		
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

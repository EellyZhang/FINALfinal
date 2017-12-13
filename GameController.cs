using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {
	public GameObject cubePrefab, nextCubePrefab;
	Vector3 gridCubePosition, nextCubePosition;
	public GameObject[,] gridCubes;
	public GameObject nextCube;
	float eachTurnTime, turnTimer, eachGameTime;
	int gridCubeMaxX, gridCubeMaxY;
	Color[] cubeColor;
	Color currentNextCubeColor;
	int coloredX;
	public GameObject blackCubes;
	public int blackX, blackY;
	bool detectKeyboardInput;
	public GameObject activeCube;
	public Text nextCubeText, gameOverText, scoreText, timerText;
	int rainbowPoints = 5;
	int sameColorPoints = 10;
	int score = 0;

	void Start() 
	{
		gridCubeMaxX = 8;
		gridCubeMaxY = 5;
		gridCubes = new GameObject[gridCubeMaxX, gridCubeMaxY];

		blackX = 0;
		blackY = 0;

		detectKeyboardInput = false;

	

		eachTurnTime = 2.0f;
		turnTimer = eachTurnTime;
		eachGameTime = 60.0f;


		cubeColor = new Color[5];
		cubeColor [0] = Color.blue;
		cubeColor [1] = Color.green;
		cubeColor [2] = Color.red;
		cubeColor [3] = Color.yellow;
		cubeColor [4] = Color.magenta;

		for (int x = 0; x < gridCubeMaxX; x++) 
		{
			for (int y = 0; y < gridCubeMaxY; y++)
			{
				gridCubePosition = new Vector3 (x*2, y*2, 0);
				gridCubes [x,y] = Instantiate (cubePrefab, gridCubePosition, Quaternion.identity);			
				gridCubes [x,y].GetComponent<Renderer>().material.color = Color.white;
				gridCubes [x, y].GetComponent<CubeBehavior> ().cubeX = x;
				gridCubes [x, y].GetComponent<CubeBehavior> ().cubeY = y;
			}
		}
		//test this linenextCubenextCube
		NewNextCube ();

//		Score ();
	}

	//set for X position, check if it is occupied, change the color there
	public void GetInThereNextCube(int y)
	{

		List<GameObject> whiteCubesInLine = new List<GameObject> ();

		for (int x = 0; x < gridCubeMaxX; x++) {
			if (gridCubes[x, y].GetComponent<Renderer>().material.color == Color.white) {
				whiteCubesInLine.Add(gridCubes[x,y]);
			}
		}

		if (whiteCubesInLine.Count == 0) {
			EndGame (false);
		}

		else {GameObject theChosenOneInTheGrid;
			theChosenOneInTheGrid = whiteCubesInLine [Random.Range (0, whiteCubesInLine.Count)];
			theChosenOneInTheGrid.GetComponent<Renderer>().material.color = currentNextCubeColor;
			theChosenOneInTheGrid.GetComponent<CubeBehavior>().isColored = true;
			Destroy(nextCube);
		}
	}

	//fail to press keyboard on time, blacken cubesselec
	public void BlackenCubes()
	{
		List<GameObject> whiteCubesAll = new List<GameObject> ();

		for (int x = 0; x < gridCubeMaxX; x++) {
			for (int y = 0; y < gridCubeMaxY; y++) {
				if (gridCubes[x, y].GetComponent<Renderer>().material.color == Color.white) 
				{
					whiteCubesAll.Add(gridCubes[x,y]);
				}
			}
		}

		blackCubes = whiteCubesAll [Random.Range (0, whiteCubesAll.Count)];
		blackCubes.GetComponent<Renderer> ().material.color = Color.black;
		gridCubes[blackX, blackY].GetComponent<CubeBehavior>().isBlacked = true;
		//cannot be destroyed
		Destroy(nextCube);
	}




	public void KeyboardInput ()
	{
		if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
		{
			GetInThereNextCube(4);
			detectKeyboardInput = true;
		}
		else if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))
		{
			GetInThereNextCube(3);
			detectKeyboardInput = true;
		}

		else if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))
		{
			GetInThereNextCube(2);
			detectKeyboardInput = true;
		}

		else if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4))
		{
			GetInThereNextCube(1);
			detectKeyboardInput = true;
		}
		else if (Input.GetKey (KeyCode.Alpha5) || Input.GetKey (KeyCode.Keypad5)) {
			GetInThereNextCube (0);
			detectKeyboardInput = true;
		} 
	}

	public void NewNextCube()
	{
		nextCubePosition = new Vector3 (20, 10, 0);
		nextCube = Instantiate (nextCubePrefab, nextCubePosition, Quaternion.identity);
		currentNextCubeColor = 	cubeColor [Random.Range (0,5) ];
		nextCube.GetComponent<Renderer>().material.color = currentNextCubeColor;

	}

	public void ProcessClick(GameObject selectedCube, int x, int y, Color selectedCubeColor)
	{
		if (selectedCubeColor != Color.white && selectedCubeColor != Color.black) 
		{
			// if there is an active cube
			if (activeCube != null) {
				if (selectedCube != activeCube) {
					activeCube.transform.localScale /= 1.2f;
					selectedCube.transform.localScale *= 1.2f;
					activeCube = selectedCube;
				} 
				else {
					activeCube.transform.localScale /= 1.2f;
					activeCube = null;
				}
			}

			//if there is no active cube yet
			else {
				selectedCube.transform.localScale *= 1.2f;
				activeCube = selectedCube;
			}

		}
		else if (selectedCubeColor == Color.white && activeCube != null) 
		{
			int xDist = selectedCube.GetComponent<CubeBehavior> ().cubeX - activeCube.GetComponent<CubeBehavior> ().cubeX;
			int yDist = selectedCube.GetComponent<CubeBehavior> ().cubeY - activeCube.GetComponent<CubeBehavior> ().cubeY;
			print ("xDist:" + xDist);
			print ("yDist:" + yDist);


			if (Mathf.Abs (xDist) <= 1 && Mathf.Abs (yDist) <= 1) 
			{
				//set the new selected cube to be active and set the old active cube to be white and inactive
				selectedCube.transform.localScale *= 1.2f;
				activeCube.transform.localScale /= 1.2f;

				selectedCube.GetComponent<Renderer> ().material.color = activeCube.GetComponent<Renderer> ().material.color;
				activeCube.GetComponent<Renderer> ().material.color = Color.white;

				activeCube = null;
				activeCube = selectedCube;
			}					
		} 
	}

	void EdgeCheck (int x, int y) {
		//make sure x and y are not at the edge of the grid
		if (x == 0 || y == 0 || x == gridCubeMaxX - 1 || y == gridCubeMaxY - 1) {
			return;
		}
	}

	bool IsRainbowPlus (int x, int y) {
		EdgeCheck (x, y);

//		if ((x == 3 && y == 3)) {
//			return true;
//		}

		Color a = gridCubes [x, y].GetComponent<Renderer> ().material.color;
		Color b = gridCubes [x+1, y].GetComponent<Renderer> ().material.color;
		Color c = gridCubes [x-1, y].GetComponent<Renderer> ().material.color;
		Color d = gridCubes [x, y+1].GetComponent<Renderer> ().material.color;
		Color e = gridCubes [x, y-1].GetComponent<Renderer> ().material.color;

		//check for colors not be white or black, if so, there's no rainbow plus
		if (a == Color.white || a == Color.black ||
		    b == Color.white || b == Color.black ||
		    c == Color.white || c == Color.black ||
		    d == Color.white || d == Color.black ||
		    e == Color.white || e == Color.black) {
			return false;
		}


		//check all colors are different
		if (a != b && a != c && a != d && a != e &&
		    b != c && b != d && b != e &&
		    c != d && c != e &&
		    d != e) {
			return true;
		} 

		else {
			return false;
		}

	}

	bool IsSameColorPlus (int x, int y) {
		EdgeCheck (x, y);

//		if ((x == 1 && y == 3)) {
//			Debug.Log ("got same color plus");
//			return true;
//		}

		if (gridCubes [x, y].GetComponent<Renderer> ().material.color != Color.white &&
			gridCubes [x, y].GetComponent<Renderer> ().material.color != Color.black &&
			gridCubes [x, y].GetComponent<Renderer> ().material.color == gridCubes [x + 1, y].GetComponent<Renderer> ().material.color &&
		    gridCubes [x, y].GetComponent<Renderer> ().material.color == gridCubes [x - 1, y].GetComponent<Renderer> ().material.color &&
		    gridCubes [x, y].GetComponent<Renderer> ().material.color == gridCubes [x, y + 1].GetComponent<Renderer> ().material.color &&
		    gridCubes [x, y].GetComponent<Renderer> ().material.color == gridCubes [x, y - 1].GetComponent<Renderer> ().material.color) {
			return true;
		}

		else {
			return false;
		}
	
	}

	void MakeBlackPlus (int x, int y) {
		EdgeCheck (x, y);

		gridCubes [x, y].GetComponent<Renderer> ().material.color = Color.black;
		gridCubes [x+1, y].GetComponent<Renderer> ().material.color = Color.black;
		gridCubes [x-1, y].GetComponent<Renderer> ().material.color = Color.black;
		gridCubes [x, y+1].GetComponent<Renderer> ().material.color = Color.black;
		gridCubes [x, y-1].GetComponent<Renderer> ().material.color = Color.black;

		//deactivate the acvite cube in a plus
		if (activeCube != null && activeCube.GetComponent<Renderer> ().material.color == Color.black) {
			activeCube.transform.localScale /= 1.2f;
			activeCube = null;
		}
	}

	void Score (){
	//check the location of the center of the plus, it can never be the edge

		List<Vector2> sameColorPluses = new List<Vector2> ();
		List<Vector2> rainbowPluses = new List<Vector2> ();

		for (int x=1; x < gridCubeMaxX - 1; x++) {
			for (int y = 1; y < gridCubeMaxY - 1; y++) {
			
				if (IsRainbowPlus (x, y)) {
					rainbowPluses.Add (new Vector2 (x, y));
				}
				if (IsSameColorPlus (x, y)) {
					sameColorPluses.Add (new Vector2 (x, y));
				}

			}
		}

		if (sameColorPluses.Count == 1 && rainbowPluses.Count == 1) {

			score += 110; 
		
		}

		if (rainbowPluses.Count > 0) {
			score += (int)Mathf.Pow (rainbowPoints, rainbowPluses.Count);
		}

		foreach (Vector2 plus in rainbowPluses) {
			MakeBlackPlus ((int)(plus.x), (int)(plus.y));
		}

		if (sameColorPluses.Count > 0) {
			score += (int)Mathf.Pow (sameColorPoints, sameColorPluses.Count);
		}

		foreach (Vector2 plus in sameColorPluses) {
			MakeBlackPlus ((int)(plus.x), (int)(plus.y));
		}
	}

	void EndGame(bool win)
	{
			if (win)
			{
			nextCubeText.text = " ";
			gameOverText.text = "CONGRATS!";
			}
			else 
			{
			nextCubeText.text = " ";
			gameOverText.text = "GGWP!";
			}

		Destroy (nextCube);

		//destroy the whole scene
		for (int x = 0; x < gridCubeMaxX ; x++) {
			for (int y = 0; y < gridCubeMaxY; y++) {
				Destroy (gridCubes [x, y]);
			}
		}

	}

	void Update()
	{
		if (Time.time < eachGameTime) {
			//press key
			if (detectKeyboardInput == false) {
				KeyboardInput ();
			}

			Score ();

			timerText.text = "Time: " + (eachGameTime - Time.time).ToString ("0.00");


			if (Time.time > turnTimer) {
				if (detectKeyboardInput == false) {
					BlackenCubes ();
					score -= 1;
					//score can never be negative
					if (score <= 0) {
						score = 0;
					}

				}

				NewNextCube ();

				detectKeyboardInput = false;
					

				turnTimer += eachTurnTime;
			}

			//update score UI
			scoreText.text = "Score: " + score;
		}
		//Times up
		else {
			//player wins
			if (score > 0){
				EndGame (true);
			}
			//player loses
			else {
				EndGame (false);
			}

		}
	}
}

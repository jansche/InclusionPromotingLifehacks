using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{

	private MapMovementToEffect script;
	private float minDistance = 13f;
	private GameObject start_img_go;
	private GameObject end_img_go;
	private GameObject instructions_img_go;
	//	private GameObject campaign_go;
	bool canSwitch = false;
	bool waitActive = false;
	//so wait function wouldn't be called many times per frame
	bool gameBegun;
	bool canEnd = false;
	bool waited = false;
	bool playerPressedNo = false;
	float levelTimer;
	bool updateTimer;
	private Text timer;
	private Button restart;

	bool show_time;
	// Use this for initialization
	void Start ()
	{
		// get game objects
		script = GameObject.FindGameObjectWithTag ("Player").GetComponent<MapMovementToEffect> ();
		start_img_go = GameObject.FindGameObjectWithTag ("start_img");
		end_img_go = GameObject.FindGameObjectWithTag ("end_img");
		instructions_img_go = GameObject.FindGameObjectWithTag ("instructions");
		timer = GameObject.FindGameObjectWithTag ("timer").GetComponent<Text> ();

		// init 
		end_img_go.SetActive (false);
		start_img_go.SetActive (true);
		instructions_img_go.SetActive (false);

		// set game start vals at beginning
		restart = GameObject.FindGameObjectWithTag ("restart").GetComponent<Button> ();
		gameBegun = true;
		levelTimer = 0.0f;
		updateTimer = true;
		show_time = false;
		Screen.SetResolution (1024, 768, true);
//		campaign_go = GameObject.FindGameObjectWithTag ("campaign");
//		restart.onClick.AddListener(() => { Debug.Log("WORKS"); Application.LoadLevel(0); } );
	}

	void Update ()
	{
		if (updateTimer && !gameBegun && show_time) {
			levelTimer += Time.deltaTime;
			timer.text = "Deine Zeit: " + (int)levelTimer;
		}
//		if (GUI.Button (Rect (1, Screen.height - 25, 75, 25), "Restart")) {
//			Application.LoadLevel (0);
//		}


		if (script.distanceToDog > minDistance) {
			if (gameBegun) {
				if (!waitActive && !waited) {
					StartCoroutine (Wait ());
					waited = true;
				}
				if (canSwitch) {
					start_img_go.SetActive (false);
					instructions_img_go.SetActive (true);
					canSwitch = false;
					gameBegun = false;
					waited = false;
				}
			}
	
			if (playerTapped ()) {
				instructions_img_go.SetActive (false);
				show_time = true;
			}


		} else {
			updateTimer = false;
			timer.text = "Super! Du hast nur " + System.Math.Round (levelTimer, 2) + " Sekunden gebraucht!";
			canEnd = true;
			waitActive = false;
			start_img_go.SetActive (false);
			instructions_img_go.SetActive (false);
			end_img_go.SetActive (true);
		
		}
		if (canEnd) {
			if (!waitActive && !waited) {
				StartCoroutine (Wait ());   
				waited = true;
			}
			if (canSwitch) {
				levelEnded ();
			}

		}
	}

	public void levelEnded ()
	{
		Application.LoadLevel (0);
		end_img_go.SetActive (false);
		canSwitch = false;
		canEnd = false;
		waited = false;
		playerPressedNo = false;
		gameBegun = false;
	}

	IEnumerator Wait ()
	{
		waitActive = true;
		yield return new WaitForSeconds (3.0f);
		canSwitch = true;
		waitActive = false;
	}

	bool playerTapped ()
	{
		//if (Input.touchCount > 0) 
		if (Input.anyKeyDown)
			return true;
		else
			return false;
	}
}

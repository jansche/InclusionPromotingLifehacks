using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.ImageEffects;

public class MapMovementToEffect : MonoBehaviour {


	private FirstPersonController fpc;
	private BlurOptimized blur;
//	private MotionBlur motionBlur;
	private AudioSource[] sources;
	private GameObject dog;
	private AudioSource bark;
	private CanvasRenderer left;
	private CanvasRenderer right;
	private Camera cam;
	private Fisheye fish;

	private float currentUpdateTime = 0f;
	private float updateStep = 0.1f;
	private int sampleDataLength = 1024;

	private float clipLoudness;
	private float[] clipSampleData;
	public float distanceToDog = 200f;

	void Awake() {
		clipSampleData = new float[sampleDataLength];
	}

	// Use this for initialization
	void Start () {
		fpc = GameObject.FindObjectOfType<FirstPersonController>();
		blur = GetComponentInChildren<BlurOptimized> ();
//		motionBlur = GetComponentInChildren<MotionBlur> ();
		sources = GameObject.FindGameObjectWithTag ("GameController").GetComponents<AudioSource>();
		dog = GameObject.FindGameObjectWithTag ("dog");
		bark = dog.GetComponent<AudioSource> ();
		left = GameObject.FindGameObjectWithTag ("left").GetComponent<CanvasRenderer> ();
		right = GameObject.FindGameObjectWithTag ("right").GetComponent<CanvasRenderer>();
		cam = GetComponentInChildren<Camera> ();
		fish = GetComponentInChildren<Fisheye> ();

		left.SetAlpha (0f);
		right.SetAlpha (0f);

	}
	
	// Update is called once per frame
	void Update () {
		Vector3 heading = dog.transform.position - cam.transform.position;
		distanceToDog = heading.magnitude;
		float a = AngleDir(cam.transform.forward, heading, cam.transform.up);
		float factor = Mathf.Abs (fpc.m_MoveDir.x) + Mathf.Abs (fpc.m_MoveDir.z);

		// -------------------------------------------//

		// While standing still, flash direction for bark
		if (!fpc.m_Jumping && factor == 0) {
			currentUpdateTime += Time.deltaTime;
			if (currentUpdateTime >= updateStep) {
				currentUpdateTime = 0f;
				bark.clip.GetData (clipSampleData, bark.timeSamples);
				clipLoudness = 0f;
				foreach (var sample in clipSampleData)
					clipLoudness += Mathf.Abs (sample);
				clipLoudness /= sampleDataLength;

			}


			// when standing backwards to dog, don't show visual hints.
			if (Vector3.Angle (cam.transform.forward, heading) < 125.0f) {
				// if player looks to the left of dog, flash the right screen
				if (a > 0) {
					left.SetAlpha (0f);
					right.SetAlpha (clipLoudness*3);
				} else {
					// otherwise the other side.
					right.SetAlpha (0f);
					left.SetAlpha (clipLoudness*3);
				}
			} else {
				left.SetAlpha (0f);
				right.SetAlpha (0f);
			}
		}

		// -------------------------------------------//

		// blur the screen while moving
		blur.blurSize = factor;
		//motionBlur.blurAmount = factor / 7;
		blur.blurIterations = (int) Map(factor , 0, 10, 0, 4);
		float fishval = (float)Map (factor, 0, 10, 0, 0.5);
		fish.strengthX = fishval;
		fish.strengthY = fishval;
		// map the street noise sound source
		sources [1].volume = (float) Map (factor, 0, 5, 0.1, 0.4);


	}

	// Processing like Map function
	public double Map(double x, double in_min, double in_max, double out_min, double out_max)
	{
		return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
	}

	float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up) {
		Vector3 perp = Vector3.Cross(fwd, targetDir);
		float dir = Vector3.Dot(perp, up);

		if (dir > 0f) {
			return 1f;
		} else if (dir < 0f) {
			return -1f;
		} else {
			return 0f;
		}
	}
}


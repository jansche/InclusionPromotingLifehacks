using UnityEngine;
using System.Collections;

public class SimpleKalmanFilter : MonoBehaviour
{
	public double Q = 0.000001;
	public double R = 0.01;
	public double P = 1, X = 0, K;

	void Start()
	{
		// PerfomKalmanTest();
		Input.gyro.enabled = true;
	}

	void Update()
	{
		// Example use

		//double psudoVar = Random.Range(0, 100);
//		double psudoVar = Input.gyro.attitude.x;
//		float kalmanVar = (float)Mathf.Round((float)KalmanUpdate(psudoVar));
//		transform.rotation = new Quaternion (Input.gyro.attitude.x, Input.gyro.attitude.y, Input.gyro.attitude.z, Input.gyro.attitude.w);
		transform.rotation = Input.gyro.attitude;

		//print("Psudo: " + psudoVar + " , " + kalmanVar);
	}

	void measurementUpdate()
	{
		K = (P + Q) / (P + Q + R);
		P = R * (P + Q) / (R + P + Q);
	}

	public double KalmanUpdate(double measurement)
	{
		measurementUpdate();

		double result = X + (measurement - X) * K;
		X = result;
		return result;
	}

	void PerfomKalmanTest()
	{
		int[] DATA = new int[16] { 0, 0, 0, 0, 1, 1, 2, 2, 2, 100, 10, 2, 3, 3, 1, 0 };

		for (int i = 0; i < DATA.Length; i++)
		{
			print(Mathf.Round((float)KalmanUpdate(DATA[i])) + ",");
		}
	}
}
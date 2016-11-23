using UnityEngine;


/// <summary>
/// Gyroscope demo. Attach to a visible object or camera.
/// </summary>
public class GyroTest:MonoBehaviour {

	public double Q = 0.000001;
	public double R = 0.01;
	public double P = 1, X = 0, K;

	void Start() {

		Input.gyro.enabled = true;

	}


	void Update() {
//		transform.rotation = new Quaternion (Input.gyro.rotationRateUnbiased.x, Input.gyro.rotationRateUnbiased.y, Input.gyro.rotationRateUnbiased.z, 0);
//		double kalmanVar = (double)Mathf.Round((float)KalmanUpdate(psudoVar));
		double kalmanX = KalmanUpdate(Input.gyro.attitude.x);
		double kalmanY = KalmanUpdate(Input.gyro.attitude.y);
		double kalmanZ = KalmanUpdate(Input.gyro.attitude.z);
		double kalmanW = KalmanUpdate(Input.gyro.attitude.w);
//		transform.rotation = new Quaternion ((float)kalmanX, (float)kalmanY, (float)kalmanZ, Input.gyro.attitude.w);
		transform.rotation = new Quaternion(Input.gyro.gravity.x, Input.gyro.gravity.y, Input.gyro.gravity.z, 0);
		Debug.Log (Input.gyro.gravity);
//		transform.rotation = Input.gyro.attitude;

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
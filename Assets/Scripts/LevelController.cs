using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	private int lv = 0;
	private GameObject levelCube;
	private float rotationAmount = 90;
	public GameObject[] levels = new GameObject[4];
	Quaternion targetRot;

	// Use this for initialization
	void Start () {
		levelCube = GameObject.Find ("LevelCube");
		targetRot = levelCube.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		levelCube.transform.rotation = Quaternion.Slerp (levelCube.transform.rotation, targetRot,
			Time.deltaTime * 5f);
	}

	//Rotate the cube to the next level
	public void RotateLevel()
	{
		lv++;
		levels [lv].SetActive(true);
		targetRot *= Quaternion.AngleAxis (rotationAmount, Vector3.back);
		levels [lv-1].SetActive(false);
	}
}

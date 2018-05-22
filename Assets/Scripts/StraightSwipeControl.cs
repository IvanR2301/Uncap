using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightSwipeControl : MonoBehaviour {

	Animator anim;
	private bool tap, swLeft, swRight, swUp, swDown;
	public bool leftLock, rightLock, upLock, downLock = false;

	private TimerandPoint tp;
	private LevelController lc;

	private Vector2 startTouch, swDelta;
	private bool isDraging = false;
	private bool pop = false;
	private bool resetTime = false;

	public ParticleSystem thisParticle;
	private Rigidbody rb;
	private Vector3 startPosition;

	private float corkForce = 300;
	private float deltaY = 0;
	public float maxDelta = 2;

	// Use this for initialization
	void Start () {
		//thisObj = this.gameObject;
		rb = gameObject.GetComponent<Rigidbody> ();
		anim = GameObject.Find ("Wine Bottle").GetComponent<Animator> ();
		startPosition = gameObject.transform.localPosition;
		tp = GameObject.Find ("GM").GetComponent<TimerandPoint>();
		lc = GameObject.Find ("GM").GetComponent<LevelController>();
	}
	
	// Update is called once per frame
	void Update () {
		tap = swLeft = swRight = swUp = swDown = false;

		//Computer input
		if (Input.GetMouseButtonDown (0)) {
			tap = true;
			isDraging = true;
			startTouch = Input.mousePosition;
			anim.SetBool("touch_On", true);
		} 
		else if (Input.GetMouseButtonUp (0)) {
			isDraging = false;
			anim.SetBool("touch_On", false);
			Reset ();
		}

		//if (Input.GetMouseButton (0)) {
		//	anim.SetBool("touch_On", true);
		//}

		//Mobile input
		if (Input.touches.Length > 0) {
			if (Input.touches [0].phase == TouchPhase.Began) {
				tap = true;
				isDraging = true;
				startTouch = Input.touches [0].position;
			} 
			else if (Input.touches [0].phase == TouchPhase.Ended ||
			         Input.touches [0].phase == TouchPhase.Canceled) 
			{
				isDraging = false;
				Reset ();
			}
		}

		//Calculate the distance
		swDelta = Vector2.zero;
		if(isDraging)
		{
			if (Input.touches.Length > 0) {
				swDelta = Input.touches [0].position - startTouch;
			} 
			else if (Input.GetMouseButton (0)) {
				swDelta = (Vector2)Input.mousePosition - startTouch;
			}
		}

		//Max swipe length
		if (swDelta.magnitude > 50) {
			//Direction
			float x = swDelta.x;
			float y = swDelta.y;

			//Left or right
			if (Mathf.Abs (x) > Mathf.Abs (y)) {
				if (x < 0) {
					swLeft = true;
				} else {
					swRight = true;
				}
			}
			//Up or down
			else {
				if (y < 0) {
					swDown = true;
				} else {
					swUp = true;
				}
			}
		}
		deltaY = (gameObject.transform.localPosition.y - startPosition.y) * 1000f;

		if (deltaY < maxDelta) {
			MotionControl();
		} 
		//When the bottle is opened
		else 
		{
			anim.SetTrigger("Pop");
			if (!resetTime) {
				//Count points and go to next level
				tp.StartCoroutine (tp.NextLevel());
				resetTime = true;
			}
			rb.isKinematic = false;
			if (!pop) {
				rb.AddForce (transform.up * corkForce);
				rb.AddForce (transform.right * 50);
			}
			if (!pop) 
			{
				thisParticle.Play();
			}
			pop = true;
		}
	}

	void Reset()
	{
		startTouch = swDelta = Vector2.zero;
		isDraging = false;
	}

	void MotionControl()
	{
		Vector3 movePosition = gameObject.transform.position;
		if (swLeft && leftLock) {
			movePosition += Vector3.left * 0.05f;
		}
		if (swRight && rightLock) {
			movePosition += Vector3.right * 0.05f;
		}
		if (swUp && upLock) {
			movePosition += Vector3.up * 0.05f;
		}
		if (swDown && downLock) {
			movePosition += Vector3.back * 0.05f;
		}
		gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, movePosition,
			3f * Time.deltaTime);
	}
}

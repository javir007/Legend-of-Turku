using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour {

	[SerializeField] private float moveSpeed = 10.0f;
	[SerializeField] private GameObject fireTrail;

	private CharacterController characterController;
	private Animator anim;
	private bool movement = true;

	private const string FIRE1 = "Fire1";
	private const string FIRE2 = "Fire2";
	private BoxCollider[] swordColliders;

	private float vertical;
	private float horizontal;

	private ParticleSystem fireTRailParticles;




	// Use this for initialization
	void Start () {
		fireTrail.SetActive (false);
		characterController = GetComponent<CharacterController> ();	
		anim = GetComponent<Animator> ();
		swordColliders = GetComponentsInChildren<BoxCollider> ();
	}
	
	// Update is called once per frame
	void Update () {
		vertical = Input.GetAxis ("Vertical");
		horizontal = Input.GetAxis ("Horizontal");

		if (!GameManager.instance.GameOver) {
			Vector3 moveDirection = new Vector3(horizontal,0,vertical);
			if (movement) {
				characterController.SimpleMove (moveDirection * moveSpeed);
			}
			float animValue = Mathf.Abs(vertical) + Mathf.Abs(horizontal);
		
			anim.SetFloat("Forward", animValue, .1f, Time.deltaTime);

			if(Input.GetAxisRaw(FIRE1)!=0){
				anim.Play("DoubleChop");

			}

			if(Input.GetAxisRaw(FIRE2)!=0){
				anim.Play("SpinAttack");
			}
		}
	}

	void FixedUpdate(){
		if (!GameManager.instance.GameOver) {
			turning ();
		}
	}

	void turning(){

		Vector3 turnDir = new Vector3 (CrossPlatformInputManager.GetAxisRaw ("Horizontal"), 0f, CrossPlatformInputManager.GetAxisRaw ("Vertical"));

		if (turnDir != Vector3.zero) {
			// Create a vector from the player to the point on the floor the raycast from the mouse hit.
			Vector3 playerToMouse = (transform.position + turnDir) - transform.position;

			// Ensure the vector is entirely along the floor plane.
			playerToMouse.y = 0f;

			// Create a quaternion (rotation) based on looking down the vector from the player to the mouse.
			Quaternion newRotatation = Quaternion.LookRotation (playerToMouse);
			//transform.rotation = newRotatation;
			transform.rotation = Quaternion.Lerp(transform.rotation, newRotatation, Time.deltaTime * 10f);

		}

	}

	public void BeginAttack(){
		foreach (var weapon in swordColliders) {
			weapon.enabled = true;
		}

	}

	public void EndAttack(){
		foreach (var weapon in swordColliders) {
			weapon.enabled = false;
		}
	}

	public void disableControl(){
		movement = false;
	}

	public void enableControl(){
		movement = true;
	}

	public void SpeedPowerUp(){
		StartCoroutine (fireTrailRoutine ());
	}

	IEnumerator fireTrailRoutine(){
		fireTrail.SetActive (true);
		moveSpeed = 10f;
		yield return new WaitForSeconds (10f);
		moveSpeed = 5f;
		fireTRailParticles = fireTrail.GetComponent<ParticleSystem> ();
		var em = fireTRailParticles.emission;
		em.enabled = false;
		yield return new WaitForSeconds (3f);
		em.enabled = true;
		fireTrail.SetActive (false);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour {

	public float damage = 5.0f;

	// Use this for initialization

	void Start () {
		transform.Rotate( new Vector3( 0, 0, 45 ) );
	}
	
	// Update is called once per frame
	Vector3 rotationEuler;
	void Update(){
		rotationEuler+= Vector3.forward*90*Time.deltaTime; //increment 30 degrees every second
		transform.rotation = Quaternion.Euler(rotationEuler);

		}
	void OnTriggerEnter(Collider Enemy){
		if(Enemy.gameObject.CompareTag("enemy"))
		{
			Enemy.gameObject.SendMessage("OnDamage", damage);
		}
	}



}

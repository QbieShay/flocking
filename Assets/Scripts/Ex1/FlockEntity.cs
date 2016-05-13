using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

class FlockEntity : MonoBehaviour{

Vector2 target;
public static float lookRadius=3f;
public static float totalForce = 2f;
public static float rotationSpeed = 3f;
Rigidbody2D rb;
Vector2 targetOrientation;
public static List<FlockEntity> flocklings;
public static Vector2 centerOfFlock;

void Start(){
	rb = GetComponent<Rigidbody2D>();
	StartCoroutine ( recalculateDirection());
	if( flocklings == null ) {
		flocklings = new List<FlockEntity>();
		foreach ( GameObject go in GameObject.FindGameObjectsWithTag("Flockling")){
			flocklings.Add( go.GetComponent<FlockEntity>());
		}

	}
}

void Update(){
	if(Input.GetKeyDown( KeyCode.Mouse0 )){
			target = Camera.main.ScreenToWorldPoint( Input.mousePosition ) ;
			}
			//FIXME
			//transform.forward = Vector2.Lerp(transform.forward, targetOrientation, 0.5f*Time.deltaTime  );
			//rb.AddForce( transform.forward * totalForce * Time.deltaTime);
			transform.position += (Vector3)targetOrientation*totalForce*Time.deltaTime;
	}

IEnumerator recalculateDirection(){
	while (true){
		Vector2 separation=Vector2.zero, cohesion=Vector2.zero;
		int neighbours=0;
		Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position,lookRadius);
		foreach (Collider2D c in cols ) {
			if(c.gameObject.tag != "Flockling") continue;
			separation += (Vector2)transform.position -(Vector2) c.gameObject.transform.position;
			neighbours++;
		}
		separation /= (float)neighbours;
		foreach(FlockEntity f in flocklings){
			centerOfFlock += (Vector2)f.transform.position;
		}
		centerOfFlock/= (float)flocklings.Count;
		Debug.Log("Flockling " + gameObject.name + " separation factor is " + separation ) ;
		targetOrientation =0.5f* (Vector2)((Vector2)target - (Vector2)transform.position).normalized + 0.3f*(Vector2)separation + (Vector2)(centerOfFlock-(Vector2)transform.position).normalized * 0.2f;
		yield return new WaitForSeconds(0.3f+UnityEngine.Random.Range(0f,0.1f));
	}
}

}

using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

class FlockEntity : MonoBehaviour{

Vector2 target;
public static float lookRadius=0.5f;
public static float totalForce = 2f;
public static float rotationSpeed = 30f;
Rigidbody2D rb;
Vector2 targetOrientation;
public static List<FlockEntity> flocklings;
public static Vector2 centerOfFlock;

void Start(){
	float b = 0f;
	float a = 1f / b;
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
			//Debug.Log ( "Rotating " + transform.forward);
			//transform.Rotate( new Vector3(0,0,1),Vector2.Angle(transform.up,targetOrientation)
					//*Time.deltaTime*rotationSpeed,Space.World);	
			transform.position += (Vector3)targetOrientation*totalForce*Time.deltaTime;
	}

//void FixedUpdate(){
////rb.AddForce( transform.up*totalForce*Time.deltaTime*100f);
//rb.velocity = transform.up*totalForce*Time.deltaTime*100f;

//}

IEnumerator recalculateDirection(){
	while (true){
		Vector2 separation=Vector2.zero, cohesion=Vector2.zero;
		int neighbours=0;	
		int yieldcount=0;
		float dist;
		while (flocklings == null ) {
			yield return null;
		}
		foreach(FlockEntity f in flocklings){
			
			dist = Vector2.Distance( f.transform.position, transform.position );
			//dist = Mathf.Clamp(dist, 0.000001f, lookRadius);
			if(dist < lookRadius ) {
				neighbours++;
				centerOfFlock += (Vector2)f.transform.position;
				separation+= ( (Vector2)transform.position - (Vector2) f.transform.position )*(dist!=0f?lookRadius/dist:10000000f);
			}	
			if(++yieldcount > 5) {
				yieldcount =0;
				yield return null;
			}
		}
		centerOfFlock/= (float)neighbours;
		cohesion = (Vector2) centerOfFlock - (Vector2) transform.position;
		targetOrientation = (Vector2)(separation + 0.3f*cohesion+ ((Vector2) target - 0.7f*(Vector2)transform.position)).normalized;
		//yield return new WaitForSeconds(UnityEngine.Random.Range(0f,0.3f));
		yield return null;
	}
}

}

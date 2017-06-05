using UnityEngine;
using System;
using System.Collections;

class CraigBoids: MonoBehaviour{
	Vector3 target;
	float tooClose;
	FlockGlobals FLOCKGLOB;
	Vector3 dir ;

	void Start(){
		FLOCKGLOB = FlockGlobals.instance;
		StartCoroutine(flock());
		dir = transform.forward;
	}

	void Update(){
		//Move toward calculated direction
		transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(
					transform.forward , dir, FLOCKGLOB.ROTATIONSPEED*Time.deltaTime, 0.0f ));
		transform.Translate( transform.forward * FLOCKGLOB.SPEED * Time.deltaTime);
	}

	public IEnumerator flock(){
		while (true){
			target = FLOCKGLOB.FLOCK_TARGET.transform.position;
			Vector3 tmp= Vector3.zero, tmpdir= Vector3.zero;
			Collider[] neighbours = Physics.OverlapSphere(transform.position,FLOCKGLOB.SIGHT_RADIUS
					);
			tooClose = FLOCKGLOB.SIGHT_RADIUS ;
			tmp += alignment(neighbours);
			yield return null;
			tmp += cohesion(neighbours);
			yield return null;
			tmp += separation(neighbours);
			tmp.Normalize();
			tmpdir = target- transform.position;
			tmpdir.Normalize();
			//The nearer the closer boid, the greater the flock component factor.
			dir = Mathf.Clamp((1f - tooClose/FLOCKGLOB.SIGHT_RADIUS),0.0f,0.6f) * tmp +
			   	Mathf.Clamp((tooClose/FLOCKGLOB.SIGHT_RADIUS),0.4f,0.8f)*tmpdir;
			//The two component are clamped because, otherwise, if the flocklings 
			//are too close to one another they will ignore the target
			dir.Normalize();
			yield return null;
		}
	}

	Vector3 alignment(Collider[] neighbours){
		Vector3 alignment = Vector3.zero;
		foreach (Collider c in neighbours){
			if(!isVisible ( c.gameObject.transform.position)) 
				continue;
			alignment+= c.gameObject.transform.forward;	
		}
		alignment.Normalize();
		return alignment;
	}

	Vector3 cohesion( Collider[] neighbours){
		Vector3 cohesion= Vector3.zero;
		float counter=0f;
		foreach(Collider c in neighbours) {
			if(!isVisible ( c.gameObject.transform.position)) 
				continue;
			counter+= 1f;
			cohesion+= c.transform.position;
		}
		cohesion-= transform.position;
		cohesion/=(float)counter;
		cohesion = cohesion - transform.position;
		cohesion.Normalize();
		return cohesion;
	}

	Vector3 separation( Collider[] neighbours){
		Vector3 separation=Vector3.zero;
		Vector3 tmp;
		foreach (Collider c in neighbours) {
			if( c.transform.position == transform.position || 
					!isVisible ( c.gameObject.transform.position))
			   	continue;
			tmp = (transform.position - c.gameObject.transform.position);
			tooClose = tmp.magnitude<tooClose?tmp.magnitude:tooClose;
			
			separation+= tmp.normalized / Mathf.Clamp(tmp.magnitude,0.0001f, float.PositiveInfinity);
		}

		return separation.normalized;
	}

	bool isVisible( Vector3 pos ) {

		return Vector3.Dot(
				transform.forward, 
				(pos - transform.position).normalized)> 
				FLOCKGLOB.VISION_ANGLE;
	}
}

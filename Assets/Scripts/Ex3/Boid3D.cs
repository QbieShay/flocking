using UnityEngine;
using System.Collections;
namespace Flock{
	class Boid3D : MonoBehaviour{
		public FlockGlobals FLOCKGLOB;
		Vector3 velocity;
		Vector3? target;
		public static float speed = 5f;
		public static float rotationSpeed = 50f;


		public void Start() {
			FLOCKGLOB = FlockGlobals.instance;
			target = null;
			StartCoroutine( recalculateDirection());
		} 
		public void Update() {
			if(Input.GetKeyDown(KeyCode.Mouse0)){
				if(Random.Range(0f,1f)<0.5f){
					GetComponent<Renderer>().material.color = Color.red;
					target = FLOCKGLOB.FLOCK_TARGET.transform.position;
				}
				else{
					GetComponent<Renderer>().material.color = Color.white;
					target = null;
				}
			}

			//float angle = Vector3.Angle( transform.forward, dir);
			//if( Vector3.Dot(transform.forward,dir)<0f ) angle = 180f-angle;
			//Debug.Log( "Desired direction for flockling is "+dir );
			//transform.Rotate( Vector3.Cross(transform.forward, dir) ,angle*rotationSpeed*3f*Time.deltaTime );
			//transform.Translate( transform.forward*speed*Time.deltaTime);
			transform.position += (Vector3)dir*speed*Time.deltaTime;
			transform.forward = Vector3.Lerp(dir,transform.forward, 0.5f*Time.deltaTime);

		}
		Vector3 dir;

		IEnumerator recalculateDirection(){
			while(true){
				//Collider2D[] flocklings = Physics2D.OverlapCircleAll(transform.position, FLOCKGLOB.SIGHT_RADIUS, 
				//LayerMask.NameToLayer("Flocklings"));
				Collider[] flocklings = Physics.OverlapSphere( transform.position, FLOCKGLOB.SIGHT_RADIUS);
				Vector3 position = (Vector3)transform.position, tmp;
				Vector3 align=Vector3.zero, cohesion=position, separation=Vector3.zero;
				float scale = (float)flocklings.Length -1f;
				int count = 0;
				foreach (Collider c in flocklings ) {
					tmp = position - (Vector3) c.transform.position;
					if(c.gameObject.tag != "Flockling" 
							|| Vector3.Dot(transform.forward,-(tmp.normalized))<FLOCKGLOB.VISION_ANGLE 
							) 
						continue;
					align +=(Vector3) c.transform.forward;
					if (position!=(Vector3)c.transform.position){
						separation += tmp.normalized/tmp.magnitude;
					}
					cohesion += (Vector3)c.transform.position;
					++count;
				}
				Vector3 center;
				scale = Mathf.Clamp( scale, 1f, float.PositiveInfinity);
				scale = (float)count;
				center = (Vector3)cohesion/scale;
				align/=scale;
				//Debug.Log("Count "+count);
				//Debug.Log("Align: " + align);
				//Debug.Log("Separation " +separation);
				//Debug.Log("Center " + center);
				//Debug.Log("Cohesion " + (center - position));
				dir = FLOCKGLOB.ALIGNMENT*align
					+ FLOCKGLOB.SEPARATION*separation
					+ FLOCKGLOB.COHESION*(center - position);

				dir.Normalize();
				if(target!=null){
				dir += (target.Value - center).normalized;
				} else {
					dir+= (center-position).normalized;
				}
				dir.Normalize();
				//Debug.DrawLine( transform.position, transform.position+ (Vector3) dir*4f);
				yield return null;}
		}
	}}

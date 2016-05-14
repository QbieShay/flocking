using UnityEngine;
using System.Collections;
namespace Flock{
	class Boid : MonoBehaviour{

		public static float SIGHT_RADIUS = 5f;
		public static float ALIGNMENT =  1f;
		public static float COHESION   = 1f;
		public static float SEPARATION   = 0f;

		Vector2 velocity;
		Vector2 target;
		public static float speed = 3f;
		public static float rotationSpeed = 10f;

		public void Start() {
			StartCoroutine( recalculateDirection());
		} 
		public void Update() {
			if(Input.GetKey(KeyCode.Mouse0)){
				target = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			}

			Debug.Log( "Target is :" + target );
			transform.Rotate( new Vector3 (0,0,1f), Vector3.Angle( transform.up, dir )*rotationSpeed*Time.deltaTime );
			transform.position += transform.up*speed*Time.deltaTime;
		}
		Vector2 dir;

		IEnumerator recalculateDirection(){
			while(true){
				Collider2D[] flocklings = Physics2D.OverlapCircleAll(transform.position, SIGHT_RADIUS);
				Vector2 align= transform.up ,cohesion= transform.position, separation = Vector2.zero;

				foreach (Collider2D c in flocklings ) {
					if( c.gameObject.name != "Flockling" ) continue;
					align +=(Vector2) c.transform.up;
					separation += ( (Vector2) transform.position - (Vector2) c.transform.position )
						* SIGHT_RADIUS / Vector2.Distance ( transform.position , c.transform.position ) ;
					cohesion += (Vector2)c.transform.position;
				}
				dir = ALIGNMENT*align/(float)flocklings.Length + SEPARATION*separation/(float)flocklings.Length 
					+ ((Vector2)cohesion/(float)flocklings.Length - (Vector2) transform.position)*COHESION;
				dir.Normalize();
				dir += ((Vector2)target - (Vector2)transform.position).normalized;
				dir /= 2f;
				Debug.Log( dir ) ;
				Debug.DrawLine( transform.position, transform.position+ (Vector3) dir*4f);
				yield return null;}
		}
	}}

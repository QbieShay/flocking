using UnityEngine;

class FlockGlobals:MonoBehaviour{
	public static FlockGlobals instance;
		
		public GameObject flockling;
		public  float SIGHT_RADIUS = 2f;
		public  float ALIGNMENT =  1f;
		public  float COHESION   = 1f;
		public  float SEPARATION   = 0.1f;
		[Range(-1f, 1f)]
		public 	float VISION_ANGLE = -0.7f;
		public  float SPEED = 10f;
		public float ROTATIONSPEED = 5f;
		public GameObject FLOCK_TARGET ;

	void Awake(){
		instance = this;
		flockling = GameObject.FindWithTag("Flockling");
	}
	
	void Update(){
		if(Input.GetKey( KeyCode.Z)){
			SIGHT_RADIUS-=.2f;
		}
		if(Input.GetKey( KeyCode.A)){
			SIGHT_RADIUS+=.2f;
		}
		if(Input.GetKey( KeyCode.X)){
			ALIGNMENT-=.1f;
		}
		if(Input.GetKey( KeyCode.S)){
			ALIGNMENT+=.1f;
		}
		if(Input.GetKey( KeyCode.C)){
			COHESION-=.1f;
		}
		if(Input.GetKey( KeyCode.D)){
			COHESION+=.1f;
		}
		if(Input.GetKey( KeyCode.V)){
			SEPARATION-=.1f;
		}
		if(Input.GetKey( KeyCode.F)){
			SEPARATION+=.1f;
		}
		if(Input.GetKey( KeyCode.B)){
			VISION_ANGLE-=.1f;
		}
		if(Input.GetKey( KeyCode.G)){
			VISION_ANGLE+=.1f;
		}

		if(Input.GetKeyDown(KeyCode.Q)){
			GameObject.Instantiate(flockling);
		}



	}
}

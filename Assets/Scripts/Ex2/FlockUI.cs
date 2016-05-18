using UnityEngine;
using UnityEngine.UI;
class FlockUI:MonoBehaviour{
	FlockGlobals glob;
	
	void Start (){
		glob = FlockGlobals.instance;
	}

	void Update(){
		GetComponent<Text>().text= "SIGHT_RADIUS :"+glob.SIGHT_RADIUS+"\nALIGNMENT :"+glob.SEPARATION+"\nCOHESION :"
			+glob.ALIGNMENT+"\nSEPARATION :"+
			glob.COHESION+"\nDOTVISION :"+glob.VISION_ANGLE;
	}

}

using UnityEngine;
using System.Collections;

public class BHV_Walk : MonoBehaviour {
	
	public float StepLength=1.0f; // speed
	
	private Vector3 prevousDirection = Vector3.zero; //internal use for random walk
	
	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	
	private Vector3 giveRandomDirection()
	{
		float posX = UnityEngine.Random.Range(-1.0F*this.StepLength, this.StepLength); 
		float posY = 0;
		float posZ = UnityEngine.Random.Range(-1.0F*this.StepLength, this.StepLength); 		
		
		//this.transform.Rotate(posX,0,0);
		
		return new Vector3(posX,posY,posZ);
	}
	
	public void walkAtRandom()
	{	
		//Do not fall at the end of the world!
		
		if( Mathf.Abs(this.gameObject.transform.position.x)>125)
			this.prevousDirection.x *= -1;
		
		if( Mathf.Abs(this.gameObject.transform.position.z)>125)
			this.prevousDirection.z *= -1;
		
		Vector3 OffSet = giveRandomDirection();
		if(prevousDirection == Vector3.zero)
		{
			//this.transform.Translate(OffSet);
			this.prevousDirection = OffSet;
		}
		//this.transform.Translate(OffSet);

		else
		{
			
			OffSet = this.prevousDirection + (OffSet*0.1f) ;
			//this.transform.Translate(this.prevousDirection + OffSet*0.1f, Space.World);
			
			this.gameObject.transform.LookAt( this.gameObject.transform.position + OffSet);
			this.gameObject.transform.Translate(0,0,StepLength,Space.Self);
			
		}
			
		//this.transform.Rotate(posX,0,0);
	}
	
	public float goTo(Vector3 _Goal)
	{
		Vector3 delta = _Goal - this.gameObject.transform.position;
		float DistanceToGoal = delta.magnitude;
		
		this.prevousDirection = delta;
//		if(DistanceToGoal<StepLength)
//		{
//			Debug.Log ("goal reached");
//			return StepLength;
//		}
//		else
//		{
			this.gameObject.transform.LookAt(_Goal);
			
			//this.gameObject.transform.Translate(delta.normalized*StepLength,Space.World);
			this.gameObject.transform.Translate(0,0,StepLength,Space.Self);
		
			return DistanceToGoal;
		//}
		
	}
	
}

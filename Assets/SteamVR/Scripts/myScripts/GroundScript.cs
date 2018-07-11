using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundScript : MonoBehaviour
{
  void OnTriggerEnter(Collider collisionObj)
  {
		if(collisionObj.transform.name == "SnowBall")
		{
      Destroy(collisionObj.gameObject);
    } 
  }

}

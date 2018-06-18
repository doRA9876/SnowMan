using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColorOnHit : MonoBehaviour
{

  // Use this for initialization
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnCollisionEnter(Collision collisionObj)
  {
		if(collisionObj.transform.name == "Floor")
		{
      return;
    }

    Vector3 collisionPos = collisionObj.transform.position;
    Vector3 thisPos = GetComponent<Transform>().position;
    Vector3 relativePos = GetRelativeCoordinate(thisPos, collisionPos);

    Debug.Log(transform.name + ":" + relativePos);
    JointObj(collisionObj.rigidbody, relativePos);
  }

  Vector3 GetRelativeCoordinate(Vector3 mainPos, Vector3 sidePos)
  {
    return new Vector3(sidePos.x - mainPos.x, sidePos.y - mainPos.y, sidePos.z - mainPos.z);
  }

  void JointObj(Rigidbody collisionName, Vector3 relativePos)
  {
    var hingeJoint = gameObject.AddComponent<HingeJoint>();
    hingeJoint.connectedBody = collisionName;
    hingeJoint.axis = relativePos;
  }
}

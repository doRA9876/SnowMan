using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
  private GameObject collision;
  private SphereCollider sphereCollider;
  private List<FixedJoint> HingeJointList = new List<FixedJoint> { null, null, null };

  // Use this for initialization
  IEnumerator Start()
  {
    collision = transform.Find("Collision").gameObject;
    sphereCollider = collision.GetComponent<SphereCollider>();

    yield return new WaitForSeconds(5);

    sphereCollider.isTrigger = true;
  }

  void OnTriggerEnter(Collider collisionObj)
  {
    string collsionObjName = collisionObj.transform.name;
    if (collsionObjName == "SnowBall" || collsionObjName == "HardSnowBall")
    {
      int emptyIndex = -1;

      for (int index = 0; index < HingeJointList.Count; index++)
      {
        if (HingeJointList[index] == null)
        {
          emptyIndex = index;
          break;
        }
      }

      if (emptyIndex == -1) return;

      // Debug.Log("joint!");
      FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
      fixedJoint.connectedBody = collisionObj.gameObject.GetComponent<Rigidbody>();
      fixedJoint.breakForce = 800;
      fixedJoint.breakTorque = 800;
      HingeJointList[emptyIndex] = fixedJoint;
    }
  }

  void OnJointBreak(float force)
  {
    // Debug.Log(force);
  }
}


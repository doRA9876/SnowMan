using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
  private GameObject _collision;
  private SphereCollider _sphereCollider;
  private List<FixedJoint> _hingeJointList = new List<FixedJoint> { null, null, null };

  // Use this for initialization
  IEnumerator Start()
  {
    _collision = transform.Find("Collision").gameObject;
    _sphereCollider = _collision.GetComponent<SphereCollider>();

    yield return new WaitForSeconds(5);

    _sphereCollider.isTrigger = true;
  }

  void OnTriggerEnter(Collider collisionObj)
  {
    string collsionObjName = collisionObj.transform.name;
    if (collsionObjName == "SnowBall" || collsionObjName == "HardSnowBall")
    {
      int emptyIndex = -1;

      for (int index = 0; index < _hingeJointList.Count; index++)
      {
        if (_hingeJointList[index] == null)
        {
          emptyIndex = index;
          break;
        }
      }

      if (emptyIndex == -1) return;

      FixedJoint fixedJoint = gameObject.AddComponent<FixedJoint>();
      fixedJoint.connectedBody = collisionObj.gameObject.GetComponent<Rigidbody>();
      fixedJoint.breakForce = 800;
      fixedJoint.breakTorque = 800;
      _hingeJointList[emptyIndex] = fixedJoint;
    }
  }

  void OnJointBreak(float force)
  {
    // Debug.Log(force);
  }
}


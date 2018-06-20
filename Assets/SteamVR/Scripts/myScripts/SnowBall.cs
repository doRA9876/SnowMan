using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
  private bool isJoint;

  // Use this for initialization
  void Start()
  {
    isJoint = false;
  }

  // Update is called once per frame
  void Update()
  {

  }

  void OnCollisionEnter(Collision collisionObj)
  {
    if (collisionObj.transform.name == "Ground" || collisionObj.transform.name == "Controller (right)")
    {
      return;
    }

    Vector3 collisionPos = collisionObj.transform.position;
    Vector3 thisPos = GetComponent<Transform>().position;
    Vector3 relativePos = GetRelativeCoordinate(thisPos, collisionPos);

    Debug.Log(transform.name + ":" + relativePos);
    if (!isJoint)
    {
      JointObj(collisionObj.rigidbody, relativePos);
    }
  }

  Vector3 GetRelativeCoordinate(Vector3 mainPos, Vector3 sidePos)
  {
    return new Vector3(sidePos.x - mainPos.x, sidePos.y - mainPos.y, sidePos.z - mainPos.z);
  }

  void JointObj(Rigidbody collisionName, Vector3 relativePos)
  {
    isJoint = true;
    var hingeJoint = gameObject.AddComponent<FixedJoint>();
    hingeJoint.connectedBody = collisionName;
    hingeJoint.axis = relativePos;
  }

  // void JointObj(Rigidbody collisionName, Vector3 sidePos)
  // {
  //   TransParent(gameObject, collisionName.gameObject);
  // }


  /// <summary>
  /// Childオブジェクトの親を変更
  /// </summary>
  /// <param 親オブジェクト="Parent"></param>
  /// <param 子オブジェクト="Child"></param>
  public static void TransParent(GameObject Parent, GameObject Child)
  {
    Child.transform.parent = Parent.transform;
  }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
  public int id;
  private GameObject system;

  //保持しているFixedJointを管理するディクショナリ
  private Dictionary<string, FixedJoint> JointDictionary;

  // Use this for initialization
  void Start()
  {
    system = GameObject.Find("System");
    JointDictionary = new Dictionary<string, FixedJoint>();
  }

  //衝突時に呼び出される事前に準備された関数
  void OnCollisionEnter(Collision collisionObj)
  {
    if (collisionObj.gameObject.transform.name == "SnowBall")
    {
      if (!JointDictionary.ContainsKey(collisionObj.gameObject.transform.name) || JointDictionary[collisionObj.gameObject.transform.name] == null)
      {
        var tmpJoint = gameObject.AddComponent<FixedJoint>();
        tmpJoint.connectedBody = collisionObj.rigidbody;
        tmpJoint.breakForce = 100;
        tmpJoint.breakTorque = 5;
        if (!JointDictionary.ContainsKey(collisionObj.gameObject.transform.name))
        {
          JointDictionary.Add(collisionObj.gameObject.transform.name, tmpJoint);
        }
        else
        {
          JointDictionary[collisionObj.gameObject.transform.name] = tmpJoint;
        }

        //接続処理はシステム側で行う。idが小さいほうがシステムへメッセージを送る。
        if (collisionObj.gameObject.GetComponent<SnowBall>().id > this.id)
        {
          system.GetComponent<SystemScript>().GroupingSnowBall(gameObject, collisionObj.gameObject);
        }
      }
    }

    if (collisionObj.gameObject.transform.name == "Ground")
    {
      Destroy(gameObject);
    }
  }

  //相対座標を取得するが今は使わない。
  Vector3 GetRelativeCoordinate(Vector3 mainPos, Vector3 sidePos)
  {
    return new Vector3(sidePos.x - mainPos.x, sidePos.y - mainPos.y, sidePos.z - mainPos.z);
  }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBall : MonoBehaviour
{
  public int id;
  private GameObject system;
  private bool isChild;

  // Use this for initialization
  void Start()
  {
    system = GameObject.Find("System");
    isChild = true;
  }

  //衝突時に呼び出される事前に準備された関数
  void OnCollisionEnter(Collision collisionObj)
  {
    if (collisionObj.transform.name != "SnowBall")
    {
      return;
    }

    //接続処理はシステム側で行う。idが小さいほうがシステムへメッセージを送る。
    if(collisionObj.gameObject.GetComponent<SnowBall>().id > this.id){
      system.GetComponent<SystemScript>().GroupingSnowBall(gameObject, collisionObj.gameObject);
    }
  }

  //相対座標を取得するが今は使わない。
  Vector3 GetRelativeCoordinate(Vector3 mainPos, Vector3 sidePos)
  {
    return new Vector3(sidePos.x - mainPos.x, sidePos.y - mainPos.y, sidePos.z - mainPos.z);
  }
}


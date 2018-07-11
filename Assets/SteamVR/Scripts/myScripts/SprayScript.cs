using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SprayScript : MonoBehaviour
{
  private GameObject controllerLeft;
  void Start()
  {
    controllerLeft = GameObject.Find("Controller (left)");
  }
  void OnTriggerStay(Collider collisionObj)
  {
    if (collisionObj.transform.name == "SnowBall")
    {
      ExecuteEvents.Execute<CtrlLeftInterface>(
        target: controllerLeft,
        eventData: null,
        functor: (reciever, y) => reciever.MakeHard(collisionObj.gameObject)
      );
    }
  }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorSprayScript : MonoBehaviour {

	private GameObject _controllerLeft;
  void Start()
  {
    _controllerLeft = GameObject.Find("Controller (left)");
  }
  void OnTriggerStay(Collider collisionObj)
  {
    if (collisionObj.transform.name == "SnowBall" || collisionObj.transform.name == "HardSnowBall")
    {
      ExecuteEvents.Execute<IVRControllerLeft>(
        target: _controllerLeft,
        eventData: null,
        functor: (reciever, y) => reciever.ChangeColor(collisionObj.gameObject)
      );
    }
  }
}

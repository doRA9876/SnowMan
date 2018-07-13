using UnityEngine;
using UnityEngine.EventSystems;

public class SprayScript : MonoBehaviour
{
  private GameObject _controllerLeft;
  void Start()
  {
    _controllerLeft = GameObject.Find("Controller (left)");
  }
  void OnTriggerStay(Collider collisionObj)
  {
    if (collisionObj.transform.name == "SnowBall")
    {
      ExecuteEvents.Execute<InterfaceCtrlLeft>(
        target: _controllerLeft,
        eventData: null,
        functor: (reciever, y) => reciever.MakeHard(collisionObj.gameObject)
      );
    }
  }
}

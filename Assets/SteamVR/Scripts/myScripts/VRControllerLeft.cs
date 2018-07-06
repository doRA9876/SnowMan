using UnityEngine;
using System.Collections.Generic;

public class VRControllerLeft : MonoBehaviour
{
  private SphereCollider controllerCollider;
  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private Vector2 touchPosition;

  private void Start()
  {
    controllerCollider = gameObject.GetComponent<SphereCollider>();
  }

  void Update()
  {
    trackedObject = GetComponent<SteamVR_TrackedObject>();
    device = SteamVR_Controller.Input((int)trackedObject.index);
    touchPosition = device.GetAxis();

    //トリガーを握っている
    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {
     
    }

    //トリガーを離した
    if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
    {

    }

    //タッチパッドをクリック
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
    {
      if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
      {
        if (touchPosition.y > 0)
        {
          //タッチパッド上をクリックした場合の処理
          
        }
        else
        {
          //下をクリック
         
        }
      }
      else
      {
        if (touchPosition.x > 0)
        {
          //タッチパッド右をクリックした場合の処理
         
        }
        else
        {
          //左をクリック 
          
        }
      }
    }
  }

  void OnTriggerEnter(Collider collisionObj)
  {
    
  }

  void OnTriggerStay(Collider collisionObj)
  {
    
  }

  void OnTriggerExit(Collider collisionObj)
  {

  }
}
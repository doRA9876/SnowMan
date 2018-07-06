using UnityEngine;
using System.Collections.Generic;

public class VRControllerLeft : MonoBehaviour
{
  private int toolMode;
  private Dictionary<string, bool> flag = new Dictionary<string, bool>();
  private GameObject ctrlModel, icepick;
  private SphereCollider controllerCollider;
  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private Vector2 touchPosition;

  private void Start()
  {
    ctrlModel = GameObject.Find("Model");
    icepick = GameObject.Find("icepick");

    controllerCollider = gameObject.GetComponent<SphereCollider>();

    flag.Add("icepick", false);
    flag.Add("ctrlModel", true);

    ctrlModel.SetActive(flag["ctrlModel"]);
    icepick.SetActive(flag["icepick"]);

    toolMode = 2;
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
          toolMode = 0;
          ChangeTool(toolMode);
        }
        else
        {
          //下をクリック
          toolMode = 2;
          ChangeTool(toolMode);
        }
      }
      else
      {
        if (touchPosition.x > 0)
        {
          //タッチパッド右をクリックした場合の処理
          toolMode = 1;
          ChangeTool(toolMode);
        }
        else
        {
          //左をクリック 
          toolMode = 3;
          ChangeTool(toolMode);

        }
      }
    }
  }

  void OnTriggerEnter(Collider collisionObj)
  {
    if (collisionObj.gameObject.name == "SnowBall")
    {
      if (toolMode == 3)
      {
        MakeHard(collisionObj.gameObject);
      }
    }
  }

  void OnTriggerStay(Collider collisionObj)
  {

  }

  void OnTriggerExit(Collider collisionObj)
  {

  }

  //使用する道具を変更
  void ChangeTool(int n)
  {
    switch (n)
    {
      case 0:
        break;

      case 1:
        break;

      case 3:
        flag["ctrlModel"] = false;
        flag["icepick"] = true;
        break;

      default:
        flag["ctrlModel"] = true;
        flag["icepick"] = false;
        break;
    }
    ctrlModel.SetActive(flag["ctrlModel"]);
    icepick.SetActive(flag["icepick"]);
  }

  void MakeHard(GameObject obj)
  {
    Debug.Log("make hard snowball");

    Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
    rigidbody.useGravity = false;
    rigidbody.isKinematic = true;
  }
}
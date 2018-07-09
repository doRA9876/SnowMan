using UnityEngine;
using System.Collections.Generic;

public class VRControllerLeft : MonoBehaviour
{
  private int toolMode;
  private Dictionary<string, bool> flag = new Dictionary<string, bool>();
  private GameObject ctrlModel, spray, sprayParticle;
  private SphereCollider controllerCollider;
  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private Vector2 touchPosition;

  private void Start()
  {
    ctrlModel = transform.Find("Model").gameObject;
    spray = transform.Find("spray").gameObject;
    sprayParticle = spray.transform.Find("SprayParticle").gameObject;

    controllerCollider = gameObject.GetComponent<SphereCollider>();

    flag.Add("spray", false);
    flag.Add("ctrlModel", true);
    flag.Add("sprayParticle", false);

    ctrlModel.SetActive(flag["ctrlModel"]);
    spray.SetActive(flag["spray"]);
    sprayParticle.SetActive(flag["sprayParticle"]);

    toolMode = 2;
  }

  void Update()
  {
    trackedObject = GetComponent<SteamVR_TrackedObject>();
    device = SteamVR_Controller.Input((int)trackedObject.index);
    touchPosition = device.GetAxis();

    //トリガーを握った
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    {
      flag["sprayParticle"] = true;
      sprayParticle.SetActive(flag["sprayParticle"]);
    }

    //トリガーを握っている
    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {

    }

    //トリガーを離した
    if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
    {
      flag["sprayParticle"] = false;
      sprayParticle.SetActive(flag["sprayParticle"]);
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
      if (toolMode == 3 && flag["sprayParticle"])
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
        flag["spray"] = true;
        break;

      default:
        flag["ctrlModel"] = true;
        flag["spray"] = false;
        break;
    }
    ctrlModel.SetActive(flag["ctrlModel"]);
    spray.SetActive(flag["spray"]);
  }

  void MakeHard(GameObject obj)
  {
    Debug.Log("make hard snowball");

    Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
    rigidbody.useGravity = false;
    rigidbody.isKinematic = true;
  }
}
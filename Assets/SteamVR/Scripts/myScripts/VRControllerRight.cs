using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class VRControllerRight : MonoBehaviour, InterfaceCtrlRight
{
  private int toolMode;
  private bool groundTouched, isCanvasMode;
  private GameObject system, grabObj, bucket, icepick, scoop, ctrlModel, manual;
  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private Vector2 touchPosition;

  private void Start()
  {
    system = GameObject.Find("System");
    bucket = transform.Find("bucket").gameObject;
    icepick = transform.Find("icepick").gameObject;
    scoop = transform.Find("scoop").gameObject;
    ctrlModel = transform.Find("Model").gameObject;
    manual = transform.Find("Manual").gameObject;

    groundTouched = false;
    isCanvasMode = false;

    toolMode = 2;

    bucket.SetActive(false);
    scoop.SetActive(false);
    icepick.SetActive(false);
    ctrlModel.SetActive(true);
    manual.SetActive(false);
  }

  void Update()
  {
    trackedObject = GetComponent<SteamVR_TrackedObject>();
    device = SteamVR_Controller.Input((int)trackedObject.index);
    touchPosition = device.GetAxis();

    if (isCanvasMode)
    {
      if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
      {
        if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
        {
          if (touchPosition.y > 0)
          {
            //タッチパッド上をクリックした場合の処理
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeHead(-1)
            );
          }
          else
          {
            //下をクリック
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeHead(1)
            );
}
        }
        else
        {
          if (touchPosition.x > 0)
          {
            //タッチパッド右をクリックした場合の処理
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeValue(10)
            );
          }
          else
          {
            //左をクリック 
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeValue(-10)
            );
          }
        }
      }
    }
    else
    {
      if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
      {
        manual.SetActive(true);
      }

      if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
      {
        manual.SetActive(false);
      }

      //トリガーを離した
      if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
      {
        ReleaseObject();
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
  }

  void OnTriggerEnter(Collider collisionObj)
  {
    string objName = collisionObj.gameObject.name;
    if ((objName == "SnowBall" || objName == "HardSnowBall") && toolMode == 3)
    {
      ShaveSnow(collisionObj.gameObject);
    }
  }

  void OnTriggerStay(Collider collisionObj)
  {
    if (collisionObj.gameObject.name == "SnowBall")
    {
      if (device != null && device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && toolMode == 2)
      {
        GrabObject(collisionObj.gameObject);
      }
    }

    if (collisionObj.gameObject.name == "Ground")
    {
      if (!groundTouched)
      {
        groundTouched = true;
      }
    }
  }

  void OnTriggerExit(Collider collisionObj)
  {
    if (collisionObj.gameObject.name == "Ground" && (device != null && device.GetPress(SteamVR_Controller.ButtonMask.Trigger)))
    {
      if (groundTouched && toolMode == 0)
      {
        ScoopSnow();
      }
    }
    groundTouched = false;
  }

  void GrabObject(GameObject obj)
  {
    if (grabObj != null) return;

    this.grabObj = obj;

    FixedJoint fixedJoint = gameObject.GetComponent<FixedJoint>();
    fixedJoint.connectedBody = obj.GetComponent<Rigidbody>();
  }

  void ReleaseObject()
  {
    FixedJoint fixedJoint = gameObject.GetComponent<FixedJoint>();
    if (fixedJoint != null)
    {
      fixedJoint.connectedBody = null;
    }
    this.grabObj = null;
  }

  //使用する道具を変更
  void ChangeTool(int n)
  {
    bucket.SetActive(false);
    scoop.SetActive(false);
    icepick.SetActive(false);
    ctrlModel.SetActive(false);

    switch (n)
    {
      case 0:
        bucket.SetActive(true);
        break;

      case 1:
        scoop.SetActive(true);
        break;

      case 3:
        icepick.SetActive(true);
        break;

      default:
        ctrlModel.SetActive(true);
        break;
    }
  }

  //バケツのみ可能
  void ScoopSnow()
  {
    var currentPosition = gameObject.transform.position;
    var systemScript = system.GetComponent<SystemScript>();

    for (int i = -2; i <= 2; i++)
    {
      for (int j = -2; j <= 2; j++)
      {
        for (int k = -2; k <= 2; k++)
        {
          systemScript.CreateSnowBall(new Vector3(currentPosition.x + i * 0.08f, currentPosition.y + k * 0.08f, currentPosition.z + j * 0.08f));
        }
      }
    }
  }

  //アイスピックのみ可能
  void ShaveSnow(GameObject obj)
  {
    Destroy(obj);
  }

  public void SwitchCanvasMode(bool flag)
  {
    isCanvasMode = flag;
    if (isCanvasMode)
    {
      ChangeTool(2);
    }
  }
}
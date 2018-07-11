using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VRControllerRight : MonoBehaviour
{
  private int toolMode;
  private bool isOpenMenu, groundTouched;
  private GameObject system, canvas, grabObj, bucket, icepick, scoop, ctrlModel;
  private SphereCollider controllerCollider;
  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private Vector2 touchPosition;

  private void Start()
  {
    system = GameObject.Find("System");
    canvas = transform.Find("Canvas").gameObject;
    bucket = transform.Find("bucket").gameObject;
    icepick = transform.Find("icepick").gameObject;
    scoop = transform.Find("scoop").gameObject;
    ctrlModel = transform.Find("Model").gameObject;
    controllerCollider = gameObject.GetComponent<SphereCollider>();

    isOpenMenu = false;
    groundTouched = false;

    toolMode = 2;

    bucket.SetActive(false);
    scoop.SetActive(false);
    icepick.SetActive(false);
    ctrlModel.SetActive(true);
  }

  void Update()
  {
    trackedObject = GetComponent<SteamVR_TrackedObject>();
    device = SteamVR_Controller.Input((int)trackedObject.index);
    touchPosition = device.GetAxis();

    //トリガーを握っている
    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {
      if (isOpenMenu)
      {
        system.GetComponent<SystemScript>().CreateSnowBall(new Vector3(Random.Range(1.0f, -1.0f), 0.06f, Random.Range(1.0f, -1.0f)));
      }
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
          isOpenMenu = !isOpenMenu;
          canvas.SetActive(isOpenMenu);
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
    if(grabObj != null) return;

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
}
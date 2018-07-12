using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class VRControllerLeft : MonoBehaviour, InterfaceCtrlLeft
{
  private int toolMode;
  private Color drawColor;
  private GameObject ctrlModel, spray, colorSpray, sprayParticle, colorSprayParticle, colorCanvas, controllerRight, system;
  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private Vector2 touchPosition;

  private void Start()
  {
    ctrlModel = transform.Find("Model").gameObject;
    spray = transform.Find("spray").gameObject;
    sprayParticle = spray.transform.Find("SprayParticle").gameObject;
    colorSpray = transform.Find("colorSpray").gameObject;
    colorSprayParticle = colorSpray.transform.Find("SprayParticle").gameObject;
    colorCanvas = GameObject.Find("ColorCanvas");
    controllerRight = GameObject.Find("Controller (right)");
    system = GameObject.Find("System");
    drawColor = system.GetComponent<ColorCanvasScript>().GetColor();

    toolMode = 2;

    spray.SetActive(false);
    colorSpray.SetActive(false);
    ctrlModel.SetActive(true);
    sprayParticle.SetActive(false);
    colorSprayParticle.SetActive(false);
    colorCanvas.SetActive(false);
  }

  void Update()
  {
    trackedObject = GetComponent<SteamVR_TrackedObject>();
    device = SteamVR_Controller.Input((int)trackedObject.index);
    touchPosition = device.GetAxis();

    //トリガーを握った
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    {
      sprayParticle.SetActive(true);
      colorSprayParticle.SetActive(true);
    }

    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
    {
      Debug.Log("Will Open Menu");
    }

    if (device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
    {
      Debug.Log("Will Close Menu");
    }

    //トリガーを離した
    if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
    {
      sprayParticle.SetActive(false);
      colorSprayParticle.SetActive(false);
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
    ctrlModel.SetActive(false);
    spray.SetActive(false);
    colorSpray.SetActive(false);
    colorCanvas.SetActive(false);
    ExecuteEvents.Execute<InterfaceCtrlRight>(
      target: controllerRight,
      eventData: null,
      functor: (reciever, y) => reciever.SwitchCanvasMode(false)
    );

    switch (n)
    {
      case 0:
        ctrlModel.SetActive(true);
        colorCanvas.SetActive(true);
        ExecuteEvents.Execute<InterfaceCtrlRight>(
          target: controllerRight,
          eventData: null,
          functor: (reciever, y) => reciever.SwitchCanvasMode(true)
        );
        break;

      case 1:
        colorSpray.SetActive(true);
        break;

      case 3:
        spray.SetActive(true);
        break;

      default:
        ctrlModel.SetActive(true);
        break;
    }
    
    drawColor = system.GetComponent<ColorCanvasScript>().GetColor();
  }

  //インタフェース実装
  public void MakeHard(GameObject obj)
  {
    Rigidbody rigidbody = obj.GetComponent<Rigidbody>();
    rigidbody.useGravity = false;
    rigidbody.isKinematic = true;

    obj.transform.name = "HardSnowBall";
  }

  public void ChangeColor(GameObject obj)
  {
    obj.GetComponent<Renderer>().material.color = drawColor;
  }
}
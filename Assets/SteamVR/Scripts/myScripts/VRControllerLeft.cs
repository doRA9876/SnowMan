using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class VRControllerLeft : MonoBehaviour, InterfaceCtrlLeft
{
  private int _toolMode;
  private Color _drawColor;
  private GameObject _ctrlModel, _spray, _colorSpray, _sprayParticle, _colorSprayParticle, _colorCanvas, _controllerRight, _system;
  private SteamVR_Controller.Device _device;
  private SteamVR_TrackedObject _trackedObject;
  private Vector2 _touchPosition;

  private void Start()
  {
    _ctrlModel = transform.Find("Model").gameObject;
    _spray = transform.Find("Spray").gameObject;
    _sprayParticle = _spray.transform.Find("SprayParticle").gameObject;
    _colorSpray = transform.Find("ColorSpray").gameObject;
    _colorSprayParticle = _colorSpray.transform.Find("SprayParticle").gameObject;
    _colorCanvas = GameObject.Find("ColorCanvas");
    _controllerRight = GameObject.Find("Controller (right)");
    _system = GameObject.Find("System");
    _drawColor = _system.GetComponent<ColorCanvasScript>().GetColor();

    _toolMode = 2;

    _spray.SetActive(false);
    _colorSpray.SetActive(false);
    _ctrlModel.SetActive(true);
    _sprayParticle.SetActive(false);
    _colorSprayParticle.SetActive(false);
    _colorCanvas.SetActive(false);
  }

  void Update()
  {
    _trackedObject = GetComponent<SteamVR_TrackedObject>();
    _device = SteamVR_Controller.Input((int)_trackedObject.index);
    _touchPosition = _device.GetAxis();

    //トリガーを握った
    if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    {
      _sprayParticle.SetActive(true);
      _colorSprayParticle.SetActive(true);
    }

    if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
    {
      Debug.Log("Will Open Menu");
    }

    if (_device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
    {
      Debug.Log("Will Close Menu");
    }

    //トリガーを離した
    if (_device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
    {
      _sprayParticle.SetActive(false);
      _colorSprayParticle.SetActive(false);
    }

    //タッチパッドをクリック
    if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
    {
      if (_touchPosition.y / _touchPosition.x > 1 || _touchPosition.y / _touchPosition.x < -1)
      {
        if (_touchPosition.y > 0)
        {
          //タッチパッド上をクリックした場合の処理
          _toolMode = 0;
          ChangeTool(_toolMode);
        }
        else
        {
          //下をクリック
          _toolMode = 2;
          ChangeTool(_toolMode);
        }
      }
      else
      {
        if (_touchPosition.x > 0)
        {
          //タッチパッド右をクリックした場合の処理
          _toolMode = 1;
          ChangeTool(_toolMode);
        }
        else
        {
          //左をクリック 
          _toolMode = 3;
          ChangeTool(_toolMode);

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
    _ctrlModel.SetActive(false);
    _spray.SetActive(false);
    _colorSpray.SetActive(false);
    _colorCanvas.SetActive(false);
    ExecuteEvents.Execute<InterfaceCtrlRight>(
      target: _controllerRight,
      eventData: null,
      functor: (reciever, y) => reciever.SwitchCanvasMode(false)
    );

    switch (n)
    {
      case 0:
        _ctrlModel.SetActive(true);
        _colorCanvas.SetActive(true);
        ExecuteEvents.Execute<InterfaceCtrlRight>(
          target: _controllerRight,
          eventData: null,
          functor: (reciever, y) => reciever.SwitchCanvasMode(true)
        );
        break;

      case 1:
        _colorSpray.SetActive(true);
        break;

      case 3:
        _spray.SetActive(true);
        break;

      default:
        _ctrlModel.SetActive(true);
        break;
    }
    
    _drawColor = _system.GetComponent<ColorCanvasScript>().GetColor();
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
    obj.GetComponent<Renderer>().material.color = _drawColor;
  }
}
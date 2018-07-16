using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class VRControllerRight : MonoBehaviour, InterfaceCtrlRight
{
  private int _toolMode;
  private bool _groundTouched, _isCanvasMode;
  private GameObject _system, _grabObj, _bucket, _icepick, _scoop, _ctrlModel, _help, _controllerLeft;
  private SteamVR_Controller.Device _device;
  private SteamVR_TrackedObject _trackedObject;
  private Vector2 _touchPosition;

  private void Start()
  {
    _system = GameObject.Find("System");
    _bucket = transform.Find("Bucket").gameObject;
    _icepick = transform.Find("Icepick").gameObject;
    _scoop = transform.Find("Scoop").gameObject;
    _ctrlModel = transform.Find("Model").gameObject;
    _help = transform.Find("Help").gameObject;
    _controllerLeft = GameObject.Find("Controller (left)");

    _groundTouched = false;
    _isCanvasMode = false;

    _toolMode = 2;

    _bucket.SetActive(false);
    _scoop.SetActive(false);
    _icepick.SetActive(false);
    _ctrlModel.SetActive(true);
    _help.SetActive(false);
  }

  void Update()
  {
    _trackedObject = GetComponent<SteamVR_TrackedObject>();
    _device = SteamVR_Controller.Input((int)_trackedObject.index);
    _touchPosition = _device.GetAxis();

    if (_isCanvasMode)
    {
      if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
      {
        if (_touchPosition.y / _touchPosition.x > 1 || _touchPosition.y / _touchPosition.x < -1)
        {
          if (_touchPosition.y > 0)
          {
            //タッチパッド上をクリックした場合の処理
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: _system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeHead(-1)
            );
          }
          else
          {
            //下をクリック
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: _system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeHead(1)
            );
}
        }
        else
        {
          if (_touchPosition.x > 0)
          {
            //タッチパッド右をクリックした場合の処理
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: _system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeValue(10)
            );
          }
          else
          {
            //左をクリック 
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: _system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeValue(-10)
            );
          }
        }
      }
    }
    else
    {
      if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
      {
        _help.SetActive(true);
      }

      if (_device.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
      {
        _help.SetActive(false);
      }

      //トリガーを離した
      if (_device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
      {
        ReleaseObject();
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
  }

  void OnTriggerEnter(Collider collisionObj)
  {
    string objName = collisionObj.gameObject.name;
    if ((objName == "SnowBall" || objName == "HardSnowBall") && _toolMode == 3)
    {
      ShaveSnow(collisionObj.gameObject);
    }
  }

  void OnTriggerStay(Collider collisionObj)
  {
    if (collisionObj.gameObject.name == "SnowBall")
    {
      if (_device != null && _device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && _toolMode == 2)
      {
        GrabObject(collisionObj.gameObject);
      }
    }

    if (collisionObj.gameObject.name == "Ground")
    {
      if (!_groundTouched)
      {
        _groundTouched = true;
      }
    }
  }

  void OnTriggerExit(Collider collisionObj)
  {
    if (collisionObj.gameObject.name == "Ground" && (_device != null && _device.GetPress(SteamVR_Controller.ButtonMask.Trigger)))
    {
      if (_groundTouched && _toolMode == 0)
      {
        ScoopSnow();
      }
    }
    _groundTouched = false;
  }

  void GrabObject(GameObject obj)
  {
    if (_grabObj != null) return;

    this._grabObj = obj;

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
    this._grabObj = null;
  }

  //使用する道具を変更
  void ChangeTool(int n)
  {
    _bucket.SetActive(false);
    _scoop.SetActive(false);
    _icepick.SetActive(false);
    _ctrlModel.SetActive(false);

    switch (n)
    {
      case 0:
        _bucket.SetActive(true);
        break;

      case 1:
        _scoop.SetActive(true);
        ExecuteEvents.Execute<InterfaceCtrlLeft>(
          target: _controllerLeft,
          eventData: null,
          functor: (reciever, y) => reciever.ActiveScoop()
        );
        break;

      case 3:
        _icepick.SetActive(true);
        break;

      default:
        _ctrlModel.SetActive(true);
        break;
    }
  }

  //バケツのみ可能
  void ScoopSnow()
  {
    var currentPosition = gameObject.transform.position;
    var systemScript = _system.GetComponent<SystemScript>();

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
    _isCanvasMode = flag;
    if (_isCanvasMode)
    {
      ChangeTool(2);
    }
  }
}
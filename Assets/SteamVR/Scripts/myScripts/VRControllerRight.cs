using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public sealed class VRControllerRight : MonoBehaviour, InterfaceCtrlRight
{
  private int _toolMode;
  private bool _groundTouched, _isCanvasMode;
  private GameObject _grabObj;
  public GameObject _system, _bucket, _icepick, _scoop, _ctrlModel, _help, _controllerLeft;
  private SteamVR_Controller.Device _device;
  private SteamVR_TrackedObject _trackedObject;
  private Vector2 _touchPosition;

  private void Start()
  {
    if (_system == null || _bucket == null || _icepick == null || _scoop == null || _ctrlModel == null || _help == null || _controllerLeft == null)
    {
      Debug.LogError("There are unattached variables!");
    }

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
        switch (GetTouchPositionOfGamePad())
        {
          case 0:
            //When clicking on the top
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: _system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeHead(1)
            );
            break;

          case 2:
            //When clicking on the under
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: _system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeHead(-1)
            );
            break;

          case 1:
            //When clicking on the ringt
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: _system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeValue(10)
            );
            break;

          case 3:
            //When clicking on the left
            ExecuteEvents.Execute<InterfaceColorCanvas>(
              target: _system,
              eventData: null,
              functor: (reciever, y) => reciever.ChangeValue(-10)
            );
            break;
          default:
            break;
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
        _toolMode = GetTouchPositionOfGamePad();
        ChangeTool(_toolMode);
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
      if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && _toolMode == 2)
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
    if (collisionObj.gameObject.name == "Ground")
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

  /************************************************
  Return an integer depending on where you clicked

                  up:0
            left:3      right:1
                  down:2

  ************************************************/
  public int GetTouchPositionOfGamePad()
  {
    if (_touchPosition.y / _touchPosition.x > 1 || _touchPosition.y / _touchPosition.x < -1)
    {
      if (_touchPosition.y > 0)
      {
        //Clicked Up
        return 0;
      }
      else
      {
        //Clicked Down
        return 2;
      }
    }
    else
    {
      if (_touchPosition.x > 0)
      {
        //Clickted Right
        return 1;
      }
      else
      {
        //Clickted Left 
        return 3;
      }
    }
  }
}
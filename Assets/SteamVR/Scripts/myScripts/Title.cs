using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
  private bool _flag1, _flag2;
  private int _seqNum;
  private GameObject _light, _vrCamera, _rightController, _leftController, _system, _titleText, _tips, _particleSystem;
  private SteamVR_Controller.Device _device;
  private SteamVR_TrackedObject _trackedObject;
  private Vector2 _touchPosition;
  // Use this for initialization
  void Start()
  {
    _flag1 = false;
    _flag2 = false;
    _seqNum = 0;
    _light = GameObject.Find("Light");
    _vrCamera = GameObject.Find("[CameraRig]");
    _rightController = gameObject;
    _leftController = GameObject.Find("Controller (left)");
    _system = GameObject.Find("System");
    _titleText = GameObject.Find("TitleText");
    _tips = GameObject.Find("Tips");
    _particleSystem = GameObject.Find("Particle System");
    _trackedObject = _rightController.GetComponent<SteamVR_TrackedObject>();
    _device = SteamVR_Controller.Input((int)_trackedObject.index);
    _touchPosition = _device.GetAxis();
  }

  // Update is called once per frame
  void Update()
  {
    if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    {
      _flag1 = true;
      _seqNum++;
    }


    // if (_flag1 && !_flag2)
    // {
    //   _vrCamera.transform.position = new Vector3(_vrCamera.transform.position.x, _vrCamera.transform.position.y - 0.05f, _vrCamera.transform.position.z + 0.05f);


    //   if (_vrCamera.transform.position.y < 0 && _vrCamera.transform.position.z > 0)
    //   {
    //     _flag2 = true;
    //     _vrCamera.transform.position = new Vector3(0, 0, 0);
    //     gameObject.GetComponent<VRControllerRight>().enabled = true;
    //     gameObject.GetComponent<Title>().enabled = false;
    //   }
    // }

    SequenceCtrl();
  }

  void SequenceCtrl()
  {
    switch (_seqNum)
    {
      case 0:
        InitTitle();
        _seqNum++;
        break;

      case 1:
        break;

      case 2:
        _titleText.SetActive(true);
        _seqNum++;
        break;

      case 3:
        MoveTitleText();
        break;

      case 4:
        _titleText.SetActive(false);
        _tips.SetActive(true);
        _seqNum++;
        break;

      case 5:
        if (MoveGround()) _seqNum += 2;
        break;

      case 6:
        _seqNum--;
        break;

      case 7:
        TerminateTitle();
        break;

      default:
        break;
    }
  }

  void InitTitle()
  {
    ChangeAllActive(false);
    _titleText.SetActive(false);
  }

  void TerminateTitle()
  {
    ChangeAllActive(true);
    gameObject.GetComponent<Title>().enabled = false;
  }

  void ChangeAllActive(bool status)
  {
    _light.SetActive(status);
    _rightController.GetComponent<VRControllerRight>().enabled = status;
    _leftController.GetComponent<VRControllerLeft>().enabled = status;
    _particleSystem.SetActive(status);
    _tips.SetActive(status);
  }

  void MoveTitleText()
  {
    _titleText.transform.position = new Vector3(_titleText.transform.position.x, _titleText.transform.position.y + 0.005f, _titleText.transform.position.z + 0.008f);
  }

  bool MoveGround()
  {
    _vrCamera.transform.position = new Vector3(_vrCamera.transform.position.x, _vrCamera.transform.position.y - 0.03f, _vrCamera.transform.position.z + 0.03f);

    if (_vrCamera.transform.position.y < 0 && _vrCamera.transform.position.z > 0)
    {
      _vrCamera.transform.position = new Vector3(0, 0, 0);
      return true;
    }

    return false;
  }
}

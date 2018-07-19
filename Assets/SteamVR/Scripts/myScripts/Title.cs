using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
  private int _seqNum;
  private GameObject _light, _vrCamera, _rightController, _leftController, _system, _titleText, _navigationText, _backgroundText,  _particleSystem, _bgm, _navigationCanvas, _room;
  private SteamVR_Controller.Device _device;
  private SteamVR_TrackedObject _trackedObject;
  private Vector2 _touchPosition;
  // Use this for initialization
  void Start()
  {
    _seqNum = 0;
    _light = GameObject.Find("Light");
    _vrCamera = GameObject.Find("[CameraRig]");
    _rightController = gameObject;
    _leftController = GameObject.Find("Controller (left)");
    _system = GameObject.Find("System");
    _particleSystem = GameObject.Find("Particle System");
    _bgm = GameObject.Find("BGM");
    _navigationCanvas = GameObject.Find("NavigationCanvas");
    _titleText = _navigationCanvas.transform.Find("TitleText").gameObject;
    _navigationText = _navigationCanvas.transform.Find("NavigationText").gameObject;
    _room = GameObject.Find("Room");
    _trackedObject = _rightController.GetComponent<SteamVR_TrackedObject>();
    _device = SteamVR_Controller.Input((int)_trackedObject.index);
    _touchPosition = _device.GetAxis();
  }

  // Update is called once per frame
  void Update()
  {
    if (_device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    {
      _seqNum++;
    }

    SequenceCtrl();
  }

  void SequenceCtrl()
  {
    switch (_seqNum)
    {
      case 0:
        InitTitle();
        _titleText.SetActive(false);
        _seqNum++;
        break;

      case 1:
        break;

      case 2:
        _bgm.SetActive(true);
        _navigationText.GetComponent<Text>().text = "移動します。";
        _navigationText.SetActive(false);
        _titleText.SetActive(true);
        _seqNum++;
        break;

      case 3:
        MoveTitleText();
        break;

      case 4:
        _bgm.SetActive(false);
        _navigationText.SetActive(true);
        _titleText.SetActive(false);
        break;

      case 5:
        ToLightUp();
        if (MoveGround()) _seqNum += 2;
        break;

      case 6:
        _seqNum--;
        break;

      case 7:
        MoveRoom();
        if(_room.transform.position.y < -5){
          _seqNum += 2;
        }
        break;

      case 8:
        _seqNum--;
        break;

      case 9:
        TerminateTitle();
        _navigationCanvas.SetActive(false);
        break;

      default:
        break;
    }
  }

  void InitTitle()
  {
    ChangeAllActive(false);
    _bgm.SetActive(false);
    _navigationCanvas.SetActive(true);
  }

  void TerminateTitle()
  {
    ChangeAllActive(true);
    gameObject.GetComponent<Title>().enabled = false;
  }

  void ChangeAllActive(bool status)
  {
    _rightController.GetComponent<VRControllerRight>().enabled = status;
    _leftController.GetComponent<VRControllerLeft>().enabled = status;
    _particleSystem.SetActive(status);
  }

  void MoveTitleText()
  {
    _titleText.GetComponent<RectTransform>().position = new Vector3(_titleText.transform.position.x, _titleText.transform.position.y + 0.005f, _titleText.transform.position.z + 0.005f);
  }

  bool MoveGround()
  {
    _vrCamera.transform.position = new Vector3(_vrCamera.transform.position.x, _vrCamera.transform.position.y - 0.01f, _vrCamera.transform.position.z + 0.01f);

    if (_vrCamera.transform.position.y < 0 && _vrCamera.transform.position.z > 0)
    {
      _vrCamera.transform.position = new Vector3(0, 0, 0);
      return true;
    }

    return false;
  }

  void ToLightUp()
  {
    Color currentColor = _light.GetComponent<Light>().color;

    _light.GetComponent<Light>().color = new Color(currentColor.r + 0.001f, currentColor.g + 0.001f, currentColor.b + 0.001f);
  }

  void MoveRoom()
  {
    _room.transform.position = new Vector3(_room.transform.position.x, _room.transform.position.y - 0.01f, _room.transform.position.z);
  }
}

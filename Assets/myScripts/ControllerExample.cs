using UnityEngine;

public class ControllerExample : MonoBehaviour
{
  private GameObject controllerR;
  private GameObject cube1;
  private GameObject prefab;

  private void Start()
  {
    controllerR = GameObject.Find("Controller (right)");
    cube1 = GameObject.Find("Cube1");
    prefab = (GameObject)Resources.Load("Prefabs/Cube");
  }

  void Update()
  {
    var trackedObject = GetComponent<SteamVR_TrackedObject>();
    var device = SteamVR_Controller.Input((int)trackedObject.index);

    if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
    {
      Debug.Log("トリガーを浅く引いた");
    }
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    {
      Debug.Log("トリガーを深く引いた");
    }

    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {
      Debug.Log("トリガーを握っている");
      var Pos3 = controllerR.transform.position;
      Debug.Log(Pos3.x);
      Debug.Log(Pos3.y);
      Debug.Log(Pos3.z);
      // GripCube();
      Instantiate(prefab, new Vector3(0, 2, 0), Quaternion.identity);
    }

    if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
    {
      Debug.Log("トリガーを離した");
    }
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
    {
      Debug.Log("タッチパッドをクリックした");
    }
    if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
    {
      Debug.Log("タッチパッドをクリックしている");
    }
    if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
    {
      Debug.Log("タッチパッドをクリックして離した");
    }
    if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
    {
      Debug.Log("タッチパッドに触った");
    }
    if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
    {
      Debug.Log("タッチパッドを離した");
    }
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
    {
      Debug.Log("メニューボタンをクリックした");
    }
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
    {
      Debug.Log("グリップボタンをクリックした");
    }

    if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
    {
      //Debug.Log("トリガーを浅く引いている");
    }
    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {
      //Debug.Log("トリガーを深く引いている");
    }
    if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
    {
      //Debug.Log("タッチパッドに触っている");
    }
  }

  private void GripCube()
  {
    cube1.transform.position = controllerR.transform.position;
  }
}

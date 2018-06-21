using UnityEngine;

public class VRControllerScript : MonoBehaviour
{
  private bool isOpenMenu;
  private GameObject system, controller, snowBall, canvas;
  private SphereCollider controllerCollider;
  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private Vector2 touchPosition;

  //ctlMode: 0 -> 転がす, 1 -> 掴む
  private int ctlMode;
  private void Start()
  {
    controller = gameObject;
    system = GameObject.Find("System");
    canvas = GameObject.Find("Canvas");
    controllerCollider = controller.GetComponent<SphereCollider>();

    isOpenMenu = false;
    canvas.SetActive(isOpenMenu);
    ctlMode = 0;
  }

  void Update()
  {
    trackedObject = GetComponent<SteamVR_TrackedObject>();
    device = SteamVR_Controller.Input((int)trackedObject.index);
    touchPosition = device.GetAxis();

    // if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   Debug.Log("トリガーを浅く引いた");
    // }

    // if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   Debug.Log("トリガーを深く引いた");
    // }

    //トリガーを握っている
    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {
      if (isOpenMenu)
      {
        system.GetComponent<SystemScript>().CreateSnowBall();
      }
    }

    // if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   Debug.Log("トリガーを離した");
    // }

    //タッチパッドをクリック
    if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
    {
      if (touchPosition.y / touchPosition.x > 1 || touchPosition.y / touchPosition.x < -1)
      {
        if (touchPosition.y > 0)
        {
          //タッチパッド上をクリックした場合の処理
          Debug.Log("Press UP");
        }
        else
        {
          //下をクリック
          isOpenMenu = !isOpenMenu;
          canvas.SetActive(isOpenMenu);
          Debug.Log("Press DOWN");
        }
      }
      else
      {
        if (touchPosition.x > 0)
        {
          //タッチパッド右をクリックした場合の処理
          ctlMode = 0;
        }
        else
        {
          //左をクリック 
          ctlMode = 1;
        }
      }
    }

    // //タッチパッドをクリックしている
    // if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
    // {
      
    // }

    // if (device.GetPressUp(SteamVR_Controller.ButtonMask.Touchpad))
    // {
    //   Debug.Log("タッチパッドをクリックして離した");
    // }
    // if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Touchpad))
    // {
    //   Debug.Log("タッチパッドに触った");
    // }
    // if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Touchpad))
    // {
    //   Debug.Log("タッチパッドを離した");
    // }
    // if (device.GetPressDown(SteamVR_Controller.ButtonMask.ApplicationMenu))
    // {
    //   Debug.Log("メニューボタンをクリックした");
    // }
    // if (device.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
    // {
    //   Debug.Log("グリップボタンをクリックした");
    // }

    // if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   //Debug.Log("トリガーを浅く引いている");
    // }
    // if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   //Debug.Log("トリガーを深く引いている");
    // }
    // if (device.GetTouch(SteamVR_Controller.ButtonMask.Touchpad))
    // {
    //   //Debug.Log("タッチパッドに触っている");
    // }
  }

  void OnTriggerEnter(Collider collisionObj)
  {
    Debug.Log("接触した");
  }

  void OnTriggerStay(Collider collisionObj)
  {
    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {
      switch (ctlMode)
      {
        case 0:
          RotateSnow(collisionObj);
          break;

        case 1:
          GripSnow(collisionObj);
          break;

        default:
          break;
      }
      collisionObj.GetComponent<Rigidbody>().useGravity = false;
    }
    else
    {
      collisionObj.GetComponent<Rigidbody>().useGravity = true;
    }
  }

  void OnTriggerExit(Collider collisionObj)
  {
    Debug.Log("離れた");
  }

  void RotateSnow(Collider collisionObj)
  {
    //関数内で衝突したy座標がよく使われるので、変数に格納
    var colObjY = collisionObj.transform.position.y;

    //雪玉を手で押すと仮定すると、水平から45度方向の部分を押す気がしたので、colObjY(雪玉の半径)*cos(45)をしたものをz方向としたベクトルを宣言
    var tmpVec = new Vector3(0, 0, colObjY * 1.414f / 2f);

    //宣言したベクトルとコントローラの角度の内積を取る。
    tmpVec = controller.transform.rotation * tmpVec;

    //y方向については無視する。
    tmpVec = new Vector3(tmpVec.x, 0, tmpVec.z);

    //雪玉の位置は、コントローラのx、z座標と半径をy座標としたものに、上のベクトルを足したものとする。
    collisionObj.transform.position = new Vector3(controllerCollider.transform.position.x, colObjY, controllerCollider.transform.position.z) + tmpVec;
  }

  //[MEMO]半径を確実に設定しないと、ぶれが生じる。また、半径は現在のところ規則性が分かっていないため、個別に定数を取るしかない。
  //方向ベクトルと円とコントローラの位置関係によって方程式を導き出し、それに合わせて作った関数。
  void GripSnow(Collider collisionObj)
  {
    var radious = 0.06f;//0.55f * collisionObj.GetComponent<Transform>().localScale.x;

    var tmpVec = new Vector3(0, -radious / 1.414f, radious / 1.414f);
    tmpVec = controller.transform.rotation * tmpVec;
    collisionObj.transform.position = controllerCollider.transform.position + tmpVec;

    if (collisionObj.transform.position.y < radious)
    {
      collisionObj.transform.position = new Vector3(collisionObj.transform.position.x, radious, collisionObj.transform.position.z);
    }
  }
}

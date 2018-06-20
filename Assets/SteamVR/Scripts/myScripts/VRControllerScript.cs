using UnityEngine;

public class VRControllerScript : MonoBehaviour
{
  private GameObject controller, snowBall;
  private SphereCollider controllerCollider;
  private int objNum;

  private SteamVR_Controller.Device device;
  private SteamVR_TrackedObject trackedObject;
  private void Start()
  {
    controller = gameObject;
    controllerCollider = controller.GetComponent<SphereCollider>();
    snowBall = (GameObject)Resources.Load("Prefabs/SnowBall");
  }

  void Update()
  {
    trackedObject = GetComponent<SteamVR_TrackedObject>();
    device = SteamVR_Controller.Input((int)trackedObject.index);

    // if (device.GetTouchDown(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   Debug.Log("トリガーを浅く引いた");
    // }
    // if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   Debug.Log("トリガーを深く引いた");
    // }

    // if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   Debug.Log("トリガーを握っている");
    //   var Pos3 = controller.transform.position;
    //   Debug.Log(Pos3.x);
    //   Debug.Log(Pos3.y);
    //   Debug.Log(Pos3.z);
    // }

    // if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
    // {
    //   Debug.Log("トリガーを離した");
    // }
    // if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
    // {
    //   Debug.Log("タッチパッドをクリックした");
    // }
    if (device.GetPress(SteamVR_Controller.ButtonMask.Touchpad))
    {
      Debug.Log("タッチパッドをクリックしている");
      Instantiate(snowBall, new Vector3(Random.Range(3.0f, -3.0f), 5, Random.Range(3.0f, -3.0f)), Quaternion.identity);
    }
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
    collisionObj.GetComponent<Rigidbody>().useGravity = false;
  }

  void OnTriggerStay(Collider collisionObj)
  {
    if (device.GetPress(SteamVR_Controller.ButtonMask.Trigger))
    {
      //RotateSnow(collisionObj);
      GripSnow2(collisionObj);
    }

  }

  void OnTriggerExit(Collider collisionObj)
  {
    Debug.Log("離れた");
    collisionObj.GetComponent<Rigidbody>().useGravity = true;
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

  void GripSnow(Collider collisionObj)
  {
    var colObjY = collisionObj.transform.position.y;
    var tmpVec = new Vector3(0, 0, (colObjY * 0.8f) * 1.414f / 2f);
    tmpVec = controller.transform.rotation * tmpVec;
    tmpVec = new Vector3(tmpVec.x, 0, tmpVec.z);

    collisionObj.transform.position = new Vector3(controllerCollider.transform.position.x, controllerCollider.transform.position.y /*- colObjY * 1.414f / 2f*/,controllerCollider.transform.position.z) + tmpVec;
  }

  //[MEMO]半径を確実に設定しないと、ぶれが生じる。また、半径は現在のところ規則性が分かっていないため、個別に定数を取るしかない。
  //方向ベクトルと円とコントローラの位置関係によって方程式を導き出し、それに合わせて作った関数。
  void GripSnow2(Collider collisionObj)
  {
    var radious = 0.06f;//0.55f * collisionObj.GetComponent<Transform>().localScale.x;

    Debug.Log(radious);

    var tmpVec = new Vector3(0, -radious / 1.414f, radious / 1.414f);
    tmpVec = controller.transform.rotation * tmpVec;
    collisionObj.transform.position = controllerCollider.transform.position + tmpVec;

    if(collisionObj.transform.position.y < radious){
      collisionObj.transform.position = new Vector3(collisionObj.transform.position.x, radious, collisionObj.transform.position.z);
    }
  }
}

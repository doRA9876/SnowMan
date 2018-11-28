using UnityEngine.EventSystems;
using UnityEngine;

public interface IVRControllerLeft : IEventSystemHandler{
  void MakeHard(GameObject Obj);
  void ChangeColor(GameObject obj);
  void ActiveScoop();
}

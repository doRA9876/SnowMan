using UnityEngine.EventSystems;
using UnityEngine;

public interface InterfaceCtrlLeft : IEventSystemHandler{
  void MakeHard(GameObject Obj);
  void ChangeColor(GameObject obj);
  void ActiveScoop();
}

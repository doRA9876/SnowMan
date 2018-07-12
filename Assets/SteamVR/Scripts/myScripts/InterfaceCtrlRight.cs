using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface InterfaceCtrlRight : IEventSystemHandler {
  void SwitchCanvasMode(bool isCanvasMode);
}

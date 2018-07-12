using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface InterfaceColorCanvas : IEventSystemHandler
{
  void ChangeHead(int delta);
  void ChangeValue(int delta);
}

/*****************************************
2018/06/21
・Systemオブジェクトを増やし、新しい雪玉の生成などを委譲した。
・雪玉同士の接続処理に困っている。
・現在は同グループ同士にすることで、位置を固定するつもりだが、Rigidbodyがあるせいか、個別で動いてしまう。
・親オブジェクトとどうにかして位置を固定したい。

2018/06/04
・雪玉が生存できるエリアを制限し、処理が重くなることを防ぐ。
・バケツによって、雪玉を生成できるようにした。
・やはり雪を固めることで、雪像ができることを考えると雪を固められるようにしたい。

******************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemScript : MonoBehaviour
{

  private GameObject snowBall;
  private int snowBallId;
  // Use this for initialization
  void Start()
  {
    snowBall = (GameObject)Resources.Load("Prefabs/SnowBall");
    snowBallId = 0;
  }

  // Update is called once per frame
  void Update()
  {

  }

  public void CreateSnowBall(Vector3 where)
  {
    var obj = Instantiate(snowBall, where, Quaternion.identity);

    obj.GetComponent<SnowBall>().id = snowBallId;
    obj.name = "SnowBall";
    snowBallId++;
  }

  public void GroupingSnowBall(GameObject obj1, GameObject obj2)
  {
    if (obj1.transform.parent == null && obj2.transform.parent == null)
    {
      GameObject newParent = new GameObject("Group");
      obj1.transform.parent = newParent.transform;
      obj2.transform.parent = newParent.transform;
    }
    else
    {
      if (obj1.transform.parent == null)
      {
        obj1.transform.parent = obj2.transform.parent;
      }
      else
      {
        obj2.transform.parent = obj1.transform.parent;
      }
    }
  }

  /// <summary>
  /// Childオブジェクトの親を変更
  /// </summary>
  /// <param 親オブジェクト="Parent"></param>
  /// <param 子オブジェクト="Child"></param>
  public static void TransParent(GameObject Parent, GameObject Child)
  {
    Child.transform.parent = Parent.transform;
  }
}

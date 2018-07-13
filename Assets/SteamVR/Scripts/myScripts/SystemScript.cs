/*****************************************
2018/06/21
・Systemオブジェクトを増やし、新しい雪玉の生成などを委譲した。
・雪玉同士の接続処理に困っている。
・現在は同グループ同士にすることで、位置を固定するつもりだが、Rigidbodyがあるせいか、個別で動いてしまう。
・親オブジェクトとどうにかして位置を固定したい。

2018/07/04
・雪玉が生存できるエリアを制限し、処理が重くなることを防ぐ。
・バケツによって、雪玉を生成できるようにした。
・やはり雪を固めることで、雪像ができることを考えると雪を固められるようにしたい。

2018/07/06
・定着材のようなものを吹きかけることによってオブジェクトをその位置に固定する。
・固定の仕方はRegidBody内のisKinematicをtrueにすることとする。
・左にそれようのスプレーを作成し、それによって吹きかける。

2018/07/09
・SnowBallは非常に多く生成されるため、あまり複雑な処理は書かない。（特に毎フレーム処理させるOnCollision()系はご法度）
・よって、SnowBallには現在では処理を書かないこととする。
・よく考えて処理を書かないと後で後悔する。
******************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemScript : MonoBehaviour
{
  private GameObject _snowBall;

  // Use this for initialization
  void Start()
  {
    _snowBall = (GameObject)Resources.Load("Prefabs/SnowBall");
  }

  public void CreateSnowBall(Vector3 where)
  {
    var obj = Instantiate(_snowBall, where, Quaternion.identity);
    obj.name = "SnowBall";
    obj.GetComponent<Rigidbody>().sleepThreshold = 3;
  }
}

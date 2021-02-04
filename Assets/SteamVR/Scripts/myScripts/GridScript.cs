using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridScript : MonoBehaviour {

  private BoxCollider _boxCollider;
  private MeshRenderer _meshRenderer;

  void Start(){
    _boxCollider = GetComponent<BoxCollider>();
    _meshRenderer = GetComponent<MeshRenderer>();

    _meshRenderer.enabled = true;
  }

	void OnTriggerEnter(Collider collisionObj){
    string collisionObjName = collisionObj.transform.name;

		if(collisionObjName == "SnowBall" || collisionObjName == "HardSnowBall"){
      _meshRenderer.enabled = true;
    }
  }
}

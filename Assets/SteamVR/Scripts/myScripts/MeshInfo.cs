#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class MeshInfo : MonoBehaviour {

    [SerializeField, Range(0.1f, 100.0f)]
    private float _pointSize = 5.0f;
    
    private Mesh _originalMesh;
    private Mesh _pointMesh;
    private Mesh _lineMesh;
    private Material _material;

    /// <summary>
    /// 初期化する
    /// </summary>
    private void Setup(){
        // MeshFilterからメッシュを取得
        _originalMesh = GetComponent<MeshFilter>().sharedMesh;
        // 頂点を表示するためのメッシュを生成
        _pointMesh = Instantiate(_originalMesh);
        _pointMesh.SetIndices(_originalMesh.triangles, MeshTopology.Points, 0);
        // ラインを表示するためのメッシュを生成
        _lineMesh = Instantiate(_pointMesh);
        _lineMesh.SetIndices(MakeIndicesForLine(_originalMesh.triangles), MeshTopology.Lines, 0);
        // マテリアルを生成
        _material = new Material(Shader.Find("MeshInfo"));
        var transformMatrix = Matrix4x4.TRS(Vector3.zero, transform.rotation, transform.lossyScale);
        _material.SetMatrix("_TransformMatrix", transformMatrix);
    }

    /// <summary>
    /// リセットする
    /// </summary>
    private void Reset(){
        _originalMesh = null;
        _pointMesh = null;
        _lineMesh = null;
    }

    private void OnEnable(){
        Setup();
    }

    private void OnDisable(){
        Reset();
    }

    private void Update(){
        if (transform.hasChanged) {
            Reset();
            Setup();
        }

        // 頂点用のメッシュとライン用のメッシュを描画
        _material.SetFloat("_PointSize", _pointSize);
        Graphics.DrawMesh(_pointMesh, transform.position, Quaternion.identity, _material, 0);
        Graphics.DrawMesh(_lineMesh, transform.position, Quaternion.identity, _material, 0);
    }

    private void OnWillRenderObject(){
        
        if (Selection.activeObject != gameObject) {
            _material.SetFloat("_Alpha", 0.0f);
            return;
        }
        else if (Camera.current.name == "SceneCamera") {
            // シーンビューではメッシュ情報を表示する
            _material.SetFloat("_Alpha", 1.0f);
        } 
        else {
            // シーンビュー以外ではメッシュ情報を非表示にする
            _material.SetFloat("_Alpha", 0.0f);
        }
    }

    /// <summary>
    /// Lineが綺麗に描画されるように頂点情報を組み直したものを得る
    /// </summary>
    private int[] MakeIndicesForLine(int[] triangles){
        int[] indices = new int[2 * triangles.Length];
        int i = 0;
        for( int t = 0; t < triangles.Length; t+=3 )
        {
            indices[i++] = triangles[t];
            indices[i++] = triangles[t + 1];
            indices[i++] = triangles[t + 1];
            indices[i++] = triangles[t + 2];
            indices[i++] = triangles[t + 2];
            indices[i++] = triangles[t];
        }
        return indices;
    }
}
#endif

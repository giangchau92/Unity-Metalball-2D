using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MetalBall : MonoBehaviour {
	public int _numberElement = 1;
	public Material _material;
	public float _speed = 1.0f;
	[Range(0, 100)]
	public float _R = 1.0f;
	[Range(0, 20)]
	public float _Gravity = 1.0f;


	Vector4[] _elementsPos;
	Vector4[] _elementVels;
	Rect _bound;

	// Use this for initialization
	void Start () {
		_bound = new Rect (0, 0, Screen.width, Screen.height);
		_elementsPos = new Vector4[_numberElement];
		_elementVels = new Vector4[_numberElement];
		for (int i = 0; i < _numberElement; i++) {
			_elementsPos [i] = new Vector4 (Screen.width, Screen.height) * Random.value;
			_elementVels [i] = Random.insideUnitCircle;
		}
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < _numberElement; i++) {
			Vector4 pos = _elementsPos [i];
			Vector4 vel = _elementVels [i];
			_elementsPos [i] = pos + vel * Time.deltaTime * _speed;
			if (_elementsPos [i].x > _bound.xMax || _elementsPos [i].x < _bound.xMin) {
				_elementVels [i] = new Vector2 (-vel.x, vel.y);
			}
			if (_elementsPos [i].y > _bound.yMax || _elementsPos [i].y < _bound.yMin) {
				_elementVels [i] = new Vector2 (vel.x, -vel.y);
			}

		}
	}

	void OnRenderImage(RenderTexture src, RenderTexture dest) {
		_material.SetFloat ("_R", _R);
		_material.SetFloat ("_Gravity", _Gravity);
		_material.SetVectorArray ("_Particles", _elementsPos);
		_material.SetInt ("_Particles_Length", _numberElement);
		Graphics.Blit(src, dest, _material);
	}
}

using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(CommonPath))]
public class CommonPathEditor : Editor
{

    CommonPath _target;
    GUIStyle style = new GUIStyle();

    void OnEnable()
    {
        //i like bold handle labels since I'm getting old:
        style.fontStyle = FontStyle.Bold;
        style.normal.textColor = Color.white;
        _target = (CommonPath)target;

        //lock in a default path name:
        if (!_target.initialized)
        {
            _target.initialized = true;
            _target.pathName = "路径";
            _target.initialName = _target.pathName;
        }
    }


    public override void OnInspectorGUI()
    {
        //path name:
        _target.pathName = EditorGUILayout.TextField("路径名称", _target.pathName);

        if (_target.pathName == "")
            _target.pathName = _target.initialName;

        //path color:
        _target.pathColor = EditorGUILayout.ColorField("路径颜色", _target.pathColor);

		if(GUILayout.Button("闭合曲线")) {
			_target.nodes[_target.nodes.Count - 1] = _target.nodes[0];
		}

        // add node;
        if (GUILayout.Button("增加节点"))
        {
            int cnt = _target.nodes.Count;
            Vector3 pos = cnt > 0 ? _target.nodes[cnt - 1] : _target.transform.position;
            if (_target.nodes.Count > 1)
            {
                Vector3 dir = pos - _target.nodes[cnt - 2];
                pos += dir.normalized * 2;
            }
            _target.nodes.Add(pos);
        }

        EditorGUILayout.BeginHorizontal();
        if(GUILayout.Button("+Z")) {
            for(int i = 0; i < _target.nodes.Count; i++) {
                _target.nodes[i] = _target.nodes[i] + Vector3.forward;
            }
        }

        if(GUILayout.Button("-Z")) {
            for(int i = 0; i < _target.nodes.Count; i++) {
                _target.nodes[i] = _target.nodes[i] - Vector3.forward;
            }
        }
        EditorGUILayout.EndHorizontal();

        //node display:
        EditorGUI.indentLevel = 3;
        for (int i = 0; i < _target.nodes.Count; i++)
        {
            _target.nodes[i] = EditorGUILayout.Vector3Field("node" + i, _target.nodes[i]);
			EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("", "");
			if (GUILayout.Button("+"))
            {
				Vector3 pos;
				if(i == _target.nodes.Count-1){  //最后一个节点
					if(_target.nodes.Count > 1) {
						pos = _target.nodes[i] - _target.nodes[i-1];
                        pos += _target.nodes[i];
					} else {
						pos = _target.transform.position;
					}
				} else {
					pos = _target.nodes[i + 1] - _target.nodes[i];
					//pos /= 2;
					pos = _target.nodes[i] + pos / 2;
				}
				_target.nodes.Insert(i+1, pos);
                break;
            }
            if (GUILayout.Button("-"))
            {
                _target.nodes.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }

        //update and redraw:
        if (GUI.changed)
        {
            EditorUtility.SetDirty(_target);
        }
    }

    void OnSceneGUI()
    {
        if (_target.enabled)
        {
            if (_target.nodes.Count > 0)
            {
                //allow path adjustment undo
                Undo.RecordObject(_target, "调整BOSS路径");

                //node handle display:
                for (int i = 0; i < _target.nodes.Count; i++)
                {
                    Vector3 pos = _target.transform.TransformPoint(_target.nodes[i]);
                    _target.nodes[i] = _target.transform.InverseTransformPoint(Handles.PositionHandle(pos, Quaternion.identity));
                    Handles.Label(pos, "node" + i, style);
                }
            }
        }
    }

}

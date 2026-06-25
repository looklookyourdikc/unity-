using UnityEngine;
using System.Collections.Generic;

[AddComponentMenu("轮廓/Outline Box")]
public class OutlineBox : MonoBehaviour
{
    public bool isActive = false;

    [ColorUsage(true, true)]
    public Color outlineColor = new Color(1f, 0.85f, 0.2f, 1f);

    [Range(0.001f, 0.15f)]
    public float outlineWidth = 0.05f;

    List<GameObject> _outlines = new List<GameObject>();
    Material _mat;

    static readonly int PidColor = Shader.PropertyToID("_OutlineColor");
    static readonly int PidWidth = Shader.PropertyToID("_OutlineWidth");

    void OnEnable()
    {
        if (isActive) Build();
    }

    void OnDisable() => Clear();

    void OnDestroy() => Clear();

    #if UNITY_EDITOR
    void OnValidate()
    {
        if (!Application.isPlaying || _mat == null) return;
        _mat.SetColor(PidColor, outlineColor);
        _mat.SetFloat(PidWidth, outlineWidth);
    }
    #endif

    public void Build()
    {
        Clear();
        if (_mat == null)
        {
            var s = Shader.Find("OutlineBox/URP");
            if (s == null) { Debug.LogError("[OutlineBox] 找不到着色器"); return; }
            _mat = new Material(s);
        }
        _mat.SetColor(PidColor, outlineColor);
        _mat.SetFloat(PidWidth, outlineWidth);

        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            var go = new GameObject("_Outline");
            go.transform.SetParent(r.transform, false);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            go.layer = r.gameObject.layer;

            var mf = r.GetComponent<MeshFilter>();
            if (mf && mf.sharedMesh)
            {
                var o = go.AddComponent<MeshFilter>();
                o.sharedMesh = mf.sharedMesh;
                var mr = go.AddComponent<MeshRenderer>();
                mr.sharedMaterial = _mat;
                mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                mr.receiveShadows = false;
                _outlines.Add(go);
                continue;
            }

            var smr = r as SkinnedMeshRenderer;
            if (smr && smr.sharedMesh)
            {
                var o = go.AddComponent<SkinnedMeshRenderer>();
                o.sharedMesh = smr.sharedMesh;
                o.bones = smr.bones;
                o.rootBone = smr.rootBone;
                o.localBounds = smr.localBounds;
                o.sharedMaterial = _mat;
                o.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                o.receiveShadows = false;
                _outlines.Add(go);
                continue;
            }

            DestroyImmediate(go);
        }
    }

    public void Clear()
    {
        for (int i = _outlines.Count - 1; i >= 0; i--)
        {
            if (_outlines[i]) DestroyImmediate(_outlines[i]);
        }
        _outlines.Clear();
    }
}

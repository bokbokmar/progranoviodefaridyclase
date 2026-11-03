#if UNITY_EDITOR
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

// Herramientas para optimizar el nivel importado de la Asset Store
public static class LevelOptimizerTool
{
    [MenuItem("Tools/Level Optimizer/Audit Scene")]
    private static void Audit()
    {
        var meshColliders = Object.FindObjectsByType<MeshCollider>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var allColliders = Object.FindObjectsByType<Collider>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        var renderers = Object.FindObjectsByType<MeshRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        int withoutLod = renderers.Count(r => r.GetComponentInParent<LODGroup>() == null);

        Debug.Log($"[Audit] Colliders totales: {allColliders.Length} | MeshColliders: {meshColliders.Length} " +
                  $"(no convexos: {meshColliders.Count(m => !m.convex)}) | " +
                  $"MeshRenderers sin LODGroup: {withoutLod} de {renderers.Length}");
    }

    // Reemplaza MeshColliders por BoxColliders en la seleccion (y sus hijos)
    [MenuItem("Tools/Level Optimizer/Selection - MeshCollider -> BoxCollider")]
    private static void ReplaceMeshColliders()
    {
        foreach (GameObject root in Selection.gameObjects)
        {
            foreach (MeshCollider mc in root.GetComponentsInChildren<MeshCollider>(true))
            {
                GameObject go = mc.gameObject;
                MeshFilter mf = go.GetComponent<MeshFilter>();
                Undo.DestroyObjectImmediate(mc);

                BoxCollider box = Undo.AddComponent<BoxCollider>(go);
                if (mf != null && mf.sharedMesh != null)
                {
                    box.center = mf.sharedMesh.bounds.center;
                    box.size = mf.sharedMesh.bounds.size;
                }
            }
        }
    }

    // Quita todos los colliders de objetos decorativos (arboles, rocas, props)
    [MenuItem("Tools/Level Optimizer/Selection - Remove All Colliders")]
    private static void RemoveColliders()
    {
        foreach (GameObject root in Selection.gameObjects)
        {
            foreach (Collider c in root.GetComponentsInChildren<Collider>(true))
            {
                Undo.DestroyObjectImmediate(c);
            }
        }
    }

    // Agrega LODGroup. Si hay hijos con nombre "..._LOD0/_LOD1/_LOD2" los usa como niveles;
    // si no, crea un solo nivel que se oculta (cull) a distancia.
    [MenuItem("Tools/Level Optimizer/Selection - Add LODGroup")]
    private static void AddLodGroup()
    {
        float[] heights = { 0.6f, 0.3f, 0.1f, 0.04f };

        foreach (GameObject root in Selection.gameObjects)
        {
            if (root.GetComponent<LODGroup>() != null) continue;

            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0) continue;

            var levels = renderers
                .Select(r => new { r, lod = ParseLod(r.name) })
                .Where(x => x.lod >= 0)
                .GroupBy(x => x.lod)
                .OrderBy(g => g.Key)
                .ToList();

            LOD[] lods;
            if (levels.Count >= 2)
            {
                lods = new LOD[levels.Count];
                for (int i = 0; i < levels.Count; i++)
                {
                    lods[i] = new LOD(heights[Mathf.Min(i, heights.Length - 1)], levels[i].Select(x => x.r).ToArray());
                }
            }
            else
            {
                lods = new[] { new LOD(0.05f, renderers) };
            }

            LODGroup group = Undo.AddComponent<LODGroup>(root);
            group.SetLODs(lods);
            group.RecalculateBounds();
        }
    }

    private static int ParseLod(string name)
    {
        Match m = Regex.Match(name, @"LOD(\d+)");
        return m.Success ? int.Parse(m.Groups[1].Value) : -1;
    }
}
#endif

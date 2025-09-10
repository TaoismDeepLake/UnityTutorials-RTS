using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerrainRuntimeFixer : MonoBehaviour
{
    [Header("可选：显式指定一份可用材质；为空则用内置 Shader 动态创建")]
    public Material pinnedTerrainMaterial;

    [Header("加载场景后是否强制关闭实例化渲染（建议先开着看看效果）")]
    public bool forceDisableDrawInstanced = true;

    // 我们把“可用的 Shader/材质”挂在一个常驻对象上，避免被 UnloadUnusedAssets 清掉
    private static Material sPinnedMaterial;     // 常驻材质
    private static Shader sTerrainStandard;      // 常驻 Shader

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // 1) 固定一份 Shader 引用（不使用 Warmup，避免你遇到的崩溃）
        sTerrainStandard = Shader.Find("Nature/Terrain/Standard");
        if (sTerrainStandard == null)
        {
            // 少数工程只带了 Diffuse；兜底再找一次
            sTerrainStandard = Shader.Find("Nature/Terrain/Diffuse");
        }

        if (sTerrainStandard == null)
        {
            Debug.LogError("[TerrainFixer] 找不到 Terrain Shader（Nature/Terrain/Standard 或 Diffuse）。请确认 Graphics > Always Included Shaders 里已添加。");
        }

        // 2) 固定一份材质
        if (pinnedTerrainMaterial != null)
        {
            sPinnedMaterial = pinnedTerrainMaterial;
        }
        else
        {
            // 从 Resources 里找你自建的材质（可选）
            var fromRes = Resources.Load<Material>("TerrainMaterial");
            if (fromRes != null) sPinnedMaterial = fromRes;
        }

        if (sPinnedMaterial == null && sTerrainStandard != null)
        {
            // 动态创建一份，标记不可卸载
            sPinnedMaterial = new Material(sTerrainStandard)
            {
                name = "##Pinned_TerrainMaterial(Runtime)"
            };
            sPinnedMaterial.hideFlags = HideFlags.DontUnloadUnusedAsset;
        }

        if (sPinnedMaterial != null)
            Debug.Log($"[TerrainFixer] Pinned material = {sPinnedMaterial.name} (shader={sPinnedMaterial.shader?.name})");

        // 3) 先修一次当前已加载场景（以防 Core 场景里就有 Terrain）
        FixTerrainsInLoadedScenes();

        // 4) 之后每次有新场景加载都修一次
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"[TerrainFixer] Scene loaded: {scene.name}, mode={mode}");
        FixTerrainsInLoadedScenes();
    }

    private void FixTerrainsInLoadedScenes()
    {
        var terrains = FindObjectsOfType<Terrain>(true);
        if (terrains.Length == 0)
        {
            Debug.Log("[TerrainFixer] No Terrain found in loaded scenes.");
            return;
        }

        foreach (var t in terrains)
        {
            // 有些项目把材质设在 materialTemplate；有的版本用 sharedMaterial
            var mat = t.materialTemplate != null ? t.materialTemplate : t.materialTemplate;

            string current = (mat != null && mat.shader != null) ? mat.shader.name :
                             (mat != null && mat.shader == null) ? "Shader=<null>" :
                             "Material=<null>";

            Debug.Log($"[TerrainFixer] Check Terrain '{t.name}': {current}, drawInstanced={t.drawInstanced}");

            bool bad =
                (mat == null) ||
                (mat.shader == null) ||
                (mat.shader.name == "Hidden/InternalErrorShader");

            if (forceDisableDrawInstanced && t.drawInstanced)
                t.drawInstanced = false;

            if (bad)
            {
                if (sPinnedMaterial != null)
                {
                    t.materialTemplate = sPinnedMaterial;
                    Debug.Log($"[TerrainFixer] -> Rebind pinned material to '{t.name}' : {sPinnedMaterial.name} ({sPinnedMaterial.shader?.name})");
                }
                else if (sTerrainStandard != null)
                {
                    var m = new Material(sTerrainStandard) { name = "##Auto_TerrainMaterial" };
                    m.hideFlags = HideFlags.DontUnloadUnusedAsset;
                    t.materialTemplate = m;
                    Debug.Log($"[TerrainFixer] -> Create & bind new material for '{t.name}' : {m.shader?.name}");
                }
                else
                {
                    Debug.LogError($"[TerrainFixer] '{t.name}' 修复失败：没有可用 Shader/材质。");
                }
            }
        }
    }
}

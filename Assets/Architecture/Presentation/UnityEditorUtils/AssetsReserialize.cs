using UnityEditor;

namespace Calc.Presentation.UnityEditorUtils
{
	internal static class AssetsReserialize
    {
        [MenuItem("Build/Force Reserialize Assets")]
        private static void ForceReserializeAssets()
        {
            AssetDatabase.ForceReserializeAssets();
        }
    }
}

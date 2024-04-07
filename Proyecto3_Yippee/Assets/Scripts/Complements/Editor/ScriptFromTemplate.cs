using UnityEditor;

namespace UtilsComplements
{
    public class ScriptFromTemplate
    {
        private const string PATH_TO_BASETEMPLATE = "";

        [MenuItem(itemName: "Assets/Create/ScriptTemplates/C# UnityBase Template", isValidateFunction: false, priority = 22)]
        public static void CreateScriptFromTemplate_BaseTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(PATH_TO_BASETEMPLATE, "newScript.cs");
        }
    }
}
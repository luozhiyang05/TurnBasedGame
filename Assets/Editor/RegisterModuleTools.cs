using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class RegisterModuleTools : EditorWindow
    {
        
        [MenuItem("QUFramework/注册模块")]
        private static void RegisterSystem()
        {
            string systemPath = Directory.GetCurrentDirectory() + @"\Assets\GameSystem\";
            string globalCSharp = Directory.GetCurrentDirectory() + @"\Assets\Framework\Global.cs";
            StringBuilder globalStr = new StringBuilder();
            StringBuilder moduleStr = new StringBuilder();
            foreach (var value in Directory.GetDirectories(systemPath))
            {
                int index = value.LastIndexOf('\\');
                var moduleName = value.Substring(index + 1);
                if (moduleName.Contains("Template")) continue;
                globalStr.Append($"using GameSystem.{moduleName};\n");
                moduleStr.Append($"\n\t\t\tthis.RegisterModule<I{moduleName}Module>(new {moduleName}Module());");
            }


            globalStr.Append(@"namespace Framework
{
    public class Global : FrameworkMgr<Global>
    {
        protected override void OnInitModule()
        {");

            globalStr.Append(moduleStr);
            globalStr.Append(@"
        }
    }
}");
            //替换Global
            try
            {
                if (File.Exists(globalCSharp)) File.Delete(globalCSharp);
            }
            catch (Exception e)
            {
                throw new Exception("删除Global失败：" + e);
            }

            try
            {
                File.WriteAllText(globalCSharp, globalStr.ToString());
            }
            catch (Exception e)
            {
                throw new Exception("生成Global失败：" + e);
            }


            foreach (var moduleName in Directory.GetDirectories(systemPath))
            {
                if (moduleName.Contains("MVCTemplate")) continue;
                Debug.Log(moduleName);
            }

            Debug.Log("生成Global文件，注册模块成功:" + globalCSharp);
            AssetDatabase.Refresh();
        }
    }
}
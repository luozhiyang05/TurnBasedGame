using System;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CreateMainModuleTools : EditorWindow
    {
        private static CreateMainModuleTools _panel;
        private static string _moduleName;

        [MenuItem("QUFramework/生成主模块")]
        public static void CreateMainModule()
        {
            //返回当前屏幕上第一个 t 类型的 EditorWindow，utility参数为是否浮动窗口
            _panel = GetWindowWithRect<CreateMainModuleTools>(new Rect(0, 0, 360, 90), false, "生成系统模块");
            _panel.Show(); //默认打开
        }


        private void OnGUI()
        {
            //文本框
            GUI.Label(new Rect(10, 20, 100, 20), "模块名称：");
            _moduleName = GUI.TextField(new Rect(80, 20, 200, 20), _moduleName);
            //确认按钮
            if (GUI.Button(new Rect(10, 50, 120, 30), "生成模块"))
                CreateFile();
        }


        private static void CreateFile()
        {
            //D:\UnityProjects\QUFrameWork
            string systemPath = Directory.GetCurrentDirectory() + @"\Assets\GameSystem\" + _moduleName + @"\";

            //判断有无该目录，无则创建
            if (Directory.Exists(systemPath))
            {
                throw new Exception($"该目录已存在{systemPath}");
            }

            try
            {
                Directory.CreateDirectory(systemPath); //创建系统根目录
                Directory.CreateDirectory(systemPath + @"\Main");
                Directory.CreateDirectory(systemPath + @"\Scripts");
                Directory.CreateDirectory(systemPath + @"\Resources");
            }
            catch (Exception e)
            {
                throw new Exception("生成目录失败：" + e);
            }
            
            //读取view的配置模板,生成view
            const string templateViewPath = "Assets/Editor/Template/TemplateOneSystem/Main/TemplateOneSystemView.cs";
            try
            {
                var viewContent = File.ReadAllText(templateViewPath);
                var newViewContent = viewContent.Replace("TemplateOneSystem", _moduleName);
                File.WriteAllText(systemPath + "Main/" + _moduleName + "View.cs", newViewContent);
            }
            catch (Exception e)
            {
                throw new Exception("生成View失败：" + e);
            }

            //生成Model
            const string templateModelPath = "Assets/Editor/Template/TemplateOneSystem/Main/TemplateOneSystemViewModel.cs";
            try
            {
                var modelContent = File.ReadAllText(templateModelPath);
                var newModelContent = modelContent.Replace("TemplateOneSystem", _moduleName);
                File.WriteAllText(systemPath + "Main/" + _moduleName + "ViewModel.cs", newModelContent);
            }
            catch (Exception e)
            {
                throw new Exception("生成Model失败：" + e);
            }

            //生成Ctrl
            const string templateCtrlPath = "Assets/Editor/Template/TemplateOneSystem/Main/TemplateOneSystemViewCtrl.cs";
            try
            {
                var ctrlContent = File.ReadAllText(templateCtrlPath);
                var newCtrlContent = ctrlContent.Replace("TemplateOneSystem", _moduleName);
                File.WriteAllText(systemPath + "Main/" + _moduleName + "ViewCtrl.cs", newCtrlContent);
            }
            catch (Exception e)
            {
                throw new Exception("生成Ctrl失败：" + e);
            }

            //创建系统cs
            const string templateModulePath = "Assets/Editor/Template/TemplateOneSystem/TemplateOneSystemModule.cs";
            try
            {
                var moduleContent = File.ReadAllText(templateModulePath);
                var newSystemContent = moduleContent.Replace("TemplateOneSystem", _moduleName);
                File.WriteAllText(systemPath + _moduleName + "Module.cs", newSystemContent);
            }
            catch (Exception e)
            {
                throw new Exception("生成System失败：" + e);
            }

            AssetDatabase.Refresh();
            _panel.Close();
        }
    }
}
using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class CreateSecondModuleTools : EditorWindow
    {
        private static CreateSecondModuleTools _panel;
        private static string _secondModuleName; //xxx\xxx

        [MenuItem("QUFramework/生成二级模块")]
        public static void CreateSecondModule()
        {
            //返回当前屏幕上第一个 t 类型的 EditorWindow，utility参数为是否浮动窗口
            _panel = GetWindowWithRect<CreateSecondModuleTools>(new Rect(0, 0, 360, 90), false, "生成二级系统模块");
            _panel.Show(); //默认打开
        }


        private void OnGUI()
        {
            //文本框
            GUI.Label(new Rect(10, 20, 100, 20), "二级模块名称：");
            _secondModuleName = GUI.TextField(new Rect(100, 20, 200, 20), _secondModuleName);
            //确认按钮
            if (GUI.Button(new Rect(10, 50, 120, 30), "生成模块"))
                CreateFile();
        }

        private static void CreateFile()
        {
            //判断输入规范。规范： xxx\xxx
            if (!_secondModuleName.Contains('\\')) throw new Exception("必须带有主模块名字！：格式 xxx\\xxx");

            //D:\UnityProjects\QUFrameWork
            var mainModuleName = _secondModuleName.Substring(0, _secondModuleName.IndexOf('\\'));
            var mainModulePath = Directory.GetCurrentDirectory() + @"\Assets\GameSystem\" + mainModuleName;
            var secondModuleName = _secondModuleName.Substring(_secondModuleName.IndexOf('\\') + 1);
            var secondModulePath = Directory.GetCurrentDirectory() + @"\Assets\GameSystem\" + _secondModuleName + @"\";

            //判断主模块是否存在
            if (!Directory.Exists(mainModulePath)) throw new Exception($"主模块{mainModuleName}不存在：" + mainModulePath);

            //判断主模块和二级模块名称是否一致
            if (secondModuleName.Equals(mainModuleName)) throw new Exception("主模块名不能和二级模块名相同！");

            //判断二级模块是否存在
            if (Directory.Exists(secondModulePath)) throw new Exception($"二级模块已存在{secondModulePath}");

            try
            {
                Directory.CreateDirectory(secondModulePath); //创建系统根目录
                Directory.CreateDirectory(secondModulePath + @"\Main");
            }
            catch (Exception e)
            {
                throw new Exception("生成目录失败：" + e);
            }
            
            //读取view的配置模板,生成view
            const string templateViewPath = "Assets/Editor/Template/TemplateOneSystem/TemplateTwoSystem/Main/TemplateTwoSystemView.cs";
            try
            {
                var viewContent = File.ReadAllText(templateViewPath);
                var newViewContent = viewContent.Replace("TemplateTwoSystem", secondModuleName);
                newViewContent = newViewContent.Replace("TemplateOneSystem", mainModuleName);
                File.WriteAllText(secondModulePath + "Main/" + secondModuleName + "View.cs", newViewContent);
            }
            catch (Exception e)
            {
                throw new Exception("生成View失败：" + e);
            }

            //生成Model
            const string templateModelPath = "Assets/Editor/Template/TemplateOneSystem/TemplateTwoSystem/Main/TemplateTwoSystemModel.cs";
            try
            {
                var modelContent = File.ReadAllText(templateModelPath);
                var newViewContent = modelContent.Replace("TemplateTwoSystem", secondModuleName);
                newViewContent = newViewContent.Replace("TemplateOneSystem", mainModuleName);
                File.WriteAllText(secondModulePath + "Main/" + secondModuleName + "Model.cs", newViewContent);
            }
            catch (Exception e)
            {
                throw new Exception("生成Model失败：" + e);
            }

            //生成Ctrl
            const string templateCtrlPath = "Assets/Editor/Template/TemplateOneSystem/TemplateTwoSystem/Main/TemplateTwoSystemCtrl.cs";
            try
            {
                var ctrlContent = File.ReadAllText(templateCtrlPath);
                var newViewContent = ctrlContent.Replace("TemplateTwoSystem", secondModuleName);
                newViewContent = newViewContent.Replace("TemplateOneSystem", mainModuleName);
                File.WriteAllText(secondModulePath + "Main/" + secondModuleName + "Ctrl.cs", newViewContent);
            }
            catch (Exception e)
            {
                throw new Exception("生成Ctrl失败：" + e);
            }

            AssetDatabase.Refresh();
            _panel.Close();
        }
    }
}
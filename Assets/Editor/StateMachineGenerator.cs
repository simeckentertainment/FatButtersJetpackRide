using UnityEngine;
using UnityEditor;
using System.IO;

namespace FSMGen
{
    public class StateMachineGenerator : EditorWindow
    {
        public GameObject HostGameObject;
        //GameObject truePrefab;
        //string HostObjPath;
        public string CharClassName;
        public string folderPath;
        string TemplateCharacterClassText;
        string CharacterClassText;
        string TemplateStateMachineText;
        string StateMachineText;
        string TemplateMasterStateText;
        string MasterStateText;
        string DefaultSuperStateText;
        string SuperStateText;
        string TemplateIdleSubStateText;
        string IdleSubStateText;
        string[] TemplateTexts;

        [MenuItem("Tools/Create Finite State Machine")]
        // Start is called before the first frame update
        public static void ShowStateMachineWindow(){
            GetWindow<StateMachineGenerator>("State Machine Generator");
        }

        void OnGUI()
        {
            //Future feature: Automatically attaching to your chosen prefab.
            //HostObjPath = AssetDatabase.GetAssetPath(HostGameObject);
            //truePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(HostObjPath);
            SetTemplateDataStrings();
            CharClassName = EditorGUILayout.TextField("Character Class Name", CharClassName);
            //GUILayout.Label("Please put the desired prefab here:");
            //HostGameObject = (GameObject)EditorGUILayout.ObjectField("Host Object", HostGameObject, typeof(GameObject), true);
            GUILayout.Label("Where will your scripts live?       " + folderPath, EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("Select Folder")){
                folderPath = EditorUtility.OpenFolderPanel("Select Folder", "", "");
                if (string.IsNullOrEmpty(folderPath))
                {
                    folderPath = "No folder selected";
                }
            }
            // Add fields and buttons here
            if (GUILayout.Button("Generate"))
            {
                GenerateStateMachine();
                AssetDatabase.Refresh();
            }
        }
        void SetTemplateDataStrings(){ //Hard-coded paths, I know.
            TemplateCharacterClassText = File.ReadAllText("Assets/Editor/StateMachineTemplate/TemplateCharacterClass.txt");
            TemplateStateMachineText = File.ReadAllText("Assets/Editor/StateMachineTemplate/TemplateStateMachine/TemplateStateMachine.txt");
            TemplateMasterStateText = File.ReadAllText("Assets/Editor/StateMachineTemplate/TemplateStateMachine/TemplateMasterState.txt");
            DefaultSuperStateText = File.ReadAllText("Assets/Editor/StateMachineTemplate/TemplateStateMachine/SuperStates/DefaultSuperState.txt");
            TemplateIdleSubStateText = File.ReadAllText("Assets/Editor/StateMachineTemplate/TemplateStateMachine/SubStates/TemplateIdleSubState.txt");
            TemplateTexts = new string[5] {TemplateCharacterClassText,TemplateStateMachineText,TemplateMasterStateText,DefaultSuperStateText,TemplateIdleSubStateText};
        }

        void GenerateStateMachine(){
            //Set Folder paths.
            string StateMachineFolderPath = folderPath+"/"+CharClassName+"StateMachine";
            string SubStateFolderPath = StateMachineFolderPath+"/SubStates";
            string SuperStateFolderPath = StateMachineFolderPath+"/SuperStates";
            string SupportScriptFolderPath = folderPath+"/"+CharClassName+"SupportScripts";

            //Set Script Paths
            string CharacterClassPath = folderPath+"/"+CharClassName+".cs";
            string StateMachinePath = StateMachineFolderPath+"/"+CharClassName+"StateMachine.cs";
            string MasterStatePath = StateMachineFolderPath+"/"+CharClassName+"MasterState.cs";
            string SubStatePath = SubStateFolderPath+"/"+CharClassName+"IdleState.cs";
            string SuperStatePath = SuperStateFolderPath+"/"+CharClassName+"DefaultSuperState.cs";


            //Ensure folders are there... duh. #JustMeaningfulNames
            EnsureFolderIsThere(StateMachineFolderPath);
            EnsureFolderIsThere(SubStateFolderPath);
            EnsureFolderIsThere(SuperStateFolderPath);
            EnsureFolderIsThere(SupportScriptFolderPath);

            //Make sure that the scripts actually reflect your character class name.
            AdaptAllScripts();

            //Create the scripts.
            CreateFile(CharacterClassPath, CharacterClassText);
            CreateFile(StateMachinePath,StateMachineText);
            CreateFile(MasterStatePath,MasterStateText);
            CreateFile(SuperStatePath,SuperStateText);
            CreateFile(SubStatePath,IdleSubStateText);
        }
        void CreateFile(string path, string contents){
            StreamWriter writer = new StreamWriter(path);
            writer.WriteLine(contents);
            writer.Close();
        }
        void EnsureFolderIsThere(string path){
            if (!Directory.Exists(path)){
                Directory.CreateDirectory(path);
            }
        }
        void AdaptAllScripts(){
            //simple string replacements for all class values within the templates.
            for(int i=0;i<TemplateTexts.Length;i++){
                TemplateTexts[i] = TemplateTexts[i].Replace("TemplateCharacterClass",char.ToUpper(CharClassName[0])+CharClassName.Substring(1));
                TemplateTexts[i] = TemplateTexts[i].Replace("templateCharacterClass",char.ToLower(CharClassName[0])+CharClassName.Substring(1));

                TemplateTexts[i] = TemplateTexts[i].Replace("TemplateStateMachine",char.ToUpper(CharClassName[0])+CharClassName.Substring(1)+"StateMachine");
                TemplateTexts[i] = TemplateTexts[i].Replace("templateStateMachine",char.ToLower(CharClassName[0])+CharClassName.Substring(1)+"StateMachine");

                TemplateTexts[i] = TemplateTexts[i].Replace("TemplateMasterState",char.ToUpper(CharClassName[0])+CharClassName.Substring(1)+"MasterState");
                TemplateTexts[i] = TemplateTexts[i].Replace("templateMasterState",char.ToLower(CharClassName[0])+CharClassName.Substring(1)+"MasterState");

                TemplateTexts[i] = TemplateTexts[i].Replace("DefaultSuperState",char.ToUpper(CharClassName[0])+CharClassName.Substring(1)+"SuperState");
                TemplateTexts[i] = TemplateTexts[i].Replace("defaultSuperState",char.ToLower(CharClassName[0])+CharClassName.Substring(1)+"SuperState");

                TemplateTexts[i] = TemplateTexts[i].Replace("TemplateIdleSubState",char.ToUpper(CharClassName[0])+CharClassName.Substring(1)+"IdleState");
                TemplateTexts[i] = TemplateTexts[i].Replace("templateIdleSubState",char.ToLower(CharClassName[0])+CharClassName.Substring(1)+"IdleState");
            }
            //And save the new results.
            CharacterClassText = TemplateTexts[0];
            StateMachineText = TemplateTexts[1];
            MasterStateText = TemplateTexts[2];
            SuperStateText = TemplateTexts[3];
            IdleSubStateText = TemplateTexts[4];
            
        }
    }
}

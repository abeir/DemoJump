using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player.FSM;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEngine;


namespace FSM
{
    
    [CreateAssetMenu(menuName = "Abeir/State Config")]
    public class StateConfig : ScriptableObject
    {
        [Serializable]
        public class Translate
        {
            public PlayerStateID source;
            public PlayerStateID target;
        }
        
        
        [Title("Default State")]
        [PropertyOrder(1)]
        public PlayerStateID defaultState = PlayerStateID.Idle;
        
        [Title("State Nodes")]
        [ValueDropdown("StateClasses", IsUniqueList = true, ExcludeExistingValuesInList = true), PropertyOrder(2)]
        public List<string> nodes;

        [Title("State Translates")]
        [Button(ButtonSizes.Medium), GUIColor(0.4f, 0.8f, 1), PropertyOrder(3)]
        private void TextEditor()
        {
            StateConfigWindow.OpenWindow(this);
        }
        
        [LabelText("Translates"), TableList, PropertyOrder(4)]
        public List<Translate> translates;

        
        [Title("Any Translates")]
        [ValueDropdown("StateIDs", IsUniqueList = true, ExcludeExistingValuesInList = true), PropertyOrder(5)]
        public List<PlayerStateID> anyTranslates;


        public List<Translate> GetTranslates()
        {
            return translates;
        }
        
        
        private IEnumerable StateClasses()
        {
            var list = new ValueDropdownList<string>();
            
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.Namespace == "Player.FSM" && type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(AStateBase)))
                    {
                        list.Add(type.Name, type.FullName);
                    }
                }
            }
            return list;
        }

        private IEnumerable StateIDs()
        {
            var list = new ValueDropdownList<PlayerStateID>();
            foreach(var stateName in Enum.GetNames(typeof(PlayerStateID)))
            {
                list.Add(stateName, Enum.Parse<PlayerStateID>(stateName));
            }
            return list;
        }

    }


    public class StateConfigWindow : OdinEditorWindow
    {
        public static void OpenWindow(StateConfig stateConfig)
        {
            var window = GetWindow<StateConfigWindow>();
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(700, 700);
            window.SetStateConfig(stateConfig);
        }
        
        
        [Button(ButtonSizes.Large), GUIColor(0, 1, 0), PropertyOrder(1)]
        private void Save()
        {
            _stateConfig.translates = ConvertFromText(translateText);
        }
        
        
        [Title("State Translates")]
        [LabelText("Translates"), TextArea(4, 50), PropertyOrder(2)]
        public string translateText;
        
        

        private StateConfig _stateConfig;

        public void SetStateConfig(StateConfig stateConfig)
        {
            _stateConfig = stateConfig;
            translateText = ConvertFromTranslates(_stateConfig.translates);
        }
        
        private string ConvertFromTranslates(List<StateConfig.Translate> translates)
        {
            if (translates.IsNullOrEmpty())
            {
                return string.Empty;
            }
            var lines = translates.Select(t => t.source + " > " + t.target).ToList();
            return string.Join("\n", lines);
        }

        private static List<StateConfig.Translate> ConvertFromText(string text)
        {
            var translates = new List<StateConfig.Translate>();
            var lines = text.Split("\n", StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                var lineInfo = line.Split(">");
                translates.Add(new StateConfig.Translate()
                {
                    source = Enum.Parse<PlayerStateID>(lineInfo[0].Trim()),
                    target = Enum.Parse<PlayerStateID>(lineInfo[1].Trim())
                });
            }
            return translates;
        }
    }
}
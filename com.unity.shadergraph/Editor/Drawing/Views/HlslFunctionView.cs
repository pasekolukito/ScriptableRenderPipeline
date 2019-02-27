﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Graphing;

namespace UnityEditor.ShaderGraph.Drawing
{
    [Serializable]
    public enum HlslSourceType { File, String };

    internal class HlslFunctionView : VisualElement
    {
        private EnumField m_Type;
        private TextField m_FunctionName;
        private TextField m_FunctionSource;
        private TextField m_FunctionBody;

        internal HlslFunctionView(CustomFunctionNode node)
        {
            styleSheets.Add(Resources.Load<StyleSheet>("Styles/HlslFunctionView"));
            Draw(node);            
        }

        private void Draw(CustomFunctionNode node)
        {
            var currentControls = this.Children().ToArray();
            for(int i = 0; i < currentControls.Length; i++)
                currentControls[i].RemoveFromHierarchy();

            m_Type = new EnumField(node.sourceType);
            m_Type.RegisterValueChangedCallback(s =>
            {
                if((HlslSourceType)s.newValue != node.sourceType)
                {
                    node.owner.owner.RegisterCompleteObjectUndo("Function Change");
                    node.sourceType = (HlslSourceType)s.newValue;
                    Draw(node);
                    node.Dirty(ModificationScope.Graph);
                }
            });

            m_FunctionName = new TextField { value = node.functionName, multiline = false };
            m_FunctionName.RegisterCallback<FocusInEvent>(s =>
            {
                if(m_FunctionName.value == CustomFunctionNode.defaultFunctionName)
                    m_FunctionName.value = "";
            });
            m_FunctionName.RegisterCallback<FocusOutEvent>(s =>
            {
                if(m_FunctionName.value == "")
                    m_FunctionName.value = CustomFunctionNode.defaultFunctionName;
                    
                if(m_FunctionName.value != node.functionName)
                {
                    node.owner.owner.RegisterCompleteObjectUndo("Function Change");
                    node.functionName = m_FunctionName.value;
                    node.Dirty(ModificationScope.Graph);
                }
            });

            m_FunctionSource = new TextField { value = node.functionSource, multiline = false };
            m_FunctionSource.RegisterCallback<FocusInEvent>(s =>
            {
                if(m_FunctionSource.value == CustomFunctionNode.defaultFunctionSource)
                    m_FunctionSource.value = "";
            });
            m_FunctionSource.RegisterCallback<FocusOutEvent>(s =>
            {
                if(m_FunctionSource.value == "")
                    m_FunctionSource.value = CustomFunctionNode.defaultFunctionSource;

                if(m_FunctionSource.value != node.functionSource)
                {
                    node.owner.owner.RegisterCompleteObjectUndo("Function Change");
                    node.functionSource = m_FunctionSource.value;
                    node.Dirty(ModificationScope.Graph);
                }
            });

            m_FunctionBody = new TextField { value = node.functionBody, multiline = true };
            m_FunctionBody.RegisterCallback<FocusInEvent>(s =>
            {
                if(m_FunctionBody.value == CustomFunctionNode.defaultFunctionBody)
                    m_FunctionBody.value = "";
            });
            m_FunctionBody.RegisterCallback<FocusOutEvent>(s =>
            {
                if(m_FunctionBody.value  == "")
                    m_FunctionBody.value = CustomFunctionNode.defaultFunctionBody;

                if(m_FunctionBody.value != node.functionBody)
                {
                    node.owner.owner.RegisterCompleteObjectUndo("Function Change");
                    node.functionBody = m_FunctionBody.value;
                    node.Dirty(ModificationScope.Graph);
                }
            });

            VisualElement typeRow = new VisualElement() { name = "Row" };
            {
                typeRow.Add(new Label("Type"));
                typeRow.Add(m_Type);
            }
            Add(typeRow);
            VisualElement nameRow = new VisualElement() { name = "Row" };
            {
                nameRow.Add(new Label("Name"));
                nameRow.Add(m_FunctionName);
            }
            Add(nameRow);
            switch(node.sourceType)
            {
                case HlslSourceType.File:
                    VisualElement sourceRow = new VisualElement() { name = "Row" };
                    {
                        sourceRow.Add(new Label("Source"));
                        sourceRow.Add(m_FunctionSource);
                    }
                    Add(sourceRow);
                    break;
                case HlslSourceType.String:
                    VisualElement bodyRow = new VisualElement() { name = "Row" };
                    {
                        bodyRow.Add(new Label("Body"));
                        bodyRow.style.height = 200;
                        bodyRow.Add(m_FunctionBody);
                    }
                    Add(bodyRow);
                    break;
            }
        }
    }
}

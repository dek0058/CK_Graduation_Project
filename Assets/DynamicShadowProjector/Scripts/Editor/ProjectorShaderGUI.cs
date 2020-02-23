//
// ProjectorShaderGUI.cs
//
// Dynamic Shadow Projector
//
// Copyright 2019 NYAHOON GAMES PTE. LTD. All Rights Reserved.
//

using UnityEngine;
using UnityEditor;

namespace DynamicShadowProjector {
	public class ProjectorShaderGUI : ShaderGUI {
		private enum ProjectorType {
			UnityProjector,
			CustomProjector,
			ProjectorForLWRP
		}
		static private string ProjectorTypeToKeyword(ProjectorType type)
		{
			switch (type) {
			case ProjectorType.CustomProjector:
				return "FSR_RECEIVER";
			case ProjectorType.ProjectorForLWRP:
				return "FSR_PROJECTOR_FOR_LWRP";
			case ProjectorType.UnityProjector:
				return "";
			}
			return null;
		}
		public override void OnGUI (MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			base.OnGUI (materialEditor, properties);
			Material material = materialEditor.target as Material;
			ProjectorType currentType = ProjectorType.UnityProjector;
			if (material.IsKeywordEnabled ("FSR_RECEIVER")) {
				currentType = ProjectorType.CustomProjector;
			} else if (material.IsKeywordEnabled ("FSR_PROJECTOR_FOR_LWRP")) {
				currentType = ProjectorType.ProjectorForLWRP;
			}
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 150;
			ProjectorType newType = (ProjectorType)EditorGUILayout.EnumPopup("Projector Type", currentType);
			EditorGUIUtility.labelWidth = oldLabelWidth;
			if (newType != currentType) {
				Undo.RecordObject (material, "Change Projector Type");
				string keyword = ProjectorTypeToKeyword (currentType);
				if (!string.IsNullOrEmpty (keyword)) {
					material.DisableKeyword (keyword);
				}
				keyword = ProjectorTypeToKeyword (newType);
				if (!string.IsNullOrEmpty (keyword)) {
					material.EnableKeyword (keyword);
				}
			}
		}
	}
}

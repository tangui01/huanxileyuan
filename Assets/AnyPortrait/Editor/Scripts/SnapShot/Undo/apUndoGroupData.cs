﻿/*
*	Copyright (c) 2017-2021. RainyRizzle. All rights reserved
*	Contact to : https://www.rainyrizzle.com/ , contactrainyrizzle@gmail.com
*
*	This file is part of [AnyPortrait].
*
*	AnyPortrait can not be copied and/or distributed without
*	the express perission of [Seungjik Lee] of [RainyRizzle team].
*
*	It is illegal to download files from other than the Unity Asset Store and RainyRizzle homepage.
*	In that case, the act could be subject to legal sanctions.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

using AnyPortrait;

namespace AnyPortrait
{

	

	/// <summary>
	/// Unity의 Undo 기능을 사용할 때, 불필요한 호출을 막는 용도
	/// "연속된 동일한 요청"을 방지한다.
	/// 중복 체크만 하는 것이므로 1개의 값만 가진다.
	/// </summary>
	public class apUndoGroupData
	{
		// Singletone
		//---------------------------------------------------
		private static apUndoGroupData _instance = new apUndoGroupData();
		public static apUndoGroupData I { get { return _instance; } }

		// Members
		//--------------------------------------------------

		private ACTION _action = ACTION.None;
		[Flags]
		public enum SAVE_TARGET : int
		{
			None = 0,
			Portrait = 1,
			Mesh = 2,
			MeshGroup = 4,
			AllMeshGroups = 8,
			Modifier = 16,
			AllModifiers = 32,		
			AllMeshes = 64
		}
		private SAVE_TARGET _saveTarget = SAVE_TARGET.None;
		private apPortrait _portrait = null;
		private apMesh _mesh = null;
		private apMeshGroup _meshGroup = null;
		private apModifierBase _modifier = null;


		//private object _keyObject = null;//키 오브젝트는 삭제. 사용하는 일이 거의 없다.
		private bool _isCallContinuous = false;//여러 항목을 동시에 처리하는 Batch 액션 중인가

		private DateTime _lastUndoTime = new DateTime();
		private bool _isFirstAction = true;

		private const float CONT_SAVE_TIME = 5.0f;//이전 : 1초마다 Cont 작업을 분절해서 Undo > 5초로 변경 (21.7.17)

		public enum ACTION
		{
			None,
			Main_AddImage,
			Main_RemoveImage,
			Main_AddMesh,
			Main_RemoveMesh,
			Main_AddMeshGroup,
			Main_RemoveMeshGroup,
			Main_AddAnimation,
			Main_RemoveAnimation,
			Main_AddParam,
			Main_RemoveParam,

			Portrait_SettingChanged,
			Portrait_BakeOptionChanged,
			Portrait_SetMeshGroup,
			Portrait_ReleaseMeshGroup,

			

			Image_SettingChanged,

			Image_PSDImport,

			MeshEdit_AddVertex,
			MeshEdit_EditVertex,
			MeshEdit_EditVertexDepth,
			MeshEdit_RemoveVertex,
			MeshEdit_ResetVertices,
			MeshEdit_RemoveAllVertices,
			MeshEdit_AddEdge,
			MeshEdit_EditEdge,
			MeshEdit_RemoveEdge,
			MeshEdit_MakeEdges,
			MeshEdit_EditPolygons,
			MeshEdit_SetImage,
			MeshEdit_SetPivot,
			MeshEdit_SettingChanged,
			MeshEdit_AtlasChanged,
			MeshEdit_AutoGen,
			MeshEdit_VertexCopied,
			MeshEdit_VertexMoved,

			MeshGroup_AttachMesh,
			MeshGroup_AttachMeshGroup,
			MeshGroup_DetachMesh,
			MeshGroup_DetachMeshGroup,
			MeshGroup_ClippingChanged,
			MeshGroup_DepthChanged,
			MeshGroup_AddBone,
			MeshGroup_RemoveBone,
			MeshGroup_RemoveAllBones,
			MeshGroup_BoneSettingChanged,
			MeshGroup_BoneDefaultEdit,
			MeshGroup_AttachBoneToChild,
			MeshGroup_DetachBoneFromChild,
			MeshGroup_SetBoneAsParent,
			MeshGroup_SetBoneAsIKTarget,
			MeshGroup_AddBoneFromRetarget,
			MeshGroup_BoneIKControllerChanged,
			MeshGroup_BoneMirrorChanged,

			MeshGroup_DuplicateMeshTransform,
			MeshGroup_DuplicateMeshGroupTransform,
			MeshGroup_DuplicateBone,


			MeshGroup_Gizmo_MoveTransform,
			MeshGroup_Gizmo_RotateTransform,
			MeshGroup_Gizmo_ScaleTransform,
			MeshGroup_Gizmo_Color,

			MeshGroup_AddModifier,
			MeshGroup_RemoveModifier,
			MeshGroup_RemoveParamSet,

			MeshGroup_DefaultSettingChanged,
			MeshGroup_MigrateMeshTransform,


			Modifier_LinkControlParam,
			Modifier_UnlinkControlParam,
			Modifier_AddStaticParamSetGroup,

			Modifier_LayerChanged,
			Modifier_SettingChanged,
			Modifier_SetBoneWeight,
			Modifier_RemoveBoneWeight,
			Modifier_RemoveBoneRigging,
			Modifier_RemovePhysics,
			Modifier_SetPhysicsWeight,
			Modifier_SetVolumeWeight,
			Modifier_SetPhysicsProperty,

			Modifier_ExtraOptionChanged,

			Modifier_Gizmo_MoveTransform,
			Modifier_Gizmo_RotateTransform,
			Modifier_Gizmo_ScaleTransform,
			Modifier_Gizmo_BoneIKTransform,
			Modifier_Gizmo_MoveVertex,
			Modifier_Gizmo_RotateVertex,
			Modifier_Gizmo_ScaleVertex,
			Modifier_Gizmo_FFDVertex,
			Modifier_Gizmo_Color,
			Modifier_Gizmo_BlurVertex,

			Modifier_ModMeshValuePaste,
			Modifier_ModMeshValueReset,
			Modifier_AddModMeshToParamSet,
			Modifier_RemoveModMeshFromParamSet,

			Modifier_RiggingWeightChanged,

			Modifier_FFDStart,
			Modifier_FFDAdapt,
			Modifier_FFDRevert,

			Anim_SetMeshGroup,
			Anim_DupAnimClip,
			Anim_ImportAnimClip,
			Anim_AddTimeline,
			Anim_RemoveTimeline,
			Anim_AddTimelineLayer,
			Anim_RemoveTimelineLayer,
			Anim_AddKeyframe,
			Anim_MoveKeyframe,
			Anim_CopyKeyframe,
			Anim_RemoveKeyframe,
			Anim_DupKeyframe,
			Anim_KeyframeValueChanged,
			Anim_AddEvent,
			Anim_RemoveEvent,
			Anim_SortEvents,
			Anim_EventChanged,

			Anim_Gizmo_MoveTransform,
			Anim_Gizmo_RotateTransform,
			Anim_Gizmo_ScaleTransform,
			Anim_Gizmo_BoneIKControllerTransform,

			Anim_Gizmo_MoveVertex,
			Anim_Gizmo_RotateVertex,
			Anim_Gizmo_ScaleVertex,
			Anim_Gizmo_FFDVertex,
			Anim_Gizmo_BlurVertex,

			Anim_Gizmo_Color,

			Anim_SettingChanged,

			ControlParam_SettingChanged,
			ControlParam_Duplicated,

			Retarget_ImportSinglePoseToMod,
			Retarget_ImportSinglePoseToAnim,

			PSDSet_AddNewPSDSet,

			MaterialSetAdded,
			MaterialSetRemoved,
			MaterialSetChanged,

			VisibilityChanged,
			GuidelineChanged,
		}

		private Dictionary<ACTION, string> _undoLabels = null;


		public static string GetLabel(ACTION action)
		{
			return I._undoLabels[action];			
		}

		// Init
		//--------------------------------------------------
		private apUndoGroupData()
		{
			_lastUndoTime = DateTime.Now;
			_isFirstAction = true;

			_undoLabels = new Dictionary<ACTION, string>();

			//중요 : 텍스트를 추가한다. (21.1.25)
			
			_undoLabels.Add(ACTION.None, "None");

			_undoLabels.Add(ACTION.Main_AddImage,			"Add Image");
			_undoLabels.Add(ACTION.Main_RemoveImage,		"Remove Image");
			_undoLabels.Add(ACTION.Main_AddMesh,			"Add Mesh");
			_undoLabels.Add(ACTION.Main_RemoveMesh,			"Remove Mesh");
			_undoLabels.Add(ACTION.Main_AddMeshGroup,		"Add MeshGroup");
			_undoLabels.Add(ACTION.Main_RemoveMeshGroup,	"Remove MeshGroup");
			_undoLabels.Add(ACTION.Main_AddAnimation,		"Add Animation");
			_undoLabels.Add(ACTION.Main_RemoveAnimation,	"Remove Animation");
			_undoLabels.Add(ACTION.Main_AddParam,			"Add Parameter");
			_undoLabels.Add(ACTION.Main_RemoveParam,		"Remove Parameter");

			_undoLabels.Add(ACTION.Portrait_SettingChanged,		"Portrait Setting Changed");
			_undoLabels.Add(ACTION.Portrait_BakeOptionChanged,	"Bake Option Changed");
			_undoLabels.Add(ACTION.Portrait_SetMeshGroup,		"Set Main MeshGroup");
			_undoLabels.Add(ACTION.Portrait_ReleaseMeshGroup,	"Release Main MeshGroup");

			_undoLabels.Add(ACTION.Image_SettingChanged,	"Set Image Property");
			_undoLabels.Add(ACTION.Image_PSDImport,			"Import PSD");

			_undoLabels.Add(ACTION.MeshEdit_AddVertex,			"Add Vertex");
			_undoLabels.Add(ACTION.MeshEdit_EditVertex,			"Edit Vertex");
			_undoLabels.Add(ACTION.MeshEdit_EditVertexDepth,	"Edit Vertex Settings");
			
			_undoLabels.Add(ACTION.MeshEdit_RemoveVertex,		"Remove Vertex");
			_undoLabels.Add(ACTION.MeshEdit_ResetVertices,		"Reset Vertices");
			_undoLabels.Add(ACTION.MeshEdit_RemoveAllVertices,	"Remove All Vertices");
			_undoLabels.Add(ACTION.MeshEdit_AddEdge,			"Add Edge");
			_undoLabels.Add(ACTION.MeshEdit_EditEdge,			"Edit Edge");
			_undoLabels.Add(ACTION.MeshEdit_RemoveEdge,			"Remove Edge");
			_undoLabels.Add(ACTION.MeshEdit_MakeEdges,			"Make Edges");
			_undoLabels.Add(ACTION.MeshEdit_EditPolygons,		"Edit Polygons");
			_undoLabels.Add(ACTION.MeshEdit_SetImage,			"Set Image");
			_undoLabels.Add(ACTION.MeshEdit_SetPivot,			"Set Mesh Pivot");
			_undoLabels.Add(ACTION.MeshEdit_SettingChanged,		"Mesh Setting Changed");
			_undoLabels.Add(ACTION.MeshEdit_AtlasChanged,		"Mesh Atals Changed");
			_undoLabels.Add(ACTION.MeshEdit_AutoGen,			"Vertices Generated");
			_undoLabels.Add(ACTION.MeshEdit_VertexCopied,		"Vertices Copied");
			_undoLabels.Add(ACTION.MeshEdit_VertexMoved,		"Vertices Moved");

			_undoLabels.Add(ACTION.MeshGroup_AttachMesh,		"Attach Mesh");
			_undoLabels.Add(ACTION.MeshGroup_AttachMeshGroup,	"Attach MeshGroup");
			_undoLabels.Add(ACTION.MeshGroup_DetachMesh,		"Detach Mesh");
			_undoLabels.Add(ACTION.MeshGroup_DetachMeshGroup,	"Detach MeshGroup");
			_undoLabels.Add(ACTION.MeshGroup_ClippingChanged,	"Clipping Changed");
			_undoLabels.Add(ACTION.MeshGroup_DepthChanged,		"Depth Changed");
			
			_undoLabels.Add(ACTION.MeshGroup_AddBone,					"Add Bone");
			_undoLabels.Add(ACTION.MeshGroup_RemoveBone,				"Remove Bone");
			_undoLabels.Add(ACTION.MeshGroup_RemoveAllBones,			"Remove All Bones");
			_undoLabels.Add(ACTION.MeshGroup_BoneSettingChanged,		"Bone Setting Changed");
			_undoLabels.Add(ACTION.MeshGroup_BoneDefaultEdit,			"Bone Edit");
			_undoLabels.Add(ACTION.MeshGroup_AttachBoneToChild,			"Attach Bone to Child");
			_undoLabels.Add(ACTION.MeshGroup_DetachBoneFromChild,		"Detach Bone from Child");
			_undoLabels.Add(ACTION.MeshGroup_SetBoneAsParent,			"Set Bone as Parent");
			_undoLabels.Add(ACTION.MeshGroup_SetBoneAsIKTarget,			"Set Bone as IK target");
			_undoLabels.Add(ACTION.MeshGroup_AddBoneFromRetarget,		"Add Bones from File");
			_undoLabels.Add(ACTION.MeshGroup_BoneIKControllerChanged,	"IK Controller Changed");
			_undoLabels.Add(ACTION.MeshGroup_BoneMirrorChanged,			"Mirror Changed");

			_undoLabels.Add(ACTION.MeshGroup_DuplicateMeshTransform,		"Duplicate Mesh Transform");
			_undoLabels.Add(ACTION.MeshGroup_DuplicateMeshGroupTransform,	"Duplicate Mesh Group Transform");
			_undoLabels.Add(ACTION.MeshGroup_DuplicateBone,					"Duplicate Bone");

			_undoLabels.Add(ACTION.MeshGroup_Gizmo_MoveTransform,		"Default Position");
			_undoLabels.Add(ACTION.MeshGroup_Gizmo_RotateTransform,		"Default Rotation");
			_undoLabels.Add(ACTION.MeshGroup_Gizmo_ScaleTransform,		"Default Scaling");
			_undoLabels.Add(ACTION.MeshGroup_Gizmo_Color,				"Default Color");

			_undoLabels.Add(ACTION.MeshGroup_AddModifier,		"Add Modifier");
			_undoLabels.Add(ACTION.MeshGroup_RemoveModifier,	"Remove Modifier");
			_undoLabels.Add(ACTION.MeshGroup_RemoveParamSet,	"Remove Modified Key");

			_undoLabels.Add(ACTION.MeshGroup_DefaultSettingChanged,	"Default Setting Changed");
			_undoLabels.Add(ACTION.MeshGroup_MigrateMeshTransform,	"Migrate Mesh Transform");

			_undoLabels.Add(ACTION.Modifier_LinkControlParam,			"Link Control Parameter");
			_undoLabels.Add(ACTION.Modifier_UnlinkControlParam,			"Unlink Control Parameter");
			_undoLabels.Add(ACTION.Modifier_AddStaticParamSetGroup,		"Add StaticPSG");

			_undoLabels.Add(ACTION.Modifier_LayerChanged,			"Change Layer Order");
			_undoLabels.Add(ACTION.Modifier_SettingChanged,			"Change Layer Setting");
			_undoLabels.Add(ACTION.Modifier_SetBoneWeight,			"Set Bone Weight");
			_undoLabels.Add(ACTION.Modifier_RemoveBoneWeight,		"Remove Bone Weight");
			_undoLabels.Add(ACTION.Modifier_RemoveBoneRigging,		"Remove Bone Rigging");
			_undoLabels.Add(ACTION.Modifier_RemovePhysics,			"Remove Physics");
			_undoLabels.Add(ACTION.Modifier_SetPhysicsWeight,		"Set Physics Weight");
			_undoLabels.Add(ACTION.Modifier_SetVolumeWeight,		"Set Volume Weight");
			_undoLabels.Add(ACTION.Modifier_SetPhysicsProperty,		"Set Physics Property");

			_undoLabels.Add(ACTION.Modifier_ExtraOptionChanged,		"Extra Option Changed");

			_undoLabels.Add(ACTION.Modifier_Gizmo_MoveTransform,	"Move Transform");
			_undoLabels.Add(ACTION.Modifier_Gizmo_RotateTransform,	"Rotate Transform");
			_undoLabels.Add(ACTION.Modifier_Gizmo_ScaleTransform,	"Scale Transform");
			_undoLabels.Add(ACTION.Modifier_Gizmo_BoneIKTransform,	"FK/IK Weight Changed");
			_undoLabels.Add(ACTION.Modifier_Gizmo_MoveVertex,		"Move Vertex");
			_undoLabels.Add(ACTION.Modifier_Gizmo_RotateVertex,		"Rotate Vertex");
			_undoLabels.Add(ACTION.Modifier_Gizmo_ScaleVertex,		"Scale Vertex");
			_undoLabels.Add(ACTION.Modifier_Gizmo_FFDVertex,		"Freeform Vertices");
			_undoLabels.Add(ACTION.Modifier_Gizmo_Color,			"Set Color");
			_undoLabels.Add(ACTION.Modifier_Gizmo_BlurVertex,		"Blur Vertices");

			_undoLabels.Add(ACTION.Modifier_ModMeshValuePaste,	"Paste Modified Value");
			_undoLabels.Add(ACTION.Modifier_ModMeshValueReset,	"Reset Modified Value");

			_undoLabels.Add(ACTION.Modifier_AddModMeshToParamSet,		"Add To Key");
			_undoLabels.Add(ACTION.Modifier_RemoveModMeshFromParamSet,	"Remove From Key");

			_undoLabels.Add(ACTION.Modifier_RiggingWeightChanged, "Weight Changed");

			_undoLabels.Add(ACTION.Modifier_FFDStart,	"Edit FFD");
			_undoLabels.Add(ACTION.Modifier_FFDAdapt,	"Adapt FFD");
			_undoLabels.Add(ACTION.Modifier_FFDRevert,	"Revert FFD");

			_undoLabels.Add(ACTION.Anim_SetMeshGroup,			"Set MeshGroup");
			_undoLabels.Add(ACTION.Anim_DupAnimClip,			"Duplicate AnimClip");
			_undoLabels.Add(ACTION.Anim_ImportAnimClip,			"Import AnimClip");
			_undoLabels.Add(ACTION.Anim_AddTimeline,			"Add Timeline");
			_undoLabels.Add(ACTION.Anim_RemoveTimeline,			"Remove Timeline");
			_undoLabels.Add(ACTION.Anim_AddTimelineLayer,		"Add Timeline Layer");
			_undoLabels.Add(ACTION.Anim_RemoveTimelineLayer,	"Remove Timeline Layer");

			_undoLabels.Add(ACTION.Anim_AddKeyframe,		"Add Keyframe");
			_undoLabels.Add(ACTION.Anim_MoveKeyframe,		"Move Keyframe");
			_undoLabels.Add(ACTION.Anim_CopyKeyframe,		"Copy Keyframe");
			_undoLabels.Add(ACTION.Anim_RemoveKeyframe,		"Remove Keyframe");
			_undoLabels.Add(ACTION.Anim_DupKeyframe,		"Duplicate Keyframe");

			_undoLabels.Add(ACTION.Anim_KeyframeValueChanged,	"Keyframe Value Changed");
			_undoLabels.Add(ACTION.Anim_AddEvent,				"Event Added");
			_undoLabels.Add(ACTION.Anim_RemoveEvent,			"Event Removed");
			_undoLabels.Add(ACTION.Anim_EventChanged,			"Event Changed");
			_undoLabels.Add(ACTION.Anim_SortEvents,				"Events Sorted");

			_undoLabels.Add(ACTION.Anim_Gizmo_MoveTransform,	"Move Transform");
			_undoLabels.Add(ACTION.Anim_Gizmo_RotateTransform,	"Rotate Transform");
			_undoLabels.Add(ACTION.Anim_Gizmo_ScaleTransform,	"Scale Transform");
			_undoLabels.Add(ACTION.Anim_Gizmo_BoneIKControllerTransform,	"FK/IK Weight Changed");

			_undoLabels.Add(ACTION.Anim_Gizmo_MoveVertex,		"Move Vertex");
			_undoLabels.Add(ACTION.Anim_Gizmo_RotateVertex,		"Rotate Vertex");
			_undoLabels.Add(ACTION.Anim_Gizmo_ScaleVertex,		"Scale Vertex");
			_undoLabels.Add(ACTION.Anim_Gizmo_FFDVertex,		"Freeform Vertices");
			_undoLabels.Add(ACTION.Anim_Gizmo_BlurVertex,		"Blur Vertices");
			_undoLabels.Add(ACTION.Anim_Gizmo_Color,			"Set Color");
			_undoLabels.Add(ACTION.Anim_SettingChanged,			"Animation Setting Changed");

			_undoLabels.Add(ACTION.ControlParam_SettingChanged, "Control Param Setting");
			_undoLabels.Add(ACTION.ControlParam_Duplicated,		"Control Param Duplicated");

			_undoLabels.Add(ACTION.Retarget_ImportSinglePoseToMod,	"Import Pose");
			_undoLabels.Add(ACTION.Retarget_ImportSinglePoseToAnim, "Import Pose");

			_undoLabels.Add(ACTION.PSDSet_AddNewPSDSet,		"New PSD Set");

			_undoLabels.Add(ACTION.MaterialSetAdded,	"Material Set Added");
			_undoLabels.Add(ACTION.MaterialSetRemoved,	"Material Set Removed");
			_undoLabels.Add(ACTION.MaterialSetChanged,	"Material Set Changed");

			_undoLabels.Add(ACTION.VisibilityChanged,	"Visibility Changed");
			_undoLabels.Add(ACTION.GuidelineChanged,	"Guideline Option Changed");
			
		}

		public void Clear()
		{
			_action = ACTION.None;
			_saveTarget = SAVE_TARGET.None;
			_portrait = null;
			_mesh = null;
			_meshGroup = null;
			_modifier = null;

			//_keyObject = null;
			_isCallContinuous = false;//여러 항목을 동시에 처리하는 Batch 액션 중인가

			_lastUndoTime = DateTime.Now;
		}




		// Functions
		//--------------------------------------------------
		/// <summary>
		/// Undo 전에 중복을 체크하기 위해 Action을 등록한다.
		/// 리턴값이 True이면 "새로운 Action"이므로 Undo 등록을 해야한다.
		/// 만약 Action 타입이 Add, New.. 계열이면 targetObject가 null일 수 있다. (parent는 null이 되어선 안된다)
		/// </summary>
		/// <returns>이어지지 않은 새로운 타입의 Undo Action이면 True</returns>
		//public bool SetAction(ACTION action, apPortrait portrait, apMesh mesh, apMeshGroup meshGroup, apModifierBase modifier, object keyObject, bool isCallContinuous, SAVE_TARGET saveTarget)
		public bool SetAction(ACTION action, apPortrait portrait, apMesh mesh, apMeshGroup meshGroup, apModifierBase modifier, bool isCallContinuous, SAVE_TARGET saveTarget)
		{	
			bool isTimeOver = false;
			double lastDeltaTime = DateTime.Now.Subtract(_lastUndoTime).TotalSeconds;
			if(lastDeltaTime > CONT_SAVE_TIME || _isFirstAction)
			{
				//Debug.Log("Undo Delta Time : " + lastDeltaTime + " > " + CONT_SAVE_TIME);

				//1초가 넘었다면 강제 Undo ID 증가
				isTimeOver = true;
				_lastUndoTime = DateTime.Now;
				_isFirstAction = false;

				
			}

			//특정 조건에서는 UndoID가 증가하지 않는다.
			//유효한 Action이고 시간이 지나지 않았다면
			if(_action != ACTION.None && !isTimeOver && isCallContinuous)
			{
				//이전과 값이 같을 때에만 Multiple 처리가 된다.
				if(	action == _action &&
					saveTarget == _saveTarget &&
					portrait == _portrait &&
					mesh == _mesh &&
					meshGroup == _meshGroup &&
					modifier == _modifier && 
					isCallContinuous == _isCallContinuous
					)
				{
					//연속 호출이면 KeyObject가 달라도 Undo를 묶는다.
					//>KeyObject는 무시
					return false;

					//if(isCallContinuous)
					//{
					//	//연속 호출이면 KeyObject가 달라도 Undo를 묶는다.
					//	//>KeyObject는 무시
					//	return false;
					//}
					//삭제 21.6.30
					//else if(keyObject == _keyObject && keyObject != null)
					//{
					//	//연속 호출이 아니더라도 KeyObject가 같으면 Undo를 묶는다.
					//	return false;
					//}
				}
			}


			#region [미사용 코드]
			//if (_action != ACTION.None && _parentMonoObject != null)
			//{
			//	if (_action == action && _parentMonoObject == parentMonoObject && isMultiple == _isMultiple)
			//	{
			//		if (_isMultiple)
			//		{
			//			//다중 처리 타입이면 -> targetObject가 달라도 연속된 액션이다.
			//			return false;
			//		}
			//		else
			//		{
			//			//Multiple 타입이 아니라면 targetObject도 동일해야한다.
			//			//단, 둘다 Null이라면 연속된 타입일 수 없다.
			//			if (targtObject == _keyObject && targtObject != null && _keyObject != null)
			//			{
			//				if (targetObject2 != null)
			//				{
			//					if(targetObject2 == _targetObject2)
			//					{
			//						return false;//연속된 Action이다.
			//					}
			//				}
			//				else
			//				{
			//					return false;//연속된 Action이다.
			//				}
			//			}
			//		}
			//	}
			//} 
			#endregion
			_action = action;

			_saveTarget = saveTarget;
			_portrait = portrait;
			_mesh = mesh;
			_meshGroup = meshGroup;
			_modifier = modifier;

			//_keyObject = keyObject;
			_isCallContinuous = isCallContinuous;//여러 항목을 동시에 처리하는 Batch 액션 중인가

			//_parentMonoObject = parentMonoObject;
			//_keyObject = targtObject;
			//_targetObject2 = targetObject2;
			//_isMultiple = isMultiple;

			//Debug.Log("Undo Regist [" + action + "]");
			return true;
		}


		/// <summary>
		/// 추가 21.6.30 : 마우스 Up, 다른 객체 선택 (작은 단위까지)시 Undo의 연속성을 초기화한다.
		/// 이 함수가 제대로 작동하면 KeyObject를 사용하지 않아도 된다.
		/// </summary>
		public void ResetContinuous()
		{
			_isFirstAction = true;
			_lastUndoTime = DateTime.Now;
		}
	}
}
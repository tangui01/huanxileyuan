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

	[Serializable]
	public class apModifier_TF : apModifierBase
	{
		// Members
		//----------------------------------------------
		//[NonSerialized]
		//private MODIFIER_TYPE[] _generalExEditableModType = new MODIFIER_TYPE[] {   MODIFIER_TYPE.Morph,
		//																		MODIFIER_TYPE.Rigging,
		//																		MODIFIER_TYPE.TF };

		// Init
		//----------------------------------------------
		//public apModifier_TF() : base()
		//{
		//}

		//public override void Init()
		//{

		//}


		// Get / Set
		//----------------------------------------------
		public override MODIFIER_TYPE ModifierType
		{
			get { return MODIFIER_TYPE.TF; }
		}

		public override apModifierParamSetGroup.SYNC_TARGET SyncTarget
		{
			get { return apModifierParamSetGroup.SYNC_TARGET.Controller; }
		}

		private const string NAME_TF_LONG = "Transform (Controller)";
		private const string NAME_TF_SHORT = "Transform (Ctrl)";

		public override string DisplayName
		{
			//get { return "Transform (Controller)"; }
			get { return NAME_TF_LONG; }
		}

		public override string DisplayNameShort
		{
			//get { return "Transform (Ctrl)"; }
			get { return NAME_TF_SHORT; }
		}
		/// <summary>
		/// Calculate 계산시 어느 단계에서 적용되는가
		/// </summary>
		public override apCalculatedResultParam.CALCULATED_VALUE_TYPE CalculatedValueType
		{
			get
			{
				return apCalculatedResultParam.CALCULATED_VALUE_TYPE.TransformMatrix |
						apCalculatedResultParam.CALCULATED_VALUE_TYPE.Color;
			}
		}

		public override apCalculatedResultParam.CALCULATED_SPACE CalculatedSpace
		{
			get { return apCalculatedResultParam.CALCULATED_SPACE.Local; }
		}

		public override apModifiedMesh.MOD_VALUE_TYPE ModifiedValueType
		{
			get
			{
				return apModifiedMesh.MOD_VALUE_TYPE.TransformMatrix |
						apModifiedMesh.MOD_VALUE_TYPE.Color;
			}
		}


		// MeshTransform + MeshGroupTransform에 적용한다.
		public override bool IsTarget_MeshTransform { get { return true; } }
		public override bool IsTarget_MeshGroupTransform { get { return true; } }
		public override bool IsTarget_Bone { get { return true; } }
		public override bool IsTarget_ChildMeshTransform { get { return true; } }


		public override bool IsUseParamSetWeight { get { return true; } }//ParamSet 자체의 OverlapWeight를 사용한다.

		//추가
		public override bool IsPhysics { get { return false; } }
		public override bool IsVolume { get { return false; } }

		///// <summary>
		///// ExEdit 중 GeneralEdit 모드에서 "동시에 작업 가능하도록 허용 된 Modifier Type들"을 리턴한다.
		///// </summary>
		///// <returns></returns>
		//public override MODIFIER_TYPE[] GetGeneralExEditableModTypes()
		//{
		//	return _generalExEditableModType;
		//}

		// Functions
		//----------------------------------------------
		public override void InitCalculate(float tDelta)
		{
			base.InitCalculate(tDelta);

			if (_calculatedResultParams.Count == 0)
			{
				return;
			}

			apCalculatedResultParam calParam = null;
			for (int iCalParam = 0; iCalParam < _calculatedResultParams.Count; iCalParam++)
			{
				calParam = _calculatedResultParams[iCalParam];
				calParam.InitCalculate();
				calParam._isAvailable = false;
			}
		}

		public override void Calculate(float tDelta)
		{
			base.Calculate(tDelta);

			CalculatePattern_Transform(tDelta);
		}

		public override void Calculate_DLL(float tDelta)
		{
			base.Calculate_DLL(tDelta);

			CalculatePattern_Transform_DLL(tDelta);
		}
	}

}
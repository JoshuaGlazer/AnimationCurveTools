
// Original code by: Joshua Glazer 
// Updated to Unity 4.3.4 by: Aurelio Provedo [aurelioprovedo@gmail.com]
using System;
using UnityEngine;
using UnityEditor;


public class AnimationCurveExtractor : EditorWindow
{
	
	
	public void Init(SerializedProperty inTargetProperty)
	{
		//keep the iterator in its current state...
		_PopupTargetAnimationCurveProperty = inTargetProperty;
	}
	
	void OnGUI()
	{
		AnimationClip anim = EditorGUILayout.ObjectField("Source Animation", _SourceAnimationClip, typeof(AnimationClip), false) as AnimationClip;
		
		if (anim != _SourceAnimationClip)
		{
			_SourceAnimationClip = anim;
			if (anim != null)
			{
				#if !UNITY_4_2
				_Curves = AnimationUtility.GetCurveBindings(anim);
				#else
				_Curves = AnimationUtility.GetAllCurves(anim);
				#endif
				
				_SelectedCurveIndex = 0;
				int curveCount = _Curves.Length;
				_CurveNames = new string[curveCount];
				for (int i = 0; i < curveCount; ++i)
				{
					if (_Curves[i].path == "")
					{
						_CurveNames[i] = _Curves[i].propertyName;
					}
					else
					{
						_CurveNames[i] = _Curves[i].path + "/" + _Curves[i].propertyName;
					}
				}
			}
			else
			{
				_Curves = null;
				_CurveNames = null;
			}
		}
		
		if (_Curves != null && _Curves.Length > 0)
		{
			_SelectedCurveIndex = EditorGUILayout.Popup("Source Curve", _SelectedCurveIndex, _CurveNames);
			
			#if !UNITY_4_2
			EditorGUILayout.CurveField("Data", AnimationUtility.GetEditorCurve(_SourceAnimationClip, _Curves[_SelectedCurveIndex]));
			#else
			EditorGUILayout.CurveField("Data", _Curves[_SelectedCurveIndex].curve);
			#endif
			
			/*
            _ShouldZeroOriginalCurve = EditorGUILayout.Toggle( "Should Zero Original Curve", _ShouldZeroOriginalCurve );
            */
			
			if (GUILayout.Button("Extract!"))
			{
				Extract();
				Close();
			}
		}
		
		
		
	}
	
	private void Extract()
	{
		#if !UNITY_4_2
		AnimationCurve sourceCurve = AnimationUtility.GetEditorCurve(_SourceAnimationClip, _Curves[_SelectedCurveIndex]);
		#else
		AnimationCurve sourceCurve = _Curves[_SelectedCurveIndex].curve;
		#endif
		
		_PopupTargetAnimationCurveProperty.animationCurveValue = AnimationCurveCopier.CreateCopy(sourceCurve);
		_PopupTargetAnimationCurveProperty.serializedObject.ApplyModifiedProperties();
		
		/*
         *we would need to copy this back in if we want it to work...
        if( _ShouldZeroOriginalCurve )
        {
            Keyframe[] keys = sourceCurve.keys;
            for( int i = 0, c = keys.Length; i < c; ++i )
            {
                keys[ i ].value = 0;
            }
            sourceCurve.keys = keys;
        }
        */
	}
	
	private SerializedProperty _PopupTargetAnimationCurveProperty;
	
	private AnimationClip _SourceAnimationClip;
	
	#if !UNITY_4_2
	private EditorCurveBinding[] _Curves;
	#else
	private AnimationClipCurveData[] _Curves;
	#endif
	
	private string[] _CurveNames;
	private int _SelectedCurveIndex;
	//private bool						_ShouldZeroOriginalCurve;
}


public static class AnimationCurveCopier
{
	public static void Copy(AnimationCurve inSource, AnimationCurve inDest)
	{
		inDest.keys = inSource.keys;
		inDest.preWrapMode = inSource.preWrapMode;
		inDest.postWrapMode = inSource.postWrapMode;
	}
	
	public static AnimationCurve CreateCopy(AnimationCurve inSource)
	{
		AnimationCurve newCurve = new AnimationCurve();
		Copy(inSource, newCurve);
		return newCurve;
	}
}


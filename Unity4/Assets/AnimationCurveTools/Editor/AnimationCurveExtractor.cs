using System;
using UnityEngine;
using UnityEditor;


public class AnimationCurveExtractor : EditorWindow
{

	
	public void Init( SerializedProperty inTargetProperty )
	{
		//keep the iterator in its current state...
		_PopupTargetAnimationCurveProperty = inTargetProperty;
	}
	
	void OnGUI()
	{
		AnimationClip anim = EditorGUILayout.ObjectField( "Source Animation", _SourceAnimationClip, typeof( AnimationClip ), false ) as AnimationClip;
		
		if( anim != _SourceAnimationClip )
		{
			if( anim != null )
			{
				_Curves = AnimationUtility.GetAllCurves ( anim );
				_SelectedCurveIndex = 0;
				int curveCount = _Curves.Length;
				_CurveNames = new string[ curveCount ];
				for( int i = 0; i < curveCount; ++i )
				{
					_CurveNames[ i ] = _Curves[ i ].path + "/" + _Curves[ i ].propertyName;
				}
			}
			else
			{
				_Curves = null;
				_CurveNames = null;
			}
			
			_SourceAnimationClip = anim;
		}
		
		if( _Curves != null && _Curves.Length > 0 )
		{
			_SelectedCurveIndex = EditorGUILayout.Popup( "Source Curve", _SelectedCurveIndex, _CurveNames );
		
			EditorGUILayout.CurveField( "Data", _Curves[ _SelectedCurveIndex ].curve );
		
			/*
			_ShouldZeroOriginalCurve = EditorGUILayout.Toggle( "Should Zero Original Curve", _ShouldZeroOriginalCurve );
			*/
			
			if( GUILayout.Button( "Extract!" ) )
			{
				Extract();
				Close ();
			}
		}
			
		
		
	}
	
	private void Extract()
	{
		AnimationCurve sourceCurve = _Curves[ _SelectedCurveIndex ].curve;
		
		_PopupTargetAnimationCurveProperty.animationCurveValue = AnimationCurveCopier.CreateCopy( sourceCurve );
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
	
	private SerializedProperty			_PopupTargetAnimationCurveProperty;
	
	private AnimationClip				_SourceAnimationClip;
	
	private AnimationClipCurveData[]	_Curves;
	private string[]					_CurveNames;
	private int							_SelectedCurveIndex;
	//private bool						_ShouldZeroOriginalCurve;
	
	
	
}

public static class AnimationCurveCopier
{
	public static void Copy( AnimationCurve inSource, AnimationCurve inDest )
	{
		inDest.keys = inSource.keys;
		inDest.preWrapMode = inSource.preWrapMode;
		inDest.postWrapMode = inSource.postWrapMode;
	}
	
	public static AnimationCurve CreateCopy( AnimationCurve inSource )
	{
		AnimationCurve newCurve = new AnimationCurve();
		Copy( inSource, newCurve );
		return newCurve;
	}
}


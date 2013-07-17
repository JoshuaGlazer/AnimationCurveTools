using System;
using UnityEngine;
using UnityEditor;


[CustomPropertyDrawer( typeof( AnimationCurve ) )]
public class BetterAnimationCurveFieldDrawer : PropertyDrawer 
{
	[MenuItem ("CONTEXT/AnimationCurve/Extract From Animation...")]
    static void ExtractAnimationCurve( MenuCommand inCommand ) 
	{
		if( _PopupTargetAnimationCurveProperty != null )
		{
			AnimationCurveExtractor aceWindow = AnimationCurveExtractor.GetWindow( typeof( AnimationCurveExtractor ) ) as AnimationCurveExtractor;
			aceWindow.Init( _PopupTargetAnimationCurveProperty );
		}
    }
	
	[MenuItem ("CONTEXT/AnimationCurve/Copy Animation Curve")]
    static void CopyAnimationCurve( MenuCommand inCommand ) 
	{
		if( _PopupTargetAnimationCurveProperty != null )
		{
			_ClipBoardAnimationCurve = AnimationCurveCopier.CreateCopy( _PopupTargetAnimationCurveProperty.animationCurveValue );
		}
    }
	
	[MenuItem ("CONTEXT/AnimationCurve/Paste Animation Curve")]
    static void PasteAnimationCurve( MenuCommand inCommand ) 
	{
		if( _PopupTargetAnimationCurveProperty != null )
		{
			_PopupTargetAnimationCurveProperty.serializedObject.Update();
			_PopupTargetAnimationCurveProperty.animationCurveValue = AnimationCurveCopier.CreateCopy( _ClipBoardAnimationCurve );
			_PopupTargetAnimationCurveProperty.serializedObject.ApplyModifiedProperties();
		}
    }
	
	
	
    // Draw the property inside the given rect
   	public override void OnGUI ( Rect inRect, SerializedProperty inProperty, GUIContent inLabel ) 
	{

		var evt = Event.current;
		if( evt.type == EventType.ContextClick ) 
		{
        	var mousePos = evt.mousePosition;
			if ( inRect.Contains( mousePos ) )
			{
				_PopupTargetAnimationCurveProperty = inProperty.Copy();
				inProperty.serializedObject.Update();
				EditorUtility.DisplayPopupMenu( new Rect( mousePos.x,mousePos.y, 0, 0 ), "CONTEXT/AnimationCurve/", null );
			}
		}
		else
		{
			inLabel = EditorGUI.BeginProperty( inRect, inLabel, inProperty );
			EditorGUI.BeginChangeCheck ();
			AnimationCurve newCurve = EditorGUI.CurveField( inRect, inLabel, inProperty.animationCurveValue );
			
			if( EditorGUI.EndChangeCheck() )
			{
				inProperty.animationCurveValue = newCurve;
			}
			
			EditorGUI.EndProperty ();
			
			
		}
		
        
        
    }
	
	private static AnimationCurve _ClipBoardAnimationCurve = new AnimationCurve();

	//meun command context isn't working, so we'll just stash it here...	
	private static SerializedProperty _PopupTargetAnimationCurveProperty = null;
	
}

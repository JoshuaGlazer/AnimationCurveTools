AnimationCurveTools
===================

Tools for copying and pasting Unity3d AnimationCurves, and for extracting AnimationCurves from imported Animations

I use AnimationCurves for a lot of things ( specifically tweens, changing colors, simulating root motion without Mecanim, faking physical interactions between adjacent pieces in a game I'm working on ) and sometimes it's really helpful to be able to copy and paste a curve from one prefab to another. Unity left out this feature, so here's a hand project that supports it.  With this custom AnimationCurve inspector, you can right click on the name of an AnimationCurve field in the inspector and select "Copy Animation Curve":


Then you can click on any other AnimationCurve field and paste that data in using the "Paste Animation Curve" option and the curve will be copied over for you to edit or use how you please.



You may have noticed that there's a third option in the popup menu- "Extract From Animation..."  Instead of copying and pasting, this will let you take data from an Animation imported as part of an FBX and copy it into your AnimationCurve. I wrote this for Max Axe because, for performance reasons on old hardware, we had to switch from Mecanim back to legacy animation and didn't want to lose the root motion built into the main character's animation.  So, I extracted the root motion channel from Max's run animation and used that to offset his position in the world after the artists had nulled the track back out.  To extract an animation, simply choose the top option from the popup menu:


This will open up a new Animation Curve window, waiting for you to select an Animation.  This can be any animation- either a standalone .Anim or an animation embedded in an .fbx.   After you've done that, you can select the transform and channel you'd like to extract, from a nicely hierarchical menu setup that mirrors your skeleton.


In this case I'm just grabbing the m_LocalPosition.z from the C_Global_ctrl since that has Max's root motion translation in it ( he doesn't move form side to side )

After that, you'll see the Data from that curve nicely display in the Data field, so you can make sure you selected what you thought you selected and not just some similarly named controller left in the exported art file.


Finally, press Extract and the data will get copied into your curve!


There you go, ready to rock with your Animation Curve Tools and your awesome new Animation Curve data.  Cheers!

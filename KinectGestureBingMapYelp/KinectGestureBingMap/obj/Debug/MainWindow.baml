   M S B A M L   `   `   `  ����d  `Microsoft.Maps.MapControl.WPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPFMicrosoft.Maps.MapControl.WPF  5   	    KinectStreams   KinectBing.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   `mRclr-namespace:Microsoft.Maps.MapControl.WPF;assembly=Microsoft.Maps.MapControl.WPF     5   	   -   5          � Title$  KinectUS��5   	   $
��1080��6   $
��1920��6(   .��5      � .+�5   
   '   !Microsoft.Maps.MapControl.WPF.Map  -   5          Name$ 	kinectMap��5   �   +   %Microsoft.Maps.MapControl.WPF.MapCore   CredentialsProviderT   NMicrosoft.Maps.MapControl.WPF.Design.ApplicationIdCredentialsProviderConverter$F @Ai2mtVcHwBo0YSBB9ojWktBBdnwgR4NzAyiLrH0lajDDr2tBHQhnA0MkG0i7C81C 6      Center<   6Microsoft.Maps.MapControl.WPF.Design.LocationConverter$ 37.7876172, -122.398832 6g      	ZoomLevel$ 12Q�6�   6�   �� 5	   
   -   5        $��camera��5	      $	��400��6   $
��Left=�6+   $	��Top=�6F   �� 5   
   -   5        $��canvas��5      $
��1920��6   $
��1080��6,   $
��Left=�6:   $	��Top=�6U   �� 5   
   -   5        $��
infoCanvas��5      � 5   
      GridButtonsStyle#��[  6   .+�5      �� -   5         ��Name$ 	textBlock��5      $
��Left=�6+   $
��Wrap=�6F   $E��?Navigation: Put your right hand up, move to preferred direction��6Z   
���  ��$��381,-76,0,76q�6�   $
��Bold"�6�   $��14(�6�   �� 5      -   5        $ 
textBlock1��5      $
��Left=�6,   $
��Wrap=�6G   $I��CZoomOut: Put both hands up apart for 3 sec, then move them together��6[   
���  ��$��381,-46,0,46q�6�   $��14(�6�   $
��Bold"�6�   �� 5      -   5        $ 
textBlock2��5      $
��Left=�6,   $
��Wrap=�6G   $H��BZoomIn: Put both hands up together for 3 sec, then move them apart��6[   
���  ��$��381,-18,0,18q�6�   $��14(�6�   $
��Bold"�6�   �� 5        StackPanelButtonsStyle#��[ 6   .+�5      �� -	   5        $��Begin��5        ButtonStyle#��[ 6)   6N   5      5      5      5      
   M S B A M L   `   `   `  ����E  AEasyMatrix, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:EasyMatrix
EasyMatrix  5   	       EasyMatrix.SettingWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   0myNamespaceclr-namespace:EasyMatrix     5   	   X TXceed.Wpf.Toolkit, Version=2.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	     � Title$  Настройки��5   	   $	��300��5   	   $	��300��5	   	    � 
ResizeMode$ NoResize=�5
   	    � DataContext 5        EasyMatrix.SettingsModel  5   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $
��0.2* �6   �� 5      -   5         �Name$
 col1��5      $
��Auto �6+   �� 5      $
��Auto �6   �� 5      $
��0.2* �6    � RowDefinitions 5   
   �� 5      �� 5      �� 5      �� 5      �� 5      �� 5      .+�5   
   �� $��1��6   $��1��5      $
��Left=�5      $��Center=�5       $��15(�5!      $��Размер шрифта��5"      %  Xceed.Wpf.Toolkit.IntegerUpDown  5#   
   -   5        $��
SizeUpDown��5#      $��1��5$      $��2��5%      $��50��5&      $��30��5'      $��Segoe UI)�5(      $��15(�5)      $��Normal$�5*      �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 0X�5+      ,  &Xceed.Wpf.Toolkit.Primitives.InputBase   Text 5,      �� *
FontSize+�� 5-   
   $��2��6   $��1��5.      $��25��5/      $
��Left=�50      $��Center=�51      $��15(�52      $��
Шрифт��53      �� 54   
   $��2��6   $��1��55      $��3��56      $	��166��57      $
��Auto��58      $��50,0,0,0q�59      $��Center=�5:      $��Center=�5;      $
��Left=�5<      $��Center=�5=      ��5>      ��    System.Windows.Media.Fonts   SystemFontFamilies	 �� Source#	 Z 
 �� SelectedIndex
 5?      �� *Font+�� 5@   
   $��3��6   $��1��5A      $��25��5B      $
��Left=�5C      $��Center=�5D      $��15(�5E      $��Округлять до:��5F        5G   
   -   5        $��Round��5G      $��3��5H      $��2��5I      $��50��5J      $��30��5K      $��Segoe UI)�5L      $��15(�5M      $��Normal$�5N      $ 0X�5O       5P      �� *Round+�� 5Q   
   $��4��6   $��1��5R      $��25��5S      $
��Left=�5T      $��Center=�5U      $��15(�5V      $-��'Включить комментарии��5W      �� 5X   
   $��4��6   $��2��5Y      $��Center=�5Z      $��Center=�5[       d� 	IsChecked 5\      �� *
Comments+5]      5^      
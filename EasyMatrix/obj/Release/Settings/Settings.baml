   M S B A M L   `   `   `  ���� E  AEasyMatrix, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:EasyMatrix
EasyMatrix      EasyMatrix.SettingWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     0myNamespaceclr-namespace:EasyMatrix     X TXceed.Wpf.Toolkit, Version=2.9.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit    � Title$  Настройки��$	��300��$	��300�� � 
ResizeMode$ NoResize=� � DataContext   EasyMatrix.SettingsModel  .���  � ColumnDefinitions �� $
��0.2* ��� -    �Name$
 col1��$
��Auto ��� $
��Auto ��� $
��0.2* � � RowDefinitions �� �� �� �� �� �� .+��� $��1��$��1��$
��Left=�$��Center=�$��15(�$��Размер шрифта��%  Xceed.Wpf.Toolkit.IntegerUpDown  -   $��
SizeUpDown��$��1��$��2��$��50��$��30��$��Segoe UI)�$��15(�$��Normal$��  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 0X�,  &Xceed.Wpf.Toolkit.Primitives.InputBase   Text �� *
FontSize+�� $��2��$��1��$��25��$
��Left=�$��Center=�$��15(�$��
Шрифт���� $��2��$��1��$��3��$	��166��$
��Auto��$��50,0,0,0q�$��Center=�$��Center=�$
��Left=�$��Center=�����    System.Windows.Media.Fonts   SystemFontFamilies	 �� Source#	 Z 
 �� SelectedIndex
 �� *Font+�� $��3��$��1��$��25��$
��Left=�$��Center=�$��15(�$��Округлять до:��  -   $��Round��$��3��$��2��$��50��$��30��$��Segoe UI)�$��15(�$��Normal$�$ 0X� �� *Round+�� $��4��$��1��$��25��$
��Left=�$��Center=�$��15(�$-��'Включить комментарии���� $��4��$��2��$��Center=�$��Center=� d� 	IsChecked �� *
Comments+
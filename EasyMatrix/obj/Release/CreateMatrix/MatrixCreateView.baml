   M S B A M L   `   `   `  ���� E  AMyControls, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;,clr-namespace:MyControls;assembly=MyControls
MyControls  E AEasyMatrix, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:EasyMatrix
EasyMatrix !   EasyMatrix.MatrixCreateView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     C
MyControls,clr-namespace:MyControls;assembly=MyControls     @d2http://schemas.microsoft.com/expression/blend/2008     ,mySpaceclr-namespace:EasyMatrix     -   $��CreateMatrix��
  J� Uid$  CreateWindow�� � Title$ Новая матрица��$	��330��$	��300��$	��300��#��� t�#��� s�$��Stretch=�$��Stretch=�
���   � � 
ResizeMode$ NoResize=�$��Disabled=� � ShowInTaskbar .   � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=� � 	Resources 2  ,System.Windows.Controls.AlternationConverter   AlternationConverter  EasyMatrix.ButtonOK  ButtonWidth��%   &	        &	           � DataContext &  EasyMatrix.MatrixCreateViewModel  .���  � ColumnDefinitions �� -   	 �Name$
	 col0��$��30 ��� -   $
	 col1������ *Width+
 �� ElementName$
 textBox���� -   $
	 col2��$
��Auto ��� -   $
	 col3��$
��Auto ��� -   $
	 col4������ *Width+$

 col2���� -   $
	 col5��$
��Auto ��� -   $
	 col6������ $
 matrix�� �� Path$ Content.Width��� -	   $
	 col7��$
��Auto ��� -
   $
	 col8��$��30 � � RowDefinitions �� -   $
	 row0��$	��90* ��� -   $
	 row1��$
��Auto ��� -   $
	 row2��$	��90* �.+��� -    ��Name$ textBox��$��1��$��1��$
��Auto��$��20��$��Center=�$��Center=�   MyControls.MyTextBox   Mask$	 Var=� J� 	AllowDrop . }�.  $��20(�$��Italic$� �� 	MaxLength$ 7�� �� TextAlignment$ Center=� �� TextWrapping$ NoWrap=����� *Heigh+$

 row1������ *CurrentChar+�� -    ��Name$	 equ��$��1��$��2��$��Center=�$��Center=����� *Width+$

 col2������ *Heigh+$

 row1������ $��20(�$��Italic$�$��=�� �� TextAlignment$ Center=��� -   $ 	dimension��$��1��$��3��$��Center=����� *Width+$

 col3������ *Heigh+$

 row1�����  �� -   $	 b1���� -   $	 d1������ *Width+$
 d1���� -   $
	 mult���� -   $	 d2������ *Width+$
 d2���� -   $	 b2��.+��� -    ��Name$	 br1��$��20(�$��Normal$�$��(��$ Center=����� *Height+$
 matrix���� -   $
 dim1��$��1��$
��Auto��$ PositiveInteger=�   MaximumValue$ 10Q�   MinimumValue$ 0Q�$��20(�$��Normal$�$ 3������ *Rows+ �� UpdateSourceTrigger$ PropertyChanged=��� -   $ x��$��2��$��20(�$��Normal$�$��x���� -   $
 dim2��$��3��$
��Auto��$ PositiveInteger=�$ 10Q�$ 1Q�$��20(�$��Normal$�$ 2������ *Col+$ PropertyChanged=��� -   $	 br2��$��4��$��20(�$��Normal$�$��)���� -   $
 equ2��$��1��$��4��$��Center=�$��Center=�$��20(�$��Normal$�$��=���� -   $	 br3��$��1��$��5��$��Center=�$��20(�$��Normal$�$��(��$ Center=��� -   $ matrix��$��1��$��6��$
��Auto��$
��Auto��$��Center=�$��Center=�$��Center=�$��Center=����� *Matrix+�� -   $	 br4��$��1��$��7��$��Center=�$��20(�$��Normal$�$��)��$ Center=��� -    ��Name$ spanel��$��2��$��1��$��7��$��Bottom=� �� Orientation$ 
Horizontal=�.+��� $��30��$��Отмена������ *ActualWidth+ �� ConverterParameter$ Width�� �� 	Converter# [ $
 spanel������ *Cancel+���� $
 CreateMatrix���� $��30��$��OK������ *ActualWidth+$ Width��# [ $
 spanel������ *OK+���� $
 CreateMatrix��
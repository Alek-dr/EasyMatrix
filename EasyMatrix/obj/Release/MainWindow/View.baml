   M S B A M L   `   `   `  ���� O  Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem  E AEasyMatrix, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:EasyMatrix
EasyMatrix 'clr-namespace:EasyMatrix
EasyMatrix 'clr-namespace:EasyMatrix
EasyMatrix    EasyMatrix.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     9System&clr-namespace:System;assembly=mscorlib     @d2http://schemas.microsoft.com/expression/blend/2008     0databindingclr-namespace:EasyMatrix     *localclr-namespace:EasyMatrix     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0myNamespaceclr-namespace:EasyMatrix     -     �Name$  window�� � Title$ 
EasyMatrix��$
��Auto��$
��Auto��$	��500��$	��500�����	#FF383838���	#FFD8D7D0 � Icon$" ../Resources/Images/Icon.ico�� � DataContext  EasyMatrix.ViewModel   � 	Resources "  EasyMatrix.MaxWidthConverter   MaxWidthConverter"  EasyMatrix.MinWidthConverter  MinWidthConverter  EasyMatrix.BottomRowHeigh  BottomRowHeigh  EasyMatrix.OneThird  OneThird  EasyMatrix.Plus25 
 Plus25  EasyMatrix.BtnWidth  BtnWidth  EasyMatrix.MatrixWrite  MatrixWrite	  EasyMatrix.One8  One8
  EasyMatrix.HelpWidth  	HelpWidth��%�   &	        &	      &	 
     &	      &	      &	      &	      &	 #     &	 (                   	  
  .���  � RowDefinitions �� -    �Name$
 Row1��$
��250* ��� -   $
 Row2��$��25 ��� -   $
 Row0��$	��130 � � ColumnDefinitions �� -   $
 col0��$
��0.2* ��� -   $
 col1��$
��0.8* �.+�� � -    �Name$ HelpGrid��$��0��$��0��$��2��$
��Auto������ 	 �� ElementName$
	 Row0�� �� -   $ helpCol��$	��100 ����� *ActualWidth+
 �� ConverterParameter$
 Width�� �� 	Converter# [ $	 window������ *ActualWidth+$
 Width��# [  $	 window���� -	    ��Name$ 	MainField��$��* �.+��� -
   $��GSplit��$��0��$��0��$��3��$��0,0,-1,0q� K� ZIndex$ 2��
���   ��� -    ��Name$ 
Helpborder��$��0��$��0��$��0,0,0,0q����	#FFB4C1D1���	#FFF3EFEF$��0,0,8,0q����� $
	 col0������ $	 GSplit��R� -   $
��Tree��$��0��$��0��.@�Q� $
��Left=�$��14(�$��Normal$�$��Виды матриц��.@� EasyMatrix.RichTextFile  -   $
��Auto��$ �� HorizontalScrollBarVisibility$ Disabled=� �� IsDocumentEnabled . {�.  �� 
IsReadOnly . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder��   File �� *file1+Q� $
��Left=�$��14(�$��Normal$�$0��*Операции над матрицами��.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file2+Q� $
��Left=�$��14(�$��Normal$�$,��&Возведение в степень��.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file3+Q� $
��Left=�$��14(�$��Normal$�$"��Преобразования��.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file4+Q� $
��Left=�$��14(�$��Normal$����� $��14(�$C��=Минор. Алгебраическое дополнение��$
��Wrap=����� $	 
Helpborder�� �� Path$ ActualWidth�.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file5+Q� $
��Left=�$��14(�$��Normal$�$��Определитель��.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file6+Q� $
��Left=�$��14(�$��Normal$�$��Ранг��.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file7+Q� $
��Left=�$��14(�$��Normal$�$%��Обратная матрица��.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file8+Q� $
��Left=�$��14(�$��Normal$�$��Команды��.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file9+Q� $
��Left=�$��14(�$��Normal$�$��О программе��.@�  -   $
��Auto��$
��Left=�$ Disabled=� . {�.  . ���� *ActualWidth+$
 Width��# [ $	 
Helpborder�� �� *file10+�� -    ��Name$ MainTextBox��$��0��$��1��$��Normal$���.  �� TextWrapping$
 Wrap=�" �� VerticalScrollBarVisibility$
 Auto=����� $	 	MainField������ $	 	MainField������ *CurrentFont+���� *
FontSize+.�� J� InputBindings �� 
 �� Key$ Return�� �� Command �� *Enter+ �� CommandParameter �� $	 MainTextBox��.���� -   $��Global��$��0��$��3��$��0��$��2��$��0,0,0,0q�$ 5��
���   �$��2,2,2,2q��� -    ��Name$ 	newMatrix��$��1��$��2,2,2,2q�$��Right=�$��Новая матрица�� 	 CenterAlignmentBtn#��[	 ���� *ActualWidth+$
 Width��# [ $	 window������ $
	 Row2������ *CreateNewMatrix+{��� *ButtonEnable+�� $��1��$��2��$��0��
���   �$��2q��� -   $��	settPanel��$��1��$��0��$��4��
���   �$��0,2,2,2q����� $
	 Row1������ $	 	newMatrix���� -   $��collectionBord��$��1��$��2��$��0��$��0q�$
��Left=�$ 1��
���   �$��2,0,2,2q����� *ActualWidth+$
 Width��# [ $	 window������ *Height+# [ $	 matrixCollection��� -   $��	PanelGrid��$��1��$��2��$��1������ $
	 col1�� �� -   $ c0��$��0.33333* ��� -   $ c1��$��0.33333* ��� -   $ c2��$��0.33333* � �� -    $ PanelRow������ $
	 Row2���� -!   $ Names������ $
	 Row0��$ Height�.+��� -"    ��Name$ Actions��$��0��$��0��$��15(�$��Normal$�$,��&Действия с матрицами�� �� TextAlignment$ Center=��� -#   $��	curMatrix��$��0��$��1��$��15(�$��Normal$�$#��Текущая матрица��$ Center=��� $��0��$��2��$��15(�$��Normal$�$��Другое��$ Center=�]� -$   $
��Dock��$��1��$��0����. .+��� -%   $��
Operations��$��0������ $
	 Row0��$ Height�.+��� -&   $��Multiply�� 
 LikeToogleButton#��[
 ���� *ActualHeight+$	 
Operations��$
 Height��# [ ���� *Mult+���� $	 MainTextBox��{��� *ButtonEnable+���� $��20(�$��Normal$�$��X��$ Center=����� *ActualWidth+# [ $
 Width��$	 Actions���� -'   $
��Plus��#��[
 ���� *ActualHeight+$	 
Operations��$
 Height��# [ ���� *Add+���� $	 MainTextBox��{��� *ButtonEnable+���� $��20(�$��Normal$�$��+��$ Center=����� *ActualWidth+# [ $
 Width��$	 Actions���� -(   $	��Sub��#��[
 ���� *ActualHeight+$	 
Operations��$
 Height��# [ ���� *Sub+���� $	 MainTextBox��{��� *ButtonEnable+���� $��20(�$��Normal$�$��-��$ Center=����� *ActualWidth+# [ $
 Width��$	 Actions���� .���� .+��� $��30��  LeftAlignmentBtn#��[ ���� *Determinant+���� $	 MainTextBox��{��� *ButtonEnable+���� $��3q�$��14(�$��Normal$�$��Определитель��$
 Left=��� $��30��#��[ ���� *	Transpose+���� $	 MainTextBox��{��� *ButtonEnable+���� $��3q�$��14(�$��Normal$�$$��Транспонировать��$
 Left=��� $��30��#��[ ���� *REF+���� $	 MainTextBox��{��� *ButtonEnable+���� $��3q�$��14(�$��Normal$�$��Метод Гаусса��$
 Left=��� $��30��#��[ ���� *Gauss_Jordan+���� $	 MainTextBox��{��� *ButtonEnable+���� $��3q�$��14(�$��Normal$�$,��&Метод Гаусса-Жордана��$
 Left=��� $
��Auto��#��[ ���� *	Inverse+���� $	 MainTextBox��{��� *ButtonEnable+���� $��3q�$��14(�$��Normal$�$A��;Обратная матрица методом Гаусса��$
 Left=�$��WrapWithOverflow=��� $
��Auto��#��[ ���� *
InverseCof+���� $	 MainTextBox��{��� *ButtonEnable+���� $��3q�$��14(�$��Normal$�$f��`Обратная матрица методом алгебраических дополнений��$
 Left=�$��WrapWithOverflow=��� $��30��#��[ ���� *Rang+���� $	 MainTextBox��{��� *ButtonEnable+���� $��3q�$��14(�$��Normal$�$��Ранг��$
 Left=��� $��30��#��[ ���� *Power+���� $	 MainTextBox��{��� *ButtonEnable+���� $��3q�$��14(�$��Normal$�$(��"Возвести в степень��$
 Left=��� $��1��$��1��$��Center=�
�������
���   �$
��Auto=�$
��Auto=����� $	 CurMatr��$ Width�.���� -)   $��CurMatr��$
��Auto��$
��Auto��$��Center=�$��Center=�
���   ����� $	 matrixCollection��$ SelectedItem�# [ ]� $��1��$��2��.+�d� $��40��$
��Left=�  ToogleButton#��[  d� 	IsChecked �� *STEP+�� $5��/ Очистить рабочую область��$	��Top=�$��13(�#��[ ���� *ActualHeight+$
	 Dock��$
 ActualHeight��# [ ���� *ClearWorkspace+���� $	 MainTextBox��{��� *ButtonEnable+�� $7��1 Очистить коллекцию матриц��$	��Top=�$��13(�#��[ ���� *ActualHeight+$
	 Dock��$
 ActualHeight��# [ ���� *ClearMatrixCollection+{��� *ButtonEnable+�� $�� Настройки��$��13(�#��[ ���� *ActualHeight+$
	 Dock��$
 ActualHeight��# [ ���� *SettingWind+���� $	 MainTextBox��{��� *ButtonEnable+�� -*    ��Name$ matrixCollection��$��2��$��0,0,1,2q�$
��Left=�$	��Top=�$��Center=�$	��Top=����	#FFA8A8A7���	#FFA8A8A7$��0q�
���   �$��0q���.  $��Disabled=�$
��Auto=����� $	 collectionBord��$ ActualWidth����� $
	 Row2��$ Height����� *MatrixCollection+  �� SelectedItem  �� *CurrentMatrix+! � ContextMenu! �� .@�m� $��Удалить��" m� Command" �� *Delete+���� # �� 
TargetType"# ��.��� $ �� Property$ � ��% �� Value$% Center=�& �� View& �� .P��� ' �� Width' �� *ActualWidth+$	 matrixCollection��$
 ActualWidth��# [ ( �� DisplayMemberBinding( �� *Name+.���� $��Имя��) J� IsHitTestVisible) .  �� ' �� *ActualWidth+$	 matrixCollection��$
 ActualWidth��# [ ( �� *Rows+.���� $��
Строк��) .  �� ' �� *ActualWidth+$	 matrixCollection��$
 ActualWidth��# [ ( �� *	Columns+.���� $��Столбцов��) .  
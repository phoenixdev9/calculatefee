   M S B A M L   `   `   `  ����   SearchAddressMap    SearchAddressMap.Window1   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Aav2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     � Title$  HQ Travel Fee Calculator��$	��631��$
��1013�� � 
ResizeMode$ CanMinimize=�.����  �� Orientation$ Vertical=� J� RenderTransformOrigin$ 0.5,0.5&�$��0,0,0,0q�z�V� .~��� ��  �� AngleY$ -0.07Q��� T�  T� Y$ 0.499Q�.+��� $ 
Horizontal=��� $ 
Horizontal=�.+��� $��Destination��$��85��$��29��$ 0.5,0.5&�z�V� .~��� �� �� T�  T� X$ 22Q��� -    ��Name$ destinationTextBox��$��23��$	��364��$	��Top=�$��10,10,10,10q�$ 0.5,0.5&�.��z�V� .~��� �� �� T� $ 0Q�$ 17Q�.���� -    ��Name$ searchButton��$��23��$	��Top=�$	��279��$��	Calculate��$��10,10,10,10q�$ 0.5,0.5&�
�������z�V� .~��� �� �� T� $	 116Q��� $ 
Horizontal=�$	��972��$	��545��.+��� -   	 ��Name$	 tabControl1��$	��520��$��Bottom=�$��5,5,5,5q�$	��493��.@��� $	��Map��$��22��$	��Top=�.���� $ Vertical=�$	��503��.+��� $ 
Horizontal=�.+��� -   
 ��Name$
 mapTypeComboBox��$��23��$	��Top=� �� SelectedIndex$ 0��$��Right=�$	��146��$��
10,10,0,10q�.@��� $��roadmap���� $��	satellite���� $��hybrid���� $��terrain���� -    ��Name$ 	zoomLabel��$��23��$	��Top=�$��Right=�$
��Zoom��$��10,10,10,10q��� -    ��Name$ 
zoomSlider��$��23��$	��Top=� � Value$ 4Q� � Maximum$ 20Q�$��Right=�$	��143�� � SmallChange$ 1Q� � LargeChange$ 2Q� �� TickPlacement$ TopLeft=� �� Interval$ 33�� �� IsSnapToTickEnabled .  �� TickFrequency$ 4Q�$��	0,10,10,0q��� -    ��Name$ border1��$��1q�
���   �$	��447��$	��466��._��� -	    ��Name$ image1��$
��Fill=�$��0,0,0,0q�$	��447��$	��466��$	��Top=��� -
   $ loadingImage��$��/Spinner-2.gif��$y�	Collapsed=�$��Center=�$��Center=��� -    ��Name$ 
HQListView��$	��469��$	��536��$
��Auto=���. $
��Auto=� �� View �� .P��� $��HQ name�� �� Width$	 250�� �� DisplayMemberBinding �� *HQName+�� $��Distance(mile)��$	 100�� �� *
Distance+�� $��Travelling Fee($)��$	 100�� �� *Fee+.@��� $	��100��$	��100��
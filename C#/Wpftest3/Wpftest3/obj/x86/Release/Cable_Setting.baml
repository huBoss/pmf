   M S B A M L   `   `   `  ���� C  ?Wpftest3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Wpftest3Wpftest3      Wpftest3.Cable_Setting   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     (localclr-namespace:Wpftest3       � Title$  Cable_Setting��$��425.449��$��368.433��.��� $��0,0,2,2q� � RowDefinitions �� $	��192 ��� .+�	� -    	�Name$ isCalcul��$��计算曲线��$��9,38,262,128q� d� 	IsChecked �� *cf.ls.isCalcul+	� -   $ noCalcul��$��不计算曲线��$��108,37,139,129q�$
 TrueY�]� -    ]�Name$ Panel_nohand��$
��Left=�$��60����.  $��9,66,0,0q�$	��Top=�$	��325��{�.  .+��� -    ��Name$ isUse_p��$+��%每几点更新一次(>=2 且 <=1000)��$��20�� J� RenderTransformOrigin$ 0.5,0.5&�$	��Top=�$��	2,5,125,0q��� -   $ isUse_d��$$��每隔多少距离更新一次��$��18��$	��Top=�$��21,7,27,-25q�$ 0.493,0.611&�$	��152��$y�Hidden=��� -    ��Name$
 pnum�� �� TextWrapping$
 Wrap=�$��5��$	��103��$��
0,-25,0,35q�$��25��$��Bottom=��� -   $
 dnum��$��25��$
 Wrap=�$��4��$	��Top=�$	��Top=�$��
-92,0,33,0q�$y�Hidden=��� $��m��$
��Left=�$��
302,51,0,0q�$	��Top=�$��24��$��33��$ 
0.515,1.12&�$y�Hidden=��� $	��个��$
��Left=�$	��Top=�$��
302,26,0,0q�$��25��$��33��$ 
0.758,0.68&��� -   $��
Panel_line��$
��Left=�$	��172��$	��Top=�$	��310��$��
10,125,0,0q�$��2��.+��� $!��请选择拟合曲线类型���� -	   	 ��Name$	 Linetype��
 �� SelectedIndex$
 0��.@��� $��straight���� $��?���� $��?���� $��请选择划线粗细���� -
   $	 Linesize��$
 1��.@��� $	��0.1���� $	��0.5���� $��1���� $	��1.5���� $��2���� $��请选择划线颜色��� $��25��.+��� $��         颜色��$	��161���� -    ��Name$ 
Line_color��$	��147���� -   $ Remain_point��$��画完线后留点��$��22��$��0,5,0,0q�$
 TrueY��� -   $��确定��$��Right=�$��75��$��
0,0,260,11q�$��22��$��Bottom=�$��1���� -   $��取消��$��Right=�$��Bottom=�$��75��$��
0,0,142,11q�$��1���� -   $��应用��$��Right=�$��Bottom=�$��75��$��	0,0,25,11q�$��1��� $��30��$	��Top=�$ 	0.49,0.48&�$��	16,4,38,0q�.+��� $��点的大小设置��$��30��$��88���� -   $ 
Point_size��$��22��$
 Wrap=�$	��0.5��$��53���� $��颜色���� -   $ Point_color��$��23��$	��120���� $
��Left=�$	��100��$��665,150,-407,0q�$��2��$	��Top=�$	��100��
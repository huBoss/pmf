   M S B A M L   `   `   `  ����C  ?Wpftest3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Wpftest3Wpftest3  5   	       Wpftest3.Cable_Setting   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   (localclr-namespace:Wpftest3     5   	     � Title$  Cable_Setting��5   	   $��425.449��6   $��368.433��60   .��5      � $��0,0,2,2q�6    � RowDefinitions 5   
   �� 5      $	��192 �6   �� 5	      .+�5   
   	� -   5         	�Name$ isCalcul��5      $��计算曲线��6)   $��9,38,262,128q�6V    d� 	IsChecked 6l   �� *cf.ls.isCalcul+	� 5   
   -   5        $ noCalcul��5      $��不计算曲线��6)   $��108,37,139,129q�69   $
 TrueY�6q   ]� 5   
   -   5         ]�Name$ Panel_nohand��5      $
��Left=�6+   $��60��6F   ��.  $��9,66,0,0q�6h   $	��Top=�6z   $	��325��6�   {�.  .+�5      �� -   5         ��Name$ isUse_p��5      $+��%每几点更新一次(>=2 且 <=1000)��6)   $��20��6O    J� RenderTransformOrigin$ 0.5,0.5&�6[   $	��Top=�6{   $��	2,5,125,0q�6�   6�   �� 5      -   5        $ isUse_d��5      $$��每隔多少距离更新一次��6)   $��18��6>   $	��Top=�6J   $��21,7,27,-25q�6b   $ 0.493,0.611&�6w   $	��152��6�   $y�Hidden=�6�   �� 5      -   5         ��Name$
 pnum��5       �� TextWrapping$
 Wrap=�6$   $��5��68   $	��103��6A   $��
0,-25,0,35q�6M   $��25��6a   $��Bottom=�6m   �� 5      -   5        $
 dnum��5      $��25��6$   $
 Wrap=�60   $��4��6D   $	��Top=�6M   $	��Top=�6e   $��
-92,0,33,0q�6z   $y�Hidden=�6�   5      �� 5   
   $��m��6   $
��Left=�6   $��
302,51,0,0q�67   $	��Top=�6K   $��24��6c   $��33��6o   $ 
0.515,1.12&�6z   $y�Hidden=�6�   �� 5   
   $	��个��6   $
��Left=�6   $	��Top=�67   $��
302,26,0,0q�6O   $��25��6c   $��33��6o   $ 
0.758,0.68&�6z   �� 5   
   -   5        $��
Panel_line��5      $
��Left=�6'   $	��172��6B   $	��Top=�6O   $	��310��6g   $��
10,125,0,0q�6s   $��2��6�   .+�5      �� $!��请选择拟合曲线类型��6   �� 5      -	   5        	 ��Name$	 Linetype��5      
 �� SelectedIndex$
 0��6)   .@�5      �� $��straight��6   �� 5      $��?��6   �� 5      $��?��6   5      �� 5      $��请选择划线粗细��6   �� 5      -
   5        $	 Linesize��5      $
 1��6)   .@�5      �� $	��0.1��6   �� 5      $	��0.5��6   �� 5       $��1��6   �� 5!      $	��1.5��6   �� 5"      $��2��6   5#      �� 5$      $��请选择划线颜色��6   � 5%      $��25��6   .+�5&      �� $��         颜色��6   $	��161��6.   �� 5'      -   5         ��Name$ 
Line_color��5'      $	��147��6-   69   5)      �� 5*      -   5        $ Remain_point��5*      $��画完线后留点��6-   $��22��6>   $��0,5,0,0q�6J   $
 TrueY�6z   5,      �� 5-   
   -   5        $��确定��5-      $��Right=�6   $��75��6:   $��
0,0,260,11q�6E   $��22��6p   $��Bottom=�6|   $��1��6�   �� 5.   
   -   5        $��取消��5.      $��Right=�6   $��Bottom=�6:   $��75��6U   $��
0,0,142,11q�6`   $��1��6�   �� 5/   
   -   5        $��应用��5/      $��Right=�6   $��Bottom=�6:   $��75��6U   $��	0,0,25,11q�6`   $��1��6�   � 50   
   $��30��6   $	��Top=�6    $ 	0.49,0.48&�68   $��	16,4,38,0q�6Z   .+�51      �� $��点的大小设置��6   $��30��6%   $��88��61   �� 52      -   5        $ 
Point_size��52      $��22��6*   $
 Wrap=�66   $	��0.5��6J   $��53��6U   �� 53      $��颜色��6   �� 54      -   5        $ Point_color��54      $��23��6*   $	��120��66   6B   55      �� 56   
   $
��Left=�6   $	��100��6+   $��665,150,-407,0q�68   $��2��6P   $	��Top=�6a   $	��100��6y   58      59      
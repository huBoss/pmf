   M S B A M L   `   `   `  ����C  ?Wpftest3, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:Wpftest3Wpftest3  5   �       Wpftest3.Database_Setting   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F   (localclr-namespace:Wpftest3     6�   -   5          � Title$  Database_Setting��5   	   $��205.995��6"   $	��364��63    � 	Resources 5       #  realtime_track_WindowViewSource  dataBase_fittingViewSource��%:   &	        &	      �� 5   
   6   �� 5   
   6   5	      .��5
      � $��0,0,2,10q�6    � DataContext# [ 6   .+�5   
   	� -   5         	�Name$ Ishand��5      $��存入��6'   $
��Left=�64   $	��Top=�6O   $��	85,59,0,0q�6�   	� 5   
   -   5        $��	notIshand��5      $��	不存入��6'   $
��Left=�65   $	��Top=�6P   $��
205,62,0,0q�6h    d� 	IsChecked$
 TrueY�6�   {�. 6�   ]� 5   
   -   5         ]�Name$ Panel_nohand��5      $
��Left=�6+   $��60��6F   ��.  $��	10,26,0,0q�6h   $	��Top=�6{   $	��325��6�   {�.  $y�Hidden=�6�   .+�5      �� -   5         ��Name$ isUse_p��5      $��每多少点保存一次��6)   $��20��6<    J� RenderTransformOrigin$ 0.357,0.542&�6H   $	��Top=�6l   $��
20,5,152,0q�6�   $y�Hidden=�6�   �� 5      -   5        $ isUse_d��5      $$��每隔多少距离保存一次��6)   $��18��6>   $	��Top=�6J   $��21,7,27,-25q�6b   $ 0.493,0.611&�6w   $	��152��6�   $y�Hidden=�6�   �� 5      -   5         ��Name$
 pnum��5      	 �� TextWrapping$
	 Wrap=�6$   $
��1000��68   $��92��6D   $��
0,-25,0,35q�6O   $y�Hidden=�6c   �� 5      -   5        $
 dnum��5      $��25��6$   $
	 Wrap=�60   $	��200��6D   $	��Top=�6O   $	��Top=�6g   $��
-92,0,33,0q�6|   $y�Hidden=�6�   5      �� 5   
   $��m��6   $
��Left=�6   $��
302,51,0,0q�67   $	��Top=�6K   $��24��6c   $��33��6o   $ 
0.515,1.12&�6z   $y�Hidden=�6�   �� 5   
   $	��个��6   $
��Left=�6   $	��Top=�67   $��
302,26,0,0q�6O   $��25��6c   $��33��6o   $ 
0.758,0.68&�6z   $y�Hidden=�6�   �� 5   
   $
��Left=�6   $��77��60   $	��Top=�6<   $	��310��6T   $��
10,125,0,0q�6`   $y�Hidden=�6t   .+�5      �� $!��请选择保存格式类型��6   �� 5      -	   5        
 ��Name$
 Linetype��5      .@�5      �� $��1��6   �� 5      $��2��6   �� 5      $��3��6   5      �� 5      -
   5        $ Remain_point��5      $0��*保存后是否在图上清除历史痕迹��6-   $��22��6F   $��0,5,0,0q�6R   6c   5      �� 5   
   -   5        $��确定��5      $
��Left=�6   $	��Top=�69   $��75��6Q   $��
30,125,0,0q�6\   6p   �� 5   
   -   5        $��取消��5      $
��Left=�6   $	��Top=�69   $��75��6Q   $��125,125,0,0q�6\   6q   �� 5    
   -   5        $��应用��5       $
��Left=�6   $	��Top=�69   $��75��6Q   $��225,125,0,0q�6\   6q   � 5!   
   $
��Left=�6   $	��116��6/   $	��Top=�6<   $	��310��6T   $��
10,207,0,0q�6`   $y�Hidden=�6t   .+�5"      �� $!��请选择要保存的数据��6   $	��309��6(   �� 5#      $��	Gps数据��6   $
 TrueY�6'   �� 5$      $��电缆位置数据��6   $
 TrueY�6(   �� 5%      $��CheckBox��6   �� 5&      $��CheckBox��6   �� 5'      $��CheckBox��6   �� 5(      $��CheckBox��6   �� 5)      $��CheckBox��6   �� 5*      $��CheckBox��6   5+      5-      5.      
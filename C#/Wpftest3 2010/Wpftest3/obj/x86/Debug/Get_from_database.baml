   M S B A M L   `   `   `  ����X  TXceed.Wpf.Toolkit, Version=2.5.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f4P:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.ToolkitXceed.Wpf.Toolkit  5   	    Wpftest3    Wpftest3.Get_from_database   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   Ntoolkit:clr-namespace:Xceed.Wpf.Toolkit;assembly=Xceed.Wpf.Toolkit     5   	   -   5          � Title$  Get_from_database��5   	   $	��360��6#   $��662.687��60   .��5      � .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dg��5      $
��Left=�6   $��	45,39,0,0q�6:   $	��Top=�6M   $	��168��6e   $	��547��6r   �� 5   
   -   5        $��导入��5      $
��Left=�6   $	��Top=�69   $��75��6Q   $��
70,228,0,0q�6\   6p   �� 5	   
   -   5        $��删除��5	      $
��Left=�6   $	��Top=�69   $��75��6Q   $��178,227,0,0q�6\   6q   �� 5
   
   -   5        $��	修改表��5
      $
��Left=�6   $��293,226,0,0q�6:   $	��Top=�6O   $��75��6g   6r   �� 5   
   -   5        $��
namesearch��5      $
��Left=�6$   $��21��6?   $��202,271,0,0q�6K    �� TextWrapping$
 Wrap=�6`   $	��try��6t   $	��Top=�6   $	��211��6�   $y�Hidden=�6�   &    Xceed.Wpf.Toolkit.DateTimeUpDown  5   
   -   5        $��
timesearch��5   !   $
��Left=�63   $��21��6N   $��202,272,0,0q�6Z   $	��Top=�6o   $	��210��6�      Format$ UniversalSortableDateTime=�6�   �� 5   
   -   5         ��Name$ search��5      $
��Left=�6#   $��25��6>   $��
69,271,0,0q�6J   $	��Top=�6^   $	��102��6v    J� RenderTransformOrigin$ 	0.074,0.6&�6�    �� SelectedIndex$ 0��6�   .@�5      �� $��按照名字搜索��6   �� 5      $��按时间搜索��6   5      �� 5   
   -	   5        $��搜索��5      $
��Left=�6   $��21��69   $��450,272,0,0q�6E   $	��Top=�6Z   $��50��6r   6}   �� 5   
   -
   5        $��searchlabel��5      $�� ��6#   $
��Left=�6.   $��24��6I   $��515,272,0,0q�6U   $	��Top=�6j   $��65��6�   5      5      
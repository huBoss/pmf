   M S B A M L   `   `   `  ���� ^  ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35tOclr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration System.Windows.Forms.Integration   Wpftest3$   Wpftest3.Realtime_track_Window   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     ]iOclr-namespace:System.Windows.Forms.Integration;assembly=WindowsFormsIntegration     -     � Title$  实时跟踪界面��$��709.582��$��861.523�� � Left$	 496��
 � Top$ 0�� � WindowStartupLocation$ Manual=� � WindowState$ 	Minimized=� � Topmost .   � WindowStyle$ SingleBorderWindow=� � 
ResizeMode$ NoResize=� � 	Resources 	 �� MergedDictionaries
 �� Source�� 	 �� $#
 Resource/ExpressionLight.xaml?�%    .���  � ColumnDefinitions �� $��0* ��� $
��173* ��� $
��422* ��� $	��65* ��� $
��194* �.+�7   1System.Windows.Forms.Integration.WindowsFormsHost  -     Name$ 	SceneHost��$��5,25,0,0q�$��3��$��2�� � ContextMenu �� -    ��Name$ 	scenemenu��.@�m� -   $��我的位置��m� -   $��历史轨迹��m� -   $��进入地下管网��m� -   $��退出地下管网��m� -   $��返回到全局图��  -	   $ tocHost��$��
0,55,417,0q�$��3��  -
   $ toolbarHost��$��30��$	��Top=�$��3��$��
0,25,417,0q��� -    ��Name$ button1��$��46��$��Right=�$��	0,207,0,0q�$	��Top=�$��70��$��4��$��请将我
放入其中��$y�Hidden=��� -   $ button2��$��30��$��Right=�$	��Top=�$��70��$��4��$��	0,258,0,0q�$��请将我移走��$y�Hidden=��� -   $ button3��$��30��$��Right=�$	��Top=�$��70��$��	0,172,0,0q�$��4��$��接收数据��$y�Hidden=��� -   $ button5��$��不画电缆��$��30��$��Right=�$��0,70,0,0q�$	��Top=�$��70��$��4��$y�Hidden=��� -   $ button6��$��拟合电缆��$��30��$��Right=�$��0,35,0,0q�$	��Top=�$��70��$��4��$y�Hidden=�o� $��5��$
��Left=�$��25��$	��Top=�$	��858�����	#FFFBFBFB$��0,0,-4,0q�.@�m� $��文件��$��0q�$��69��.@�m� $��新建��$
��Left=�$	��200��.@�m� -   $��测量��$
��Left=�$	��100��m� -   $��从数据库中导入...��$
��Left=�$	��200��m� $��设置��$��25��$��0q�$	��Top=� J� RenderTransformOrigin$ 0.926,0.691&�.@�m� -   $��电缆��$
��Left=�$	��200��m� -   $��	数据库��$
��Left=�$	��200���� -   $��BTStart��$��22��$	��Top=�$��4��$��	8,2,133,0q�$��开始����Z� �� -   $��BTPause��$��22��$��Right=�$	��Top=�$��53��$��4��$��0,2,75,0q�$��暂停��{�.  $ 0.453,0.909&���Z� �� -   $��BTEnd��$��22��$	��Top=�$��4��$��
124,2,17,0q�$��结束��{�.  ��Z� �� -   $��	摄像头��$��3��$
��Left=�$��0,2,0,0q�$	��Top=�$��53�����	#FFF9F6F6$��22���� -   $��LBname��$��Label��$��2��$
��Left=�$��	256,4,0,0q�$	��Top=�$ 	0.5,0.125&�$y�Hidden=��� -   $��Videobtn��$��	VideoLink��$��2��$
��Left=�$��0,3,0,0q�$	��Top=�$��86�����	#FFFDF6F6
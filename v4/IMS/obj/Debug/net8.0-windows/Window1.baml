   M S B A M L   `   `   `  ����  Inventory managment    Inventory_managment.Audit   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  
Audit Logs��5   	   $
��Auto��6   $
��Auto��6*   $	��450��67   $	��800��6G   ���#F0F4F8 � 
ResizeMode$ NoResize=�5       � WindowStartupLocation$ CenterScreen=�64    � 	Resources 5      &   System.Windows.Controls.DataGrid �� 
TargetType �� Property �� Value=  7System.Windows.Controls.Primitives.DataGridColumnHeader��%  '        '  0    '�� �    �� 5   
   "  6   .�5      ��  � �� �#FFFFFF6+   �� 5	       � �� �#0080806,   �� 5
       � ��$ 1q�60   �� 5        �@ AlternatingRowBackground �#F1F1F169   �� 5       �@ 	RowHeight$ 30Q�6*   �� 5       � ��$ 12(�6)   5      �� 5   
   "  6   .�5      ��  � �� �#0080806+   �� 5       � ��
 �����6+   �� 5       � ��$
 Bold"�6+   �� 5       � ��$ 5q�6(   5      �� 5   
   " ��6   .�5      ��  � ��$ 0,5,0,5q�6'   �� 5       � �� �#3336+   5      5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5       $��* �6   .+�5$   
   �� $��
Audit Logs��6   $��28(�6&   $
��Bold"�64   ���#008080$��10q�6[   $��Center=�6g     5'   
   -   5          Name$ AuditDataGrid��5'      $��1��6*   	   AutoGenerateColumns$	 False��67   
   CanUserAddRows$
 False��6S      AlternatingRowBackground �#EAEAEA   HeadersVisibility$ Column=�5(   6      GridLinesVisibility$	 All=�6Q   ���#CCC$��1q�5)   &   $��10q�6:      RowHeaderWidth$ 0Q�6F      Columns 5*      0  *System.Windows.Controls.DataGridTextColumn  5+      ,  &System.Windows.Controls.DataGridColumn   Header$ Log ID��6%      Width5  /System.Windows.Controls.DataGridLengthConverter$	 100 6O   1  +System.Windows.Controls.DataGridBoundColumn   Binding 65   �� *LogID+6O     5,      $ User ID��6%   $	 120 6Q    66   �� *UserID+6Q     5-      $ Action��6%   $	 200 6P    65   �� *Action+6P     5.      $ Table Affected��6%   $	 200 6_    6=   �� *TableAffected+6_     5/      $ Action Time��6%   $	 180 6z    6:   �� *
ActionTime+ �� StringFormat$ dd-MM-yyyy HH:mm��6z     50      $ Description��6%   $ * 6Z    6:   �� *Description+6Z   52      53      54      
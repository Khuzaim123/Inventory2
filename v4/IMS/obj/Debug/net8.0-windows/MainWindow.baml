   M S B A M L   `   `   `  ����  Inventory managment    InventoryApp.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$!  Inventory Management System��5   	   $	��600��6-   $	��610��6:   ���#F0F4F8 � 	Resources 5         MainButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� CornerRadius �� Triggers P� Property	 P� Value3  -System.Windows.Media.Effects.DropShadowEffect
   
BlurRadius   Color   ShadowDepth��%�  &	        �� 5
   
   " ��6(   .�5      ��  � �� �#0080806+   �� 5       � ��
 �����6+   �� 5       � ��$ Segoe UI)�6+   �� 5       � ��$ 16(�6)   �� 5       � ��$ SemiBold"�6+   �� 5       � ��$ 15q�6(   �� 5       � ��$ 10q�6'   �� 5       �@�Cursor$
 Hand��6'   �� 5       � ��$ Center=�64   �� 5       � ��$ Center=�62   �� 5       � ��$ 0q�60   �� 5       � �� 5      �� 5      " ��6&   .R�5      �� #��z��6!   $ 10��5   !   #��z��5   !   #��z��5   !   ._�5      �� $��Center=�6/   $��Center=�6L   5      5       5"       5#      P� 5$       �@J�IsMouseOver$
	 True��61   . �5%      ��  � ��
 ��� �63   �� 5&       �@J�Effect 5'        5(      $
 10Q�6/   $ DarkGray��6?   $ 4Q�6P   5*      5+      5-      5.      .��51      � ��52   
   �� 53       �� ImageSource$ 
/main.jpeg��6    � RowDefinitions 59   
   �� 5:      $
��Auto �6   �� 5;      $��* �6   .+�5?   
   � 
����� �$	��100��6!   .+�5@      �� $,��&Welcome to Inventory Management Portal��6   
�������$��Segoe UI)�5B      $��26(�5C      $
��Bold"�5D      $��Center=�5E      $��Center=�5F      5G      �� 5J   
   $��1��6   ���#FFFFFF J� Opacity$
 0.85Q�63   $��100,80,100,80q�6B   $ 15��6Y   ._�5L      �� $��Center=�6   $��Center=�66   .+�5N      �� $%��Choose an option to get started��6   
�����p�$��Segoe UI)�5P      $��18(�5Q      $��SemiBold"�5R      $��0,0,0,20q�5S      $��Center=�5T      �� 5W      -   5        $��Login��5W      #��[  6)   $	��150��6R   $��50��6^   6j   �� 5X      -   5        $��Signup��5X      #��[  6*   $	��150��6S   $��50��6_   6k   �� 5[      ���#E0E0E0$��1��6.   $	��200��69   $��20q�6E   5\      5]      5^      5_      
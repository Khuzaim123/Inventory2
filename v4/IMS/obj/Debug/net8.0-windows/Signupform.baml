   M S B A M L   `   `   `  ����  Inventory managment    InventoryApp.Signupform   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  SignUp��5   	   $	��500��6   $	��400��6%    � 
ResizeMode$ NoResize=�61    � WindowStartupLocation$ CenterScreen=�6G   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   .+�5   
   �� $��0��6   ���#008080 �� Padding$ 10q�63   ._�5      �� $��Signup��6   $��28(�6&   $
��Bold"�64   
�������$��Center=�6Y   5      �� 5   
   $��1��6    �� CornerRadius$ 10��6   
�������$��20q�6C   $ 20q�6O    J� Opacity$	 0.9Q�6\   ._�5      �� .+�5      �� $��	Username:��6   $��SemiBold"�6-   
�����p�$��0,0,0,5q�6Z   �� 5      -   5         ��Name$ UsernameTextBox��5      $��35��63   $	��0,5q�6?   ���#F9F9F9$��5q�6a   $��16(�6m   �� 5      $��	Password:��6   $��SemiBold"�6-   
�����p�$��10,0,0,5q�6Z   G� 5      -   5         G�Name$ PasswordBox��5      $��35��63   $	��0,5q�6?   ���#F9F9F9$��5q�6a   $��16(�6m   �� 5      $��Role:��6   $��SemiBold"�6)   
�����p�$��10,0,0,5q�6V   �� 5      -   5        	 ��Name$	 RoleComboBox��5      $��35��61   $	��0,5q�6=   ���#F9F9F9$��16(�6_   .@�5      �� $��Admin��6#   �� 5       $��Manager��6#   �� 5!      $��Staff��6#   5"      �� 5%      -   5        $��Signup��5%      $��45��6E   ���#008080
�������$��SemiBold"�6y   $��0,20,0,0q�6�   
 � Cursor$

 Hand��6�   $��16(�6�   ��5&      �� 5'       �� 
TargetType" ��6*   .R�5(      � .+�5)   "   ��  ��Name$ border��6)   #��z��69   $ 10��6c   
������ ._�5*   &   �� $��Center=�67   $��Center=�6T   5+   #   5,       �� Triggers 5-      P� 5.   "    P� Property �@J�IsMouseOver P� Value$
 True��6A   . �5/   &   ��  �� 
TargetName$ border��6-    �� Property � �� �� Value
 ��� �6W   50   #   P� 51   "    � ��$
 True��6?   . �52   &   �� $ border��6-    � ��
 �OO/�6W   53   #   55      57      58      59      5:      5;      
   M S B A M L   `   `   `  ����  Inventory managment,    &Inventory_managment.Inventory_Tracking   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Inventory Tracking��5   	   $	��600��6$   $	��800��61   ���#F0F4F8.��5      � .+�5   
   �� $��Inventory Tracking��6   $��24(�6.   $
��Bold"�6<   $��Center=�6N   $��	0,10,0,10q�6k   ���#0080806~   �� 5	   
   $��0,48,20,-28q�6   .@�5      �� $��	Locations��6   .��5      �� $��10q�6   .+�5      �� $��Add New Location:��6    $
��Bold"�69   ���#0080806K   �� 5      $��Location Name:��6    $
��Bold"�66   ���#333$��0,5,0,0q�6Z   �� 5      -   5         ��Name$ txtLocationName��5      $	��200��67   $��0,5,0,10q�6C   �� 5      $��Address:��6    $
��Bold"�60   ���#333$��0,5,0,0q�6T   �� 5      -   5        $ txtLocationAddress��5      $	��200��6:   $��0,5,0,10q�6F   �� 5      $��Phone Number:��6    $
��Bold"�65   ���#333$��0,5,0,0q�6Y   �� 5      -   5        $ txtLocationPhone��5      $	��200��68   $��0,5,0,10q�6D   �� 5      -   5        $��Add Location��5      $	��200��6Q   ���#008080
�������$
��Bold"�6�   $��0,5,0,10q�6�   �� 5      $��Existing Locations:��6    $
��Bold"�6;   ���#008080$��0,20,0,0q�6b   �� 5      -   5         ��Name$ lstLocations��5      $	��150��64   
����������#3336T   5      5       �� 5#      $��Batches��6   .��5$      �� $��10q�6   .+�5%      �� $��Add New Batch:��6    $
��Bold"�66   ���#0080806H   �� 5(      $��Product ID:��6    $
��Bold"�63   ���#333$��0,5,0,0q�6W   �� 5)      -   5        $ txtBatchProductID��5)      $	��200��69   $��0,5,0,10q�6E   �� 5,      $��Batch Number:��6    $
��Bold"�65   ���#333$��0,5,0,0q�6Y   �� 5-      -   5        $ txtBatchNumber��5-      $	��200��66   $��0,5,0,10q�6B   �� 50      $��Expiry Date:��6    $
��Bold"�64   ���#333$��0,5,0,0q�6X   �� 51      -   5        $ txtBatchExpiry��51      $	��200��66   $��0,5,0,10q�6B   �� 54      $��	Quantity:��6    $
��Bold"�61   ���#333$��0,5,0,0q�6U   �� 55      -	   5        $ txtBatchQuantity��55      $	��200��68   $��0,5,0,10q�6D   �� 58      $��Select Location:��6    $
��Bold"�68   ���#008080$��0,5,0,5q�6_   �� 59      -
   5         ��Name$ cmbBatchLocation��59      $	��200��69   $��0,5,0,10q�6E   .@�5:      �� $��Multan��6'   �� 5;      $��Lahore��6'   �� 5<      $��	Islamabad��6'   �� 5=      $��United Kingdom��6'   5>      �� 5@      -   5        $��	Add Batch��5@      $	��200��6K   ���#008080
�������$
��Bold"�6   $��0,5,0,10q�6�   �� 5B      $��Existing Batches:��6    $
��Bold"�69   ���#008080$��0,20,0,0q�6`   �� 5C      -   5        $ 
lstBatches��5C      $	��150��62   
����������#3336R   5D      5E      �� 5H      $��Stock Movements��6   .��5I      �� $��	10,0,-2,0q�6   $	��503��60   .+�5J      �� $��Record Stock Movement:��6    $
��Bold"�6>   ���#0080806P   �� 5K      $��Product ID:��6    $
��Bold"�63   ���#333$��0,5,0,0q�6W   �� 5L      -   5        $ txtMovementProductID��5L      $	��200��6<   $��0,5,0,10q�6H   �� 5M      $��	Quantity:��6    $
��Bold"�61   ���#333$��0,5,0,0q�6U   �� 5N      -   5        $ txtMovementQuantity��5N      $	��200��6;   $��0,5,0,10q�6G   �� 5P      $��Movement Type:��6    $
��Bold"�66   ���#008080$��0,5,0,5q�6]   �� 5Q      -   5        $ cmbMovementType��5Q      $	��200��68   $��0,5,0,10q�6D   .@�5R      �� $��IN��6'   �� 5S      $	��OUT��6'   �� 5T      $��TRANSFER��6'   �� 5U      $��
ADJUSTMENT��6'   5V      �� 5X      $��Source Location:��6    $
��Bold"�68   ���#008080$��0,5,0,5q�6_   �� 5Y      -   5        $ cmbSourceLocation��5Y      $	��200��6:   $��0,5,0,10q�6F   .@�5Z      �� $��Multan��6'   �� 5[      $��Lahore��6'   �� 5\      $��	Islamabad��6'   �� 5]      $��United Kingdom��6'   5^      �� 5`      $��Destination Location:��6    $
��Bold"�6=   ���#008080$��0,5,0,5q�6d   �� 5a      -   5        $ cmbDestinationLocation��5a      $	��200��6?   $��0,5,0,10q�6K   .@�5b      �� $��Multan��6'   �� 5c      $��Lahore��6'   �� 5d      $��	Islamabad��6'   �� 5e      $��United Kingdom��6'   5f      �� 5h      -   5        $��Record Movement��5h      $	��200��6W   ���#008080
�������$
��Bold"�6�   $��0,5,0,10q�6�   �� 5j      $��Movement History:��6    $
��Bold"�69   ���#008080$��0,20,0,0q�6`   �� 5k      -   5        $ lstMovementHistory��5k      $	��150��6:   
����������#3336Z   5l      5m      5n      5o      5p      
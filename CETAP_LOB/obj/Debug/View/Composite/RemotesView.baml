   M S B A M L   `   `   `  ����Q  MIntellibox, Version=1.3.0.0, Culture=neutral, PublicKeyToken=8e428f51fac3a5f0K4clr-namespace:FeserWard.Controls;assembly=IntelliboxFeserWard.Controls  5      D @CETAP_LOB, Version=1.1.0.1, Culture=neutral, PublicKeyToken=nullC&clr-namespace:CETAP_LOB.View.CompositeCETAP_LOB.View.Composite 5	      *   $CETAP_LOB.View.Composite.RemotesView   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      f bGalaSoft.MvvmLight.Platform, Version=5.3.0.19032, Culture=neutral, PublicKeyToken=5f873c45e98af8a1-Custom http://www.galasoft.ch/mvvmlight  5      ?auto4clr-namespace:FeserWard.Controls;assembly=Intellibox  5      :d2http://schemas.microsoft.com/expression/blend/2008  5      c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5      2local&clr-namespace:CETAP_LOB.View.Composite  5	      Dmc;http://schemas.openxmlformats.org/markup-compatibility/2006  5
      Z VFirstFloor.ModernUI, Version=1.0.9.0, Culture=neutral, PublicKeyToken=bc9b0c37bf06c6a90mui&http://firstfloorsoftware.com/ModernUI  5      6  0FirstFloor.ModernUI.Windows.Controls.ModernFrame    	KeepAlive$  FalseX�5      $��1q�5       � DataContext 5      �� *	Remotes+   Locator �� Source# [  ��5      �� 5   
    �� Color 6   �� *(&SettingsAppearance.SelectedAccentColor+# [  .��5      � $
��1896��5      $	��766��5      $��0,0,0,0q�5      $
��Left=�5      $	��Top=�5       � ColumnDefinitions 5   
   �� 5      $	��10* �6   �� 5      $	��40* �6    � RowDefinitions 5   
   �� 5      $	��39* �6   �� 5      $	��25* �6   �� 5!      $
��319* �6   .+�5#   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dataGrid��5$   
   $��2��5%   
   $��1��5&   
   $��	5,0,337,0q�5'   
   $��1q�5(   
   $   HorizontalScrollBarVisibility$ Visible=�5)   
      SelectionMode$ Extended=�5-   
   	 �� IsTextSearchEnabled	 .  
 �� IsTextSearchCaseSensitive
 .  "   VerticalScrollBarVisibility$ Visible=�50   
   $
��1050��60   $	��700��6@   $��0,0,5,0q�6P   $��Stretch=�6b   ��5*   
   �� *Remotes.Composit1+# [   �� SelectedItem 5+   
   �� *Remotes.SelectedWriter+# [   �� SelectedValue 5,   
   �� *Remotes.SelectedWriter+# [  ��51      �� 52       6"   �� *(&SettingsAppearance.SelectedAccentColor+# [  59      �� 5:   
   $��1��5;   
   $	��681��5<   
   $��44��5=   
   $��56,0,0,0q�5>   
   $
��Left=�5?   
   $��Center=�5@   
   $+��%Remote Scores for emailing to Clients��5A   
   $��36(�5B   
   �� 5C   
   $��1��5D   
   $	��171��5E   
   $��41��5F   
   $��Center=�5G   
   $��Selected Writer��5H   
   $��24(�5I   
   {�.  $	��Top=�6*   � 5J   
   $��2��6    5K      �� 5L      $	��25* �6    �� 5M      $	��25* �6    �� 5N      $	��25* �6    �� 5O      $	��25* �6    �� 5P      $	��25* �6    �� 5Q      $	��25* �6    �� 5R      $	��25* �6    �� 5S      $	��25* �6    �� 5T      $	��25* �6    �� 5U      $	��25* �6    �� 5V      $	��25* �6    �� 5W      $	��25* �6    .+�5Y      �� $��Generate PDF��6   $
��Left=�6,   $��	229,0,0,0q�6G   $��11��6Z   $��Center=�6h   $��48��5Z      $	��146��6   ��6*   �� *IndividualReportCommand+�� 5[      $��Center=�6   $��44��66   $��Center=�6B   $	��359��6]    �� Orientation$ 
Horizontal=�6i   .+�5\      �� $��
First Name��6   $��28��6-   $	��145��69   $��5,0,0,0q�6E   $��18(�6V   �� 5]      $	��194��6@   $��32��6L   $��10,0,5,0q�6X   ��6   �� *SelectedWriter.Name+6X   5^      �� 5_      $��Center=�6   $��44��66   $��Center=�6B   $	��359��6]   $ 
Horizontal=�6i   $��1��6�   .+�5`      �� $��Surname��6   $��28��6*   $	��145��66   $��5,0,0,0q�6B   $��18(�6S   �� 5a      $	��194��6C   $��32��6O   $��10,0,5,0q�6[   ��6   �� *SelectedWriter.Surname+6[   5b      �� 5c      $��Center=�6   $��44��66   $��Center=�6B   $	��359��6]   $ 
Horizontal=�6i   $��2��6�   .+�5d      �� $��SA ID��6   $��28��6(   $	��145��64   $��5,0,0,0q�6@   $��18(�6Q   �� 5e      $	��194��6@   $��32��6L   $��10,0,5,0q�6X   ��6   �� *SelectedWriter.SAID+6X   5f      �� 5g      $��Center=�6   $��44��66   $��Center=�6B   $	��359��6]   $ 
Horizontal=�6i   $��4��6�   .+�5h      �� $��Date Of Birth��6   $��28��60   $	��145��6<   $��5,0,0,0q�6H   $��18(�6Y   �� 5i      $	��194��6?   $��32��6K   $��10,0,5,0q�6W   ��6   �� *SelectedWriter.DOB+6W   5j      �� 5k      $��Center=�6   $��44��66   $��Center=�6B   $	��359��6]   $ 
Horizontal=�6i   $��8��6�   .+�5l      �� $��AL Score��6   $��28��6+   $	��145��67   $��5,0,0,0q�6C   $��18(�6T   �� 5m      $	��194��6C   $��32��6O   $��10,0,5,0q�6[   ��6   �� *SelectedWriter.QLScore+6[   5n      �� 5o      $��Center=�6   $��44��66   $��Center=�6B   $	��359��6]   $ 
Horizontal=�6i   $��7��6�   .+�5p      �� $��Venue��6   $��28��6(   $	��145��64   $��5,0,0,0q�6@   $��18(�6Q   �� 5q      $	��194��6E   $��32��6Q   $��10,0,5,0q�6]   ��6   �� *SelectedWriter.VenueName+6]   5r      �� 5s      $��Center=�6   $��44��66   $��Center=�6B   $	��359��6]   $ 
Horizontal=�6i   $��6��6�   .+�5t      �� $��Date Of Test��6   $��28��6/   $	��145��6;   $��5,0,0,0q�6G   $��18(�6X   �� 5u      $	��194��6?   $��32��6K   $��10,0,5,0q�6W   ��6   �� *SelectedWriter.DOT+6W   5v      �� 5w      $��Center=�6   $��44��66   $��Center=�6B   $	��359��6]   $ 
Horizontal=�6i   $��5��6�   .+�5x      �� $��NBT Reference��6   $��28��60   $	��145��6<   $��5,0,0,0q�6H   $��18(�6Y   �� 5y      $	��194��6A   $��32��6M   $��10,0,5,0q�6Y   ��6   �� *SelectedWriter.RefNo+6Y   5z      �� 5{      $��Center=�6   $��44��66   $	��Top=�6B   $	��359��6Z   $ 
Horizontal=�6f   $��0,9,0,0q�6   $��3��6�   $��2��6�   .+�5|      �� $��Passport/Foreign ID��6   $��28��66   $	��160��6B   $��5,0,0,0q�6N   $��18(�6_   �� 5}      $	��177��6E   $��32��6Q   $��5,0,0,0q�6]   ��6   �� *SelectedWriter.ForeignID+6]   5~      �� 5      $
��Left=�6   $��44��64   $��Center=�6@   $	��359��6[   $ 
Horizontal=�6g   $��9��6�   $��5,0,0,0q�6�   .+�5�      �� $��QL Score��6   $��28��6+   $	��145��67   $��5,0,0,0q�6C   $��18(�6T   �� 5�      $	��194��6D   $��32��6P   $��10,0,5,0q�6\   ��6   �� *SelectedWriter.QLScore+6\   5�      �� 5�      $
��Left=�6   $��44��64   $��Center=�6@   $	��359��6[   $ 
Horizontal=�6g   $��10��6�   $��5,0,0,0q�6�   .+�5�      �� $��
Math Score��6   $��28��6-   $	��145��69   $��5,0,0,0q�6E   $��18(�6V   �� 5�      $	��194��6E   $��32��6Q   $��10,0,5,0q�6]   $��18(�6o   ��6   �� *SelectedWriter.MATScore+6o   5�      5�      �� 5�   
   $��1��6   $��1��6%   $ 
Horizontal=�62   $��56,0,0,0q�6K   .+�5�      �� $2��,Search record by name, Surname or NBT Number��6   $��Center=�6K   $��5,0,5,0q�6f   $��16(�6w   $	��368��6�   $��28��6�   �� 5�       �� TextWrapping$
 Wrap=�5�      $	��146��6*   $��Center=�66   $��10,0,5,0q�6Q   $��14(�6c   $��28��6q   $��Center=�6}   ��5�      �� *SearchString+ �� UpdateSourceTrigger$ PropertyChanged=�5�   }   5�      5�      5�      
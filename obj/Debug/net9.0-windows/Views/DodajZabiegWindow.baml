   M S B A M L   `   `   `  ����  MenadzerRoslin&     MenadzerRoslin.DodajZabiegWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Dodaj zabieg��5   	   $	��770��6   $	��700��6+    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   ���#FAFCFA � 	Resources 5         PrimaryGreen  
LightGreen  	DarkGreen  AccentGreen  BackgroundGreen  PrimaryBrush �� Color  
LightBrush  	DarkBrush  AccentBrush 	 BackgroundBrush 
 HeaderStyle �� 
TargetType �� Property �� Value  
LabelStyle  ComboBoxStyle  DatePickerStyle(  "System.Windows.Controls.DatePicker  TextBoxStyle  ButtonStyle  CheckBoxStyle��%@  &	        &	      &	 >     &	 ]     &	 |     &	 �     7   &	 �     7  &	 �     7  &	 �     7  &		 �     7  &	
     7  &	     &	 �    7  &	 �    7  &	 {    7  &	 X    &	 �    ��5
   
   #2E7D32��6.   ��5   
   #C8E6C9��6,   ��5   
   #1B5E20��6+   ��5   
   #66BB6A��6-   ��5   
   #F1F8E9��61   �� 5   
   8   6/   �� 5   
   8   6-   �� 5   
   8   6,   �� 5   
   8   6.   �� 5   
   8   62   �� 5   
   " ��6$   .�5      ��  � ��$ 24(�6)   �� 5       � ��$
 Bold"�6+   �� 5       � ��8   6+   �� 5       � ��$ 0,0,0,20q�6'   �� 5       �@��TextAlignment$ Center=�6.   5      �� 5   
   " ��6#   .�5       ��  � ��$ 14(�6)   �� 5!       � ��$ SemiBold"�6+   �� 5"       � �� �#3E53456+   �� 5#       � ��$ 0,5,0,5q�6'   5$      �� 5&   
   " ��6&   .�5'      ��  � ��$ 38��6'   �� 5(       � ��$	 8,8q�6(   �� 5)       � ��$ 0,0,0,20q�6'   �� 5*       � ��
 �����6+   �� 5+       � ��8   6,   �� 5,       � ��$ 1q�60   5-      �� 5/   
   "  6(   .�50      ��  � ��$ 38��6'   �� 51       � ��$ 0,0,0,20q�6'   �� 52       � ��8   6,   �� 53       � ��$ 1q�60   54      �� 56   
   " ��6%   .�57      ��  � ��$
 10,8q�6(   �� 58       � ��$ 0,0,0,20q�6'   �� 59       � ��
 �����6+   �� 5:       � ��8   6,   �� 5;       � ��$ 1q�60   5<      �� 5>   
   " ��6$   .�5?      ��  � ��$ 40��6'   �� 5@       � ��$	 120��6)   �� 5A       � ��$
 15,5q�6(   �� 5B       � ��$	 8,0q�6'   �� 5C       � ��$ SemiBold"�6+   �� 5D       � ��$ 14(�6)   �� 5E       � ��$ 0q�60   �� 5F       �@�Cursor$
 Hand��6'   5G      �� 5I   
   " ��6&   .�5J      ��  � ��$ 0,5,0,20q�6'   �� 5K       � �� �#3E53456+   �� 5L       � ��$ Center=�69   �� 5M       � ��$ 14(�6)   5N      5O      .��5Q      �� 
�������$��15q�6     �� CornerRadius$ 10��6,   $��1q�6>   #��[ 6R   ._�5R   
   � $��25q�6   	 � RowDefinitions	 5S      �� 5T      $
��Auto �6    �� 5U      $��* �6    �� 5V      $
��Auto �6    .+�5Z      �� $��0��6   #��[	 5[      $ 10��5\      
 �� Padding$
 20q�5]      ._�5^      �� $&�� Dodaj nowy zabieg pielęgnacyjny��6   #��[
 5_      5`      �� 5c      $��1��6   $��0,25,0,0q�6&   .+�5d      �� $��	Roślina:��6   #��[ 6,   �� 5e      #��[	 6   #��[ 5f      $��1q�5g      $ 4��5h      $��0,0,0,20q�5i      $��38��5j      ._�5k      �� -   5         ��Name$ RoslinaTextBlock��5k        �� Padding$
 12,8q�5l      #��[ 5m      $��SemiBold"�5n      5o      �� 5q      $��Typ zabiegu:��6   #��[ 60   �� 5r      -   5         ��Name$ TypZabieguComboBox��5r      #��[ 67   .@�5s      �� $��
Podlewanie��6#   �� 5t      $��
Nawożenie��6#   �� 5u      $��Przesadzanie��6#   �� 5v      $��Przycinanie��6#   �� 5w      $��Spryskiwanie��6#   �� 5x      $��Czyszczenie liści��6#   �� 5y      $
��Inne��6#   5z      � 5|       � ColumnDefinitions 5}      �� 5~      $��* �6+   �� 5      $��* �6+   .+�5�      �� $��0��6!   $��0,0,10,0q�61   .+�5�      �� $��Data wykonania:��6$   #��[ 6;     5�      -   5          Name$ DataWykonaniaDatePicker��5�   %   #��[ 6F   5�      �� 5�      $��1��6!   $��10,0,0,0q�61   $��Bottom=�6C   .+�5�      �� -   5         ��Name$  DodajPrzypomnienieCheckBox��5�   #   #��[ 5�   !    d� 	IsChecked$ FalseY�5�   !   $.��(Dodaj przypomnienie o następnym zabiegu��5�   !   5�      5�      �� 5�      $��Opis zabiegu:��6   #��[ 61   �� 5�      -   5         ��Name$ OpisTextBox��5�      $	��150��5�      #��[ 5�       �� TextWrapping$
 Wrap=�5�       �� AcceptsReturn$
 True��5�      $	��Top=�5�      5�      �� 5�      $��2��6   #��[	 5�      $ 10��5�      $��0,20,0,0q�5�      $
 15q�5�      ._�5�      ��  �� Orientation$ 
Horizontal=�6   $��Center=�66   .+�5�      �� $��Anuluj��6   #��[ 5�       �� IsCancel$
 True��5�      ���#E0E0E0���#424242 5�      ��%^   '��       �� 5�      " ��6$   .�5�   "   ��  �@��CornerRadius$ 5��6A   5�      5�      5�      �� 5�      -   5        $��Dodaj zabieg��5�      #��[ 5�       �� 	IsDefault$
 True��5�      #��[ 5�      
������� 5�      ��%^   '��       �� 5�      " ��6$   .�5�   "   ��  �@��CornerRadius$ 5��6A   5�      5�      5�      5�      5�      5�      5�      5�      
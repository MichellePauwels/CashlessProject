   M S B A M L   `   `   `  ����U  Qnmct.ba.cashlessproject.ui, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null[2clr-namespace:nmct.ba.cashlessproject.ui.ViewModel$nmct.ba.cashlessproject.ui.ViewModel  5      @    :nmct.ba.cashlessproject.ui.View.Vereniging.VerenigingLogin   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Avm2clr-namespace:nmct.ba.cashlessproject.ui.ViewModel     5        � DataContext  5	      <   6nmct.ba.cashlessproject.ui.ViewModel.LoginVerenigingVM  5
   
   .��5      �  � ColumnDefinitions 5   
   �� 5      $
��261* �6   �� 5      $
��259* �6    � RowDefinitions 5   
   �� 5      $	��87* �6   �� 5      $	��56* �6   �� 5      $
��Auto �6   $��72��6*   �� 5      $	��46* �6   �� 5      $��58 �6   .+�5   
   �� -   5         ��Name$ Title��5       	  Title#��[  6#   $��Login��6B   �� 5   
   $��50��6   $��60��6   $
��Fill=�6'   $��View/Images/beaver.png��66    J� Opacity$	 0.1Q�6V   $��191.5,10,10,103.5q�6d   $��1��6   �� 5   
    	 Label#��[ 6   $��1��63   $��Gebruikersnaam��6@   �� 5   
   #��[ 6   $��1��63   $��Paswoord��6@   $��1��6P   �� 5   
   -   5         ��Name$ txtUser��5       	 Input#��[ 6#   $��2��6B   ��6O   �� *
Username+G� 5   
   -   5         G�Name$ txtPwd��5        InputPWD#��[ 6&   $��2��6H   $��1��6U   �� 5   
   -   5        $ txbError��5        ErrorMessage#��[ 6&   $��3��6c   $��10,0,10,14.5q�6p   $��2��6�   ��6L   �� *Error+6�   �� 5   
   -   5         ��Name$ btnLogin��5      $��Login��6#     
ButtonBlue#��[ 63   $��4��6�   $��2��6�   ��6W   �� *LoginCommand+��6x   ��  �� ElementName$ txtPwd��6�   5       5!      
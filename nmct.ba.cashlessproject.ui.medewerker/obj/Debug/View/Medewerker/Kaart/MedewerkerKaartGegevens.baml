   M S B A M L   `   `   `  ����)  %nmct.ba.cashlessproject.ui.medewerkerY    Snmct.ba.cashlessproject.ui.medewerker.View.Medewerker.Kaart.MedewerkerKaartGegevens   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .��5      �   � ColumnDefinitions  5	   
   �� 5
      $
��261* �6   �� 5      $
��259* �6    � RowDefinitions 5   
   �� 5      $��87 �6   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��2* �6   �� 5      $��28 �6   �� 5      $��58 �6   .+�5   
   �� -   5         ��Name$ Title��5       	  Title#��[  6#   $��Klant��6B   �� 5   
    	 Label#��[ 6   $��1��63   $��Card ID��6@   �� 5   
     Text#��[ 6   $��1��62   $��2��6?   $��17q�6O   ��6[   �� *&$LoggedInCostumer.Rijksregisternummer+ �� UpdateSourceTrigger$ PropertyChanged=��� 5   
   #��[ 6   $��2��63   $��Naam kaarthouder��6@   �� 5   
   #��[ 6   $��2��62   $��2��6?   $��17q�6O   ��6[   �� *LoggedInCostumer.CostumerName+$ PropertyChanged=��� 5   
   #��[ 6   $��3��63   $��Huidig saldo��6@   �� 5   
   #��[ 6   $��3��62   $��2��6?   $��17q�6O   �� 5      $
�€ ��6    �� 5      �6   �� *LoggedInCostumer.Balance+$ PropertyChanged=�5      �� 5   
     ErrorMessage#��[ 6   $��2��6Q   $��4��6e   $��5q�6r   ��6:   �� *Error+6r   �� 5    
     
ButtonBlue#��[ 6   $��Klant ophalen��65   $��2��6M   $��5��6a   ��6n   �� *GetCostumerCommand+5!      5"      
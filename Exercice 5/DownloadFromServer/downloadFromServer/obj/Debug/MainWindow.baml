   M S B A M L   `   `   `  ????M  IdownloadFromServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:downloadFromServerdownloadFromServer  5   	   #    downloadFromServer.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:downloadFromServer     5   	     ? Title$  Download From Server??5   	   $	??661??6&   $	??761??63    ? 
ResizeMode$ NoResize=?6?   ??5	      ?? 5
   
    ?? ImageSource$ /Downloading-Files.png??6   .??5      ? .+?5   
   ?? -   5         ??Name$ myTitle??5      ???#FFA64E$??Center=?66   $??Center=?6Z   $??Download From Server??6|   $??Center=?6?   $??0,31,0,0q?6?   $	??Top=?6?   $	??641??6?   $??87??6?   $??Times New Roman)?6?   $??54(?6  $
??Bold"?6%  $??Normal$?67   J? Effect 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      5      ?? 5   
   -   5         ??Name$ requestedFile??5      $??Center=?6Q   $??Center=?6s   $??	0,432,0,0q?6?    ?? TextWrapping$
 Wrap=?6?   $	??Top=?6?   $	??230??6?   $??41??6?   $??18(?6?   $??Times New Roman)?6?   $??	10,0,10,0q?6  ?? 5   
   -   5         ??Name$ download??5      $??Center=?6:   $??Center=?6^   $??Download??6?   $??Center=?6?   $??	0,505,0,0q?6?   $	??Top=?6?   ???#aaaaaa$	??168??6  $??52??6  $??24(?6  $
??Bold"?6-  $??0,0,0,0q?6?   ? Cursor$
 Hand??6Y  {?.  ??6?   Z? 	 ? 	Resources	 5      
 ?? 
TargetType ?? Property ?? Value??%_   '??       ?? 5      "
 ??6   .?5      ??  ?@??CornerRadius$ 10??65   5      5      ??5      ?? 5      "
 ??6   .?5      ??  ? ?? ?#cF863E63   ?? 5        ? ?? 5!      ?? 5"       ?? 
TargetType" ??6.   .R?5#   "   ?? #??z??6)   ._?5$   &   ?? $??Center=?67   $??Center=?6T   5%   #   5&      5(       ?? Triggers 5)      P? 5*       P? Property ?@J?IsMouseOver P? Value . . ?5+      ??  ? ?? ?#9F561E6;   5,      5.       51        52      55      58      59      
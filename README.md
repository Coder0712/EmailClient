# EmailClient

Der E-Mail-Client ist ein privates Projekt, welches ich aus persönlichem Interesse bearbeite. 
Der Client wird stetig weiterentwickelt. 

E-Mails können täglich abgeholt werden

E-Mails können empfangen werden und gesendet werden. 

Die Nutzeroberfläche wurde mit WPF erstellt. 

Die Architektur basiert auf MVVM. 

Er speichert die Texte der E-Mails in einer Textdatei, die restlichen Inhalte werden in einer Datenbank gespeichert. 

Ist eine Datenbank nicht vorhanden wird diese eingerichtet. 

Die E-Mails werden in einem separaten Thread abgeholt und dann an den Hauptthread übergeben. 
Einzelne Prozesse, wie die Herstellung der Verbindung zum Mail-Server, werden asynchron ausgeführt.  

Die Verbindung zum Server wird über die Klasse ServerService erstellt. Es existiert nur ein Objekt dieser Klasse zur Laufzeit.

An eine E-Mail können Anhänge hinzugefügt werden. 


Features, die noch hinzugefügt werden: 

Anhänge von empfangenen E-Mails werden gespeichert. 

E-Mails lassen sich öffnen und der Inhalt wird angezeigt, zusammen mit dem Anhang. 

Weitere Anpassungen für konsequente Umsetzung von MVVM. 

E-Mails können nicht nur vom aktuellen Tag abgeholt werden.

Prüfung neuer E-Mails in einem gewissen Intervall.

Beheben von Bugs beim Abholen der E-Mails.

uvm. 




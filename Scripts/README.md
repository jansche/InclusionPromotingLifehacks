# Catch Longie - barrierefreies Smartphone-Spiel
In dem Spiel geht es darum, den Hund Longie zu fangen. Durch Swipen nach links oder rechts des Smartphones lässt sich die Laufrichtung des Spielers (First Person) verändern. Durch Kippen nach vorne bewegt sich der Spieler nach vorne. Die Sicht und der Sound verschwimmen. Der Spieler muss stehen bleiben um sich wieder zu orientieren und dem Bellen von Longie oder deren Visualisierung (“wuff”) zu folgen. 

## Installation
Projektordner in Unity öffnen.

## Dokumentation
Model - View - Controller. 
Game Controller Script (auf app-GO) ist für die Steuerung des Spiels zuständig.
MapMovementToEffect Script (auf FirstPersonCharacter GO) steuert die Image Processing Effekte (Blur & Fisheye) und Sound
FirstPersonController Script (auf FirstPersonController GO) reagiert auf die Bewegung des Smartphones und steuert den Character.

Weitere Dokumentation im Code.

Das Game ist unter folgendem Link zu finden: https://www.dropbox.com/sh/cxqtcsgz2atoe9s/AAAaOgwwfOWbKQRi4q7vfv3qa?dl=0

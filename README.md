﻿MorseBuzzer
============

Morse code buzzer for Netduino

Based on code from the Sound/Motion chapter of Getting Started with Netduino book

Takes a string provided in the variable morseText and uses the speaker to buzz it out in Morse code.

Morse code speed is adjusted by changing the beatsPerMinute variable. 500 sounds like it might be something around 15-20 wpm.

TODO
====
* ~~Rewrite sound producing part to make changing the generated tone easier~~
* ~~Make the random call sign generator work~~

IDEAS
=====
* Generate random letters to play
* ~~Generate random "call sign" to play~~
* Play the same letter over and over
* Play the alphabet over and over
* Adjustable speed using a pot
* Switch modes using a push button or switch
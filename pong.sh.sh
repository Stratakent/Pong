#!/bin/bash

#Author: Brian Nguyen
#Course: CPSC223n
#Semester: Fall 2016
#Assignment: 6
#Due: 17 December 2016.

#This is the bash script shell file required to compile this program.

echo First remove old binary files
rm *.dll
rm *.exe

echo View the list of source files
ls -l

echo Compile pongalgorithm.cs to create the file: pongalgorithm.dll
mcs -target:library -out:pongalgorithm.dll pongalgorithm.cs

echo Compile ponginterface.cs to create the file: ponginterface.dll
mcs -target:library -r:System.Drawing.dll -r:System.Windows.Forms.dll -r:pongalgorithm.dll -out:ponginterface.dll ponginterface.cs

echo Compile pongmain.cs and link the two previously created dll files to create an executable file. 
mcs -r:System -r:System.Windows.Forms -r:ponginterface.dll -out:Fibo.exe pongmain.cs

echo View the list of files in the current folder
ls -l

echo Run the Assignment 4 program.
./Fibo.exe

echo The script has terminated.













# Flood Fill Game ~~Alpha~~
 
Source code of the Flood it. 

Flood it game implements **the basic functionality** of matrix and **the save state feature**. 
Most of the basic functionality is dedicated is matrix parsing and state keeping
Save state feature is used in when a user does the action on the game

## Open in the Emulator right now

#### Just need to install Visual Studio and xamarin to test it

## Modify something

On ViewModel you can change type of colors and matrix size + you can add ActiveSquares handler for bigger matrix (CPU intensive) but for 50x50 this is playable


####How to code and how to play it

This game is a puzzle. Its one player mode and your goal is to finish it with minimal color changes.
 Your Color field is at upper left and at the moment you select another color the pattern will follow this way.
 1. Lets say you have color A and at the left is color B and downside is color C
 
 A B A A B
 C B B B B
 C C B A C
 
 2. Your next move is B for example and on the your pattern 
 
 B B A A B
 C B B B B
 C C B C C
 
 3. then C
 
 C C A A C
 C C C C C
 C C C C C
 
 4. Win the when your board is will same properties.

 A A A A A
 A A A A A
 A A A A A

#####CODE
Game Model
 You have Fields and preferences.
 Fields contains some properties as Color, IsActive, Index.
 And from this element we create Matrix AxB that we can scale how much we want
Algorithem
 This algorithem is based on matrix and manipulating with that matrix 
 1. We check color boundries (neightbours) and we create some kind of chain (ordered list) that we will change other colors based on that new chain that will form every new move.
 2. We have While function that will sovle problem of happening color change of very far neightbour that has some color as ours.
 3.Preferences are Play Counter (SCORE) and refresh button for generating new matrix.
 
 

### Project structure

```
ColorFoldApp.
             ├── Model              # Model Properties, attributes ... etc
             ├── ViewModel          # Actual Data
             └── View               # Views that will apper to gamers ! ```
   

# Match-3

## Introduction
The goal of match-three games is to form lines, chains, or groups of three or more of the same elements. 
Your task is to line up the tiles and achieve three-in-a-row. 
When this happens, it causes a chain reaction of more tiles to match, before the line or matches disappear.

<table align="center">
<tr>
<td>
<img src="https://user-images.githubusercontent.com/41696219/223495641-d255abb5-3723-43a8-936b-2b42eddaad56.png" width="300">
</td>
</tr>
</table>
</div>


## Code Architecture
This Unity match-3 game was developed using clean code principles and a clean architecture approach to ensure that the code is modular, maintainable, and scalable. 
In the game's code architecture, the different managers have specific responsibilities and are responsible for coordinating different aspects of the game's logic.

**Grid Manager:** This manager is responsible for generating and managing the game board grid, including creating the game board, filling it with tiles. This manager is primarily concerned with the game's board mechanics and ensuring that the board functions correctly.

**Level Manager:** This manager is responsible for managing the game's levels and determining when the game is won or lost. The level manager coordinates with the grid manager to check for level completion and handles transitions between levels.

**Match Manager:** This manager is responsible for identifying and tracking matches on the game board. It coordinates with the grid manager to check for matches and tracks the score and other gameplay statistics.

**Swap Manager:** This manager is responsible for handling the swapping of tiles on the game board. It coordinates with the tile input manager to detect player input and ensures that tile swaps are valid and do not break the game board's mechanics.

**Tile Input Manager:** This manager is responsible for detecting and handling player input on the game board. It coordinates with the swap manager to initiate tile swaps and ensures that player input does not break the game board's mechanics.

By separating the concerns of each manager, the code architecture ensures that each manager has a clear and specific responsibility, making the codebase easier to understand, maintain, and modify in the future. This approach also facilitates testing, as each manager can be tested independently of the others, which helps to identify and isolate bugs and issues more efficiently.

## Plugins
* Dotween
* Unitask

## Gameplay Videos

https://user-images.githubusercontent.com/41696219/223495249-526054af-844c-411a-8558-7f9115d2f5a2.mp4

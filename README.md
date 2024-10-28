This is a work-in-progress toolset for a potential fan translation of Konami's Playstation 2 Dating Sim _Tokiemeki Memorial 3:Yakusoku no Ano Basho de_

**SET-UP INSTRUCTIONS**
-Extract the 5 numbered Data.BIN files from the game using an .iso editor.

-Specify paths to the data files.



**FEATURES**

Currently, it can:
-extract text from Data5.BIN

-locate compressed game data in the other 4 data BIN files

-extract said game data into seperate files

-(attempt to) decompress the game data. My implementation of the game's decompression routine is WIP.



**PLANNED FEATURES**

-Refine the decompression to make a full extraction of the game script possible.

-Ability to disable the game's anti-tamper functionality.

-Ability to insert a modified script into the game.

-Swap out the game's texture files.

-This is low priority beccause it's not exactly necessary for the translation project, but I'd also like to eventually be able to extract the game's 3D models.

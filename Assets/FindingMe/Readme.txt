This 2D Platformer Controller was originally made by Sebastian Lague.
YouTube Channel:
https://www.youtube.com/user/Cercopithecan

YouTube Playlist:
https://www.youtube.com/playlist?list=PLFt_AvWsXl0f0hqURlhyIoAabKPgRsqjz

If you want to support him, he has a Patreon page:
https://www.patreon.com/SebastianLague

You can find the original Source Code here:
https://github.com/SebLague/2DPlatformer-Tutorial

Source Code was published under the MIT License.


I personally only added the 'double Jump' mechanic and fixed a warning and some other issues.

Notes on implementation:
The player object should have a player-specific layer assigned to it.
This layer must NOT be included in the Controller2D's collision mask.
Moving platforms should contain the player layer in their passenger mask.

See Demo.scene for more information.

Graphics by pzUH:
http://opengameart.org/users/pzuh
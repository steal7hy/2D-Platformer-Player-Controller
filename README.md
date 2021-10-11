# 2D-Platformer-Player-Controller

WARNING: This code uses "rigidbody2d.velocity" instead of using "rigidbody2d.addforce". If you would like to meticulously code every part of how you want your player to move then I think this script might be of some value. Unity doesn't recommend hard coding the velocity values and instead recommends to use AddForce instead. After quite sometime researching and learning more about the game engine, I now too recommend using AddForce instead of hard coding the velocity values. Here is an excellent thread to learn more: https://gamedev.stackexchange.com/questions/113178/when-should-i-use-velocity-versus-addforce-when-dealing-with-player-objects

WARNING (UPDATED): I previously mentioned that using "rigidbody2d.addforce" is probably better than using "rigidbody2d.velocity" but after learning more about Unity's physics system, it doesn't even matter. I personally think directly changing the velocity is better since you can have full control over the player's movement over everything. I'm planning on creating a very dynamic 3d platformer player controller soon to learn more about it but I'm confident that the advice people give for not changing velocity is pretty weak. And yes, I will create a new repository for the 3d platformer player controller when it's done. It will be under the MIT License.

Hi! Here you can use my 2d platformer player controller for free! I haven't optimized it yet so the code is still pretty messy but feel free to use it if you'd like!

It supports Coyote time, Jump buffering, Halved gravity jump peak, and Jump corner correction.

Written for the Unity Engine.

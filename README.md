# Missile-Defense

An attempt at building a missile defense system using Unity and NEAT:

Goal of the project is to teach a missile defense system to shoot down incoming missiles using neuroevolution of augmented topologies (NEAT).


Created some general terrain, added planes and a missile spawn. The planes were added to test if the NEAT algorithm could discern between incoming rockets and civillian aircraft.

<img src="https://github.com/crumpl07/MissileDefense/blob/main/Video/MissileStuff_01.gif" width="1000" height="500"/>


Added enemy missiles and explosion upon impact with a friendly missile. The friendly missiles now track the enemy missile (not using NEAT). Missile tracking is currently done using unity funtions LookAt() and Transform.forward. These will need to be changed at a later date.
![Alt Text](https://github.com/crumpl07/MissileDefense/blob/main/Video/MissileStuff_05.gif)

The references are: 

Brackeys. (2012, December 22). Retrieved January 19, 2021, from https://www.youtube.com/channel/UCYbK_tjZ2OrIZFBvU6CCMiA

Unity User Manual (2019.4 LTS). (2021, January 19). Retrieved Winter, 2020-2021, from https://docs.unity3d.com/Manual/index.html.

Stanely, K. (2014, December 12). The NeuroEvolution of Augmenting Topologies (NEAT) Users Page. Retrieved January 20, 2021, from https://www.cs.ucf.edu/~kstanley/neat.html

Center of Arms Control and Non-Proliferation. (2021, January 27). GMD: Frequently Asked Questions. https://armscontrolcenter.org/issues/missile-defense/gmd-frequently-asked-questions/. 

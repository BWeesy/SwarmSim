# SwarmSim

A 2D experiment for exploring mazes, serverless architecture and teenytiny pixels.
This acts as a testing ground for using VSCode to publish directly to an Azure Function App, foregoing an App Service or even a server in the traditional sense.

The aim for this is to provide a cluster of stateless functions that together can be used to simlutate a swarm of drones exploring a maze together. This will eventually allow the system to work with any front end that can create and display frames of the simulation, opening up avenues for user interaction, probably allowing them to design the maze the drones explore.

## TODO

-Write maze generation in InitFrame
-Write basic drone explore logic
-Write NextStep function
-Add master/slave relationship between drones

## Done

-Add drone to map in InitFrame
-Add more drones in InitFunction

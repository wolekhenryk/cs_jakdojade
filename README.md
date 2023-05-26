
# cs_jakdojade

This is a project which demonstrates how graph algorithms are implemented in the C# languages, using various techniques. Algorithms used in this project vary, from as simple as breadth-first-search (BFS) to more advanced ones, such as Dijkstra's algorithm. Another notable purpose of this project was to show how to handle both weighted and non-weighted graph structures, using robust hash tables, adjacency lists and other data structures.


## General info

This project was built on:

![C#](https://img.shields.io/badge/Programming_Language-C%23-blue.svg?)

![.NET](https://img.shields.io/badge/.NET_FRAMEWORK-5C2D91.svg?logo=.net&logoColor=white?style=for-the-badge&logo=appveyor)





## Documentation

Although this project is a console application, one can provide any arbitrary test file of format ```.txt/.in``` which satisies the following requirements:

- The first line consists of 2 32-bit signed integers separated by single space:
```
WIDTH HEIGHT
```
- The next ```HEIGHT``` lines of length ```WIDTH``` represent a desired map. The map can contain characters denoted by the following regular expression : ```[A-Z0-9*.#]```. Each line must end with ```\n```.

- After that, the program will read number of flight connections, if there are any. The first line consist of a single 32-bit signed integer called ```n```, and the next ```n``` lines are of the following format:

```
FROM TO DURATION
```

- This is a representation of a one-way flight connection which connects cities of name ```FROM``` and ```TO``` in ```DURATION``` minutes.

- Finally, user provides the number of paths they wish to find. This line consists of a single 32-bit signed integer called ```m```, and the next ```m``` lines are of format 

```START DESTINATION TYPE```

- User provides a starting city denoted as ```START```, desired destination and an integer ```TYPE```, which can be either 0 or 1. 1 means that all cities which must be traversed in order to find a path will be displayed, while 0 means they will not.

If those requirements are met, this program is guaranteed to provide the user with the shortest possible path between 2 desired cities.
## Run Locally

Clone the project

```bash
  git clone https://github.com/wolekhenryk/cs_jakdojade.git
```

Go to the project directory

```bash
  cd cs_jakdojade
```

Open the ```.sln``` file using Visual Studio with ```.NET``` installed, build and run!


## Features

- Adjusting mode of found paths
- Massive performance capabilities
- Precise and efficient paths


## Authors

- [@wolekhenryk](https://www.github.com/wolekhenryk)


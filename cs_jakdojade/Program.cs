using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Jakdojade_CS {
  internal class Program {
    private static bool InsideMap(ValueTuple<int, int> mapSize, ValueTuple<int, int> position) {
      var (i, j) = position;
      var (height, width) = mapSize;

      return i >= 0 && j >= 0 && i < height && j < width;
    }
    private static ValueTuple<int, int> FindCityPosition(IReadOnlyList<string> map, int i, int j) {
      var shifts = new List<(int, int)> { (0, -1),  (-1, 0), (0, 1), (1, 0),
                                          (-1, -1), (-1, 1), (1, 1), (1, -1) };
      var mapSize = (map.Count, map[0].Length);
      foreach (var tuple in shifts) {
        var (deltaI, deltaJ) = tuple;
        if (!InsideMap(mapSize, (i + deltaI, j + deltaJ)))
          continue;
        if (char.IsLetterOrDigit(map[i + deltaI][j + deltaJ]))
          return tuple;
      }
      return (int.MinValue, int.MaxValue);
    }

    private static string FindCityName(IReadOnlyList<string> map, int i, int j) {
      var startIndex = j;
      var endIndex = j;

      while (endIndex < map[i].Length && char.IsLetterOrDigit(map[i][endIndex]))
        endIndex++;

      while (startIndex >= 0 && char.IsLetterOrDigit(map[i][startIndex]))
        startIndex--;

      return map[i].Substring(startIndex + 1, endIndex - startIndex - 1);
    }

    private static void BreadthFirstSearch(IReadOnlyList<IReadOnlyList<int>> grid,
                                           int x,
                                           int y,
                                           int start,
                                           int width,
                                           int height,
                                           ref Dictionary<int, List<Tuple<int, int>>> graph,
                                           ref List<List<bool>> visited) {
      var q = new Queue<Tuple<int, int, int>>();
      var neighbors = new List<Tuple<int, int>>();

      q.Enqueue(new Tuple<int, int, int>(x, y, 0));

      while (q.Count != 0) {
        var (i, j, distance) = q.Dequeue();

        if (grid[i][j] >= 0 && grid[i][j] != start) {
          var neighborId = grid[i][j];
          neighbors.Add(new Tuple<int, int>(neighborId, distance));

          continue;
        }

        if (i > 0 && !visited[i - 1][j] && grid[i - 1][j] != -2) {
          q.Enqueue(new Tuple<int, int, int>(i - 1, j, distance + 1));
          visited[i - 1][j] = true;
        }
        if (i < height - 1 && !visited[i + 1][j] && grid[i + 1][j] != -2) {
          q.Enqueue(new Tuple<int, int, int>(i + 1, j, distance + 1));
          visited[i + 1][j] = true;
        }
        if (j > 0 && !visited[i][j - 1] && grid[i][j - 1] != -2) {
          q.Enqueue(new Tuple<int, int, int>(i, j - 1, distance + 1));
          visited[i][j - 1] = true;
        }
        if (j < width - 1 && !visited[i][j + 1] && grid[i][j + 1] != -2) {
          q.Enqueue(new Tuple<int, int, int>(i, j + 1, distance + 1));
          visited[i][j + 1] = true;
        }
      }

      graph.Add(start, neighbors);
    }

    private static List<List<bool>> CreateBooleanGrid(int height, int width) {
      var grid = new List<List<bool>>();

      for (var i = 0; i < height; i++) {
        var row = new List<bool>();

        for (var j = 0; j < width; j++)
          row.Add(false);

        grid.Add(row);
      }

      return grid;
    }

    private static List<List<int>> GenerateGrid(int height, int width) {
      var grid = new List<List<int>>();

      for (var i = 0; i < height; i++) {
        var row = new List<int>();

        for (var j = 0; j < width; j++)
          row.Add(-3);

        grid.Add(row);
      }

      return grid;
    }

    private static Tuple<int, int> FindCoords(int cityId,
                                              int width) => new Tuple<int, int>(cityId / width,
                                                                                cityId % width);

    private static void Dijkstra(IReadOnlyDictionary<int, List<Tuple<int, int>>> graph,
                                 int startId,
                                 int endId,
                                 int queryType,
                                 IReadOnlyDictionary<int, string> idToCity,
                                 IReadOnlyDictionary<string, int> cityToId) {
      var distancesDictionary = idToCity.ToDictionary(kvp => kvp.Key, kvp => int.MaxValue);

      var prev = idToCity.ToDictionary(kvp => kvp.Key, kvp => -1);
      distancesDictionary[startId] = 0;

      var pq = new PriorityQueue<(int, int), int>(Comparer<int>.Default);
      pq.Enqueue((startId, 0), 0);

      while (pq.Count != 0) {
        var (foundNeighbor, foundDistance) = pq.Dequeue();
        if (foundDistance > distancesDictionary[foundNeighbor])
          continue;

        foreach (var neighbor in graph[foundNeighbor]) {
          var (nextNeighbor, nextDistance) = neighbor;
          var newDistance = nextDistance + foundDistance;
          if (newDistance < distancesDictionary[nextNeighbor]) {
            distancesDictionary[nextNeighbor] = newDistance;
            prev[nextNeighbor] = foundNeighbor;
            pq.Enqueue((nextNeighbor, newDistance), newDistance);
          }
        }
      }

      if (queryType == 0) {
        Console.WriteLine(distancesDictionary[endId]);
      } else {
        Console.Write($"{distancesDictionary[endId]} ");
        var path = new List<int>();

        var current = endId;
        while (current != startId) {
          path.Add(current);
          current = prev[current];
        }

        path.Reverse();
        path.RemoveAt(path.Count - 1);
        path.ForEach(i => Console.Write($"{idToCity[i]} "));

        Console.WriteLine();
      }
    }

    private static void Main(string[] args) {
      var input = Console.ReadLine()?.Split(' ').ToList().Select(int.Parse);

      var width = (input ?? throw new InvalidOperationException()).First();
      var height = input.Last();

      var graph = new Dictionary<int, List<Tuple<int, int>>>();
      var cityMap = new List<string>(height);

      for (var i = 0; i < height; i++) {
        var line = Console.ReadLine();
        line = line.Trim();
        cityMap.Add(line);
      }

      var cityToId = new Dictionary<string, int>();
      var idToCity = new Dictionary<int, string>();

      for (var i = 0; i < height; i++) {
        for (var j = 0; j < width; j++) {
          if (cityMap[i][j] != '*')
            continue;
          var (dI, dJ) = FindCityPosition(cityMap, i, j);
          var (cityI, cityJ) = (i + dI, j + dJ);
          var result = FindCityName(cityMap, cityI, cityJ);
          var cityPosId = i * width + j;

          cityToId.Add(result, cityPosId);
          idToCity.Add(cityPosId, result);
        }
      }

      var grid = GenerateGrid(height, width);
      var noRoads = true;

      for (var i = 0; i < height; i++) {
        for (var j = 0; j < width; j++) {
          if (cityMap[i][j] == '.' || char.IsLetterOrDigit(cityMap[i][j]))
            grid[i][j] = -2;
          if (cityMap[i][j] == '*')
            grid[i][j] = i * width + j;
          if (cityMap[i][j] == '#') {
            grid[i][j] = -1;
            noRoads = false;
          }
        }
      }

      var stopwatch = new Stopwatch();
      stopwatch.Start();

      if (!noRoads) {
        /*Parallel.ForEach(idToCity, kvp => {
          var key = kvp.Key;
          var visited = CreateBooleanGrid(height, width);
          var (x, y) = FindCoords(key, width);
          BreadthFirstSearch(grid, x, y, key, width, height, ref graph, ref visited);
        });*/
        foreach (var kvp in idToCity) {
          var key = kvp.Key;
          var visited = CreateBooleanGrid(height, width);
          var (x, y) = FindCoords(key, width);
          BreadthFirstSearch(grid, x, y, key, width, height, ref graph, ref visited);
        }
      }

      var flightCount = int.Parse(Console.ReadLine() ?? "0");
      for (var i = 0; i < flightCount; i++) {
        var line = Console.ReadLine().Split(' ');

        var fromId = cityToId[line[0]];
        var toId = cityToId[line[1]];
        var duration = int.Parse(line[2]);

        graph[fromId].Add(new Tuple<int, int>(toId, duration));
      }

      var queryList = new List<(int, int, int)>();
      var queryCount = int.Parse(Console.ReadLine() ?? "0");
      for (var i = 0; i < queryCount; i++) {
        var line = Console.ReadLine().Split(' ');

        var fromId = cityToId[line[0]];
        var toId = cityToId[line[1]];
        var type = int.Parse(line[2]);

        queryList.Add((fromId, toId, type));
      }

      Parallel.ForEach(queryList, triple => {
        var (from, to, type) = triple;
        Dijkstra(graph, from, to, type, idToCity, cityToId);
      });

      stopwatch.Stop();
      var elapsedTime = stopwatch.Elapsed.TotalMilliseconds;

      Console.WriteLine($"Elapsed time:{elapsedTime}");

      Console.Read();
    }
  }
}

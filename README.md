# NotSoRandom
PoC for predicting the .net Random class.

This was made just for fun and doesn't really have any purpose.

The program has a prediction class that basically rebuilds the .net Random class and clones itself a few times to show you what the next output will be.

## Example:
```cs
Random r = new Random();
Prediction p = new Prediction(p);
p.Print(0, 100000);
//Output:
//----------PREDICTION----------
//Random.Next() = 1993566476
//Random.Next(max) = 0
//Random.Next(min, max) = 92832
//Random.NextDouble() = 0,928326731979999
//------------------------------
Console.WriteLine("Random value: " + r.Next(0, 100000));
//Output:
//Random value: 92832
```

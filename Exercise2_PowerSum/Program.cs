Console.WriteLine(PowerSum(100,2));

static int PowerSum(int x, int n)
{
	if (n is < 2 or > 10)
		throw new ArgumentOutOfRangeException(nameof(n), "cannot be greater than 10 or less than 2");
	if (x is < 1 or > 1000)
		throw new ArgumentOutOfRangeException(nameof(x), "cannot be greater than 1000 or less than 1");

	
	return Ps(x, n,0, 1, 0);
}

static int Ps(int x, int n, int res, int index, int depth)
{
	if (res == x && depth > 1) return 1;
	if (res > x) return 0;
	
	var count = 0;
	var max = (int)Math.Pow(x, 1.0 / n) + 1;
	
	for (var i = index; i <= max; i ++)
	{
		count += Ps(x, n, res + (int)Math.Pow(i, n), i + 1, depth + 1);
	}

	return count;
}
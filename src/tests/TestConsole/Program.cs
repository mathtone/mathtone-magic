
await Task.WhenAll(Enumerable.Range(0, 10).Select(DoSomething));

static async Task DoSomething(int id) {
	await Task.Delay(100);
	var t1 = Thread.CurrentThread.ManagedThreadId;
	//await Task.Delay(TimeSpan.);
	var t2 = Environment.CurrentManagedThreadId;
	Console.WriteLine($"id: {id}, t1: {t1}, t2: {t2}");
}
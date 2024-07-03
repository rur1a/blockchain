using blockchain;
using blockchain.Blockain;
using blockchain.HashFunctions;

var app = new PopulationCensusApp();
app.RegisterPerson(new Person("John", "Brown"));
app.RegisterPerson(new Person("Alice", "White"));
app.PrintAll();

record Person(string Name, string Surname);
class PopulationCensusApp
{
	private readonly TypedBlockchain<Person> _blockchain;

	public PopulationCensusApp()
	{
		_blockchain = new TypedBlockchain<Person>(new Blockhain(new SHA256Hash()));
	}

	public void RegisterPerson(Person person)
	{
		_blockchain.AddBlock(person);
	}

	public void PrintAll()
	{
		foreach (var person in _blockchain)
		{
			Console.WriteLine(person);
		}
	}
}

using blockchain.Blockain;
using blockchain.Blockain.Rules;
using blockchain.HashFunctions;

namespace blockchain.Apps;

record Person(string Name, string Surname);
class PopulationCensusApp
{
	private readonly TypedBlockchain<Person> _blockchain;

	public PopulationCensusApp()
	{
		_blockchain = new TypedBlockchain<Person>(
			new Blockhain(new SHA256Hash()), 
			new DuplicationRule<Person>());
	}

	public void RegisterPerson(Person person)
	{
		var block = _blockchain.BuildBlock(person);
		_blockchain.AddBlock(block);
	}

	public void PrintAll()
	{
		foreach (var person in _blockchain)
		{
			Console.WriteLine(person);
		}
	}
}
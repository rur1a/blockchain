using blockchain.HashFunctions;

namespace blockchain.Blockain;

class BlockchainBuild
{
	private readonly IHashFunction _hashFunction;
	private string _tail;

	public BlockchainBuild(IHashFunction hashFunction, string tail)
	{
		_hashFunction = hashFunction;
		_tail = tail;
	}

	public void AddBlock(Block data)
	{
		_tail = data.Hash;
	}

	public Block BuildBlock(string data)
	{
		var hash = _hashFunction.GetHash(_tail + data);
		var block = new Block(_tail, data, hash);
		return block;
	}
}
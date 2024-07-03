class BlockchainBuild
{
	private readonly IHashFunction _hashFunction;
	private string _tail;

	public BlockchainBuild(IHashFunction hashFunction, string tail)
	{
		_hashFunction = hashFunction;
		_tail = tail;
	}

	public Block AddBlock(string data)
	{
		var hash = _hashFunction.GetHash(_tail + data);
		var block = new Block(_tail, data, hash);
		_tail = hash;
		return block;
	}
}
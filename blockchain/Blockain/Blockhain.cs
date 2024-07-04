using System.Collections;
using blockchain.HashFunctions;

namespace blockchain.Blockain;

record Block(string ParentHash, string Data, string Hash);
class Blockhain : IEnumerable<Block>
{
	private readonly IHashFunction _hashFunction;
	private readonly BlockchainBuild _blockchainBuild;
	private readonly List<Block> _blocks = new();

	public Blockhain(IHashFunction hashFunction)
	{
		_hashFunction = hashFunction;
		_blockchainBuild = new BlockchainBuild(hashFunction, null);
	}
	
	public Block BuildBlock(string data)
	{
		var block = _blockchainBuild.AddBlock(data);
		return block;

	}

	public void AddBlock(Block block)
	{
		var tail = _blocks.LastOrDefault();
		if (block.ParentHash == tail?.Hash)
		{
			var expectedHash = _hashFunction.GetHash(block.ParentHash + block.Data);
			if(expectedHash==block.Hash)
				_blocks.Add(block);
			else throw new ApplicationException($"Block hash ({block.Hash}) is incorrect");
			
		}
		else
			throw new ApplicationException($"Block hash ({block.Hash}) is incorrect, because of parent hash ({block.ParentHash})");
	}

	public IEnumerator<Block> GetEnumerator() => _blocks.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
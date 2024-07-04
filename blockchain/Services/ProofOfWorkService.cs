using blockchain.Blockain;
using blockchain.Blockain.Rules;

namespace blockchain.Services;

class ProofOfWorkService<T>
{
	private readonly TypedBlockchain<T> _blockchain;
	private readonly Func<T, T> _nextVariant;
	private readonly IProofOfWorkRule<T> _proofOfWorkRule;

	public ProofOfWorkService(TypedBlockchain<T> blockchain, Func<T, T> nextVariant, IProofOfWorkRule<T> proofOfWorkRule)
	{
		_blockchain = blockchain;
		_nextVariant = nextVariant;
		_proofOfWorkRule = proofOfWorkRule;
	}

	public T Proof(int height, T block)
	{
		for (int i = 0; i < int.MaxValue; i++)
		{
			var lowLevelBlock = _blockchain.BuildBlock(block);
			if (_proofOfWorkRule.Execute(height, lowLevelBlock.Hash))
				return block;
			block = _nextVariant(block);
		}

		throw new ApplicationException("Block is not possible to build. Try again after some blocks will be put.");
	}
}
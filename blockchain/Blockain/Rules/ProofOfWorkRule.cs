namespace blockchain.Blockain.Rules;

interface IProofOfWorkRule<T> : IRule<T>
{
	bool Execute(int height, string hash);
}

class ProofOfWorkRule<T> : IProofOfWorkRule<T>
{
	public void Execute(IEnumerable<TypedBlock<T>> previousBlocks, TypedBlock<T> nextBlock)
	{
		var height = previousBlocks.Count();
		if (!Execute(height, nextBlock.Hash))
			throw new ApplicationException("Proof of work is incorrect for this block.");
	}

	public bool Execute(int height, string hash)
	{
		var complexity = (int)(Math.Log2(height + 1) + 1);
		for (int i = 0; i < complexity; i++)
		{
			if (hash[i] != '0')
				return false;
		}

		return true;
	}
}
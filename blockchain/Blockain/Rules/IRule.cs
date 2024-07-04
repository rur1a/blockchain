namespace blockchain.Blockain.Rules;

interface IRule<T>
{
	void Execute(IEnumerable<TypedBlock<T>> previousBlocks, TypedBlock<T> nextBlock);
}

class AmountRule : IRule<TransactionBlock>
{
	public void Execute(IEnumerable<TypedBlock<TransactionBlock>> previousBlocks, TypedBlock<TransactionBlock> nextBlock)
	{
		long balance = 100;
		var currentUser = nextBlock.Data.Data.From;
		
		foreach (var previousBlock in previousBlocks)
		{
			if (previousBlock.Data.Data.From == currentUser)
				balance -= previousBlock.Data.Data.Amount;
			else if (previousBlock.Data.Data.To == currentUser)
				balance += previousBlock.Data.Data.Amount;
		}

		if (balance < nextBlock.Data.Data.Amount)
			throw new ApplicationException($"User has not enough funds. Balance is {balance}");
	}
}
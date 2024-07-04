namespace blockchain.Blockain.Rules;

class DuplicationRule<T> : IRule<T>
{
	void IRule<T>.Execute(IEnumerable<TypedBlock<T>> previousBlocks, TypedBlock<T> nextBlock)
	{
		if(previousBlocks.Any(x=>x.Data.Equals(nextBlock.Data)))
			throw new ApplicationException($"Person {nextBlock.Data} is already registered");
	}
}
using blockchain.Apps;

namespace blockchain.Blockain.Rules;

class OwnershipRule : IRule<NFTBlock>
{
	public void Execute(IEnumerable<TypedBlock<NFTBlock>> previousBlocks, TypedBlock<NFTBlock> nextBlock)
	{
		if (nextBlock.Data.Data.From == nextBlock.Data.Data.To)
		{
			if (previousBlocks.Any(x => x.Data.Data.Art == nextBlock.Data.Data.Art))
				throw new ApplicationException("You are trying to register the work of art that is already registered");
		}
		else
		{
			foreach (var block in previousBlocks.Reverse())
			{
				if (block.Data.Data.Art == nextBlock.Data.Data.Art)
				{
					if(block.Data.Data.To == nextBlock.Data.Data.From)
						return;
					throw new ApplicationException("You are trying to transfer the work of art that you dont own");
				}
				
			}
			throw new ApplicationException("You are trying to transfer the work of art that has not been yet registered");
		}
	}
}

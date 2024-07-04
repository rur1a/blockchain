using System.Text.Json;
using blockchain.Encryption;

namespace blockchain.Blockain.Rules;

class SignedRule : IRule<TransactionBlock>
{
	private readonly IEncryptor _encryptor;

	public SignedRule(IEncryptor encryptor)
	{
		_encryptor = encryptor;
	}

	public void Execute(IEnumerable<TypedBlock<TransactionBlock>> previousBlocks, TypedBlock<TransactionBlock> nextBlock)
	{
		var transcactionString = JsonSerializer.Serialize(nextBlock.Data.Data);
		if (!_encryptor.VerifySign(nextBlock.Data.Data.From, transcactionString, nextBlock.Data.Sign))
			throw new ApplicationException("Block is signed incorrectly");
	}
}
using System.Text.Json;
using blockchain.Encryption;

namespace blockchain.Blockain.Rules;

interface ISignedBlock<TData>
{
	TData Data { get; }
	string PublicKey { get; }
	string Sign { get; }
}

class SignedRule<TBlock, TData> : IRule<TBlock> where TBlock : ISignedBlock<TData>
{
	private readonly IEncryptor _encryptor;

	public SignedRule(IEncryptor encryptor)
	{
		_encryptor = encryptor;
	}
	

	public void Execute(IEnumerable<TypedBlock<TBlock>> previousBlocks, TypedBlock<TBlock> nextBlock)
	{
		var transcactionString = JsonSerializer.Serialize(nextBlock.Data.Data);
		if (!_encryptor.VerifySign(nextBlock.Data.PublicKey, transcactionString, nextBlock.Data.Sign))
			throw new ApplicationException("Block is signed incorrectly");
	}
}

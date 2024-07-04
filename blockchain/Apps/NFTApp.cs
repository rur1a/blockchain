using System.Text.Json;
using blockchain.Blockain;
using blockchain.Blockain.Rules;
using blockchain.Encryption;
using blockchain.HashFunctions;

namespace blockchain.Apps;

record NFTTransfer(string Art, string From, string To);

record NFTBlock(NFTTransfer Data, string Sign) : ISignedBlock<NFTTransfer>
{
	public string PublicKey => Data.From;
};

public class NFTApp
{
	private readonly IEncryptor _encryptor;
	private readonly TypedBlockchain<NFTBlock> _blockchain;

	public NFTApp(IEncryptor encryptor)
	{
		_encryptor = encryptor;
		_blockchain = new TypedBlockchain<NFTBlock>(
			new Blockhain(new SHA256Hash()),
			new SignedRule<NFTBlock, NFTTransfer>(_encryptor), new OwnershipRule());
	}

	public void RegisterArt(string art, KeyPair author) => TransferArt(art, author, author.PublicKey);

	public void TransferArt(string art, KeyPair owner, string toPublicKey)
	{
		var data = new NFTTransfer(art, owner.PublicKey, toPublicKey);
		var sign = _encryptor.Sign(owner.PrivateKey, JsonSerializer.Serialize(data));
		var block = new NFTBlock(data, sign);
		var b = _blockchain.BuildBlock(block);
		_blockchain.AddBlock(b);
	}
}

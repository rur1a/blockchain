using System.Text.Json;
using blockchain.Blockain;
using blockchain.Blockain.Rules;
using blockchain.Encryption;
using blockchain.HashFunctions;
using blockchain.Services;

namespace blockchain.Apps;

record NFTTransfer(string Art, string From, string To);

record NFTBlock(NFTTransfer Data, string Sign, int Nonce) : ISignedBlock<NFTTransfer>
{
	public string PublicKey => Data.From;
};

public class NFTApp
{
	private readonly IEncryptor _encryptor;
	private readonly TypedBlockchain<NFTBlock> _blockchain;
	private readonly ProofOfWorkService<NFTBlock> _proofOfWorkService;
	
	public NFTApp(IEncryptor encryptor)
	{
		_encryptor = encryptor;
		var proofOfWorkRule = new ProofOfWorkRule<NFTBlock>();
		_blockchain = new TypedBlockchain<NFTBlock>(
			new Blockhain(new SHA256Hash()),
			new SignedRule<NFTBlock, NFTTransfer>(_encryptor), new OwnershipRule(), proofOfWorkRule);
		_proofOfWorkService = new ProofOfWorkService<NFTBlock>(
			_blockchain,
			x => x with { Nonce = x.Nonce + 1},
			proofOfWorkRule);
	}

	public void RegisterArt(string art, KeyPair author) => TransferArt(art, author, author.PublicKey);

	public void TransferArt(string art, KeyPair owner, string toPublicKey)
	{
		var data = new NFTTransfer(art, owner.PublicKey, toPublicKey);
		var sign = _encryptor.Sign(owner.PrivateKey, JsonSerializer.Serialize(data));
		var block = _proofOfWorkService.Proof(_blockchain.Count(),new NFTBlock(data, sign,0));
		var b = _blockchain.BuildBlock(block);
		_blockchain.AddBlock(b);
	}
}

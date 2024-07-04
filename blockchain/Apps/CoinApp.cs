using System.Text.Json;
using blockchain.Blockain;
using blockchain.Blockain.Rules;
using blockchain.Encryption;
using blockchain.HashFunctions;
using blockchain.Services;

namespace blockchain.Apps;

record Transaction(string From, string To, long Amount);


record TransactionBlock(Transaction Data, string Sign, int Nonce) : ISignedBlock<Transaction>
{
	public string PublicKey => Data.From;
}
class CoinApp
{
	private readonly IEncryptor _encryptor;
	private readonly ProofOfWorkService<TransactionBlock> _proofOfWorkService;
	private readonly TypedBlockchain<TransactionBlock> _blockchain;

	public CoinApp(IEncryptor encryptor)
	{
		_encryptor = encryptor;
		_blockchain = new TypedBlockchain<TransactionBlock>(
			new Blockhain(new SHA256Hash()),
			new SignedRule<TransactionBlock,Transaction>(_encryptor), new AmountRule(), new ProofOfWorkRule<TransactionBlock>());
		_proofOfWorkService =
			new ProofOfWorkService<TransactionBlock>(
				_blockchain,
				x => x with { Nonce = x.Nonce + 1 },
				new ProofOfWorkRule<TransactionBlock>());
	}
	
	public void AddTransaction(TransactionBlock transactionBlock)
	{
		var block = _blockchain.BuildBlock(transactionBlock);
		_blockchain.AddBlock(block);
	}
	
	public void PerformTransaction(KeyPair from, string toPublicKey, long amount)
	{
		var transaction = new Transaction(from.PublicKey, toPublicKey, amount);
		var transactionString = JsonSerializer.Serialize(transaction);
		var sign = _encryptor.Sign(from.PrivateKey, transactionString);
		var transactionBlock = _proofOfWorkService.Proof(_blockchain.Count(), new TransactionBlock(transaction, sign,0));
		AddTransaction(transactionBlock);
	}
}
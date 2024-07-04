using System.Text.Json;
using blockchain.Blockain;
using blockchain.Blockain.Rules;
using blockchain.Encryption;
using blockchain.HashFunctions;

namespace blockchain;

record Transaction(string From, string To, long Amount);
record TransactionBlock(Transaction Data, string Sign);
class CoinApp
{
	private readonly IEncryptor _encryptor;
	private readonly TypedBlockchain<TransactionBlock> _blockchain;

	public CoinApp(IEncryptor encryptor)
	{
		_encryptor = encryptor;
		_blockchain = new TypedBlockchain<TransactionBlock>(
			new Blockhain(new SHA256Hash()),
			new SignedRule(_encryptor), new AmountRule());
	}
	
	public void AddTransaction(TransactionBlock transactionBlock)
	{
		_blockchain.AddBlock(transactionBlock);
	}
	
	public void PerformTransaction(KeyPair from, string toPublicKey, long amount)
	{
		var transcaction = new Transaction(from.PublicKey, toPublicKey, amount);
		var transactionString = JsonSerializer.Serialize(transcaction);
		var sign = _encryptor.Sign(from.PrivateKey, transactionString);
		var transactionBlock = new TransactionBlock(transcaction, sign);
		AddTransaction(transactionBlock);
	}
}
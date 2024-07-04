using System.Text.Json;
using blockchain;
using blockchain.Encryption;

var rsaEncryptor = new RSAEncryptor();
var coinApp = new CoinApp(rsaEncryptor);
var user1 = rsaEncryptor.GenerateKeys();
var user2 = rsaEncryptor.GenerateKeys();
var user3 = rsaEncryptor.GenerateKeys();

coinApp.PerformTransaction(user1, user2.PublicKey, 50);
coinApp.PerformTransaction(user2, user3.PublicKey, 125);

var transaction = new Transaction(user1.PublicKey, user2.PublicKey, 200);
var sign = rsaEncryptor.Sign(user2.PrivateKey, JsonSerializer.Serialize(transaction));
coinApp.AddTransaction(new TransactionBlock(transaction,sign));

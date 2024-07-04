using blockchain.Apps;
using blockchain.Encryption;

var rsaEncryptor = new RSAEncryptor();
var nftApp = new NFTApp(rsaEncryptor);
var user1 = rsaEncryptor.GenerateKeys();
var user2 = rsaEncryptor.GenerateKeys();
var user3 = rsaEncryptor.GenerateKeys();

nftApp.RegisterArt("Mona Lisa", user1);

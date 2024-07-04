using blockchain;
using blockchain.Apps;
using blockchain.Encryption;

var rsaEncryptor = new RSAEncryptor();
var nftApp = new NFTApp(rsaEncryptor);
var user1 = rsaEncryptor.GenerateKeys();
var user2 = rsaEncryptor.GenerateKeys();
var user3 = rsaEncryptor.GenerateKeys();

var monaLisa = "Mona Lisa";
nftApp.RegisterArt(monaLisa, user1);
nftApp.TransferArt(monaLisa, user1, user2.PublicKey);
nftApp.TransferArt(monaLisa, user2, user3.PublicKey);

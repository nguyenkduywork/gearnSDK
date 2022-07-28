# gearnSDK

<ol>
  <li> To Start, go to https://github.com/ChainSafe/web3.unity/releases and download the web3.unitypackage v1.3.0, then import it into your game </li>
  <li> In Unity, open Assets/Web3Unity/Scences/WalletLogin, then go to File/Build Settings then click Add Open Scenes, make sure the WalletLogin scene is
    placed as the first scene, before the main game. </li>
  <li> In Assets/GearnSDK, you will see prefabs and scripts with detailed comments and explications.
  
  <li> Demo: <a href="https://www.youtube.com/watch?v=apa6zE67mM4"> Click here </a> </li>
</ol>

<h2> Prefabs </h2>
<Table>
  <tr>
    <th> BuyNFT </th>
    <th> GetBalances </th>
    <th> MintButton </th>
  </tr>
  <tr>
    <td> Contains a button that will open Opensea Marketplace </td>
    <td> Contains text fields that shows your ETH Rinkeby and ERC20 balances </td>
    <td> Contains a button and its script to mint IRC token, this script can be easily changed to call custom <b>Solidity</b> scripts (you need to change its abi and function parameters) </td>
  </tr>
<tr>
    <th> RefreshButton </th>
    <th> NFTShowcase </th>
    <th> GetUserAddress </th>
  </tr>
  <tr>
    <td> Contains a button that refreshes our balances </td>
    <td> Contains a scene, scripts and prefabs to see your wallet's ERC1155 tokens </td>
    <td> Contains a text field which shows your wallet's public address </td>
  </tr>
  <tr>
    <th> ReturnToGame </th>
    <th> Web3 Database </th>
  </tr>
  <tr>
    <td> Contains a button and a script to go back to the game scene </td>
    <td> Contains 4 folders: Scenes, Materials, Scripts and Resources. In the Resources folder contains a txt file which you can
    put your NFT addresses in. This acts as a database for NFTs. ATTENTION: The Materials folder only works if put inside the Resources folder (Line 54 FindUserErc1155.cs)</td>
  </tr>  
</Table>

<h2> Scripts </h2>
<ul>
  <li> ETHBalance: a script that show your ETH Rinkeby balance. </li>
  <li> ERC20BalanceGearn: a script that show your ERC20 token balance. </li>
  <li> GetWalletAddress. </li>
  <li> RefreshButton. </li>
  <li> MintToken: A script that: verify the user has in game Diamonds, then try to convert them into IRC2 tokens on the Rinkeby blockchain to the user's wallet (Contract address of IRC is: 0x9B7E19548fb3E11CF24cd2140FBE9271ef6E61EF) </li>
  <li> CallSolidityFuncForButton: A general script to call simple Solidity functions. Here I use mint function as an example </li>
  <li> BurnButton: A script to call the Burn function from Solidity and assign it to a button. Useful for "Web3 shopping" in game </li>  
  <li> ApplyNftTexture: A script that changes the material of a game object into the user's desired NFT's image </li>
  <li> NftScene_ButtonListener: A script that is used to save the wallet address and bring it to the nft showcase scene when the user clicks on the button "Your NFTs" </li>
  <li> JSONReader: A script that reads the contents in the JSON Text file </li>
  <li> FindUserGameNft: A script that compares the NFTs that the user has with the NFTs stored inside the JSON Text file, then apply the texture to a game object </li>
</ul>


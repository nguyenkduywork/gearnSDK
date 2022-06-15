# gearnSDK

<ol>
  <li> To Start, go to https://github.com/ChainSafe/web3.unity/releases and download the web3.unitypackage v1.3.0, then import it into your game </li>
  <li> In Unity, open Assets/Web3Unity/Scences/WalletLogin, then go to File/Build Settings then click Add Open Scenes, make sure the WalletLogin scene is
    placed as the first scene, before the main game. </li>
  <li> In Assets/GearnSDK, you will see prefabs and scripts with detailed comments and explications.
</ol>

<h2> Prefabs </h2>
<Table>
  <tr>
    <th> BuyNFT </th>
    <th> ETH </th>
  </tr>
  <tr>
    <td> Contains a button that will open Opensea Marketplace </td>
    <td> Contains a text field that shows your ETH Rinkeby balance </td>
  </tr>
</Table>

<h2> Scripts </h2>
<ul>
  <li> ETHBalance: a script that show your ETH Rinkeby balance </li>
  <li> ERC20BalanceGearn: a script that show your ERC20 token balance </li>
  <li> GetWalletAddress </li>
  <li> RefreshButton </li>
  <li> MintToken: A script that: verify the user has more than 185000 in game cash, if the condition is verified, takes all in game cash,
       then mint 185 IRC tokens on the Rinkeby blockchain to the user's wallet (Contract address of IRC is: 0xD40d1f9854e989225c88935E79d2EF0033d4369c) </li>

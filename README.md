# gearnSDK
A Mobile Idle game, connected with Web3 using Metamask

for IDLE restaurant, the things to change are:

GoToBalanceScene.cs:

line 19: DataManager.Instance.database.diamond = PlayerPrefs.GetInt("InGameMoney");

line 34: int currentInGameBalance = DataManager.Instance.database.diamond;

Apart from that, you may need to resize text box, etc. according to ios/android/ different screen sizes

# gearnSDK
A Mobile Idle game, connected with Web3 using Metamask

for IDLE restaurant, the things to change are:

GoToBalanceScene.cs:

line 19: DataManager.Instance.database.diamond = PlayerPrefs.GetInt("InGameMoney");

line 34: int currentInGameBalance = DataManager.Instance.database.diamond;

ReturnToGameScene.cs: change loadScene index (line 13)

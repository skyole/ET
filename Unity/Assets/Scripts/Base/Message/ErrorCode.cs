namespace Model
{
	public static class ErrorCode
	{
        public const int ERR_Success = 0;
        public const int ERR_RpcFail = 1;
        public const int ERR_AccountOrPasswordError = 2;
        public const int ERR_ConnectGateKeyError = 3;
        public const int ERR_ReloadFail = 4;
        public const int ERR_NotFoundUnit = 5;

        public const int ERR_AccountOnline = 10;
        public const int ERR_AccountAlreadyRegister = 11;
        public const int ERR_QueryUserInfoError = 12;
        public const int ERR_JoinRoomError = 13;
        public const int ERR_StartMatchError = 14;
        public const int ERR_UserMoneyLessError = 15;
        public const int ERR_PlayCardError = 16;
    }
}